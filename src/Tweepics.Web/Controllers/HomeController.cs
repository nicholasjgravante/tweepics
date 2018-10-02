using Microsoft.AspNetCore.Mvc;
using Tweepics.Web.Services;
using Tweepics.Web.Models;
using Tweepics.Web.ViewModels.Home;

namespace Tweepics.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITags _tags;

        public HomeController(ITags tags)
        {
            _tags = tags;
        }

        public IActionResult Index()
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Tags = _tags.GetAllTags();
            model.TweetQueryInputModel = new TweetQueryInputModel();

            return View(model);
        }
    }
}
