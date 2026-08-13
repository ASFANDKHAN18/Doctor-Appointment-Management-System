using hospitalMS.Models;
using hospitalMS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace hospitalMS.Controllers
{
    public class AdminController : Controller
    {
        HospitalDBEntities db = new HospitalDBEntities();
        public ActionResult Index()
        {

            ViewBag.TotalDoctors = db.Doctors.Count();
            ViewBag.TotalPatients = db.Patients.Count();
            ViewBag.TotalAppointments = db.Appointments.Count();
            ViewBag.PendingAppointments = db.Appointments.Count(x => x.Status == "Pending");


            ViewBag.RecentAppointments = db.Appointments.Include("Patient").Include("Doctor").OrderByDescending(x => x.CreatedAt).Take(5).ToList();

            return View();
        }

        public ActionResult Doctors()
        {
            var doctors = db.Doctors.ToList();
            return View(doctors);
        }

        public ActionResult AddDoctor()
        {
            ViewBag.Departments = new SelectList(db.Departments, "Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult AddDoctor(DoctorVM model)
        {
            if (ModelState.IsValid)
            {


                string imagePath = "/assets/images/doctors/default.png";

                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() +
                                      Path.GetExtension(model.ImageFile.FileName);

                    string folderPath = Server.MapPath("~/assets/images/doctors/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fullPath = Path.Combine(folderPath, fileName);

                    model.ImageFile.SaveAs(fullPath);

                    imagePath = "/assets/images/doctors/" + fileName;
                }

                if (db.Users.Any(x => x.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");

                    ViewBag.Departments = new SelectList(db.Departments, "Id", "Name");

                    return View(model);
                }



                User user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "Doctor",
                    CreatedAt = DateTime.Now
                };

                db.Users.Add(user);
                db.SaveChanges();

                Doctor doctor = new Doctor()
                {
                    UserId = user.Id,
                    Name = model.Name,
                    DepartmentId = model.DepartmentId,
                    Specialization = model.Specialization,
                    Fee = model.Fee,
                    IsAvailable = model.IsAvailable,
                    AvailableDays = model.AvailableDays,
                    AvailableTime = model.AvailableTime,
                    Image = imagePath
                };

                db.Doctors.Add(doctor);

                db.SaveChanges();

                TempData["Success"] = "Doctor Added Successfully.";

                return RedirectToAction("Doctors");
            }

            ViewBag.Departments = new SelectList(db.Departments, "Id", "Name");

            return View(model);
        }

        public ActionResult EditDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            ViewBag.Departments = new SelectList(
                db.Departments,
                "Id",
                "Name",
                doctor.DepartmentId
            );

            DoctorVM vm = new DoctorVM()
            {
                Id = doctor.Id,
                Name = doctor.Name,
                DepartmentId = doctor.DepartmentId,
                Specialization = doctor.Specialization,
                Fee = doctor.Fee,
                IsAvailable = doctor.IsAvailable,
                AvailableDays = doctor.AvailableDays,
                AvailableTime = doctor.AvailableTime,
                Image = doctor.Image
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditDoctor(DoctorVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(
                    db.Departments,
                    "Id",
                    "Name",
                    vm.DepartmentId
                );

                Doctor oldDoctor = db.Doctors.Find(vm.Id);

                if (oldDoctor != null)
                {
                    vm.Image = oldDoctor.Image;
                }

                return View(vm);
            }

            Doctor doctor = db.Doctors.Find(vm.Id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            // Update Fields
            doctor.Name = vm.Name;
            doctor.DepartmentId = vm.DepartmentId;
            doctor.Specialization = vm.Specialization;
            doctor.Fee = vm.Fee;
            doctor.IsAvailable = vm.IsAvailable;
            doctor.AvailableDays = vm.AvailableDays;
            doctor.AvailableTime = vm.AvailableTime;

            // Image Upload
            if (vm.ImageFile != null && vm.ImageFile.ContentLength > 0)
            {
                string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(vm.ImageFile.FileName);

                string folderPath = Server.MapPath("~/assets/images/doctors/");

                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                string fullPath = System.IO.Path.Combine(folderPath, fileName);

                vm.ImageFile.SaveAs(fullPath);

                doctor.Image = "/assets/images/doctors/" + fileName;
            }

            db.SaveChanges();

            return RedirectToAction("Doctors");
        }
        public ActionResult ToggleDoctorStatus(int id)
        {
            Doctor doctor = db.Doctors.Find(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            doctor.IsAvailable = !(doctor.IsAvailable ?? false);

            TempData["Success"] = doctor.IsAvailable == true
                ? "Doctor activated successfully."
                : "Doctor deactivated successfully.";

            db.SaveChanges();

            return RedirectToAction("Doctors");
        }

        //Department start_______________________________

        public ActionResult Departments()
        {
            var departments = db.Departments.ToList();
            return View(departments);
        }


        public ActionResult AddDepartment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                department.CreatedAt = DateTime.Now;
                department.IsActive = true;

                db.Departments.Add(department);
                db.SaveChanges();

                TempData["Success"] = "Department added successfully.";
                return RedirectToAction("Departments");
            }

            return View(department);
        }


        public ActionResult EditDepartment(int id)
        {
            Department department = db.Departments.Find(id);

            if (department == null)
                return HttpNotFound();

            return View(department);
        }

        [HttpPost]
        public ActionResult EditDepartment(Department department)
        {
            if (!ModelState.IsValid)
                return View(department);

            Department dbDepartment = db.Departments.Find(department.Id);

            if (dbDepartment == null)
                return HttpNotFound();

            dbDepartment.Name = department.Name;

            db.SaveChanges();

            TempData["Success"] = "Department updated successfully.";

            return RedirectToAction("Departments");
        }

        public ActionResult ToggleDepartmentStatus(int id)
        {
            Department department = db.Departments.Find(id);

            if (department == null)
                return HttpNotFound();

            department.IsActive = !department.IsActive;

            db.SaveChanges();

            TempData["Success"] = department.IsActive
                ? "Department activated successfully."
                : "Department deactivated successfully.";

            return RedirectToAction("Departments");
        }


        //Appointment start--------------------------

        public ActionResult Appointments()
        {
            var appointments = db.Appointments
                                 .OrderByDescending(x => x.CreatedAt)
                                 .ToList();

            return View(appointments);
        }



        public ActionResult EditAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            ViewBag.Doctors = new SelectList(
                db.Doctors.Where(x => x.IsAvailable == true),
                "Id",
                "Name",
                appointment.DoctorId
            );

            ViewBag.StatusList = new SelectList(new[]
            {
             "Pending",
             "Approved",
             "Completed",
             "Cancelled"
              }, appointment.Status);

            return View(appointment);
        }




        [HttpPost]
        public ActionResult EditAppointment(Appointment model)
        {
            Appointment appointment = db.Appointments.Find(model.Id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            appointment.DoctorId = model.DoctorId;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.AppointmentTime = model.AppointmentTime;
            appointment.Status = model.Status;

            db.SaveChanges();

            TempData["Success"] = "Appointment updated successfully.";

            return RedirectToAction("Appointments");
        }


        public ActionResult Patients()
        {
            var patients = db.Patients
                             .OrderByDescending(x => x.CreatedAt)
                             .ToList();

            return View(patients);
        }

        public ActionResult PatientDetails(int id)
        {
            Patient patient = db.Patients.Find(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }
    }

}