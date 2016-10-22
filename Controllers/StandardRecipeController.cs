using RecipeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeCalculator.Controllers
{
    public class StandardRecipeController : Controller
    {
        // GET: StandardRecipe
        public ActionResult Index()
        {
            return View();
        }

        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public ActionResult StandardRecipe()
        {
            return View();
        }

        [HttpGet]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetddlIngredientList()
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    var result = rce.tblIngredients.Select(
                        a => new
                        {
                            a.ingredient_id,
                            a.ingredient_name,
                            a.preparation_type,
                            a.unit,
                            a.weight
                        }).ToList();

                    var UnitList = rce.tblUnits.Select(
                        a => new
                        {
                            a.unit_id,
                            a.unit_name
                        }).ToList();
                    var StdRecipeList = rce.tblStandardRecipes.Select(
                        a => new
                        {
                            a.standard_recipe_id,
                            a.standard_recipe_name,
                            a.preparation_type,
                            a.unit,
                        }).ToList();

                    return Json(new { result = result, UnitList = UnitList, StdRecipeList = StdRecipeList, success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult SaveStandardRecipe(StandardRecipeVM standardrecipe, IngredientVM[] ingredient)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    var results = rce.tblStandardRecipes.Where(a => a.standard_recipe_name == standardrecipe.standard_recipe_name).ToList();
                    if (results.Count == 0)
                    {
                        tblStandardRecipe tsr = new tblStandardRecipe();
                        tsr.standard_recipe_name = standardrecipe.standard_recipe_name;
                        tsr.yield_factor = standardrecipe.yield_factor;
                        rce.tblStandardRecipes.Add(tsr);
                        rce.SaveChanges();

                        var search_st_rec_id = rce.tblStandardRecipes.Where(b => b.standard_recipe_name == standardrecipe.standard_recipe_name).FirstOrDefault();
                        for (int i = 0; i < ingredient.Length; i++)
                        {
                            tblStandardRecipeIngredient tsri = new tblStandardRecipeIngredient();
                            tsri.standard_recipe_id = search_st_rec_id.standard_recipe_id;
                            tsri.ingredient_id = ingredient[i].ingredient_id;
                            tsri.weight = Convert.ToDecimal(ingredient[i].weights);
                            tsri.unit_name = ingredient[i].unit;

                            rce.tblStandardRecipeIngredients.Add(tsri);
                        }
                        rce.SaveChanges();
                        return Json(new { success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "This Recipe Already Exists" }, JsonRequestBehavior.AllowGet);
                    }

                    //return Json(new { result = result, success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetIngredientList(IngredientVM[] ingredientList)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {
                    var nutritionList = rce.tblNutritions.Select(
                        a => new
                        {
                            a.id,
                            a.nutrition_name,
                            a.tag_name,
                            a.unit
                        }).ToList();

                    var ingredientNutritionList = rce.tblIngredientNutritions
                      .Select(a => new
                      {
                          a.tblIngredient.ingredient_name,
                          a.tblNutrition.nutrition_name,
                          a.ingredient_id,
                          a.tblNutrition.id,
                          a.nutrition_weight,
                          a.retention_factor
                      }).ToList();

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;

                    List<List<String>> matrix = new List<List<String>>();
                    decimal total_weight = 0;
                    for (int i = 0; i < ingredientList.Length; i++)
                    {


                        if (ingredientList[i].ingredient_id != 0)
                        {
                            int id = ingredientList[i].ingredient_id;
                            decimal weight = Convert.ToDecimal(ingredientList[i].weights);

                            string unit = ingredientList[i].unit;
                           

                            var search_ingredient = rce.tblIngredients.Where(a => a.ingredient_id == id).Select(
                                a => new
                                {
                                    a.ingredient_id,
                                    a.ingredient_name,
                                    a.preparation_type,
                                    a.unit,
                                    a.weight
                                }).FirstOrDefault();

                            //if (unit == search_ingredient.unit)
                            //{
                            //    unit = ingredientList[i].unit;
                            //}

                            decimal converted_weight = 0;
                            if(unit == "mg")
                            {
                                converted_weight = (weight / 1000); // Mg to Gram conversion
                            }
                            else if(unit == "kg")
                            {
                                converted_weight = (weight * 1000); // Kg to Gram conversion
                            }
                            else if(unit == "g")
                            {
                                converted_weight = weight;
                            }
                            else if (unit == "mcg") // Microgram to Gram conversion
                            {
                                converted_weight = (weight / 1000000);
                            }

                            total_weight += converted_weight;


                            row = new Dictionary<string, object>();
                            row.Add("id", search_ingredient.ingredient_id);
                            row.Add("ingredient_name", search_ingredient.ingredient_name);
                            row.Add("preparation_type", search_ingredient.preparation_type);
                            row.Add("weight", weight);
                            row.Add("unit", unit);//search_ingredient.unit);
                            for (int j = 0; j < nutritionList.Count; j++)
                            {
                                decimal sum = 0;
                                int flag = 0;
                                for (int k = 0; k < ingredientNutritionList.Count; k++)
                                {
                                      
                                    if (ingredientNutritionList[k].ingredient_name == search_ingredient.ingredient_name && ingredientNutritionList[k].ingredient_id == search_ingredient.ingredient_id && ingredientNutritionList[k].nutrition_name == nutritionList[j].nutrition_name)
                                    {
                                        decimal RF = (ingredientNutritionList[k].nutrition_weight * (converted_weight / search_ingredient.weight) * (ingredientNutritionList[k].retention_factor) ?? 1);
                                        //sum+=RF;
                                        row.Add(nutritionList[j].nutrition_name, RF);                                        
                                        flag = 1;
                                        sum += RF;
                                    }

                                }
                                //row.Add(nutritionList[j].nutrition_name + "_sum", sum);
                                if (flag == 0)
                                {
                                    row.Add(nutritionList[j].nutrition_name, 0);
                                }
                            }

                            rows.Add(row); 
                           
                        }
                    }

                    List<Dictionary<string, object>> Totalrows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> total = new Dictionary<string, object>();

                    if (rows.Count!=0)
                    {

                        for (int l = 0; l < nutritionList.Count; l++)
                        {
                            
                            decimal sum = 0;                               
                            for (int k = 0; k < rows.Count; k++)
                            {
                                sum += Convert.ToDecimal(rows[k].Where(a => a.Key == nutritionList[l].nutrition_name).Select(a => a.Value).FirstOrDefault());
                            }

                            total.Add(nutritionList[l].nutrition_name, sum);
                            total.Add(nutritionList[l].nutrition_name+"_unit", nutritionList[l].unit);
                        }
                        total.Add("total_weight", total_weight);
                        total.Add("unit", "g");
                     }
                     Totalrows.Add(total); 

                     return Json(new { result = nutritionList, ingredients = rows, total = Totalrows, success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}