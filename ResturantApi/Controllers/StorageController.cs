using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types

namespace ResturantApi.Controllers
{
    [Route("api/blob")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private AppSettings _options;
        public StorageController(IOptions<AppSettings> options)
        {
            _options = options.Value;
        }

    }
}