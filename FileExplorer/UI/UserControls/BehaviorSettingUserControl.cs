using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.UserControls
{
    public abstract class BehaviorSettingUserControl : UserControl
    {
        protected virtual void SetBehaviors(DependencyObject destination, BehaviorCollection source)
        {
            var behaviors = Interaction.GetBehaviors(destination);

            if (behaviors is not null)
            {
                foreach (var dependencyObj in source)
                {
                    if (dependencyObj is Behavior behavior)
                    {
                        behavior.Attach(destination);
                        behaviors.Add(dependencyObj);
                    }
                }
            }
        }
    }
}
