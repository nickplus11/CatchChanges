using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatchChangesREST
{
    public static class HttpHelper
    {
        public static async Task<T> GetModel<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}