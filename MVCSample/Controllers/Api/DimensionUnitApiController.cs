using System.Web.Http;
using MVCSample.Models;
using MVCSample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MVCSample.Controllers.Api
{
	public class DimensionUnitController: ApiController
	{
		protected StorageContext db = new StorageContext();

		[HttpGet]
		public async Task<IEnumerable<DimensionUnit>> Get()
		{
			return db.GetDimensionUnits();
		}

		[HttpGet]
		public async Task<DimensionUnit> Get(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new DimensionUnit(id));

			return db.GetDimensionUnit(privateEntity.PartitionKey, privateEntity.RowKey).GetPublicEntity<DimensionUnit>();
		}

		[HttpPost]
		public string Post(DimensionUnit entity)
		{
			if (ModelState.IsValid)
			{
				db.InsertDimensionUnit(entity);

				entity.PublicId = entity.GetPublicId();
				return entity.GetPublicEntity<DimensionUnit>().PublicId;
			}

            return string.Empty;
		}

		[HttpPut]
		public void Put(string id, DimensionUnit entity)
		{
			if (ModelState.IsValid)
			{
				db.UpdateDimensionUnit(entity.GetPrivateEntity<DimensionUnit>());
			}
		}

		[HttpDelete]
		public async Task Delete(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new DimensionUnit() { PublicId = id });

			var entity = db.GetDimensionUnit(privateEntity.PartitionKey, privateEntity.RowKey);
			
            if (entity == null)
                throw new System.Exception("DimensionUnit entity not found, delete failed.");

			await db.DeleteDimensionUnitAsync(entity);
		}

		protected override void Dispose(bool disposing)
        {
            db.Dispose(disposing);
            base.Dispose(disposing);
        }
	}
}