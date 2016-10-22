using RecipeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeCalculator.Controllers
{
    public class NutritionController : Controller
    {
        // GET: Recipe
        public ActionResult Index()
        {
            return View();
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult Nutritions()
        {

            return View();
        }

        //public ActionResult Ingredients()
        //{
        //    return View();
        //}

        //public ActionResult NutribtionFacts()
        //{
        //    return View();
        //}
        //public ActionResult StandardRecipe()
        //{
        //    return View();
        //}
        //public ActionResult Calculator()
        //{
        //    return View();
        //}
        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult SaveNewNutrition(NutritionVM nutrition)
        {
            using ( RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                var search_nutrition = rce.tblNutritions.Where(a => a.nutrition_name == nutrition.nutrition_name).ToList();
                if(search_nutrition.Count == 0)
                {
                    try
                    {
                        //  int id = Int32.Parse(nutrition.unit);
                        //  string unit =  rce.tblUnits.Where(a => a.unit_id == id).Select(a => a.unit_name).FirstOrDefault();
                        tblNutrition tblnutrition = new tblNutrition();
                        tblnutrition.nutrition_name = nutrition.nutrition_name;
                        tblnutrition.unit = nutrition.unit;
                        tblnutrition.tag_name = nutrition.tag_name;

                        rce.tblNutritions.Add(tblnutrition);
                        rce.SaveChanges();
                        return Json(new { success = true, successMessage = "Successfully Saved" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                    } 
                }
                else
                {
                    return Json(new { success = false, errorMessage = "Nutrition already exists" }, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetNutritionList()
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    var result = rce.tblNutritions.Select(
                        a => new
                        {
                            a.id,
                            a.nutrition_name,
                            a.tag_name,
                            a.unit
                        }).ToList();

                    var unitList = rce.tblUnits.Select(
                        a => new
                        {
                            a.unit_id,
                            a.unit_name
                        }).ToList();

                    return Json(new { result = result, UnitList = unitList, success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                } 
            }
        }


        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult EditNutrition()
        {
            return View();
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetNutritionById(int id)
        {
            using ( RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {               
                try
                {
                    var result = rce.tblNutritions.Where(a=>a.id == id).Select(
                        a => new
                        {
                            a.id,
                            a.nutrition_name,
                            a.tag_name,
                            a.unit
                        }).FirstOrDefault();

                    return Json(new { result = result, success = true }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                } 
            }

        }

        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult UpdateNutrition(int id, NutritionVM nutrition)
        {
            using ( RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
               
                try
                {

                    tblNutrition tblnutrition = new tblNutrition();
                    tblnutrition.id = id;
                    tblnutrition.nutrition_name = nutrition.nutrition_name;
                    tblnutrition.unit = nutrition.unit;
                    tblnutrition.tag_name = nutrition.tag_name;

                    rce.tblNutritions.Attach(tblnutrition);
                    var entry = rce.Entry(tblnutrition);
                    entry.State = EntityState.Modified;
                    rce.SaveChanges();
                    return Json(new { success = true, successMessage = "Successfully Updated" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                } 
            }

        }

        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult DeleteNutrition(int id)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    tblNutrition tblnutrition = new tblNutrition();
                    var removeItem = rce.tblNutritions.SingleOrDefault(a => a.id == id);
                    if (removeItem != null)
                    {
                        rce.tblNutritions.Remove(removeItem);
                        rce.SaveChanges();

                        return Json(new { success = true, successMessage = "Successfully Deleted" });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Item Not Found" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}