using hospitalMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hospitalMS.Controllers
{
    public class DoctorController : Controller
    {
        HospitalDBEntities db = new HospitalDBEntities();
        public ActionResult Index(int? departmentId)
        {
            var doctors = db.Doctors.Include("Department").AsQueryable();

            if (departmentId != null)
            {
                doctors = doctors.Where(d => d.DepartmentId == departmentId);
            }

            ViewBag.Departments = db.Departments.ToList();
            ViewBag.SelectedDepartment = departmentId;

            return View(doctors.ToList());
        }

        public ActionResult DoctorDetail(int id)
        {
            var doctor =db.Doctors.FirstOrDefault(x=> x.Id == id);
            return View(doctor);
        }
    }
}