using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            await LockFile(filePath);

            try
            {
                await File.WriteAllTextAsync(
                    filePath,
                    JsonSerializer.Serialize(model, options),
                    Encoding.Default,
                    token);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await UnlockFile(filePath);
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

            await LockFile(filePath);

            try
            {
                return JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(filePath, Encoding.Default, token), options);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await UnlockFile(filePath);
            }
        }

        private JsonSerializerOptions GetDefaultOptions()
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
                    await Task.Delay(new TimeSpan(0, 0, 2));
                }
            }
        }

        private async Task UnlockFile(string filePath)
        {
            if (_lockStream == null)
            {
                return;
            }

            await _lockStream.DisposeAsync();
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
