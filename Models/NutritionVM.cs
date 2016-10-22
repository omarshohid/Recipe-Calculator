using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeCalculator.Models
{
    public class NutritionVM
    {
        public int id { get; set; }
        public int nutrition_id { get; set; }
        public string nutrition_name { get; set; }
        public string unit { get; set; }
        public string tag_name { get; set; }
        public decimal retention_factor { get; set; }
        public decimal nutrition_weight { get; set; }
    }
}