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
    
    public partial class tblIngredientNutrition
    {
        public int id { get; set; }
        public int ingredient_id { get; set; }
        public int nutrition_id { get; set; }
        public decimal retention_factor { get; set; }
        public decimal nutrition_weight { get; set; }
        public Nullable<int> OwnRForGroup { get; set; }
    
        public virtual tblIngredient tblIngredient { get; set; }
        public virtual tblNutrition tblNutrition { get; set; }
    }
}
