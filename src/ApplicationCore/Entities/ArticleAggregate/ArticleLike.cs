using ApplicationCore.Common;
using ApplicationCore.SharedKernel;

namespace ApplicationCore.Entities.ArticleAggregate
{
    public class ArticleLike : Entity
    {
        public Article Article { get; private set; }
        public string UserId { get; private set; }

        private ArticleLike()
        {
        }

        public ArticleLike(Article article, string userId)
        {
            Contract.Require(article != null, "Article is required.");
            Contract.Require(!string.IsNullOrWhiteSpace(userId), "User id is required.");

            Article = article;
            UserId = userId;
        }
    }
}
