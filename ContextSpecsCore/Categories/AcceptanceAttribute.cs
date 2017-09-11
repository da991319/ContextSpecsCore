using System;
using ContextSpecsCore.Annotations;
using NUnit.Framework;

namespace ContextSpecsCore.Categories
{
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    public sealed class AcceptanceAttribute : CategoryAttribute
    {
        public AcceptanceAttribute()
            : base("Acceptance")
        {
        }
    }
}