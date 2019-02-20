using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _newsRepository.GetAllAsync();
            var model = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();

            return View("ArticleList", model);
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
        public IActionResult PublishArticle(ArticleEditViewModel model)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpPut]
        public IActionResult EditArticle(ArticleEditViewModel model)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpDelete]
        public IActionResult DeleteArticle(long id)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy = "RequirePublisherRole")]
        [HttpGet]
        public async Task<IActionResult> GetMyArticles()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var articles = await _newsRepository.GetAllByAuthorIdAsync(user.Id);
            var model = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();

            return View("ArticleList", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
