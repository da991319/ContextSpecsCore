# ContextSpecsCore

Based on project [nunit.specifications](https://github.com/derekgreer/nunit.specifications) by [derekgreer](https://github.com/derekgreer).
This implemention adds a Database Context Specification. Subject Under Test (sut) are instanciated by Autofac IOC.
ContextSpecification coming soon...

## Setup

* Register all necessary services in Autofac
```C#
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
			builder.RegisterType(typeOf(someRepo))
						.AsSelf()
		}
	}
}
```
* Get the database ready
```C#
/// <summary>
/// Get the database ready (create, run migrations etc)
/// </summary>
/// <param name="scope"></param>
/// <returns></returns>
private async Task GetDbReady(ILifetimeScope scope)
{
   var migratior = scope.Resolve<IMigrator>();
   migrator.RunAllMigrations();
}
```

## Write some tests
```C#
public class RepositorySpecs
{
	[Integration]
	public class when_saving_item_into_database : DatabaseContextSpecification<SomeRepo>
	{
		private static Item item;
		private static ICollection<Item> results;

		private Establish context = () =>
		{
			item = new Item{Description= "some description"};
		};

		private Because of = async (sut) =>
		{
			await sut.Upsert(item);
			results = await sut.AllAsync();
		};

		private It should_have_inserted_the_item = () => Assert.That(results.Count, Is.EqualTo(1));
	}
}
```
