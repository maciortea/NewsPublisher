using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
        public IActionResult GetMyArticles()
        {
            return View("MyArticles", new List<ArticleViewModel>());
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public IActionResult GetAllArticles()
        {
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
