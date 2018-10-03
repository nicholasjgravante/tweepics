using Tweepics.Web.Models;
using Tweepics.Web.ViewModels.Display;

namespace Tweepics.Web.Services
{
    public interface ITweetResults
    {
        DisplayTweetsViewModel GetTweetResults(TweetQueryModel query);
    }
}
