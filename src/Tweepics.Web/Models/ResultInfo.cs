using System;
using System.Collections.Generic;
using System.Linq;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public class ResultInfo
    {
        public List<PublicOfficial> Officials { get; set; }
        public List<PublicOfficial> USSenators { get; set; }
        public List<string> States { get; set; }
        public List<string> Parties { get; set; }
        public Dictionary<string, decimal> TweetPercentageByParty { get; set; }
        public int ThirtyDayTweetCount { get; set; }

        public ResultInfo(List<TweetsByOfficial> tweetsByOfficial)
        {
            List<PublicOfficial> allOfficials = tweetsByOfficial.Select(x => x.PublicOfficial).ToList();

            List<Tweet> allTweets = tweetsByOfficial.SelectMany(x => x.Tweets).ToList();

            Officials = allOfficials.OrderBy(official => official.Office.State).ToList();

            USSenators = GetUSSenators(allOfficials);

            States = GetUniqueStates(allOfficials);

            Parties = GetUniqueParties(allOfficials);

            TweetPercentageByParty = GetTweetPercentageByParty(allOfficials);

            ThirtyDayTweetCount = GetThirtyDayTweetCount(allTweets);
        }

        public List<PublicOfficial> GetUSSenators(List<PublicOfficial> allOfficials)
        {
            List<PublicOfficial> senators = new List<PublicOfficial>();
            senators = allOfficials.Where(official => official.Office.Government == "Federal" && official.Office.Title == "Senator")
                                   .OrderBy(official => official.Office.State)
                                   .ToList();

            return senators;
        }

        public List<string> GetUniqueStates(List<PublicOfficial> allOfficials)
        {
            List<string> uniqueStates = new List<string>();
            uniqueStates = allOfficials.Select(official => official.Office.State).ToHashSet().ToList();

            uniqueStates.Sort();

            return uniqueStates;
        }

        public List<string> GetUniqueParties(List<PublicOfficial> allOfficials)
        {
            List<string> uniqueParties = new List<string>();
            uniqueParties = allOfficials.Select(official => official.Party).ToHashSet().ToList();

            uniqueParties.Sort();

            return uniqueParties;
        }

        public Dictionary<string, decimal> GetTweetPercentageByParty(List<PublicOfficial> allOfficials)
        {
            Dictionary<string, decimal> percentageByParty = new Dictionary<string, decimal>
            {
                {"Democratic", 0 },
                {"Republican", 0 },
                {"Independent", 0 },
                {"Other", 0 }
            };

            int democratTweets = 0;
            int republicanTweets = 0;
            int independentTweets = 0;
            int otherTweets = 0;

            foreach (var official in allOfficials)
            {
                if (official.Party.Contains("democratic"))
                {
                    democratTweets++;
                }
                else if (official.Party.Contains("republican"))
                {
                    republicanTweets++;
                }
                else if (official.Party.Contains("independent"))
                {
                    independentTweets++;
                }
                else
                {
                    otherTweets++;
                }
            }

            int allOfficialsCount = allOfficials.Count;

            percentageByParty["Democratic"] = democratTweets / allOfficialsCount;
            percentageByParty["Republican"] = republicanTweets / allOfficialsCount;
            percentageByParty["Independent"] = independentTweets / allOfficialsCount;
            percentageByParty["Other"] = otherTweets / allOfficialsCount;

            return percentageByParty;
        }

        public int GetThirtyDayTweetCount(List<Tweet> allTweets)
        {
            DateTime now = DateTime.Now;
            DateTime thirtyDaysAgo = now.AddDays(-30.0);

            int thirtyDayTweetCount = 0;

            foreach (var tweet in allTweets)
            {
                if (tweet.Created >= thirtyDaysAgo)
                {
                    thirtyDayTweetCount++;
                }
            }

            return thirtyDayTweetCount;
        }
    }
}
