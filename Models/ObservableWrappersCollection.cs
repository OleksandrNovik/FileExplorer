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
        public async Task AddEnumeration(IEnumerable<DirectoryItemWrapper> items)
        {
            foreach (var item in items)
            {
                Add(item);

                if ((item.Attributes & FileAttributes.Hidden) == 0)
                {
                    item.Thumbnail = new BitmapImage();
                    await item.UpdateThumbnailAsync();
                }
            }
        }
    }
}
