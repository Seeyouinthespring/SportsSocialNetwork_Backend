using System;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class TimingIntervalModel
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }

    public class VisitorsNumberViewModel 
    { 
        public int Number { get; set;} 
    }
}
