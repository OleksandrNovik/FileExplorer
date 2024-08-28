#nullable enable
using Models.Contracts.Additional;
using System.Threading.Tasks;

namespace Models.Contracts.Storage
{
    public interface IThumbnailProvider
    {
        public IThumbnail Thumbnail { get; }
        public Task UpdateThumbnailAsync(int size);
        public void UpdateThumbnail(int size);
    }
}
