using RecipeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeCalculator.Controllers
{
    public class UnitController : Controller
    {
        // GET: Unit
        public ActionResult Index()
        {
            return View();
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult AddUnit()
        {
            return View();
        }

        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult SaveNewUnit(UnitVM unit)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    tblUnit tblunit = new tblUnit();
                    tblunit.unit_name = unit.unit_name;
                    rce.tblUnits.Add(tblunit);
                    rce.SaveChanges();
                    return Json(new { success = true, successMessage = "Successfully Saved" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetUnitList()
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    var result = rce.tblUnits.Select(
                        a => new
                        {
                            a.unit_id,
                            a.unit_name
                        }).ToList();

                    return Json(new { result = result, success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult EditUnit()
        {
            return View();
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetUnitById(int id)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    var result = rce.tblUnits.Where(a => a.unit_id == id).Select(
                        a => new
                        {
                            a.unit_id,
                            a.unit_name
                        }).FirstOrDefault();
                    return Json(new { result = result, success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult UpdateUnit(int id,UnitVM unit)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    tblUnit tblunit = new tblUnit();
                    tblunit.unit_id = id;
                    tblunit.unit_name = unit.unit_name;
                    rce.tblUnits.Attach(tblunit);
                    var entry = rce.Entry(tblunit);
                    entry.State = EntityState.Modified;
                    rce.SaveChanges();
                    return Json(new { success = true, successMessage = "Successfully Updated" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}