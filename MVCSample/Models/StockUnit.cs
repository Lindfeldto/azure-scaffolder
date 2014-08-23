using BlueMarble.Shared.Azure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSample.Models
{
    public class StockUnit : BlueMarble.Shared.Azure.Storage.Table.Entity
    {
        public StockUnit() : base() { }
        public StockUnit(string publicId) : base(publicId) { }

        [RelatedTable(Type = typeof(MVCSample.Models.StockItem))]
        public string StockItemPublicId { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public int StockIn { get; set; }
        public string ReceiptNumber { get; set; }

        public int StockOut { get; set; }
        public string InvoiceNumber { get; set; }
    }
}