using DocOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DocOffice.Controllers
{
  public class DoctorsController : Controller
  {
    private readonly DocOfficeContext _db;

    public DoctorsController(DocOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Doctors.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.SpecialtyId = new SelectList(_db.Specialties, "SpecialtyId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Doctor doctor, int SpecialtyId)
    {
      _db.Doctors.Add(doctor);
      _db.SaveChanges();
      if (SpecialtyId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() {DoctorId = doctor.DoctorId, SpecialtyId = SpecialtyId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisDoctor = _db.Doctors
        .Include(doctor => doctor.JoinEntities)
        .ThenInclude(join => join.Doctor)
        .Include(doctor => doctor.JoinEntSpec)
        .ThenInclude(join => join.Doctor)
        .FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(thisDoctor);
    }

    public ActionResult Edit(int id)
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      ViewBag.SpecialtyId = new SelectList(_db.Specialties, "SpecialtyId", "Name");
      return View(thisDoctor);
    }

    [HttpPost]
    public ActionResult Edit(Doctor doctor)
    {
      _db.Entry(doctor).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(thisDoctor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Doctor thisDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      _db.Doctors.Remove(thisDoctor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    
    public ActionResult AddSpecialty(int id)
    {
      Doctor thisDoctor = _db.Doctors
        .Include(doctor => doctor.JoinEntities)
        .ThenInclude(join => join.Doctor)
        .FirstOrDefault(doctor => doctor.DoctorId == id);
      ViewBag.SpecialtyId = new SelectList(_db.Specialties, "SpecialtyId", "Name");
      return View(thisDoctor);
    }

    [HttpPost]
    public ActionResult AddSpecialty(Doctor doctor, int SpecialtyId)
    {
      if (SpecialtyId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() {SpecialtyId = SpecialtyId, DoctorId = doctor.DoctorId});
        _db.SaveChanges();
      }
    return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteSpecialty(int joinId)
    {
      var thisJoin = _db.DoctorSpecialty.FirstOrDefault(specialty => specialty.DoctorSpecialtyId == joinId);
      _db.DoctorSpecialty.Remove(thisJoin);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
