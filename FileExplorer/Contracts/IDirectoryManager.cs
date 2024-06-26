namespace FileExplorer.Contracts
{
    public interface IDirectoryManager
    {
        public bool TryCreateFile(string dirName, string fileName);
    }
}
