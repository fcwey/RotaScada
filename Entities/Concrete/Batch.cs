using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Batch : IEntity
    {
        public int Id { get; set; }
        public string BatchCode { get; set; } = string.Empty;   // Örn: "BATCH-20261018-001"
        public int RecipeId { get; set; }
        public int OperatorUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } = "RUNNING";         // RUNNING, COMPLETED, ABORTED
        public string? CompletionNote { get; set; }

        // Navigation
        public virtual Recipe? Recipe { get; set; }
    }
}
