using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public interface IPublicOfficialData
    {
        List<PublicOfficial> GetAll();
    }
}
