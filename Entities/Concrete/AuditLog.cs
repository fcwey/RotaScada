using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class AuditLog : IEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; } = "System";
        public string UserIpAddress { get; set; } = "127.0.0.1";
        public string ActionType { get; set; } = string.Empty; // "RECIPE_UPDATE", "PLC_START", "LOGIN"
        public string ModuleName { get; set; } = string.Empty; // "RecipeManager", "PlcController"
        public string Description { get; set; } = string.Empty;
        public string? OldValues { get; set; }                // JSON Formatında
        public string? NewValues { get; set; }                // JSON Formatında
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsSuccess { get; set; } = true;
    }
}
