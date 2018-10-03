using Microsoft.AspNetCore.Mvc;
using Tweepics.Web.Models;
using Tweepics.Web.Services;
using Tweepics.Web.ViewModels.Display;

namespace Tweepics.Web.Controllers
{
    public class DisplayController : Controller
    {
        private ITweetResults _tweetResults;

        public DisplayController(ITweetResults tweetResults)
        {
            _tweetResults = tweetResults;
        }

        public IActionResult Tweets(TweetQueryInputModel inputQuery)
        {
            if (ModelState.IsValid)
            {
                TweetQueryModel query = new TweetQueryModel();
                query.QueryType = inputQuery.QueryType;
                query.OriginalQuery = inputQuery.OriginalQuery;
                query.Page = inputQuery.Page;
                query.FilterOptions = new FilterOptions
                {
                    Officials = inputQuery.Officials,
                    States = inputQuery.States,
                    Parties = inputQuery.Parties
                };

                DisplayTweetsViewModel model = _tweetResults.GetTweetResults(query);

                if (model == null)
                {
                    return View("TweetsNotFound");
                }

                return View(model);
            }
            else
            {
                // To do: Display validation errors
                return View("TweetsNotFound");
            }

        }
    }
}
