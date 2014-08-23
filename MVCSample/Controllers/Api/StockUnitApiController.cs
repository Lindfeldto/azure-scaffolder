using System.Web.Http;
using MVCSample.Models;
using MVCSample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MVCSample.Controllers.Api
{
	public class StockUnitController: ApiController
	{
		protected StorageContext db = new StorageContext();

		[HttpGet]
		public async Task<IEnumerable<StockUnit>> Get()
		{
			return db.GetStockUnits();
		}

		[HttpGet]
		public async Task<StockUnit> Get(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new StockUnit(id));

			return db.GetStockUnit(privateEntity.PartitionKey, privateEntity.RowKey).GetPublicEntity<StockUnit>();
		}

		[HttpPost]
		public string Post(StockUnit entity)
		{
			if (ModelState.IsValid)
			{
				db.InsertStockUnit(entity);

				entity.PublicId = entity.GetPublicId();
				return entity.GetPublicEntity<StockUnit>().PublicId;
			}

            return string.Empty;
		}

		[HttpPut]
		public void Put(string id, StockUnit entity)
		{
			if (ModelState.IsValid)
			{
				db.UpdateStockUnit(entity.GetPrivateEntity<StockUnit>());
			}
		}

		[HttpDelete]
		public async Task Delete(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new StockUnit() { PublicId = id });

			var entity = db.GetStockUnit(privateEntity.PartitionKey, privateEntity.RowKey);
			
            if (entity == null)
                throw new System.Exception("StockUnit entity not found, delete failed.");

			await db.DeleteStockUnitAsync(entity);
		}

		protected override void Dispose(bool disposing)
        {
            db.Dispose(disposing);
            base.Dispose(disposing);
        }
	}
}