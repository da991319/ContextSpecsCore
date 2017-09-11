using System;
using ContextSpecsCore.Annotations;

namespace ContextSpecsCore
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    public sealed class SubjectAttribute : Attribute
    {
        public SubjectAttribute(string subject)
        {
            Subject = subject;
        }

        public string Subject { get; set; }
    }
}