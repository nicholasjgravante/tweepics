using System.Collections.Generic;
using Tweepics.Core.Tag;

namespace Tweepics.Web.Services
{
    public interface ITags
    {
        List<Tags> GetAll();
    }
}
