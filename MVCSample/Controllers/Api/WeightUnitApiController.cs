using System.Web.Http;
using MVCSample.Models;
using MVCSample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MVCSample.Controllers.Api
{
	public class WeightUnitController: ApiController
	{
		protected StorageContext db = new StorageContext();

		[HttpGet]
		public async Task<IEnumerable<WeightUnit>> Get()
		{
			return db.GetWeightUnits();
		}

		[HttpGet]
		public async Task<WeightUnit> Get(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new WeightUnit(id));

			return db.GetWeightUnit(privateEntity.PartitionKey, privateEntity.RowKey).GetPublicEntity<WeightUnit>();
		}

		[HttpPost]
		public string Post(WeightUnit entity)
		{
			if (ModelState.IsValid)
			{
				db.InsertWeightUnit(entity);

				entity.PublicId = entity.GetPublicId();
				return entity.GetPublicEntity<WeightUnit>().PublicId;
			}

            return string.Empty;
		}

		[HttpPut]
		public void Put(string id, WeightUnit entity)
		{
			if (ModelState.IsValid)
			{
				db.UpdateWeightUnit(entity.GetPrivateEntity<WeightUnit>());
			}
		}

		[HttpDelete]
		public async Task Delete(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new WeightUnit() { PublicId = id });

			var entity = db.GetWeightUnit(privateEntity.PartitionKey, privateEntity.RowKey);
			
            if (entity == null)
                throw new System.Exception("WeightUnit entity not found, delete failed.");

			await db.DeleteWeightUnitAsync(entity);
		}

		protected override void Dispose(bool disposing)
        {
            db.Dispose(disposing);
            base.Dispose(disposing);
        }
	}
}