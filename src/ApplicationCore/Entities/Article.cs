using ApplicationCore.Common;
using ApplicationCore.SharedKernel;
using System;

namespace ApplicationCore.Entities
{
    public class Article : Entity
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public DateTime PublishDate { get; private set; }
        public string AuthorId { get; private set; }

        private Article(string title, string body, string authorId)
        {
            Contract.Require(!string.IsNullOrWhiteSpace(title), "Title is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(body), "Body is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(authorId), "Author is required.");

            Title = title;
            Body = body;
            PublishDate = DateTime.Now;
            AuthorId = authorId;
        }
    }
}
