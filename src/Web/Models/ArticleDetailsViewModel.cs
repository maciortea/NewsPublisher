using System;

namespace Web.Models
{
    public class ArticleDetailsViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
    }
}
