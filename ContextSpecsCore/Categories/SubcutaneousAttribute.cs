using System;
using ContextSpecsCore.Annotations;
using NUnit.Framework;

namespace ContextSpecsCore.Categories
{
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    public sealed class SubcutaneousAttribute : CategoryAttribute
    {
        public SubcutaneousAttribute() : base("Subcutaneous")
        {
        }
    }
}