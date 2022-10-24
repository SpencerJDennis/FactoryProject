using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;

    public EngineersController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Engineer> model = _db.Engineers.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.EngineerId = new SelectList(_db.Machines, "EngineerId", "EngineerName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Engineer engineer)
    {
      _db.Engineers.Add(engineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisEngineer = _db.Engineers
        .Include(engineer => engineer.JoinEntities)
        .ThenInclude(join => join.Machines)
        .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    public ActionResult Edit(int id)
    {
      var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult Edit(Engineer engineer)
    {
      _db.Entry(engineer).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = engineer.EngineerId});
    }

    public ActionResult Delete(int id)
    {
      var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      List<EngineerMachine> thisEngineersJoins = _db.EngineerMachine.ToList();
      foreach(EngineerMachine engineermachine in thisEngineersJoins)
      {
        if (engineermachine.EngineerId == id)
        {
          _db.EngineerMachine.Remove(engineermachine);
        }
      }
      _db.Engineers.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    
    public ActionResult AddMachine(int id)
    {
      Engineer thisEngineer = _db.Engineers
        .FirstOrDefault(engineer => engineer.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "MachineName");
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult AddMachine(Engineer engineer, int MachineId)
    {
      if (MachineId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() {MachineId = MachineId, EngineerId = engineer.EngineerId});
        _db.SaveChanges();
      }
    return RedirectToAction("Details", new {id = engineer.EngineerId});
    }

     public ActionResult Remove(int id)
    {
      var thisJoin = _db.EngineerMachine.FirstOrDefault(engineermachine => engineermachine.EngineerMachineId == id);
      return View(thisJoin);
    }

    [HttpPost, ActionName("Remove")]
    public ActionResult RemoveConfirmed(int id)
    {
      var thisJoin = _db.EngineerMachine.FirstOrDefault(engineermachine => engineermachine.EngineerMachineId == id);
      _db.EngineerMachine.Remove(thisJoin);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisJoin.EngineerId});
    }
  }
}
