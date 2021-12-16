namespace Me.One.Core.DependencyInjection
{
    public class DependencyParameter
    {
        public DependencyParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }
    }
}