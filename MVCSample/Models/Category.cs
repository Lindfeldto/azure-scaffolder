using BlueMarble.Shared.Azure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSample.Models
{
    public class Category : LookupEntity
    {
        [RelatedTable(Type = typeof(MVCSample.Models.Category))]
        public string ParentCategoryPublicId { get; set; }
    }
}