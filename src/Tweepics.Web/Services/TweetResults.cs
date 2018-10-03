using System.Collections.Generic;
using System.Linq;
using Tweepics.Core.Models;
using Tweepics.Web.Models;
using Tweepics.Web.ViewModels.Display;
using X.PagedList;

namespace Tweepics.Web.Services
{
    public class TweetResults : ITweetResults
    {
        private ITags _tagData;
        private ITweetData _tweetData;
        private ITweetsByOfficialData _tweetsByOfficial;

        public TweetResults(ITags tagData, ITweetData tweetData,
                            ITweetsByOfficialData tweetPlusOfficial)
        {
            _tagData = tagData;
            _tweetData = tweetData;
            _tweetsByOfficial = tweetPlusOfficial;
        }

        public DisplayTweetsViewModel GetTweetResults(TweetQueryModel query)
        {
            List<Tweet> tweets = new List<Tweet>();

            if (query.QueryType.ToLower() == "search")
            {
                tweets = _tweetData.FindBySearch(query.OriginalQuery);
            }
            else if (query.QueryType.ToLower() == "tag")
            {
                List<string> tags = _tagData.GetAllTags();

                if (!tags.Contains(query.OriginalQuery))
                {
                    return null;
                }
                else
                {
                    tweets = _tweetData.FindByTag(query.OriginalQuery);
                }
            }
            else
            {
                return null;
            }

            if (tweets == null)
            {
                return null;
            }
            else
            {
                List<TweetsByOfficial> originalResults = _tweetsByOfficial.MatchTweetsToOfficial(tweets);

                var pageNumber = query.Page ?? 1;

                var onePageOfTweets = originalResults.SelectMany(x => x.Tweets)
                                                     .OrderByDescending(tweet => tweet.CreatedAt)
                                                     .ToList()
                                                     .ToPagedList(pageNumber, 25);

                TweetFilter filter = new TweetFilter();
                List<TweetsByOfficial> results = filter.FilterResults(originalResults, query.FilterOptions);

                onePageOfTweets = results.SelectMany(x => x.Tweets)
                                         .OrderByDescending(tweet => tweet.CreatedAt)
                                         .ToList()
                                         .ToPagedList(pageNumber, 25);

                var model = new DisplayTweetsViewModel();
                model.QueryType = query.QueryType;
                model.Tweets = onePageOfTweets;
                model.OriginalQuery = query.OriginalQuery;
                model.FiltersSelected = query.FilterOptions;
                model.ResultInfo = new ResultInfo(originalResults);

                return model;
            }
        }
    }
}
