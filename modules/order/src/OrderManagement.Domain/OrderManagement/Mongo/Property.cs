﻿using MongoDB.Bson;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain
{
    public class Property
    {
        public ObjectId Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public PropertyTypeEnum Type { get; set; }
        public string Value { get; set; }
        public int Priority { get; set; }
        public CodingTypeEnum CodingType { get; set; }
        public List<DropDownItem> DropDownItems { get; set; }
    }

    public class DropDownItem
    {
        public string Title { get; set; }
        public int Value { get; set; }
    }
}
