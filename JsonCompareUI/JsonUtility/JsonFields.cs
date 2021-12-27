using System.Text.Json;

namespace TestTask.JsonUtility
{
    public struct JsonField : IJsonField
    {
        public bool SameField { get; set; }

        public string ValueString
        {
            get
            {
                switch (JsonProperty.Value.ValueKind)
                {
                    case JsonValueKind.String:
                        return JsonProperty.Value.GetString();

                    case JsonValueKind.Number:
                        return JsonProperty.Value.GetDouble().ToString();

                    case JsonValueKind.True:
                        return true.ToString();

                    case JsonValueKind.False:
                        return false.ToString();

                    case JsonValueKind.Object:
                        return string.Empty;

                    case JsonValueKind.Array:
                        return ArrayToString();

                    default:
                        return "";
                }
            }
        }

        public IJsonField[] ChildFields { get; set; }

        public JsonProperty JsonProperty { get; set; }

        internal JsonField(JsonProperty jsonProperty, IJsonField[] childFields = null, bool sameField = false)
        {
            JsonProperty = jsonProperty;

            ChildFields = childFields;
            SameField = sameField;
        }

        private string ArrayToString()
        {
            var type = JsonValueKind.Undefined;

            foreach (var value in JsonProperty.Value.EnumerateArray())
            {
                type = value.ValueKind;
                break;
            }
            
            return $"[Array of {type} type]";
        }
    }
}
