using ApplicationCore.Common;
using ApplicationCore.SharedKernel;
using System;

namespace ApplicationCore.Entities.ArticleAggregate
{
    public class Comment : Entity
    {
        public Article Article { get; private set; }
        public string UserId { get; private set; }
        public string Username { get; private set; }
        public string Text { get; private set; }
        public DateTime CreatedOn { get; private set; }

        private Comment()
        {
        }

        public Comment(Article article, string userId, string username, string text)
        {
            Contract.Require(article != null, "Article is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(userId), "User id is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(text), "Text is required.");

            Article = article;
            UserId = userId;
            Username = username;
            Text = text;
            CreatedOn = DateTime.Now;
        }
    }
}
