using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Tweepics.Core.Models;
using Tweepics.Core.Database;

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
            connectionString = _configuration["ConnectionString"];

            TagReader tagReader = new TagReader(connectionString);
            List<Tag> tagsAllData = new List<Tag>();
            tagsAllData = tagReader.ReadAll();

            List<string> tagCategories = new List<string>();
            tagCategories = tagsAllData.Select(tag => tag.Name).ToList();

            return tagCategories;
        }
    }
}
