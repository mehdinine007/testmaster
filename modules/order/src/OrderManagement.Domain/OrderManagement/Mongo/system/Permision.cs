using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.OrderManagement.Mongo.system
{
    public class Permision
    {
        public ObjectId Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public int LevelId { get; set; }
    }
}
