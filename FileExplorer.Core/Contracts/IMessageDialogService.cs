using System.Threading.Tasks;

namespace FileExplorer.Core.Contracts
{
    public interface IMessageDialogService
    {
        public Task ShowDialogAsync(string title, string message);
    }
}
