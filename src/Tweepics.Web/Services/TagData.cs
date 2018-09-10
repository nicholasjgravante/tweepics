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

        public List<string> GetAllTags()
        {
            connectionString = _configuration["ConnectionString"];

            TagReader tagReader = new TagReader(connectionString);
            List<Tag> tagsAllData = new List<Tag>();
            tagsAllData = tagReader.ReadAll();

            List<string> tagCategories = new List<string>();
            tagCategories = tagsAllData.Select(tag => tag.Name).ToList();

            return tagCategories;
        }

        public List<List<string>> TagsInSublists()
        {
            connectionString = _configuration["ConnectionString"];

            TagReader tagReader = new TagReader(connectionString);
            List<Tag> tagsAllData = new List<Tag>();
            tagsAllData = tagReader.ReadAll();

            List<string> tags = new List<string>();
            tags = tagsAllData.Select(tag => tag.Name).ToList();

            List<List<string>> tagsInThrees = new List<List<string>>();
            List<string> intermediaryList = new List<string>();

            while (tags.Count > 0)
            {
                if (tags.Count >= 3)
                {
                    tagsInThrees.Add(new List<string> { tags[0], tags[1], tags[2] });

                    tags.RemoveAt(2);
                    tags.RemoveAt(1);
                    tags.RemoveAt(0);
                }

                else if (tags.Count == 2)
                {
                    tagsInThrees.Add(new List<string> { tags[0], tags[1], "" });

                    tags.RemoveAt(1);
                    tags.RemoveAt(0);
                }

                else if (tags.Count == 1)
                {
                    tagsInThrees.Add(new List<string> { tags[0], "", ""});

                    tags.RemoveAt(0);
                }
            }
            return tagsInThrees;
        }
    }
}
