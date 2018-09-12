using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tweepics.Core.Models;
using Tweepics.Web.Services;
using Tweepics.Web.ViewModels.Display;
using X.PagedList;
using X.PagedList.Mvc.Core;

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

        public IActionResult Topic(string tag, int? page)
        {
            List<string> tags = _tagData.GetAllTags();

            if (!tags.Contains(tag))
            {
                return Content("No such tag was found.");
            }

            List<Tweet> tweets = _tweetData.FindByTag(tag);

            if (tweets == null)
            {
                return Content("No tweets were found.");
            }
            else
            {
                var pageNumber = page ?? 1;
                var onePageOfTweets = tweets.ToPagedList(pageNumber, 25);

                var model = new DisplayTweetsViewModel();
                model.Tweets = onePageOfTweets;
                model.Topic = tag;
                model.ThirtyDayCount = TweetMetrics.TweetsInLastThirtyDays(tweets);

                return View(model);
            }
        }
    }
}
