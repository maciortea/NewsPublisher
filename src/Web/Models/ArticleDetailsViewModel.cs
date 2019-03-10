using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class ArticleDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
        public bool LikedByMe { get; set; }
        public int LikesCount { get; set; }
        public List<ArticleComment> Comments { get; set; }
    }
}
