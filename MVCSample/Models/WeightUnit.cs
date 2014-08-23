using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSample.Models
{
    public class WeightUnit : LookupEntity
    {
        public WeightUnit() : base() { }
        public WeightUnit(string publicId) : base(publicId) { }
    }
}