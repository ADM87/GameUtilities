namespace ADM87.GameUtilities.Utilities
{
    public static class Assert
    {
        public static void IsNotNull(object obj, string message = null)
        {
            if (obj == null || obj.Equals(null))
            {
                throw new System.ArgumentNullException(message);
            }
        }

        public static void IsNull(object obj, string message = null)
        {
            if (obj != null && !obj.Equals(null))
            {
                throw new System.ArgumentException(message);
            }
        }

        public static void IsTrue(bool condition, string message = null)
        {
            if (!condition)
            {
                throw new System.ArgumentException(message);
            }
        }

        public static void IsFalse(bool condition, string message = null)
        {
            if (condition)
            {
                throw new System.ArgumentException(message);
            }
        }

        public static void AreEqual<T>(T expected, T actual, string message = null)
        {
            if (!expected.Equals(actual))
            {
                throw new System.ArgumentException(message);
            }
        }

        public static void AreNotEqual<T>(T expected, T actual, string message = null)
        {
            if (expected.Equals(actual))
            {
                throw new System.ArgumentException(message);
            }
        }
    }
}