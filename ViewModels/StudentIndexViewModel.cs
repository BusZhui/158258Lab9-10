using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class StudentIndexViewModel
    {
            public IQueryable<Student> Students { get; set; }
            public string Search { get; set; }
            public IEnumerable<CampusWithCount> CatsWithCount { get; set; }
            public string Campus { get; set; }

            public IEnumerable<SelectListItem> CatFilterItems
            {
                get
                {
                    var allCats = CatsWithCount.Select(cc => new SelectListItem
                    {
                        Value = cc.CampusName,
                        Text = cc.CatNameWithCount
                    });
                    return allCats;
                }
            }
        }
        public class CampusWithCount
        {
            public int StudentCount { get; set; }
            public string CampusName { get; set; }
            public string CatNameWithCount
            {
                get
                {
                    return CampusName + " (" + StudentCount.ToString() + ")";
                }
            }
        }
    
}