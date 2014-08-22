using System.Web.Mvc;
using System.Threading.Tasks;

namespace MVCSample.Controllers
{
	public class CategoryController: Controller
	{
		public async Task<ActionResult> Index()
		{
			return View();
		}

		public async Task<ActionResult> Create()
		{
			return View();
		}

		public async Task<ActionResult> Edit(string id)
		{
			ViewBag.PublicId = id;

			return View();
		}

		public async Task<ActionResult> Details(string id)
		{
			ViewBag.PublicId = id;

			return View();
		}

		public async Task<ActionResult> Delete(string id)
		{
			ViewBag.PublicId = id;

			return View();
		}

		public void Dispose()
		{
		}
	}
}