using System;
using System.ComponentModel.DataAnnotations;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class CommentDtoModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
    }

    public class CommentViewModel : CommentDtoModel 
    {
        public string AuthorId { get; set; }

        public ApplicationUserMessageViewModel Author { get; set; }

        public DateTime Date { get; set; }

        public long PlaygroundId { get; set; }
    }
}
