using System;
using ContextSpecsCore.Annotations;
using NUnit.Framework;

namespace ContextSpecsCore.Categories
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    public sealed class UnitAttribute : CategoryAttribute
    {
        public UnitAttribute()
            : base("Unit")
        {
        }
    }
}