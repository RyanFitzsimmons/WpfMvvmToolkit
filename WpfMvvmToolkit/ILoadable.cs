using System.Threading.Tasks;

namespace WpfMvvmToolkit
{
    public interface ILoadable
    {
        Task Load();
        void Unload();
    }
}
