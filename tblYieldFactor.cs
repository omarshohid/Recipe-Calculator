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
    
    public partial class tblYieldFactor
    {
        public int id { get; set; }
        public string Ingredient_name { get; set; }
        public Nullable<decimal> Yield_factor { get; set; }
        public Nullable<bool> own_YF_or_group { get; set; }
    }
}
