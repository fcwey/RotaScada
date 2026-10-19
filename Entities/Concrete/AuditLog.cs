using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class AuditLog : IEntity
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; } = string.Empty;  // SETPOINT_CHANGE, RECIPE_DOWNLOAD, MANUAL_VALVE
        public string Description { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string ClientIp { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
