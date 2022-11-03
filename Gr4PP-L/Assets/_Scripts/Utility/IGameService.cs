namespace Managers
{
    public interface IGameService
    {
        /// <summary>
        /// Returns the name of this service, which is likely just the typename
        /// </summary>
        /// <returns>The name of the service instance</returns>
        string GetName();

        /// <summary>
        /// Initializes the values for this service.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Unregisters all delegates and prepares this class for destruction
        /// </summary>
        void Destroy();
    }
}