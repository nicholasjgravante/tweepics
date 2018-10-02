using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public interface ITweetsByOfficialData
    {
        List<TweetsByOfficial> MatchTweetsToOfficial(List<Tweet> tweets);
    }
}
