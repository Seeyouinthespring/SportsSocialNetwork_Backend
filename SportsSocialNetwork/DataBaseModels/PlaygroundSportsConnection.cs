using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.DataBaseModels
{
    public class PlaygroundSportsConnection : BaseEntity
    {
        [ForeignKey(nameof(Playground))]
        public long PlaygroundId { get; set; }
        public Playground Playground { get; set; }

        [ForeignKey(nameof(Sport))]
        public long SportId {get; set;}
        public Sport Sport { get; set; }
    }
}
