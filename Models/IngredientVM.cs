using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeCalculator.Models
{
    public class IngredientVM
    {
        public string Choice_id { get; set; }
        public int ingredient_id { get; set; }
        public string ingredient_name { get; set; }
        public string preparation_type { get; set; }
        public decimal weight { get; set; }
        public string weights { get; set; }
        public string unit { get; set; }
        
    }
}