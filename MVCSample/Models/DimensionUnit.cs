using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSample.Models
{
    public class DimensionUnit : LookupEntity
    {
        public DimensionUnit() : base() { }
        public DimensionUnit(string publicId) : base(publicId) { }
    }
}