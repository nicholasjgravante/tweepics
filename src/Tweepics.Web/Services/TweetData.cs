using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Tweepics.Core.Models;
using Tweepics.Core.Database;
using System.Linq;

namespace Tweepics.Web.Services
{
    public class TweetData : ITweetData
    {
        private readonly IConfiguration _configuration;

        public TweetData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Tweet> FindByTag(string tag)
        {
            string connectionString = _configuration["ConnectionString"];

            TweetReader tweetFinder = new TweetReader(connectionString);
            List<Tweet> tweets = tweetFinder.FindTweetsByTag(tag);

            if (tweets == null)
            {
                return null;
            }
            else
            {
                return tweets;
            }
        }

        public List<Tweet> FindBySearch(string searchQuery)
        {
            string connectionString = _configuration["ConnectionString"];

            TweetReader tweetFinder = new TweetReader(connectionString);
            List<Tweet> tweets = tweetFinder.SearchUsersAndTweetContent(searchQuery);

            if (tweets == null)
            {
                return null;
            }
            else
            {
                return tweets;
            }
        }
    }
}