namespace ADM87.GameUtilities.Linq
{
    public static class Enumerable
    {
        /// <summary>
        /// Performs the specified operation on each element of the collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to iterate over.</param>
        /// <param name="operation">The operation to perform on each element.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> operation)
        {
            foreach (T element in collection)
                operation(element);
        }
    }
}
