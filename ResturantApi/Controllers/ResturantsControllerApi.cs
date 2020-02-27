using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using ResturantDAL;

namespace ResturantApi.Controllers
{
    [Authorize]
    [Route("api/resturants")]
    [ApiController]
    public class ResturantsControllerApi : ControllerBase
    {
        private readonly IFileProvider _fileProvider;
        private readonly IMongoDbDataContext _mongoDbDataContext;
        private readonly ILogger _logger;

        public ResturantsControllerApi(IFileProvider fileProvider, IMongoDbDataContext mongoDbDataContext, ILogger<ResturantsControllerApi> logger)
        {
            _fileProvider = fileProvider;
            _mongoDbDataContext = mongoDbDataContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetResturants()
        {
            try
            {
                //IFileInfo fileInfo = _fileProvider.GetFileInfo();
                //var readStream = fileInfo.CreateReadStream();
                _logger.LogInformation("Give all resturants json data");
                return File("resturant-data.json", "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        private bool IsExist(string id)
        {
            return _mongoDbDataContext.GetResturant.Find(_ => _.ObjectId == ObjectId.Parse(id)).Any();
        }

        [HttpGet("{id}", Name = "GetResurant")]
        public async Task<ActionResult> GetResturant(string id)
        {
            try
            {
                if (!ObjectId.TryParse(id, out var validId))
                    return BadRequest();
                FilterDefinition<Resturant> filter = Builders<Resturant>.Filter.Eq("_id", validId);
                var result = await _mongoDbDataContext.GetResturant.Find(filter)
                    .Project(x => new { x.ObjectId, x.Image, x.Name, x.Neighborhood, x.CuisineType })
                    .ToListAsync();
                if (result.Count == 0)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while connecting with mongo database");
                return StatusCode(500, ex);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Resturant model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                _logger.LogInformation("adding new resturants record in collection");
                //await _mongoDbDataContext.GetResturant.Indexes.CreateOneAsync(
                //    new CreateIndexModel<Resturant>(
                //        Builders<Resturant>.IndexKeys.Ascending(x => x.Name),
                //        new CreateIndexOptions() { Unique = true }));
                await _mongoDbDataContext.GetResturant.InsertOneAsync(model);
                return CreatedAtRoute("GetResurant", new { id = model.ObjectId }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while connecting with mongo database");
                return StatusCode(500, ex);
            }
        }
    }
}