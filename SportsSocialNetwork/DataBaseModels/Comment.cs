using System.ComponentModel.DataAnnotations.Schema;

namespace SportsSocialNetwork.DataBaseModels
{
    public class Comment : BaseDateEntity
    {
        [ForeignKey(nameof(Playground))]
        public long PlaygroundId { get; set; }
        public Playground Playground { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public string Text { get; set; }
    }
}
