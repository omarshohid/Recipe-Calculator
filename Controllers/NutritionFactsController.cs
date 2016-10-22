using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeCalculator.Controllers
{
    public class NutritionFactsController : Controller
    {
        // GET: NutritionFacts
        public ActionResult Index()
        {
            return View();
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult NutritionFacts()
        {
            return View();
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetNutritionList()
        {
            try
            {
                using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
                {
                    var nutritionList = rce.tblNutritions
                       .Select(a => new
                       {
                           a.nutrition_name,
                           a.unit,
                           a.tag_name,
                           a.id
                       }).ToList();
                    return Json(new { result = nutritionList, success = true }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetIngredientList()
        {
            var s = "";
            try
            {
                using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
                {
                    var ingredientNutritionList = rce.tblIngredientNutritions
                        .Select(a => new
                        {
                            a.tblIngredient.ingredient_name,
                            a.tblNutrition.nutrition_name,
                            a.tblNutrition.id,
                            a.retention_factor,
                            a.nutrition_weight
                        }).ToList();

                    var ingredientList = rce.tblIngredients
                       .Select(a => new
                       {
                           a.ingredient_name,
                           a.unit,
                           a.weight,
                           a.ingredient_id,
                           a.preparation_type
                       }).ToList();

                    var nutritionList = rce.tblNutritions
                       .Select(a => new
                       {
                           a.nutrition_name,
                           a.unit,
                           a.tag_name,
                           a.id
                       }).ToList();
                    
                     List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                     Dictionary<string, object> row;

                     List<List<String>> matrix = new List<List<String>>();
                     for (int i = 0; i < ingredientList.Count; i++ )
                     {
                         s = ingredientList[i].ingredient_id.ToString(); ;
                         row = new Dictionary<string, object>();
                         row.Add("id", ingredientList[i].ingredient_id);
                         row.Add("ingredient_name", ingredientList[i].ingredient_name);
                         row.Add("preparation_type", ingredientList[i].preparation_type);
                         row.Add("weight", ingredientList[i].weight);
                         row.Add("unit", ingredientList[i].unit);
                         for (int j=0; j<nutritionList.Count; j++)
                         {
                             int flag = 0;
                             for(int k=0; k<ingredientNutritionList.Count; k++)
                             {
                                 
                                 if(ingredientNutritionList[k].ingredient_name == ingredientList[i].ingredient_name && ingredientNutritionList[k].nutrition_name == nutritionList[j].nutrition_name)
                                 {
                                     row.Add(nutritionList[j].nutrition_name, ingredientNutritionList[k].nutrition_weight);
                                     flag = 1;
                                 }
                                 
                             }
                             if(flag  == 0)
                             {
                                     row.Add(nutritionList[j].nutrition_name, 0);
                             }
                         }

                         rows.Add(row);
                     }
                         //foreach(var item in ingredientList)
                         //{
                         //    foreach(var nut in nutritionList)
                         //    {

                         //    }
                         //}
                     return Json(new { result = rows, success = true }, JsonRequestBehavior.AllowGet);
                }

           
                }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message+s  }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}