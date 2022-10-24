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

      public ActionResult Index()
      {
        ViewBag.Machines = new List<Machine>(_db.Machines);
        return View(_db.Engineers.ToList());
      }
    }
}