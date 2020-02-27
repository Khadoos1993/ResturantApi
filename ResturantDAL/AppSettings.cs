using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResturantApi
{
    public class AppSettings
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string SecretKey { get; set; }
        public string StorageConnectionString { get; set; }
    }
}
