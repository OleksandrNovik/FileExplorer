namespace Models.Contracts.Storage
{
    /// <summary>
    /// Contract that allows to launch directory item
    /// </summary>
    public interface ILaunchable
    {
        /// <summary>
        /// Launches item in default application
        /// </summary>
        public void Launch();
    }
}
