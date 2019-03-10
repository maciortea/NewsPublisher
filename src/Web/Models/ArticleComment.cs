using System;

namespace Web.Models
{
    public class ArticleComment
    {
        public string Text { get; set; }
        public string Username { get; set; }
        public DateTime CreatedOn { get; set; }

        public ArticleComment(string text, string username, DateTime createdOn)
        {
            Text = text;
            Username = username;
            CreatedOn = createdOn;
        }
    }
}
