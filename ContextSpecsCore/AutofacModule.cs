using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace ContextSpecsCore
{
	public class AutofacModule : Autofac.Module
	{
		/// <summary>
		/// register any component, services necessary for DatabaseContectSpecification to 
		/// create the subject under test (sut)
		/// </summary>
		/// <param name="builder"></param>
		protected override void Load(ContainerBuilder builder)
		{
			
		}
	}
}
