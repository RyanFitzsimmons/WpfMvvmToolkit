using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Serialization
{
    public class JsonSerializationService : IJsonSerializationService
    {
        public async Task SerializeToFile<T>(string filePath, T model, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            if (options == null)
            {
                options = GetDefaultOptions();
            }

            await LockFile(filePath).ConfigureAwait(false);

            try
            {
                await File.WriteAllTextAsync(
                    filePath,
                    JsonSerializer.Serialize(model, options),
                    Encoding.Default,
                    token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await UnlockFile(filePath).ConfigureAwait(false);
            }
        }

        public async Task<T?> DeserializeFromFile<T>(string filePath, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            if (options == null)
            {
                options = GetDefaultOptions();
            }

            await LockFile(filePath).ConfigureAwait(false);

            try
            {
                return JsonSerializer.Deserialize<T>(
                    await File.ReadAllTextAsync(filePath, Encoding.Default, token).ConfigureAwait(false),
                    options);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await UnlockFile(filePath).ConfigureAwait(false);
            }
        }

        public JsonSerializerOptions GetDefaultOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
            };
        }

        private FileStream? _lockStream;

        private async Task LockFile(string filePath)
        {
            var file = new FileInfo(filePath + ".lock");
            _lockStream = null;
            while (_lockStream == null)
            {
                try
                {
                    _lockStream = file.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                }
                catch (IOException)
                {
                    await Task.Delay(new TimeSpan(0, 0, 2)).ConfigureAwait(false);
                }
            }
        }

        private async Task UnlockFile(string filePath)
        {
            if (_lockStream == null)
            {
                return;
            }

            await _lockStream.DisposeAsync().ConfigureAwait(false);
            DeleteLockFile(filePath);
        }

        private void DeleteLockFile(string filePath)
        {
            try
            {
                File.Delete(filePath + ".lock");
            }
            catch
            {

            }
        }
    }
}
