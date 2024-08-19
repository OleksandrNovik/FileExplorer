#nullable enable
using Models.Contracts.Additional;
using System.Threading.Tasks;

namespace Models.Contracts.Storage
{
    //TODO: Maybe to provide bitmap image create class provider (so DirectoryItemWrappers and other can be created outside main thread with no problem)
    public interface IThumbnailProvider
    {
        public IThumbnail Thumbnail { get; }
        public Task UpdateThumbnailAsync(int size);
        public void UpdateThumbnail(int size);
    }
}
