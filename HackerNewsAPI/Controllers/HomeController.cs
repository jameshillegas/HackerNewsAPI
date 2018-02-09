using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HackerNewsAPI.Utilities;
using HackerNewsAPI.Models;
using System.Threading.Tasks;

namespace HackerNewsAPI.Controllers
{
    public class HomeController : Controller
    {
        private IHackerNewsService hackerNewsSvc;

        public HomeController()
        {
            hackerNewsSvc = new HackerNewsService();
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var stories = await hackerNewsSvc.GetBestStories();
                return View(stories);
            }
            catch (Exception)
            {
                ViewBag.error = "There was a problem retrieving the stories.";
                return View(new List<Story>());
            }
        }

        public async Task<ActionResult> Search(string text)
        {
            try
            {
                ViewBag.SearchQuery = text;
                var stories = await hackerNewsSvc.GetBestStories();
                var results = stories.Where(t => t.Title.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1);
                return View(results);
            }
            catch (Exception)
            {
                ViewBag.error = "There was a problem retrieving the stories.";
                return View("Index", new List<Story>());
            }
        }
    }
}