using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpCustKanaController : Controller
    {
        // GET: InpCustKanaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: InpCustKanaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InpCustKanaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InpCustKanaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InpCustKanaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InpCustKanaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InpCustKanaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InpCustKanaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
