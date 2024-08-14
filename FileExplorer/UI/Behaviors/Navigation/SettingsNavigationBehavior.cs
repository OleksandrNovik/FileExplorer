using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;

namespace FileExplorer.UI.Behaviors.Navigation
{
    public class SettingsNavigationBehavior : BaseNavigationBehavior<string>
    {
        public SettingsNavigationBehavior() : base(
            App.GetService<ISettingsNavigationService>(),
            App.GetService<IBasicPageService<string>>())
        { }
    }
}
