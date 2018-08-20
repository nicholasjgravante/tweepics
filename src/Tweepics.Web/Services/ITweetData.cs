using System.Collections.Generic;
using Tweepics.Core.Parse;

namespace Tweepics.Web.Services
{
    public interface ITweetData
    {
        List<TweetData> GetAll();
        List<TweetData> FindByTag(string topic);
    }
}
