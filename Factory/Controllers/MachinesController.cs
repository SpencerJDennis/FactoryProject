using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Machine> model = _db.Machines.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
			ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "MachineName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Machine machine)
    {
      _db.Machines.Add(machine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisMachine = _db.Machines
        .Include(machine => machine.JoinEntities)
        .ThenInclude(join => join.Engineers)
        .FirstOrDefault(machine => machine.MachineId == id);
      return View(thisMachine);
    }

    public ActionResult Edit(int id)
    {
      var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult Edit(Machine machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId});
      }
        _db.Entry(machine).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Details", new {id = machine.MachineId});
    }

    public ActionResult Delete(int id)
    {
      var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      return View(thisMachine);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      List<EngineerMachine> thisMachineJoins = _db.EngineerMachine.ToList();
      foreach(EngineerMachine engineermachine in thisMachineJoins)
      {
        if(engineermachine.MachineId == id)
        {
          _db.EngineerMachine.Remove(engineermachine);
        }
      }
      _db.Machines.Remove(thisMachine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddEngineer(int id)
    {
      Machine thisMachine = _db.Machines
      .FirstOrDefault(machine => machine.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "EngineerName");
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult AddEngineer(Machine machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() {EngineerId = EngineerId, MachineId = machine.MachineId});
        _db.SaveChanges();
      }
    return RedirectToAction("Details", new {id = machine.MachineId});
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
      return RedirectToAction("Details", new { id = thisJoin.MachineId});
    }
  }
}