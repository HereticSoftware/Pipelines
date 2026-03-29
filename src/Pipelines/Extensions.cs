namespace Pipelines;

#pragma warning disable IDE0270 // Null check can be simplified

internal static class Extensions
{
    /// <param name="serviceProvider">The service provider instance. Cannot be null.</param>
    extension<T>(IServiceProvider serviceProvider) where T : notnull
    {
        /// <summary>
        /// Retrieves a required service of the specified type from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>The resolved service instance of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a service of type <typeparamref name="T"/> is not registered in the service provider.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetRequiredService()
        {
            var serviceType = typeof(T);
            var service = serviceProvider.GetService(serviceType);
            if (service is null)
            {
                throw new InvalidOperationException($"Required service of type `{typeof(T).Name}` not registered.");
            }

            return Unsafe.As<object?, T>(ref service);
        }
    }
}
