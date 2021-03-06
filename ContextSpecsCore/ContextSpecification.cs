using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ContextSpecsCore
{
   
    [TestFixture]
    public abstract class ContextSpecification<TSystemUnderTest> where TSystemUnderTest : class
    {
	    private const BindingFlags SpecPropertyBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

		protected TSystemUnderTest sut;

		public delegate Task Because(TSystemUnderTest sut);

        public delegate void Cleanup();

        public delegate void Establish();

        public delegate void It();

        protected Exception Exception;

	    

        public IEnumerator GetEnumerator()
        {
            return GetObservations().GetEnumerator();
        }

        [OneTimeSetUp]
        public virtual async Task TestFixtureSetUp()
        {
            InitializeContext();
	        CreateSut();
            InvokeEstablish();
            await InvokeBecause();
        }

	    protected virtual void CreateSut()
	    {
			throw new NotImplementedException();
		}

	    protected void InitializeContext()
        {
            var t = GetType();
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            InvokeCleanup();
        }

        protected void InvokeEstablish()
        {
            var types = new Stack<Type>();
            var type = GetType();

            do
            {
                types.Push(type);
                type = type.GetTypeInfo().BaseType;
            } while (type != typeof(ContextSpecification<TSystemUnderTest>));

            foreach (var t in types)
            {
                var fieldInfos = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic |
                                             BindingFlags.FlattenHierarchy);

                FieldInfo establishFieldInfo = null;
                foreach (var info in fieldInfos)
                    if (info.FieldType.Name.Equals("Establish"))
                        establishFieldInfo = info;

                Delegate establish = null;

                if (establishFieldInfo != null) establish = establishFieldInfo.GetValue(this) as Delegate;
                if (establish != null) Exception = Catch.Exception(() => establish.DynamicInvoke(null));
            }
        }

        protected async Task InvokeBecause()
        {
            if (Exception != null)
                return;

            var t = GetType();

            var fieldInfos = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic |
                                         BindingFlags.FlattenHierarchy);

            FieldInfo becauseFieldInfo = null;
            foreach (var info in fieldInfos)
                if (info.FieldType.Name.Equals("Because"))
                    becauseFieldInfo = info;

            Delegate because = null;

            if (becauseFieldInfo != null) because = becauseFieldInfo.GetValue(this) as Delegate;
            if (because != null) Exception = await Catch.Exception(async () => await (Task) because.DynamicInvoke(sut));
        }

        void InvokeCleanup()
        {
            try
            {
                var t = GetType();

                var fieldInfos = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic |
                                             BindingFlags.FlattenHierarchy);

                FieldInfo cleanupFieldInfo = null;
                foreach (var info in fieldInfos)
                    if (info.FieldType.Name.Equals("Cleanup"))
                        cleanupFieldInfo = info;

                Delegate cleanup = null;

                if (cleanupFieldInfo != null) cleanup = cleanupFieldInfo.GetValue(this) as Delegate;
                if (cleanup != null) cleanup.DynamicInvoke(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public IEnumerable GetObservations()
        {
            var t = GetType();

            var categoryName = "Uncategorized";
            var description = string.Empty;

            var categoryAttributes = t.GetTypeInfo().GetCustomAttributes(typeof(CategoryAttribute), true);
            var subjectAttributes = t.GetTypeInfo().GetCustomAttributes(typeof(SubjectAttribute), true);

            if (categoryAttributes.Any())
            {
                var categoryAttribute = (CategoryAttribute) categoryAttributes.First();
                categoryName = categoryAttribute.Name;
            }

            if (subjectAttributes.Any())
            {
                var subjectAttribute = (SubjectAttribute) subjectAttributes.First();
                description = subjectAttribute.Subject;
            }

            var fieldInfos = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic |
                                         BindingFlags.FlattenHierarchy);
            var itFieldInfos = new List<FieldInfo>();

            foreach (var info in fieldInfos)
                if (info.FieldType.Name.Equals("It"))
                    itFieldInfos.Add(info);

            foreach (var it in itFieldInfos)
            {
                var data = new TestCaseData(it.GetValue(this));
                data.SetDescription(description);
                data.SetName(it.Name.Replace("_", " "));
                data.SetCategory(categoryName);
                yield return data;
            }
        }



		[Test]
        [SpecificationSource("GetObservations")]
        public void Observations(It observation)
        {
            if (Exception != null)
                throw Exception;

            observation();
        }
    }
}