using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.Utilities.Interceptors;
using Core.Utilities.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Exception
{
    public class ExceptionLogAspect : MethotInterception
    {
        private LoggerServiceBase _loggerServiceBase;

        public ExceptionLogAspect(Type loggerService)
        {
            if (loggerService.BaseType != typeof(LoggerServiceBase))
            {

                throw new System.Exception(AspectMessages.WrongLoggerType);
            }
            _loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(loggerService);
        }

        protected override void OnExeption(IInvocation invocation, System.Exception e)
        {
            LogDetailWithExcetion logDetailWithExcetion = GetLogDetail(invocation);
            logDetailWithExcetion.ExceptionMessage = e.Message;
            _loggerServiceBase.Error(logDetailWithExcetion);
        }

        private LogDetailWithExcetion GetLogDetail(IInvocation invocation)
        {
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value = invocation.Arguments[i],
                    Type = invocation.Arguments[i] != null ? invocation.Arguments[i].GetType().Name : "" //invocation.Arguments[i].GetType().Name
                });
            }
            var logDetailWithException = new LogDetailWithExcetion
            {
                MethodName = invocation.Method.Name,
                LogParameters = logParameters,
            };
            return logDetailWithException;
        }
    }
}
