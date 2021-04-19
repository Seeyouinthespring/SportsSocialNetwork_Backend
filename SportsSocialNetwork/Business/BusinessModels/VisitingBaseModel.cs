using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class VisitingBaseModel
    {
        public string MemberId { get; set; }

        public long AppointmentId { get; set; }

        public byte Status { get; set; }
    }
}
