using System;

namespace SportsSocialNetwork.DataBaseModels
{
    public abstract class BaseDateEntity : BaseEntity
    {
        public DateTime Date { get; set; }
    }
}
