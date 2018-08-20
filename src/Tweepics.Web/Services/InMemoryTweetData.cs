using System.Collections.Generic;
using Tweepics.Core.Database.Operations;
using Tweepics.Core.Parse;
using Microsoft.Extensions.Configuration;

namespace Tweepics.Web.Services
{
    public class InMemoryTweetData : ITweetData
    {
        private IConfiguration _configuration;

        public InMemoryTweetData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<TweetData> GetAll()
        {
            TweetsFromDB dbTweets = new TweetsFromDB();
            List<TweetData> tweets = new List<TweetData>();

            string connectionString = _configuration["ConnectionString"];

            tweets = dbTweets.Read(connectionString);

            return tweets;
        }

        public List<TweetData> FindByTag(string tag)
        {
            List<TweetData> tweets = new List<TweetData>();

            TweetsByTag tweetFinder = new TweetsByTag();
            tweets = tweetFinder.Find(tag, _configuration["ConnectionString"]);

            return tweets;
        }
    }
}