using System.ComponentModel.DataAnnotations;

namespace SportsSocialNetwork.DataBaseModels
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
