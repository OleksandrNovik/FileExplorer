#nullable enable
using FileExplorer.Core.Contracts.General;
using Models.StorageWrappers;
using Models.TabRelated;

namespace FileExplorer.Core.Contracts
{
    public interface IPageTypesService : IBasicPageService<string>
    {
        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir);
    }
}
