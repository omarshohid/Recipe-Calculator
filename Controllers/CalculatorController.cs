using Microsoft.Reporting.WebForms;
using RecipeCalculator.Models;
using RecipeCalculator.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        // GET: Calculator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calculator()
        {
            return View();
        }

        //[HttpGet]
        //[RecipeCalculator.Utility.SessionManger.CheckUserSession]
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
                        a => new { 
                            a.preparation_type,
                            a.standard_recipe_id,
                            a.standard_recipe_name,
                            a.unit,
                            a.weight,
                            a.yield_factor
                        }).ToList();

                    return Json(new { result = result, UnitList = UnitList, StdRecipeList = StdRecipeList, success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        //[HttpPost]
        //[RecipeCalculator.Utility.SessionManger.CheckUserSession]
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
                          a.retention_factor,
                          a.nutrition_weight,
                      }).ToList();

                    var yieldFactors = rce.tblYieldFactors
                        .Select(a => new
                        {
                            a.id,
                            Ingredient_name = a.Ingredient_name, // + " (" + a.Yield_factor+")",
                            a.Yield_factor,
                            a.own_YF_or_group
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
                            int unitflag = 0;

                            decimal converted_weight = 0;
                            if (unit == "mg")
                            {
                                converted_weight = (weight / 1000); // Mg to Gram conversion
                            }
                            else if (unit == "kg")
                            {
                                converted_weight = (weight * 1000); // Kg to Gram conversion
                            }
                            else if (unit == "g")
                            {
                                converted_weight = weight;
                            }
                            else if (unit == "mcg")
                            {
                                converted_weight = (weight / 1000000); // Mcg to Gram conversion
                            }
                            else
                            {
                                converted_weight = weight;
                                unitflag = 1;
                            }

                            total_weight += converted_weight;


                            row = new Dictionary<string, object>();
                            row.Add("id", search_ingredient.ingredient_id);
                            row.Add("ingredient_name", search_ingredient.ingredient_name);
                            row.Add("preparation_type", search_ingredient.preparation_type);
                            row.Add("weight", weight);
                            if(unitflag ==0)
                            {
                                row.Add("unit", "g");
                            }
                            else
                            {
                                row.Add("unit", unit);//search_ingredient.unit);
                            }
                          
                            for (int j = 0; j < nutritionList.Count; j++)
                            {
                                decimal sum = 0;
                                int flag = 0;
                                for (int k = 0; k < ingredientNutritionList.Count; k++)
                                {

                                    if (ingredientNutritionList[k].ingredient_name == search_ingredient.ingredient_name && ingredientNutritionList[k].ingredient_id == search_ingredient.ingredient_id && ingredientNutritionList[k].nutrition_name == nutritionList[j].nutrition_name)
                                    {
                                        decimal RF = (ingredientNutritionList[k].nutrition_weight * (converted_weight / search_ingredient.weight) * (ingredientNutritionList[k].retention_factor) ?? 1 );
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

                    decimal[] array = new decimal[nutritionList.Count];
                   // array[0] = "Nutrition";
                    if (rows.Count != 0)
                    {
                      
                        for (int l = 0; l < nutritionList.Count; l++)
                        {


                            decimal sum = 0;
                            for (int k = 0; k < rows.Count; k++)
                            {
                                sum += Convert.ToDecimal(rows[k].Where(a => a.Key == nutritionList[l].nutrition_name).Select(a => a.Value).FirstOrDefault());
                            }

                            total.Add(nutritionList[l].nutrition_name, sum);
                            total.Add(nutritionList[l].nutrition_name + "_unit", nutritionList[l].unit);

                            string nu_name = nutritionList[l].nutrition_name;
                            var search_nutrition = rce.tblNutritions.Where(a => a.nutrition_name == nu_name).Select(
                                a => new
                                {
                                    a.nutrition_name,
                                    a.unit,
                                    a.tag_name,
                                }).FirstOrDefault();

                            if(search_nutrition.unit == "g")
                            {
                                array[l] = sum;
                            }
                            else if(search_nutrition.unit == "mg")
                            {
                                array[l] = (sum/1000);
                            }
                            else if (search_nutrition.unit == "kg")
                            {
                                array[l] = (sum * 1000);
                            }
                            else if (search_nutrition.unit == "mcg")
                            {
                                array[l] = (sum / 1000000); // Mg to Gram conversion
                            }
                            else
                            {
                                array[l] = sum;

                            }
                           
                        }
                        total.Add("total_weight", total_weight);
                        total.Add("unit", "g");
                    }
                    Totalrows.Add(total);

                    return Json(new { result = nutritionList, ingredients = rows, total = Totalrows, array = array, Yieldfactors= yieldFactors, success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

       // [HttpPost]
        public ActionResult GetReport(string Selectedingredients)
        {
            bool user = User.Identity.IsAuthenticated;
            string[] parts = Selectedingredients.Split('/');

            string RecipeName = parts[0] != "undefined" ? parts[0] : "Please insert Recipe name";
            decimal YF= 1;
            if(parts[parts.Length - 1] != "undefined")
            {
                YF = Convert.ToDecimal(parts[parts.Length - 1]);
            }
             

            IngredientVM[] ingredientList = new IngredientVM[parts.Length-2];
            for (int i = 1; i < parts.Length - 1; i++ )
            {
                IngredientVM invm = new IngredientVM();
                string[] subPart = parts[i].Split('-');
                invm.ingredient_id = subPart[0] !="undefined"?Convert.ToInt32(subPart[0]) : 0;
                invm.weights = subPart[1] !="undefined" ? subPart[1] : "0";
                invm.unit = subPart[2] !="undefined" ? subPart[2] : "";
                ingredientList[i - 1] = invm;                           
            }

                //if (ingredientList != null)
                //{
                using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
                {
                    //try
                    //{
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
                          a.retention_factor,
                          a.nutrition_weight,
                      }).ToList();

                    var yieldFactors = rce.tblYieldFactors
                        .Select(a => new
                        {
                            a.id,
                            Ingredient_name = a.Ingredient_name + " (" + a.Yield_factor + ")",
                            a.Yield_factor,
                            a.own_YF_or_group
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
                            int unitflag = 0;

                            decimal converted_weight = 0;
                            if (unit == "mg")
                            {
                                converted_weight = (weight / 1000); // Mg to Gram conversion
                            }
                            else if (unit == "kg")
                            {
                                converted_weight = (weight * 1000); // Kg to Gram conversion
                            }
                            else if (unit == "g")
                            {
                                converted_weight = weight;
                            }
                            else if (unit == "mcg")
                            {
                                converted_weight = (weight / 1000000); // Mcg to Gram conversion
                            }
                            else
                            {
                                converted_weight = weight;
                                unitflag = 1;
                            }

                            total_weight += converted_weight;


                            row = new Dictionary<string, object>();
                            row.Add("id", search_ingredient.ingredient_id);
                            row.Add("ingredient_name", search_ingredient.ingredient_name);
                            row.Add("preparation_type", search_ingredient.preparation_type);
                            row.Add("weight", weight);
                            if (unitflag == 0)
                            {
                                row.Add("unit", "g");
                            }
                            else
                            {
                                row.Add("unit", unit);//search_ingredient.unit);
                            }

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
                    decimal[] array = new decimal[nutritionList.Count];

                    DataTable dt = new DataTable();
                    DataColumn dtColumn;
                    DataRow dtRow;


                    dtColumn = new DataColumn();
                    dtColumn.DataType = System.Type.GetType("System.String");
                    dtColumn.ColumnName = "Nutrition";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = System.Type.GetType("System.Decimal");
                    dtColumn.ColumnName = "NutritionWeight";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = System.Type.GetType("System.String");
                    dtColumn.ColumnName = "Unit";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = System.Type.GetType("System.String");
                    dtColumn.ColumnName = "Group";
                    dt.Columns.Add(dtColumn);


                    if (user == true)
                    {

                    
                    // array[0] = "Nutrition";
                    if (rows.Count != 0)
                    {

                        for (int l = 0; l < nutritionList.Count; l++)
                        {


                            decimal sum = 0;
                            for (int k = 0; k < rows.Count; k++)
                            {
                                sum += Convert.ToDecimal(rows[k].Where(a => a.Key == nutritionList[l].nutrition_name).Select(a => a.Value).FirstOrDefault());
                            }

                            //total.Add(nutritionList[l].nutrition_name, sum);
                            //total.Add(nutritionList[l].nutrition_name + "_unit", nutritionList[l].unit);

                            dtRow = dt.NewRow();
                            dtRow["Nutrition"] = nutritionList[l].nutrition_name;
                            dtRow["NutritionWeight"] = sum *YF;
                            dtRow["Unit"] = nutritionList[l].unit;

                            dt.Rows.Add(dtRow);


                            string nu_name = nutritionList[l].nutrition_name;
                            var search_nutrition = rce.tblNutritions.Where(a => a.nutrition_name == nu_name).Select(
                                a => new
                                {
                                    a.nutrition_name,
                                    a.unit,
                                    a.tag_name,
                                }).FirstOrDefault();

                            if (search_nutrition.unit == "g")
                            {
                                array[l] = sum;
                            }
                            else if (search_nutrition.unit == "mg")
                            {
                                array[l] = (sum / 1000);
                            }
                            else if (search_nutrition.unit == "kg")
                            {
                                array[l] = (sum * 1000);
                            }
                            else if (search_nutrition.unit == "mcg")
                            {
                                array[l] = (sum / 1000000); // Mg to Gram conversion
                            }
                            else
                            {
                                array[l] = sum;

                            }

                        }

                    }
                     //   total.Add("total_weight", total_weight);
                     //   total.Add("unit", "g");                   
                     // Totalrows.Add(total);
                    }
                    else
                    {
                        if (rows.Count != 0)
                        {

                            for (int l = 0; l < nutritionList.Count; l++)
                            {


                                decimal sum = 0;
                                for (int k = 0; k < rows.Count; k++)
                                {
                                    sum += Convert.ToDecimal(rows[k].Where(a => a.Key == nutritionList[l].nutrition_name).Select(a => a.Value).FirstOrDefault());
                                }

                                //total.Add(nutritionList[l].nutrition_name, sum);
                                //total.Add(nutritionList[l].nutrition_name + "_unit", nutritionList[l].unit);
                                if (nutritionList[l].id >=30 && nutritionList[l].id<=55 || nutritionList[l].id == 116)
                                {

                                    dtRow = dt.NewRow();
                                    if (nutritionList[l].nutrition_name.Contains("Dietary fibre or if missing dietary fibre value"))
                                    {
                                        dtRow["Nutrition"] = "Total dietary fibre";
                                    }
                                    else if (nutritionList[l].nutrition_name.Contains("Vitamin E (in alpha-tocopherol equivalents)"))
                                    {
                                        dtRow["Nutrition"] = "Vitamin E";
                                    }
                                    else if (nutritionList[l].nutrition_name.Contains("Niacin equivalent"))
                                    {
                                        dtRow["Nutrition"] = "Niacin";
                                    }
                                    else if (nutritionList[l].nutrition_name.Contains("Vitamin C (mainly L-Ascorbic acid)"))
                                    {
                                        dtRow["Nutrition"] = "Vitamin C";
                                    }
                                    else if (nutritionList[l].nutrition_name.Contains("Beta-carotene equivalents or [beta-carotene]"))
                                    {
                                        dtRow["Nutrition"] = "Beta Carotene";
                                    }
                                    else if (nutritionList[l].nutrition_name.Contains("Vitamin A (expressed in retinol activity equivalents)"))
                                    {
                                        dtRow["Nutrition"] = "Vitamin A";
                                    }
                                    else
                                    {
                                        dtRow["Nutrition"] = nutritionList[l].nutrition_name;
                                    }
                                    dtRow["NutritionWeight"] = sum * YF;
                                    dtRow["Unit"] = nutritionList[l].unit;

                                    if (nutritionList[l].id >= 30 && nutritionList[l].id <= 36)
                                    {
                                        dtRow["Group"] = "A";
                                    }
                                    else if (nutritionList[l].id >= 37 && nutritionList[l].id <= 44)
                                    {
                                        dtRow["Group"] = "B";
                                    }
                                    else if (nutritionList[l].id >= 45 && nutritionList[l].id <= 48 || nutritionList[l].id == 54)
                                    {
                                        dtRow["Group"] = "D";
                                    }
                                    else if (nutritionList[l].id == 116)
                                    {
                                        dtRow["Group"] = "E";
                                    }
                                    else
                                    {
                                        dtRow["Group"] = "C";
                                    }
                                    dt.Rows.Add(dtRow); 
                                }


                                string nu_name = nutritionList[l].nutrition_name;
                                var search_nutrition = rce.tblNutritions.Where(a => a.nutrition_name == nu_name).Select(
                                    a => new
                                    {
                                        a.nutrition_name,
                                        a.unit,
                                        a.tag_name,
                                    }).FirstOrDefault();

                                if (search_nutrition.unit == "g")
                                {
                                    array[l] = sum;
                                }
                                else if (search_nutrition.unit == "mg")
                                {
                                    array[l] = (sum / 1000);
                                }
                                else if (search_nutrition.unit == "kg")
                                {
                                    array[l] = (sum * 1000);
                                }
                                else if (search_nutrition.unit == "mcg")
                                {
                                    array[l] = (sum / 1000000); // Mg to Gram conversion
                                }
                                else
                                {
                                    array[l] = sum;

                                }

                            }

                        }
                    }




                    LocalReport lr = new LocalReport();
                    string path="";
                    if (user == true)
                    {
                        path = Path.Combine(Server.MapPath("~/Reports/NutritionFacts.rdlc"));
                    }
                    else
                    {
                        path = Path.Combine(Server.MapPath("~/Reports/NutritionFactsCommon.rdlc"));
                    }
                    
                    if (System.IO.File.Exists(path))
                    {
                        lr.ReportPath = path;
                    }
                                  
                    ReportParameterCollection reportParameters = new ReportParameterCollection();
                    reportParameters.Add(new ReportParameter("RecipeName", RecipeName));                   

                    lr.SetParameters(reportParameters);
                    ReportDataSource rd;

                    rd = new ReportDataSource("ReportDataSet", dt);
                    lr.DataSources.Add(rd);

                    string reportType = "pdf";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo =

                    "<DeviceInfo>" +
                    "  <OutputFormat>" + "pdf" + "</OutputFormat>" +
                    "</DeviceInfo>";

                    Warning[] warnings;
                    string[] streams;
                    byte[] renderedBytes;

                    renderedBytes = lr.Render(
                        reportType,
                        deviceInfo,
                        out mimeType,
                        out encoding,
                        out fileNameExtension,
                        out streams,
                        out warnings);

                    return File(renderedBytes, mimeType);

                    //  return Json(new { result = nutritionList, ingredients = rows, total = Totalrows, array = array, Yieldfactors = yieldFactors, success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);

                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            return File("","","");
                    //        }
                    //    } 
                    //}
                    //else
                    //{
                    //    return Content("Please insert data carefully!");
                }
        }

        //[HttpPost]
        //[RecipeCalculator.Utility.SessionManger.CheckUserSession]
        public JsonResult GetStandardRecipeById(int stdrecipeid)
        {
            using (RecipeCalculatorEntities rce = new RecipeCalculatorEntities())
            {
                try
                {

                    var std_rec_ingredientList = rce.tblStandardRecipeIngredients.Where(a => a.standard_recipe_id == stdrecipeid).Select(
                        a => new
                        {
                            a.tblIngredient.ingredient_id,
                            a.tblIngredient.ingredient_name,
                            weights = a.weight,
                           unit = a.unit_name,                            
                        }).ToList();


                    return Json(new {ingredientList =std_rec_ingredientList,  success = true, successMessage = "Successfully Saved" }, JsonRequestBehavior.AllowGet);
                }
                catch(Exception ex)
                {
                    return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

    }
}