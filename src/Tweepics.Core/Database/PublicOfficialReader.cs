using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class PublicOfficialReader
    {
        private readonly string _connectionString;

        public PublicOfficialReader(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }

        public List<PublicOfficial> ReadFromDb()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                List<PublicOfficial> officials = new List<PublicOfficial>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * from representatives"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string tweepicsId = dataReader[0].ToString();
                    string incumbent = dataReader[1].ToString();
                    string firstName = dataReader[2].ToString();
                    string middleName = dataReader[3].ToString();
                    string lastName = dataReader[4].ToString();
                    string suffix = dataReader[5].ToString();
                    string officeGovernment = dataReader[6].ToString();
                    string officeBranch = dataReader[7].ToString();
                    string officeState = dataReader[8].ToString();
                    string officeTitle = dataReader[9].ToString();
                    string party = dataReader[10].ToString();
                    long twitterId = Convert.ToInt64(dataReader[11]);
                    string twitterScreenName = dataReader[12].ToString();

                    Name name = new Name(firstName, middleName, lastName, suffix);
                    Office office = new Office(officeGovernment, officeBranch, officeState, officeTitle);

                    officials.Add(new PublicOfficial(tweepicsId, incumbent, name, office, party, twitterId, twitterScreenName));
                }
                dataReader.Close();
                connection.Close();

                return officials;
            }
        }

        public List<PublicOfficial> ReadFromFile(string filePath)
        {
            string fileText = File.ReadAllText($"{filePath}");

            List<string> rawDataStrings = new List<string>();
            rawDataStrings = fileText.Split("\n").ToList();

            List<PublicOfficial> officials = new List<PublicOfficial>();

            foreach (string line in rawDataStrings)
            {
                List<string> oneLineOfData = new List<string>();
                oneLineOfData = line.Split(",").ToList();

                if (oneLineOfData.Any())
                {
                    string incumbent = oneLineOfData[0];
                    string firstName = oneLineOfData[1];
                    string middleName = oneLineOfData[2];
                    string lastName = oneLineOfData[3];
                    string suffix = oneLineOfData[4];
                    string officeGovernment = oneLineOfData[5];
                    string officeBranch = oneLineOfData[6];
                    string officeState = oneLineOfData[7];
                    string officeTitle = oneLineOfData[8];
                    string party = oneLineOfData[9];
                    long twitterId = 0;

                    if (oneLineOfData[10] != string.Empty)
                    {
                        twitterId = Convert.ToInt64(oneLineOfData[10]);
                    }

                    string twitterScreenName = oneLineOfData[11];

                    Name name = new Name(firstName, middleName, lastName, suffix);
                    Office office = new Office(officeGovernment, officeBranch, officeState, officeTitle);

                    officials.Add(new PublicOfficial(incumbent, name, office, party, twitterId, twitterScreenName));
                }
                else
                    break;
            }
            return officials;
        }


    }
}
