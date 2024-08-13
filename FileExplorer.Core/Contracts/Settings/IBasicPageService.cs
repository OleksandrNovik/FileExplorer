#nullable enable
using System;

namespace FileExplorer.Core.Contracts.Settings
{
    /// <summary>
    /// Provides page types for a frame element for a navigation in settings modal window
    /// </summary>
    public interface IBasicPageService
    {
        /// <summary>
        /// If key is provided returns corresponding page. In case of an error or null parameter returns default page 
        /// </summary>
        public Type GetPage(string? key);
    }
}
