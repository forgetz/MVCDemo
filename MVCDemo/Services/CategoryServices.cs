using MVCDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCDemo.Services
{
    public class CategoryServices
    {
        public List<category> GetAll()
        {
            using (var db = new MVCDemoEntities())
            {
                return db.categories.ToList();
            }
        }

        public category GetById(int id)
        {
            using (var db = new MVCDemoEntities())
            {
                return db.categories.Where(s=>s.id == id).FirstOrDefault();
            }
        }
    }
}