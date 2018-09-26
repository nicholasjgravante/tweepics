using System;

namespace Tweepics.Core.Models
{
    public class PublicOfficial
    {
        public string TweepicsId { get; set; }
        public Name Name { get; set; }
        public Office Office { get; set; }
        public string Party { get; set; }
        public long TwitterId { get; set; }
        public string TwitterScreenName { get; set; }

        public PublicOfficial () { }

        public PublicOfficial(Name name, Office office, string party, long twitterId, string twitterScreenName)
            : this ("", name, office, party, twitterId, twitterScreenName) { }

        public PublicOfficial(string tweepicsId, Name name, Office office, string party, long twitterId, 
                              string twitterScreenName)
        {
            TweepicsId = tweepicsId;
            Name = name;
            Office = office;
            Party = party;
            TwitterId = twitterId;
            TwitterScreenName = twitterScreenName;
        }
    }
}
