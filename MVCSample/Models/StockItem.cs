using BlueMarble.Shared.Azure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSample.Models
{
    public class StockItem : BlueMarble.Shared.Azure.Storage.Table.Entity
    {
        public string StockKeepingUnit { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal CostPrice { get; set; }
        public decimal ListPrice { get; set; }

        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }

        [RelatedTable(Type=typeof(MVCSample.Models.DimensionUnit))]
        public string DimensionUnitPublicId { get; set; }

        public int Weight { get; set; }

        [RelatedTable(Type = typeof(MVCSample.Models.WeightUnit))]
        public string WeightUnitPublicId { get; set; }

        [RelatedTable(Type = typeof(MVCSample.Models.Category))]
        public string CategoryPublicId { get; set; }
    }
}