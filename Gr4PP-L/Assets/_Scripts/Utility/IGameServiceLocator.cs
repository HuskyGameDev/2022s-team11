using System.Collections.Generic;
namespace Managers
{
    /// <summary>
    /// Service locator for <see cref="IGameService"> services
    /// </summary>
    public interface IGameServiceLocator 
    {
        /// <summary>
        /// Gets the service with the specified type
        /// </summary>
        /// <typeparam name="T">The type of the service to find</typeparam>
        /// <returns>The service, if found</returns>
        T Get<T>() where T : IGameService;

        /// <summary>
        /// Registers a new service in the locator
        /// </summary>
        /// <param name="service">The service instance to register</param>
        /// <typeparam name="T">The type of the service</typeparam>
        void Register<T>(T service) where T : IGameService;

        /// <summary>
        /// Unregisters the service with the given type
        /// </summary>
        /// <typeparam name="T">The type to unregister</typeparam>
        void Unregister<T>() where T : IGameService;
    }
}