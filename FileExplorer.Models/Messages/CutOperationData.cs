using FileExplorer.Models.Contracts.Storage.Directory;
using System.Collections.Generic;

namespace FileExplorer.Models.Messages
{
    public sealed class CutOperationData
    {
        public IDirectory CutDirectory { get; }
        public ICollection<string> Paths { get; }

        public CutOperationData(IDirectory cutDirectory, ICollection<string> paths)
        {
            CutDirectory = cutDirectory;
            Paths = paths;
        }
    }
}
