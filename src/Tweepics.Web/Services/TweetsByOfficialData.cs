using System.Collections.Generic;
using System.Linq;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public class TweetsByOfficialData : ITweetsByOfficialData
    {
        private IPublicOfficialData _publicOfficials;

        public TweetsByOfficialData(IPublicOfficialData publicOfficials)
        {
            _publicOfficials = publicOfficials;
        }

        public List<TweetsByOfficial> MatchTweetsToOfficial(List<Tweet> tweets)
        {
            if (tweets == null)
            {
                return null;
            }

            List<PublicOfficial> officials = _publicOfficials.GetAll();

            List<TweetsByOfficial> tweetsByOfficial = new List<TweetsByOfficial>();

            foreach (var official in officials)
            {
                List<Tweet> officialTweets = tweets.Where(tweet => tweet.UserID == official.TwitterId).ToList();

                if (officialTweets.Any())
                {
                    tweetsByOfficial.Add(new TweetsByOfficial(officialTweets, official));
                }
            }

            return tweetsByOfficial;
        }
    }
}
