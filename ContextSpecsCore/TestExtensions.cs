using NUnit.Framework;

namespace ContextSpecsCore
{
	public static class TestExtensions
	{
		public static void ShouldBeEqualTo<T>(this T subject, T item)
		{
			Assert.Equals(subject, item);
		}
	}
}
