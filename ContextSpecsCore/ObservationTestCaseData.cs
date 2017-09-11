namespace ContextSpecsCore
{
    public class ObservationTestCaseData<T> where T : class
    {
        public ContextSpecification<T>.It Observation { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}