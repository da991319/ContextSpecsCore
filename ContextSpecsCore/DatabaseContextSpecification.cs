using System;
using System.Threading.Tasks;
using Autofac;

namespace ContextSpecsCore
{
	public abstract class DatabaseContextSpecification<T> : ContextSpecification<T> where T : class
	{
		private static IContainer Container { get; set; }
		protected Func<T, Task> BecauseOf;
		public override async Task TestFixtureSetUp()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<AutofacModule>();
			Container = builder.Build();

			using (var scope = Container.BeginLifetimeScope())
			{
				InitializeContext();
				CreateSut(scope);
				await GetDbReady(scope);
				InvokeEstablish();
				await InvokeBecause();
			}
		}

		/// <summary>
		/// Get the database ready (create, run migrations etc)
		/// </summary>
		/// <param name="scope"></param>
		/// <returns></returns>
		private async Task GetDbReady(ILifetimeScope scope)
		{
		}

		private void CreateSut(ILifetimeScope scope)
		{
			sut = scope.Resolve<T>();
		}

	}
}
