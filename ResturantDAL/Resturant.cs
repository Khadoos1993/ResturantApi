using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantDAL
{
    public class Resturant
    {
        [BsonId]
        [JsonProperty("objectId")]
        public ObjectId ObjectId { get; set; }

        [BsonIgnore]
        public int ResturantId { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        public string Neighborhood { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Please enter the address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter the address")]
        [Column(TypeName = "varchar(50)")]
        public string CuisineType { get; set; }
    }
}
