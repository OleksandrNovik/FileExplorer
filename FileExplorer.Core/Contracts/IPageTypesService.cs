#nullable enable
using FileExplorer.Core.Contracts.General;
using Models.Enums;

namespace FileExplorer.Core.Contracts
{
    /// <summary>
    /// Basic page service that accepts <see cref="StorageContentType"/> as parameter
    /// </summary>
    public interface IPageTypesService : IBasicPageService<StorageContentType>;
}
