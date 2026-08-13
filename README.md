# Doctor Appointment Management System

A web-based doctor appointment management system built with **ASP.NET MVC 5, C#, Entity Framework 6, and SQL Server**.

Features

* Doctor listing and management
* Department management
* Online doctor appointment booking
* Patient registration through appointment booking
* Appointment availability checking
* Doctor available days and time slots
* Appointment status management
* Patient list and patient details
* Recent appointments dashboard
* Admin dashboard with statistics
* Search functionality for doctors, patients, and appointments
* Responsive admin panel

Admin Panel

The admin panel provides:

* Dashboard
* Doctors
* Departments
* Appointments
* Patients
* Patient details
* Appointment status management

Dashboard

The dashboard displays:

* Total doctors
* Total patients
* Total appointments
* Pending appointments
* Recent appointments

Technologies

* **ASP.NET MVC 5**
* **C#**
* **Entity Framework 6**
* **SQL Server**
* **HTML5**
* **CSS3**
* **Bootstrap**
* **JavaScript**
* **jQuery**
* **Bootstrap Icons**
* **SweetAlert2**

Database

The application uses **SQL Server** with **Entity Framework Database First**.

Main entities:

* Doctor
* Department
* Patient
* Appointment

Appointment Flow

```text
Patient
   ↓
Select Doctor
   ↓
Select Date & Time
   ↓
Enter Patient Information
   ↓
Appointment Created
   ↓
Status: Pending
   ↓
Admin Manages Appointment
```

How to Run

1. Clone or download the repository.
2. Open the solution in **Visual Studio**.
3. Restore the required NuGet packages.
4. Create the SQL Server database.
5. Update the database connection string in `Web.config`.
6. Build the project.
7. Run the application using Visual Studio.

> `Web.config` is not included in this repository because the database connection is specific to the local development environment.

Project Structure

```text
Controllers/
Models/
ViewModels/
Views/
App_Start/
Admin-assets/
assets/
Content/
Scripts/
```

Purpose

This project was developed as a practical **ASP.NET MVC** project to demonstrate MVC architecture, Entity Framework, SQL Server integration, database relationships, appointment management, validation, and admin dashboard development.

Author

**Asfand Khan**

Built as an ASP.NET MVC project for learning and portfolio development.
