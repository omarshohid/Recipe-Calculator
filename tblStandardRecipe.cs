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
    
    public partial class tblStandardRecipe
    {
        public tblStandardRecipe()
        {
            this.tblStandardRecipeIngredients = new HashSet<tblStandardRecipeIngredient>();
        }
    
        public int standard_recipe_id { get; set; }
        public string standard_recipe_name { get; set; }
        public string preparation_type { get; set; }
        public Nullable<decimal> weight { get; set; }
        public string unit { get; set; }
        public decimal yield_factor { get; set; }
    
        public virtual ICollection<tblStandardRecipeIngredient> tblStandardRecipeIngredients { get; set; }
    }
}
