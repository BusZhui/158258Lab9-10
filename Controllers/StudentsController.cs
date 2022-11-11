using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class StudentsController : Controller
    {
        private WebApplication3Context db = new WebApplication3Context();

        // GET: Students
        //public ActionResult Index(string campus)
        //{
        //    var students = db.Students.Include(s => s.Campus);
        //    //return View(students.ToList());
        //    if(!String.IsNullOrEmpty(campus))
        //    {
        //        students = students.Where(s => s.Campus.Name == campus);
        //    }
        //    return View(students.ToList());
        //}

        //public ActionResult Index(string campus,string search)
        //{
        //     var students = db.Students.Include(s => s.Campus);
        //if (!String.IsNullOrEmpty(search)) 
        //{
        //    students = students.Where(s => s.Name.Contains(search) ||
        //    s.Address.Contains(search) ||
        //    s.Campus.Name.Contains(search));
        //    ViewBag.Search = search; 
        //}
        //var campus2 = students.OrderBy(s => s.Campus.Name).Select(s
        //=> s.Campus.Name).Distinct();
        // if (!String.IsNullOrEmpty(campus))
        // {
        // students = students.Where(p => p.Campus.Name == campus);
        // }
        //ViewBag.Campus = new SelectList(campus2);
        //return View(students.ToList());
        // }


        public ActionResult Index(string campus, string search)
        {
            StudentIndexViewModel viewModel = new StudentIndexViewModel();
            var students = db.Students.Include(p => p.Campus);
          
            if (!String.IsNullOrEmpty(search))
            {
                students = students.Where(s => s.Name.Contains(search) ||
                s.Address.Contains(search) ||
                s.Campus.Name.Contains(search));
                viewModel.Search = search;
            }
           
            viewModel.CatsWithCount = from matchingStudents in students
                                      where
                                      matchingStudents.CampusID != null
                                      group matchingStudents by
                                      matchingStudents.Campus.Name into
                                      catGroup
                                      select new CampusWithCount()
                                      {
                                          CampusName = catGroup.Key,
                                          StudentCount = catGroup.Count()
                                      };
       
       
            if (!String.IsNullOrEmpty(campus))
            {
                students = students.Where(s => s.Campus.Name == campus);
            }
            
            viewModel.Students = students;
           
            return View(viewModel);
        }



        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.CampusID = new SelectList(db.Campus, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,CampusID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampusID = new SelectList(db.Campus, "Id", "Name", student.CampusID);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampusID = new SelectList(db.Campus, "Id", "Name", student.CampusID);
            return View(student);
        }

        // POST: Students/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,CampusID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampusID = new SelectList(db.Campus, "Id", "Name", student.CampusID);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
