using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.OrderManagement.Mongo
{
    public class SystemPermision
    {
        public ObjectId Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public int LevelId { get; set; }
    }
}
