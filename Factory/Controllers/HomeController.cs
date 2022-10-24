using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
    public class HomeController : Controller
    {

      private readonly FactoryContext _db;

      public HomeController(FactoryContext db)
      {
        _db = db;
      }

      [HttpGet("/")]
      public ActionResult Login()
      {
        return View();
      }

      public ActionResult Index()
      {
        ViewBag.AllEngineers = _db.Engineers.ToList();
        ViewBag.AllMachines = _db.Machines.ToList();
        return View();
      }
    }
}