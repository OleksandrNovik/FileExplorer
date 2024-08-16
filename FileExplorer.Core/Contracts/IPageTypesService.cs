#nullable enable
using FileExplorer.Core.Contracts.General;
using Models.Storage.Windows;
using Models.TabRelated;

namespace FileExplorer.Core.Contracts
{
    public interface IPageTypesService : IBasicPageService<string>
    {
        public TabModel CreateTabFromDirectory(DirectoryWrapper? directory);
    }
}
