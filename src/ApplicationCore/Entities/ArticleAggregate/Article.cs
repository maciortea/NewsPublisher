using ApplicationCore.Common;
using ApplicationCore.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Entities.ArticleAggregate
{
    public class Article : Entity
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public DateTime PublishDate { get; private set; }
        public string AuthorId { get; private set; }

        private readonly List<ArticleLike> _likes = new List<ArticleLike>();
        public IReadOnlyList<ArticleLike> Likes => _likes.AsReadOnly();

        private Article()
        {
        }

        public Article(string title, string body, string authorId)
        {
            Contract.Require(!string.IsNullOrWhiteSpace(title), "Title is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(body), "Body is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(authorId), "Author is required.");

            Title = title;
            Body = body;
            PublishDate = DateTime.Now;
            AuthorId = authorId;
        }

        public void Edit(string title, string body)
        {
            Contract.Require(!string.IsNullOrWhiteSpace(title), "Title is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(body), "Body is required.");

            Title = title;
            Body = body;
        }

        public void LikeOrDislike(string userId)
        {
            ArticleLike articleLike = _likes.SingleOrDefault(al => al.UserId == userId);
            if (articleLike != null)
            {
                _likes.Remove(articleLike);
            }
            else
            {
                _likes.Add(new ArticleLike(this, userId));
            }
        }
    }
}
