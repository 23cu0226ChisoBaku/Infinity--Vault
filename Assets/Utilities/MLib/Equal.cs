namespace MLibrary
{

    public static partial class MLib
    {
        internal static bool IsEqual<T>(T a, T b)
        {
            switch(a)
            {
                case System.Byte:
                case System.Int16:
                case System.Int32:
                case System.Int64:
                {
                    return a.Equals(b);
                }
                case System.Single:
                case System.Double:
                {
                    return IsRealEqualImpl(a,b);   
                }
                default:
                {
                    throw new System.Exception("Unsupported type");
                }
            }
        }

        private static bool IsRealEqualImpl<T>(T a, T b)
        {
            if (typeof(T) == typeof(float))
            {
                const float EPSILON = 1E-05f;
                return System.Math.Abs((float)(object)a - (float)(object)b) < EPSILON ;
            }
            else if (typeof(T) == typeof(double))
            {
                const double EPSILON = 1E-10f;
                return System.Math.Abs((double)(object)a - (double)(object)b) < EPSILON;
            }
            else
            {
                throw new System.Exception("Why");
            }
        }


    }
}
