using System;

namespace Tweepics.Core.Models
{
    public class PublicOfficial
    {
        public string TweepicsId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string State { get; set; }
        public string Party { get; set; }
        public long TwitterId { get; set; }
        public string TwitterScreenName { get; set; }

        public PublicOfficial () { }

        public PublicOfficial(string firstName, string middleName, string lastName, string state, 
                              string party, long twitterId, string twitterScreenName)
            : this ("", firstName, middleName, lastName, state, party, twitterId, twitterScreenName) { }

        public PublicOfficial(string tweepicsId, string firstName, string middleName, string lastName, 
                              string state, string party, long twitterId, string twitterScreenName)
        {
            TweepicsId = tweepicsId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            FullName = firstName + " " + middleName + " " + lastName;
            State = state;
            Party = party;
            TwitterId = twitterId;
            TwitterScreenName = twitterScreenName;
        }
    }
}
