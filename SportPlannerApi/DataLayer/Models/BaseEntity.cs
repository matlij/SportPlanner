using System;

namespace SportPlannerIngestion.DataLayer.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
