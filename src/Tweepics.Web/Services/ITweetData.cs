using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public interface ITweetData
    {
        List<Tweet> FindByTag(string tag);
        List<Tweet> FindBySearch(string searchQuery);
    }
}
