using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
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
            var model = new ArticleDetailsViewModel
            {
                Title = article.Title,
                Body = article.Body,
                PublishDate = article.PublishDate,
                Author = author.UserName
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
