using Helpers.General;
using Microsoft.UI.Xaml.Media.Imaging;
using Models.StorageWrappers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace Models
{
    public class ObservableWrappersCollection : ObservableCollection<DirectoryItemWrapper>
    {
        public async Task AddWrapperItemsAsync(ICollection<DirectoryItemWrapper> items)
        {
            this.AddRange(items);

            await UpdateThumbnailsAsync(items);
        }
        public async Task UpdateThumbnailsAsync(ICollection<DirectoryItemWrapper> items)
        {
            foreach (var item in items)
            {
                //TODO: fix thumbnails
                if ((item.Attributes & FileAttributes.Hidden) == 0)
                {
                    item.Thumbnail = new BitmapImage();
                    await item.UpdateThumbnailAsync();
                }
            }
        }
    }
}
