namespace Me.One.Core.DependencyInjection
{
    public static class DependencyResolver
    {
        public static IResolveDependencies Instance { get; private set; }

        public static void SetResolver(IResolveDependencies resolver)
        {
            Instance = resolver;
        }
    }
}