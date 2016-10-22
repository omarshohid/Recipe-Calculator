using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeCalculator.Models
{
    public class StandardRecipeVM
    {
        public int standard_recipe_id { get; set; }
        public string standard_recipe_name { get; set; }
        public string preparation_type { get; set; }
        public decimal weight { get; set; }
        public string unit { get; set; }
        public decimal yield_factor { get; set; }

    }
}