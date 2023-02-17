using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Serialization
{
    public interface IJsonSerializationService
    {
        Task<T?> DeserializeFromFile<T>(string filePath, JsonSerializerOptions? options = null, CancellationToken token = default);
        Task SerializeToFile<T>(string filePath, T model, JsonSerializerOptions? options = null, CancellationToken token = default);
    }
}