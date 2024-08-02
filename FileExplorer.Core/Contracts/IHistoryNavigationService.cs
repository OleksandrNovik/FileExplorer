﻿using Models;

namespace FileExplorer.Core.Contracts
{
    public interface IHistoryNavigationService
    {
        public TabNavigationHistoryModel History { get; set; }
        public DirectoryNavigationInfo CurrentDirectory { get; set; }
        public bool CanGoForward { get; }
        public bool CanGoBack { get; }

        /// <summary>
        /// Should be called when DirectoryPageViewModel navigates forward through directory
        /// Method saves previous location, so user can go back
        /// </summary>
        /// <param name="location"> New location we have navigated to </param>
        public void GoForward(DirectoryNavigationInfo location);

        /// <summary>
        /// Should be called when DirectoryPageViewModel navigates up directory
        /// Method sets new location as current directory and saves previous location, so we can always go forward to it
        /// </summary>
        /// <param name="location"></param>
        public void GoBack(DirectoryNavigationInfo location);

        /// <summary>
        /// Sets forward directory as current directory and saves previous location to be back
        /// </summary>
        public void GoForward();

        /// <summary>
        /// Sets back directory as current directory and saves previous location to be forward
        /// </summary>
        public void GoBack();
    }
}