#nullable enable
using FileExplorer.Core.Contracts.Settings;
using Models.StorageWrappers;
using Models.TabRelated;

namespace FileExplorer.Core.Contracts
{
    public interface IPageTypesService : IBasicPageService
    {
        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir);
    }
}
