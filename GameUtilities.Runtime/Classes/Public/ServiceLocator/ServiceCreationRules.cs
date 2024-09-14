namespace ADM87.GameUtilities.Services
{
    public enum EServiceLifeTime
    {
        /// <summary> The service is created every time it is requested. </summary>
        Transient,
        /// <summary> The service is created once and shared across all requests. </summary>
        Singleton
    }
}
