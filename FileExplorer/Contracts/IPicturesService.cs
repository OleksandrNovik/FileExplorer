#nullable enable
using FileExplorer.Models;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace FileExplorer.Contracts
{
    public interface IPicturesService
    {
        public IRandomAccessStream? TryGetCachedThumbnail(string key);
        public Task<IRandomAccessStream> GetThumbnailForItem(DirectoryItemWrapper item);

    }
}
