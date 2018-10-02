using System.Collections.Generic;
using System.Linq;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public class TweetFilter
    {
        public List<TweetsByOfficial> FilterResults(List<TweetsByOfficial> resultsPreFilter, FilterOptions filterOptions)
        {
            List<TweetsByOfficial> results = resultsPreFilter;

            if (filterOptions.Officials != null && filterOptions.Officials.Any())
            {
                results = FilterByPublicOfficial(results, filterOptions.Officials);
            }

            if (filterOptions.States != null && filterOptions.States.Any())
            {
                results = FilterByState(results, filterOptions.States);
            }

            if (filterOptions.Parties != null && filterOptions.Parties.Any())
            {
                results = FilterByParty(results, filterOptions.Parties);
            }

            return results;
        }

        public List<TweetsByOfficial> FilterByPublicOfficial(List<TweetsByOfficial> tweetsByOfficial, List<string> officials)
        {
            List<string> officialsLowercase = officials.ConvertAll(official => official.ToLower());

            List<TweetsByOfficial> results = new List<TweetsByOfficial>();

            foreach (var entry in tweetsByOfficial)
            {
                if (officialsLowercase.Contains(entry.PublicOfficial.Name.FirstLast.ToLower()))
                {
                    results.Add(entry);
                }
            }

            return results;
        }

        public List<TweetsByOfficial> FilterByState(List<TweetsByOfficial> tweetsByOfficial, List<string> states)
        {
            List<string> statesLowercase = states.ConvertAll(state => state.ToLower());

            List<TweetsByOfficial> results = new List<TweetsByOfficial>();

            foreach(var entry in tweetsByOfficial)
            {
                if (statesLowercase.Contains(entry.PublicOfficial.Office.State.ToLower()))
                {
                    results.Add(entry);
                }
            }

            return results;
        }

        public List<TweetsByOfficial> FilterByParty(List<TweetsByOfficial> tweetsByOfficial, List<string> parties)
        {
            List<string> partiesLowercase = parties.ConvertAll(party => party.ToLower());

            List<TweetsByOfficial> results = new List<TweetsByOfficial>();

            foreach (var entry in tweetsByOfficial)
            {
                if (partiesLowercase.Contains(entry.PublicOfficial.Party.ToLower()))
                {
                    results.Add(entry);
                }
            }

            return results;
        }
    }
}
