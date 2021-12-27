using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestTask.JsonUtility
{
    internal sealed class JsonConverter
    {
        /// <summary>
        /// Converts JSON format files to JSON fields in type of IJsonField, that provides information about JSON properties 
        /// </summary>
        /// <returns>Array of JSON fields in type of IJsonField for current JSON files</returns>
        internal static async Task<IJsonField[][]> GetJsonAsFields(string[] paths)
        {
            var jsonFields = new IJsonField[paths.Length][];

            for (var i = 0; i < paths.Length; i++)
            {
                jsonFields[i] = WriteJsonToFields(await ReadJsonFromFile(paths[i]));
            }

            return jsonFields;
        }

        private static async Task<IEnumerable<JsonProperty>> ReadJsonFromFile(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var jsonDocument = await JsonDocument.ParseAsync(fs))
                {
                    return jsonDocument.RootElement.Clone().EnumerateObject();
                }
            }
        }

        private static IJsonField[] WriteJsonToFields(IEnumerable<JsonProperty> jsonCollection)
        {
            var array = new List<JsonProperty>(jsonCollection);
            var jsonFields = new IJsonField[array.Count];

            for (var i = 0; i < array.Count; i++)
            {
                var item = array[i];

                if (item.Value.ValueKind == JsonValueKind.Object)
                {
                    using (var enumerator = item.Value.EnumerateObject())
                    {
                        jsonFields[i] = new JsonField(item, WriteJsonToFields(enumerator));
                    }
                }
                else jsonFields[i] = new JsonField(item);
            }
            
            return jsonFields;
        }
    }
}
