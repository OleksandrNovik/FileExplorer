#nullable enable
using System;

namespace FileExplorer.Core.Contracts.General
{
    /// <summary>
    /// Provides page types for a frame element for a navigation in settings modal window
    /// </summary>
    public interface IBasicPageService<in TParameter>
    {
        /// <summary>
        /// If key is provided returns corresponding page. In case of an error or null parameter returns default page 
        /// </summary>
        public Type GetPage(TParameter? key);
    }
}
