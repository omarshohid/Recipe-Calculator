//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RecipeCalculator
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblIngredient
    {
        public tblIngredient()
        {
            this.tblStandardRecipeIngredients = new HashSet<tblStandardRecipeIngredient>();
            this.tblIngredientNutritions = new HashSet<tblIngredientNutrition>();
        }
    
        public int ingredient_id { get; set; }
        public string ingredient_name { get; set; }
        public string preparation_type { get; set; }
        public Nullable<decimal> weight { get; set; }
        public string unit { get; set; }
        public string FoodCoodeWithGroupid { get; set; }
        public string FoodnameinBengali { get; set; }
        public string ScientificName { get; set; }
    
        public virtual ICollection<tblStandardRecipeIngredient> tblStandardRecipeIngredients { get; set; }
        public virtual ICollection<tblIngredientNutrition> tblIngredientNutritions { get; set; }
    }
}
