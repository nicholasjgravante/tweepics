using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Tweepics.Core.Models;
using Tweepics.Core.Database;
using System.Linq;

namespace Tweepics.Web.Services
{
    public class InMemoryTweetData : ITweetData
    {
        private IConfiguration _configuration;

        public InMemoryTweetData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Tweet> GetAll()
        {
            string connectionString = _configuration["ConnectionString"];

            TweetReader dbTweets = new TweetReader(connectionString);
            List<Tweet> tweets = new List<Tweet>();
            tweets = dbTweets.ReadAll();

            return tweets;
        }

        public List<Tweet> FindByTag(string tag)
        {
            string connectionString = _configuration["ConnectionString"];

            TweetReader tweetFinder = new TweetReader(connectionString);
            List<Tweet> tweets = new List<Tweet>();
            tweets = tweetFinder.FindTweetsByTag(tag);

            if (tweets == null)
            {
                return null;
            }
            else
            {
                tweets = tweets.OrderByDescending(tweet => tweet.CreatedAt).ToList();
                return tweets;
            }
        }
    }
}