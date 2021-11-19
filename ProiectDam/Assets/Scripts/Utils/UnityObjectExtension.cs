namespace Utilities
{
    public static class UnityObjectExtension
    {
        public static bool IsNull(this UnityEngine.Object @object)
            => ReferenceEquals(@object, null);

        public static bool IsNotNull(this UnityEngine.Object @object)
                => !ReferenceEquals(@object, null);
    }
}
