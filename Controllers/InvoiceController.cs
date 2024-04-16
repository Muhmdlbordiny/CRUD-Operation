using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zone.Data;
using Zone.Models;
using Zone.ViewModels;

namespace Zone.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }
        private List<InvoiceInfoViewModel> Infov()
        {
            var invoices = _context.Invoices.ToList();
            List<InvoiceInfoViewModel> invoiceInfos = new List<InvoiceInfoViewModel>();
            foreach(var invoice in invoices)
            {
                var itemname = _context.Items.Find(invoice.ItemId).Name;
                var storename = _context.Stores.Find(invoice.StoreId).Name;

                AboutViewModel detail = new AboutViewModel 
                { StoreName = storename, ItemName = itemname, Quantity = invoice.Quantity };
                invoiceInfos.Add(new InvoiceInfoViewModel { InvoiceId = invoice.Id, About = detail });


            }
            return invoiceInfos;
        }

        public IActionResult Index()
        {
            List<SelectListItem> stores = new List<SelectListItem>();
            foreach(var item in _context.Stores)
            {
                stores.Add(new SelectListItem { Value= item.Id.ToString(),Text=item.Name});
            }
            List<SelectListItem> items = new List<SelectListItem>();
            foreach(var item in _context.Items)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text=item.Name });
            }
            ViewBag.Stores = stores;
            ViewBag.Items = items;

            return View(Infov());
        }
		public PartialViewResult GetData()
        {
            return PartialView("Sett",Infov());
        }
		[HttpPost]
		public ActionResult Save([FromBody] Invoice invoice)
		{
			_context.Invoices.Add(new Invoice { StoreId = invoice.StoreId, ItemId = invoice.ItemId, Quantity = invoice.Quantity });
			var iteminstore = _context.IteminStores.Where(a => a.StoreId == invoice.StoreId && a.ItemId == invoice.ItemId).FirstOrDefault();
			if (iteminstore == null)
				_context.IteminStores.Add(new IteminStore { StoreId = invoice.StoreId, ItemId = invoice.ItemId, Quantity = invoice.Quantity });
			else
				iteminstore.Quantity += invoice.Quantity;
			_context.SaveChanges();

			return Json("Success Save");
		}
		[HttpPost]
		public ActionResult Delete(int id)
		{
			var Invoice = _context.Invoices.Find(id);
			_context.Remove(Invoice);
			_context.SaveChanges();

			return Json("success delete");
		}
		[HttpPost]
		public ActionResult Edit([FromBody] Invoice Invoice)
		{
			var editted = _context.Invoices.Find(Invoice.Id);
			//editted.Name = Invoice.Name;
			_context.SaveChanges();

			return Json("Done Edit");
		}
		[HttpPost]
		public ActionResult Available([FromBody] AvailableViewModel availableViewModel)
		{
			var store = _context.Stores.Find(availableViewModel.StoreId);
			var item = _context.Items.Find(availableViewModel.ItemId);
			int? availablequantity = _context.IteminStores.Where
                (q => q.StoreId == store.Id && q.ItemId == item.Id)
                .Select(q => q.Quantity).FirstOrDefault();
			int quantity = availablequantity ?? 0;
			return Json(quantity);
		}
		public ActionResult AutoComplate(string term)
		{
			List<string> invoicesid = _context.Invoices.Where
				(a => a.Id.ToString().Contains(term))
				.Select(a => a.Id.ToString()).ToList();
			return Json(invoicesid);
		}
		public PartialViewResult Search(string text)
		{

			var invoices = Infov().Where
				(a => a.InvoiceId.ToString().Contains(text)).ToList();
			return PartialView("Sett", invoices);
		}

	}
}
