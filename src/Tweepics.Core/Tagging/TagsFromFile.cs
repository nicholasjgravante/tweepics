using System.IO;
using System.Linq;
using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Core.Tagging
{
    public class TagsFromFile
    {
        public List<Tag> Read(string tagsFolderPath, string fileName)
        {
            string fileText = File.ReadAllText($@"{tagsFolderPath}\{fileName}");

            List<string> rawDataStrings = new List<string>();
            rawDataStrings = fileText.Split("\n").ToList();

            List<Tag> tagsKeywords = new List<Tag>();

            foreach (string line in rawDataStrings)
            {
                List<string> tagsAndKeywords = new List<string>();
                tagsAndKeywords = line.Split(": ").ToList();

                if (tagsAndKeywords.Any())
                {
                    string tagName = tagsAndKeywords[0];
                    string keywordString = tagsAndKeywords[1];

                    tagsKeywords.Add(new Tag(tagName, keywordString));
                }
                else
                    break;
            }
            return tagsKeywords;
        }
    }
}