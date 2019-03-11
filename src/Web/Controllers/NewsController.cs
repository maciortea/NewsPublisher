using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.ArticleAggregate;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INewsRepository _newsRepository;

        public NewsController(UserManager<IdentityUser> userManager, INewsRepository newsRepository)
        {
            _userManager = userManager;
            _newsRepository = newsRepository;
        }

        [Authorize(Policy = "RequirePublisherOrUser")]
        [HttpGet]
        public async Task<IActionResult> Articles()
        {
            var articles = await _newsRepository.GetAllAsync();
            var model = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();

            return View("ArticleList", model);
        }

        [Authorize(Policy = "RequirePublisherOrUser")]
        [HttpGet]
        public async Task<IActionResult> Article(int id)
        {
            Article article = await _newsRepository.GetByIdAsync(id);
            IdentityUser author = await _userManager.FindByIdAsync(article.AuthorId);
            IdentityUser loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);

            List<ArticleComment> comments = article.Comments
                .Select(c => new ArticleComment(c.Text, c.Username, c.CreatedOn))
                .OrderBy(c => c.CreatedOn)
                .ToList();

            var model = new ArticleDetailsViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Body = article.Body,
                PublishDate = article.PublishDate,
                Author = author.UserName,
                LikedByMe = article.Likes.Any(l => l.UserId == loggedUser.Id),
                LikesCount = article.Likes.Count,
                Comments = comments
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpGet]
        public IActionResult PublishArticle()
        {
            return View(new ArticleEditViewModel());
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpPost]
        public async Task<IActionResult> PublishArticle(ArticleEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            var article = new Article(model.Title, model.Body, user.Id);

            await _newsRepository.AddAsync(article);

            return RedirectToAction("GetMyArticles");
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpGet]
        public async Task<IActionResult> EditArticle(int id)
        {
            Article article = await _newsRepository.GetByIdAsync(id);
            var model = new ArticleEditViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Body = article.Body
            };

            return View(model);
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpPost]
        public async Task<IActionResult> EditArticle(ArticleEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Article article = await _newsRepository.GetByIdAsync(model.Id);
            article.Edit(model.Title, model.Body);

            await _newsRepository.UpdateAsync(article);

            return RedirectToAction("GetMyArticles");
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpGet]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            Article article = await _newsRepository.GetByIdAsync(id);

            await _newsRepository.DeleteAsync(article);

            return RedirectToAction("GetMyArticles");
        }

        [Authorize(Policy = "RequirePublisherOrUser")]
        [HttpPost]
        public async Task<IActionResult> LikeOrDislikeArticle(int id)
        {
            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Article article = await _newsRepository.GetByIdAsync(id);

            article.LikeOrDislike(user.Id);
            await _newsRepository.UpdateAsync(article);

            return Ok(article.Likes.Count);
        }

        [Authorize(Policy = "RequirePublisherOrUser")]
        [HttpPost]
        public async Task<IActionResult> Comment(int id, string text)
        {
            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Article article = await _newsRepository.GetByIdAsync(id);

            article.AddComment(user.Id, User.Identity.Name, text);
            await _newsRepository.UpdateAsync(article);

            Comment addedComment = article.Comments.Last();
            var model = new ArticleComment(addedComment.Text, addedComment.Username, addedComment.CreatedOn);

            return PartialView("_Comment", model);
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpGet]
        public async Task<IActionResult> GetMyArticles()
        {
            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            var articles = await _newsRepository.GetAllByAuthorIdAsync(user.Id);
            var model = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();

            return View("MyArticles", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
