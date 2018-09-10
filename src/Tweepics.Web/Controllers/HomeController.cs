using Microsoft.AspNetCore.Mvc;
using Tweepics.Web.Services;

namespace Tweepics.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITags _tags;

        public HomeController(ITags tags)
        {
            _tags = tags;
        }

        public IActionResult AllTags()
        {
            var model = _tags.TagsInSublists();

            return View(model);
        }
    }
}
