using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Tweepics.Tag
{
    public class ReadTagsFromFile
    {
        public List<Tags> Read(string tagsFolderPath, string fileName)
        {
            string fileText = File.ReadAllText($@"{tagsFolderPath}\{fileName}");

            List<string> rawDataStrings = new List<string>();
            rawDataStrings = fileText.Split("\n").ToList();

            List<Tags> tagsKeywords = new List<Tags>();

            foreach (string line in rawDataStrings)
            {
                List<string> tagsAndKeywords = new List<string>();
                tagsAndKeywords = line.Split(": ").ToList();

                if (tagsAndKeywords.Any())
                {
                    string tagCategory = tagsAndKeywords[0].ToLower();
                    string keywordString = tagsAndKeywords[1].ToLower();

                    tagsKeywords.Add(new Tags(tagCategory, keywordString));
                }
                else
                    break;
            }
            return tagsKeywords;
        }
    }
}