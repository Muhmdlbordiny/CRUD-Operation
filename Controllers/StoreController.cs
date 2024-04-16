using Microsoft.AspNetCore.Mvc;
using Zone.Data;
using Zone.Models;
using Zone.ViewModels;

namespace Zone.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View(_context.Stores.ToList());
        }
        public PartialViewResult GetData()
        {
            return PartialView("Sett",_context.Stores.ToList());
        }
        [HttpPost]
        public ActionResult Save([FromBody] Store store)
        {
            _context.Add(new Store { Name =store.Name,Address=store.Address});
            _context.SaveChanges();
            return Json("Success Save");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var store = _context.Stores.Find(id);
            _context.Remove(store);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult Edit([FromBody] Store store)
        {
            var edited = _context.Stores.Find(store.Id);
            edited.Name = store.Name;
            edited.Address = store.Address;
            _context.SaveChanges();
            return Json("Success Edited");
        }
        public ActionResult Info(int id)
        {
            var stores = _context.IteminStores.Where(a => a.StoreId == id).ToList();
            var storename = _context.Stores.Find(id).Name;
            List<AboutViewModel> about = new List<AboutViewModel>();
            foreach (var item in stores)
            {
                var itemname = _context.Items.Find(item.ItemId).Name;

                AboutViewModel details = new AboutViewModel { ItemName = itemname, Quantity = item.Quantity };
                about.Add(details);
            }
            var info = new StoreinfoViewModel
            {
                StoreName = storename,
                About = about
            };


            return Json(info);
        }
        public ActionResult AutoComplate(string term)
        {
            List<string> storenames = _context.Stores.Where(a => a.Name.Contains(term)).Select(a => a.Name).ToList();
            return Json(storenames);
        }
        public PartialViewResult Search(string text)
        {
            var storenames = _context.Stores.Where(a => a.Name.Contains(text)).ToList();
            return PartialView("Sett", storenames);
        }

    }
}
