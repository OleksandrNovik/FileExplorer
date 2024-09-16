namespace FileExplorer.Core.Contracts.Storage
{
    public interface INameValidator
    {
        /// <summary>
        /// String that contains illegal characters for a name
        /// </summary>
        public string IlleagalCharacters { get; }

        /// <summary>
        /// Checks the name for illegal characters or illegal naming overall
        /// </summary>
        /// <param name="name"> Name to validate </param>
        /// <returns> True if name is invalid, false if name is good to go </returns>
        public bool IsInvalid(string name);
    }
}
