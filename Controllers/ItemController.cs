using Microsoft.AspNetCore.Mvc;
using Zone.Data;
using Zone.Models;
using Zone.ViewModels;

namespace Zone.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Items.ToList());
        }
        public PartialViewResult GetData()
        {
            return PartialView("Sett", _context.Items.ToList());
        }
        [HttpPost]
        public ActionResult Save([FromBody] Item item)
		{
			_context.Add(new Item { Name = item.Name });
			_context.SaveChanges();
			return Json("done");
		}
		[HttpGet]
		public ActionResult Delete(int id)
		{

			var item = _context.Items.Find(id);
			_context.Items.Remove(item);
			_context.SaveChanges();
			return RedirectToAction("Index");
			#region
			//Item p = _context.Items.FirstOrDefault(s => s.Id = id);
			//_context.Items.Remove(p);
			//_context.SaveChanges();
			//return RedirectToAction("Index");
			#endregion
		}
		[HttpPost]
		 public ActionResult Edit([FromBody] Item item)
		{
			var editted = _context.Items.Find(item.Id);
			editted.Name = item.Name;
			_context.SaveChanges();

			return Json("Success Editted");
		}
		public ActionResult Info(int id)
		{
			var items = _context.IteminStores.Where(x => x.ItemId == id).ToList();
			var itemname = _context.Items.Find(id).Name;
			List<AboutViewModel> about = new List<AboutViewModel>();
			foreach(var item in items)
			{
				var storename = _context.Stores.Find(item.StoreId).Name;
				AboutViewModel details = new AboutViewModel { StoreName = storename, Quantity = item.Quantity };
				about.Add(details);
			}
			var info = new IteminfoViewModel
			{
				ItemName = itemname,
				About = about
			};
			return Json(info);
		}
		public ActionResult AutoComplate(string term)
		{
			List<string> itemnames = _context.Items.Where
				(a => a.Name.Contains(term)).Select(a => a.Name).ToList();
			return Json(itemnames);
		}
		public PartialViewResult Search(string text)
		{
			var itemnames = _context.Items.Where(a => a.Name.Contains(text)).ToList();
			return PartialView("Sett", itemnames);
		}
	}
}
