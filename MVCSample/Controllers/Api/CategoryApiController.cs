using System.Web.Http;
using MVCSample.Models;
using MVCSample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MVCSample.Controllers.Api
{
	public class CategoryController: ApiController
	{
		protected StorageContext db = new StorageContext();

		[HttpGet]
		public async Task<IEnumerable<Category>> Get()
		{
			return db.GetCategories();
		}

		[HttpGet]
		public async Task<Category> Get(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new Category(id));

			return db.GetCategory(privateEntity.PartitionKey, privateEntity.RowKey).GetPublicEntity<Category>();
		}

		[HttpPost]
		public string Post(Category entity)
		{
			if (ModelState.IsValid)
			{
				db.InsertCategory(entity);

				entity.PublicId = entity.GetPublicId();
				return entity.GetPublicEntity<Category>().PublicId;
			}

            return string.Empty;
		}

		[HttpPut]
		public void Put(string id, Category entity)
		{
			if (ModelState.IsValid)
			{
				db.UpdateCategory(entity.GetPrivateEntity<Category>());
			}
		}

		[HttpDelete]
		public async Task Delete(string id)
		{
			var privateEntity = BlueMarble.Shared.Azure.Storage.Table.Entity.GetPrivateEntity(new Category() { PublicId = id });

			var entity = db.GetCategory(privateEntity.PartitionKey, privateEntity.RowKey);
			
            if (entity == null)
                throw new System.Exception("Category entity not found, delete failed.");

			await db.DeleteCategoryAsync(entity);
		}

		protected override void Dispose(bool disposing)
        {
            db.Dispose(disposing);
            base.Dispose(disposing);
        }
	}
}