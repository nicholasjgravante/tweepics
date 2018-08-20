using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
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

        public List<string> GetAll()
        {
            TagsFromDB tagReader = new TagsFromDB();
            List<Tags> tagsAllData = new List<Tags>();
            tagsAllData = tagReader.Read(connectionString);

            List<string> tagCategories = new List<string>();
            tagCategories = tagsAllData.Select(tag => tag.Tag).ToList();

            return tagCategories;
        }
    }
}
