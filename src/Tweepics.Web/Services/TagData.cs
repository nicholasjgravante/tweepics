using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Tweepics.Core.Database.Operations;
using Tweepics.Core.Tag;

namespace Tweepics.Web.Services
{
    public class TagData : ITags
    {
        private IConfiguration _configuration;
        private string connectionString;

        public TagData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public List<Tags> GetAll()
        {
            TagsFromDB tagReader = new TagsFromDB();
            List<Tags> tags = new List<Tags>();

            tags = tagReader.Read(connectionString);

            return tags;
        }
    }
}
