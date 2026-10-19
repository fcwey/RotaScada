using DataAccess.Abstract;
using Entities.Enums;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace DataAccess.Concrete.S7Net
{
    public class S7DriverDal : IS7DriverDal, IDisposable
    {
        private Plc? _plc;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private string _ipAddress = "192.168.0.1";
        private CpuType _cpuType = CpuType.S71200;
        private short _rack = 0;
        private short _slot = 1;
        private bool _disposed = false;

        public bool IsConnected => _plc != null && _plc.IsConnected;

        public async Task<bool> ConnectAsync(string ipAddress, CpuType cpuType = CpuType.S71200, short rack = 0, short slot = 1)
        {
            _ipAddress = ipAddress;
            _cpuType = cpuType;
            _rack = rack;
            _slot = slot;

            await _semaphore.WaitAsync();
            try
            {
                if (_plc != null && _plc.IsConnected)
                    _plc.Close();

                _plc = new Plc(_cpuType, _ipAddress, _rack, _slot);
                await _plc.OpenAsync();
                return _plc.IsConnected;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Disconnect()
        {
            _semaphore.Wait();
            try
            {
                if (_plc != null && _plc.IsConnected)
                {
                    _plc.Close();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> PingAsync(string? ipAddress = null)
        {
            string targetIp = ipAddress ?? _ipAddress;
            using var ping = new Ping();
            try
            {
                var reply = await ping.SendPingAsync(targetIp, 1000);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

  

        public async Task<T?> ReadClassAsync<T>(T instance, int dbNumber, int startByte = 0) where T : class, new()
        {
            await _semaphore.WaitAsync();
            try
            {
                EnsureConnected();
                await _plc!.ReadClassAsync(instance, dbNumber, startByte);
                return instance;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task WriteClassAsync<T>(T instance, int dbNumber, int startByte = 0) where T : class
        {
            await _semaphore.WaitAsync();
            try
            {
                EnsureConnected();
                await _plc!.WriteClassAsync(instance, dbNumber, startByte);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<object?> ReadTagAsync(DataType dataType, int db, int startByte, VarType varType, int varCount = 1)
        {
            await _semaphore.WaitAsync();
            try
            {
                EnsureConnected();
                return await _plc!.ReadAsync(dataType, db, startByte, varType, varCount);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task WriteTagAsync(DataType dataType, int db, int startByte, object value)
        {
            await _semaphore.WaitAsync();
            try
            {
                EnsureConnected();
                await _plc!.WriteAsync(dataType, db, startByte, value);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> ReadBitAsync(DataType dataType, int db, int startByte, int bitNumber)
        {
            await _semaphore.WaitAsync();
            try
            {
                EnsureConnected();
                var result = await _plc!.ReadAsync(dataType, db, startByte, VarType.Bit, 1);
                if (result is bool boolValue) return boolValue;

                // Byte maskeleme yöntemi
                byte byteValue = Convert.ToByte(result);
                return (byteValue & (1 << bitNumber)) != 0;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task WriteBitAsync(DataType dataType, int db, int startByte, int bitNumber, bool value)
        {
            await _semaphore.WaitAsync();
            try
            {
                EnsureConnected();
                // S7.Net DBX formatında bit yazımı: e.g. "DB1.DBX0.1"
                string address = $"{dataType}{db}.DBX{startByte}.{bitNumber}";
                if (dataType == DataType.Memory) address = $"M{startByte}.{bitNumber}";

                await _plc!.WriteAsync(address, value);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task PulseBitAsync(DataType dataType, int db, int startByte, int bitNumber, int durationMs = 500)
        {
            await WriteBitAsync(dataType, db, startByte, bitNumber, true);
            await Task.Delay(durationMs);
            await WriteBitAsync(dataType, db, startByte, bitNumber, false);
        }

        public async Task<bool> ExecuteCommandWithAckAsync(
            int cmdDb, int cmdByte, int cmdBit,
            int ackDb, int ackByte, int ackBit,
            int timeoutMs = 3000)
        {
            // 1. Komut Biti Gönderilir (TRUE)
            await WriteBitAsync(DataType.DataBlock, cmdDb, cmdByte, cmdBit, true);

            using var cts = new CancellationTokenSource(timeoutMs);
            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    bool ack = await ReadBitAsync(DataType.DataBlock, ackDb, ackByte, ackBit);
                    if (ack)
                    {
                        // 2. Onay Alındı, Komut Biti Sıfırlanır (FALSE)
                        await WriteBitAsync(DataType.DataBlock, cmdDb, cmdByte, cmdBit, false);
                        return true;
                    }
                    await Task.Delay(50, cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // Zaman aşımı: Komut biti güvenlik amacıyla sıfırlanır
                await WriteBitAsync(DataType.DataBlock, cmdDb, cmdByte, cmdBit, false);
            }
            return false;
        }

        public async Task<List<DataItem>> ReadMultipleTagsAsync(List<DataItem> dataItems)
        {
            await _semaphore.WaitAsync();
            try
            {
                EnsureConnected();
                await _plc!.ReadMultipleVarsAsync(dataItems);
                return dataItems;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private void EnsureConnected()
        {
            if (_plc == null || !_plc.IsConnected)
            {
                _plc = new Plc(_cpuType, _ipAddress, _rack, _slot);
                _plc.Open();
                if (!_plc.IsConnected)
                {
                    throw new InvalidOperationException($"PLC ile iletişim kurulamadı. IP: {_ipAddress}");
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_plc != null && _plc.IsConnected)
                {
                    _plc.Close();
                }
                _semaphore.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}

