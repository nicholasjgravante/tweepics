using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tweepics.Web.Services;

namespace Tweepics.Web.Controllers
{
    //[Route("[controller]/[action]")]
    public class DisplayController : Controller
    {
        private ITweetData _tweetData;
        private IConfiguration _configuration;

        public DisplayController(ITweetData tweetData,
                                 IConfiguration configuration)
        {
            _tweetData = tweetData;
            _configuration = configuration;
        }

        public IActionResult Tweets()
        {
            var model = _tweetData.GetAll();

            return View(model);
        }

        public IActionResult Topic(string tag)
        {
            var model = _tweetData.FindByTag(tag);

            if(model==null)
            {
                return Content("No tweets were found.");
            }
            return View(model);
        }
    }
}
