using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _5_17classwork.Models
{
    public class IndexVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public string User { get; set; }
    }
}