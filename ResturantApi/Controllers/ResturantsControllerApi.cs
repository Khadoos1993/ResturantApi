using System;
using System.Collections.Generic;
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
                _logger.LogInformation("Give all resturants json data");
                var resturant = _mongoDbDataContext.Get();
                return Ok(resturant);
                //IFileInfo fileInfo = _fileProvider.GetFileInfo();
                //var readStream = fileInfo.CreateReadStream();
                
                //return File("resturant-data.json", "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id:Length(24)}", Name = "GetResurant")]
        public ActionResult<Resturant> GetResturant(string id)
        {
            try
            {
                if (!ObjectId.TryParse(id, out var validId))
                    return BadRequest();
                //FilterDefinition<Resturant> filter = Builders<Resturant>.Filter.Eq("_id", validId);
                var result = _mongoDbDataContext.Get(validId);

                //var result = await _mongoDbDataContext.GetResturant.Find(filter)
                //    .Project(x => new { x.ObjectId, x.Image, x.Name, x.Neighborhood, x.CuisineType })
                //    .ToListAsync();
                if (result == null)
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
        public ActionResult<Resturant> Create([FromBody]Resturant model)
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
                _mongoDbDataContext.Create(model);
                return CreatedAtRoute("GetResurant", new { id = model.ObjectId.ToString() }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while connecting with mongo database");
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, [FromBody]Resturant bookIn)
        {
            if (!ObjectId.TryParse(id, out var validId))
                return BadRequest();
            var resturant = _mongoDbDataContext.Get(validId);

            if (resturant == null)
            {
                return NotFound();
            }

            _mongoDbDataContext.Update(validId, resturant);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            if (!ObjectId.TryParse(id, out var validId))
                return BadRequest();
            //var resturant = _mongoDbDataContext.Get(validId);

            //if (resturant == null)
            //{
            //    return NotFound();
            //}

            _mongoDbDataContext.Remove(validId);

            return NoContent();
        }
    }
}