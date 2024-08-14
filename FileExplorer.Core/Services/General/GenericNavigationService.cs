#nullable enable
using FileExplorer.Core.Contracts.General;

namespace FileExplorer.Core.Services.General
{
    /// <summary>
    /// Generic base class that provides necessary logic for a navigation service 
    /// </summary>
    /// <typeparam name="TParameter"> Type of navigation parameter </typeparam>
    public abstract class GenericNavigationService<TParameter> : BaseNavigationService, IBasicNavigationService<TParameter>
    {
        /// <summary>
        /// Page service to identify what type of page to use with provided key
        /// </summary>
        protected readonly IBasicPageService<TParameter> pageService;

        protected GenericNavigationService(IBasicPageService<TParameter> pageService)
        {
            this.pageService = pageService;
        }

        /// <inheritdoc />
        public virtual void NavigateTo(TParameter? tag = default, object? parameter = null)
        {
            var pageType = pageService.GetPage(tag);
            UseNavigationFrame(pageType, parameter);
        }
    }
}
