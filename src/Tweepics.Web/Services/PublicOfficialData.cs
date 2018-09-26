using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Tweepics.Core.Database;
using Tweepics.Core.Models;

namespace Tweepics.Web.Services
{
    public class PublicOfficialData : IPublicOfficialData
    {
        private readonly IConfiguration _configuration;

        public PublicOfficialData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<PublicOfficial> GetAll()
        {
            string connectionString = _configuration["ConnectionString"];

            PublicOfficialReader publicOfficialReader = new PublicOfficialReader(connectionString);
            List<PublicOfficial> officials = new List<PublicOfficial>();
            officials = publicOfficialReader.ReadFromDb();

            return officials;
        }
    }
}
