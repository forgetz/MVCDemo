using MVCDemo.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCDemo.Services
{
    public class ProductServices
    {
        public List<product> GetAll()
        {
            using (var db = new MVCDemoEntities())
            {
                return db.products.OrderByDescending(s => s.id).ToList();
            }
        }

        public product GetById(int id)
        {
            using (var db = new MVCDemoEntities())
            {
                return db.products.Where(s => s.id == id).FirstOrDefault();
            }
        }

        public void Add(product product)
        {
            product.created_date = DateTime.Now;
            product.created_by = "web";

            using (var db = new MVCDemoEntities())
            {
                db.products.Add(product);
                db.SaveChanges();
            }
        }

        public void Update(product product)
        {
            using (var db = new MVCDemoEntities())
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new MVCDemoEntities())
            {
                var product = db.products.Where(s => s.id == id).FirstOrDefault();
                if (product != null)
                {
                    db.products.Remove(product);
                    db.SaveChanges();
                }
            }

            //using (var db = new MVCDemoEntities())
            //{
            //    db.USP_PRODUCTS_DELETE(id);
            //}
        }
    }
}