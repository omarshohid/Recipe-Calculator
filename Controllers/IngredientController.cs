using RecipeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeCalculator.Controllers
{
    public class IngredientController : Controller
    {
        // GET: Ingredient
        public ActionResult Index()
        {
            return View();
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult Ingredients()
        {
            return View();
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult IngredientList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetddlNutritionList()
        {
            using(RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
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

                        var units = rce.tblUnits.Select(
                            a => new
                            {
                                a.unit_id,
                                a.unit_name
                            }).ToList();

                        return Json(new { result = result, UnitList = units, success = true }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                    } 
	        }
        }

        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult SaveIngredient(IngredientVM ingredient, NutritionVM[] nutrition)
        {
            decimal converted_weight = 0;
            try
            {
               
                using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
                {
                    

                    var results = rce.tblIngredients.Where(a=>a.ingredient_name == ingredient.ingredient_name).ToList();
                               
                    if (results.Count == 0)
                    {
                        if (ingredient.unit == "g") // Gram to Gram conversion
                        {
                            converted_weight = ingredient.weight;
                        }
                        else if(ingredient.unit == "mg")  // Milligrams to Grams conversion
                        {
                            converted_weight = (ingredient.weight / 1000);
                        }
                        else if(ingredient.unit == "kg")  // Kg to Grams conversion
                        {
                            converted_weight = (ingredient.weight * 1000);
                        }
                        else if(ingredient.unit =="mcg") // Microgram to Gram conversion
                        {
                            converted_weight = (ingredient.weight / 1000000); 
                        }



                        tblIngredient tblingredient = new tblIngredient();
                        tblingredient.ingredient_name = ingredient.ingredient_name;
                        tblingredient.preparation_type = ingredient.preparation_type;
                        tblingredient.weight = converted_weight;
                        tblingredient.unit = "g";
                        rce.tblIngredients.Add(tblingredient);
                        rce.SaveChanges();

                        var ingredient_id = rce.tblIngredients.Where(b => b.ingredient_name == ingredient.ingredient_name).FirstOrDefault();
                        for (int i = 0; i < nutrition.Length; i++)
                        {
                            tblIngredientNutrition tin = new tblIngredientNutrition();
                            tin.ingredient_id = ingredient_id.ingredient_id;
                            tin.nutrition_id = nutrition[i].nutrition_id;
                            tin.retention_factor = (nutrition[i].retention_factor);
                            tin.nutrition_weight = nutrition[i].nutrition_weight;
                           // tin.nutrition_weight_with_RF = nutrition[i].nutrition_weight*()

                            rce.tblIngredientNutritions.Add(tin);
                        }

                        rce.SaveChanges();

                        return Json(new { success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "This Ingredient Already Exists" }, JsonRequestBehavior.AllowGet);
                    }   
                }              
            }
            catch (Exception ex)
            {

                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult EditIngredient()
        {
            return View();
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetIngredientById(int id)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    var result = rce.tblIngredients.Where(a=>a.ingredient_id == id).Select(
                        a => new
                        {
                           a.ingredient_id,
                           a.ingredient_name,
                           a.preparation_type,
                           a.unit,
                           a.weight
                        }).FirstOrDefault();
                    var ddlNutrition = rce.tblNutritions.Select(
                    a => new
                    {
                        a.id,
                        a.nutrition_name,
                        a.unit,
                        a.tag_name
                    }).ToList();

                    var nutritionList = rce.tblIngredientNutritions.Where(a => a.ingredient_id == id).Select(
                       a => new
                       {
                           a.nutrition_id,
                           a.tblNutrition.nutrition_name,
                           a.tblNutrition.unit,
                           a.retention_factor,
                           a.nutrition_weight,

                       }).ToList();
                   
                    return Json(new { result = result, ddlnutrition = ddlNutrition, nutritionlist = nutritionList,  success = true }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult UpdateIngredient(IngredientVM ingredient, NutritionVM[] nutrition)
        {
            decimal converted_weight = 0;
            try
            {
                if (ingredient.unit == "g")
                {
                    converted_weight = ingredient.weight;
                }
                else if (ingredient.unit == "mg")  // Milligrams to Grams conversion
                {
                    converted_weight = (ingredient.weight / 1000);
                }
                else if (ingredient.unit == "kg")  // Kg to Grams conversion
                {
                    converted_weight = (ingredient.weight * 1000);
                }

                using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
                {
                    //var singred = rce.tblIngredients.Where(x => x.ingredient_id == ingredient.ingredient_id).Select(s => new { 
                    //    s.ingredient_id,                       
                        
                    //}).ToList();
                    tblIngredient ti = new tblIngredient();
                    ti.ingredient_id = ingredient.ingredient_id;
                    ti.ingredient_name = ingredient.ingredient_name;
                    ti.preparation_type = ingredient.preparation_type;
                    ti.unit = "g";
                    ti.weight = converted_weight;
                    rce.tblIngredients.Attach(ti);
                    var entry = rce.Entry(ti);
                    entry.State = EntityState.Modified;
                    rce.SaveChanges();

                    var removeList = from c in rce.tblIngredientNutritions where c.ingredient_id == ingredient.ingredient_id select c;

                   foreach(var item in removeList)
                   {
                       rce.tblIngredientNutritions.Remove(item);
                   }
                   rce.SaveChanges();
                   for (int i = 0; i < nutrition.Length; i++)
                   {
                       tblIngredientNutrition tin = new tblIngredientNutrition();
                       tin.ingredient_id = ingredient.ingredient_id;
                       tin.nutrition_id = nutrition[i].nutrition_id;
                       tin.retention_factor = (nutrition[i].retention_factor);
                       tin.nutrition_weight = nutrition[i].nutrition_weight;
                       rce.tblIngredientNutritions.Add(tin);
                   }

                   rce.SaveChanges();

                    return Json(new { success = true, successMessage = "Successfully Updated" }, JsonRequestBehavior.AllowGet);                 
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}