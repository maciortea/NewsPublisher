using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ArticleEditViewModel
    {
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Body")]
        public string Body { get; set; }
    }
}
