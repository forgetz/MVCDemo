using MVCDemo.Data;
using MVCDemo.ModelViews;
using MVCDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            ProductServices pds = new ProductServices();
            CategoryServices cgs = new CategoryServices();
            var allproduct = pds.GetAll();

            List<ProductDetail> list = new List<ProductDetail>();

            allproduct.ForEach(item => list.Add(new ProductDetail() {
                product = item,
                category_name = item.category_id.HasValue ? cgs.GetById(item.category_id.Value).name : ""
            }));

            //foreach (var item in allproduct)
            //{
            //    list.Add(new ProductDetail() {
            //        product = item,
            //        category_name = item.category_id.HasValue ? cgs.GetById(item.category_id.Value).name : ""
            //    });
            //}
            
            return View(list);
        }

        public ActionResult Add()
        {
            ProductDetail model = new ProductDetail();
            model.CategoryList = PopCategoryDDL();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(ProductDetail model)
        {
            ProductServices pds = new ProductServices();
            pds.Add(model.product);
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            ProductServices pds = new ProductServices();
            ProductDetail model = new ProductDetail();
            model.CategoryList = PopCategoryDDL();
            model.product = pds.GetById(id);
            return View("Add", model);
        }

        [HttpPost]
        public ActionResult Edit(ProductDetail model)
        {
            ProductServices pds = new ProductServices();
            pds.Update(model.product);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ProductServices pds = new ProductServices();
            pds.Delete(id);
            return Content("OK");
        }

        private SelectList PopCategoryDDL()
        {
            CategoryServices cgs = new CategoryServices();
            var list = cgs.GetAll().ToList();
            return new SelectList(list, "id", "name");
        }


    }
}
