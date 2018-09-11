using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tweepics.Core.Models;
using Tweepics.Web.Services;
using Tweepics.Web.ViewModels.Display;

namespace Tweepics.Web.Controllers
{
    //[Route("[controller]/[action]")]
    public class DisplayController : Controller
    {
        private ITweetData _tweetData;
        private IConfiguration _configuration;
        private ITags _tagData;

        public DisplayController(ITweetData tweetData,
                                 ITags tagData,
                                 IConfiguration configuration)
        {
            _tweetData = tweetData;
            _configuration = configuration;
            _tagData = tagData;
        }

        public IActionResult Tweets()
        {
            var model = _tweetData.GetAll();

            return View(model);
        }

        public IActionResult Topic(string tag)
        {
            List<string> tags = _tagData.GetAllTags();

            if (!tags.Contains(tag))
            {
                return Content("No such tag was found.");
            }

            List<Tweet> tweets = _tweetData.FindByTag(tag);

            var model = new DisplayTweetsViewModel();
            model.Tweets = tweets;
            model.Topic = tag;

            if (model.Tweets == null)
            {
                return Content("No tweets were found.");
            }

            return View(model);
        }
    }
}
