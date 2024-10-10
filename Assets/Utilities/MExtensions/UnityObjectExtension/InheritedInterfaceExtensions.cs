public static class UnityObjectInheritedInterfaceExtensions
{
    public static bool IsAlive<T>(this T Interface) where T: class
    {
        if (Interface is UnityEngine.Object obj)
        {
            return obj != null;
        }
        else
        {
            return Interface != null;
        }
    } 
}