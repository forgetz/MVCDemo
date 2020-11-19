using MVCDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.ModelViews
{
    public class ProductDetail
    {
        public product product { get; set; }
        public string category_name { get; set; }
        public SelectList CategoryList { get; set; }
    }
}