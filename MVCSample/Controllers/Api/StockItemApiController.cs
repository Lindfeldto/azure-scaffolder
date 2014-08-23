using System.Web.Http;
using MVCSample.Models;
using MVCSample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MVCSample.Controllers.Api
{
	public class StockItemController: ApiController
	{
		protected StorageContext db = new StorageContext();

		[HttpGet]
		public async Task<IEnumerable<StockItem>> Get()
		{
			return db.GetStockItems();
		}

		[HttpGet]
		public async Task<StockItem> Get(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new StockItem(id));

			return db.GetStockItem(privateEntity.PartitionKey, privateEntity.RowKey).GetPublicEntity<StockItem>();
		}

		[HttpPost]
		public string Post(StockItem entity)
		{
			if (ModelState.IsValid)
			{
				db.InsertStockItem(entity);

				entity.PublicId = entity.GetPublicId();
				return entity.GetPublicEntity<StockItem>().PublicId;
			}

            return string.Empty;
		}

		[HttpPut]
		public void Put(string id, StockItem entity)
		{
			if (ModelState.IsValid)
			{
				db.UpdateStockItem(entity.GetPrivateEntity<StockItem>());
			}
		}

		[HttpDelete]
		public async Task Delete(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new StockItem() { PublicId = id });

			var entity = db.GetStockItem(privateEntity.PartitionKey, privateEntity.RowKey);
			
            if (entity == null)
                throw new System.Exception("StockItem entity not found, delete failed.");

			await db.DeleteStockItemAsync(entity);
		}

		protected override void Dispose(bool disposing)
        {
            db.Dispose(disposing);
            base.Dispose(disposing);
        }
	}
}