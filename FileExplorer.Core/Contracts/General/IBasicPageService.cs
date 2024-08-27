#nullable enable
using System;

namespace FileExplorer.Core.Contracts.General
{
    /// <summary>
    /// Provides page types for a frame element for navigation
    /// </summary>
    public interface IBasicPageService<in TParameter>
    {
        /// <summary>
        /// Returns page type depending on provided key
        /// </summary>
        public Type GetPage(TParameter? key);
    }
}
