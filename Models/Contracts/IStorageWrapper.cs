using System.Threading.Tasks;

namespace Models.Contracts
{
    public interface IStorageWrapper
    {
        public void Rename();
        public void Move(string destination);
        public void Copy(string destination);
        public void Delete();
        public Task RecycleAsync();

    }
}
