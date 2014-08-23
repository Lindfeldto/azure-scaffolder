﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSample.Models
{
    public class LookupEntity : BlueMarble.Shared.Azure.Storage.Table.Entity
    {
        public LookupEntity() : base() { }
        public LookupEntity(string publicId) : base(publicId) { }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Order { get; set; }
    }
}