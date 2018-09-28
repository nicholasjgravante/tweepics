using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public interface ITweetData
    {
        List<Tweet> GetAll();
        List<Tweet> FindByTag(string topic);
        List<Tweet> FindBySearch(string query);
    }
}
