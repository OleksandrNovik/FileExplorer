namespace Models.Contracts
{
    public interface ISettingsModel
    {
        /// <summary>
        /// Saves data that is held in model to a local settings
        /// </summary>
        public void SaveSettings();
    }
}
