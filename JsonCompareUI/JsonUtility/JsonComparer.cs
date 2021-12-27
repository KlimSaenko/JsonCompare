using System.Text.Json;

namespace TestTask.JsonUtility
{
    internal sealed class JsonComparer
    {
        internal static void CompareJson(IJsonField[] jsonCollection1, IJsonField[] jsonCollection2)
        {
            if (jsonCollection1 == null || jsonCollection2 == null) return;

            for (var i = 0; i < jsonCollection1.Length; i++)
            {
                var item1 = jsonCollection1[i];
                
                foreach (var item2 in jsonCollection2)
                {
                    if (item1.JsonProperty.NameEquals(item2.JsonProperty.Name))
                    {
                        var valueKind1 = item1.JsonProperty.Value.ValueKind;
                        var valueKind2 = item2.JsonProperty.Value.ValueKind;

                        if (valueKind1 == valueKind2 || valueKind1 == JsonValueKind.False && valueKind2 == JsonValueKind.True
                            || valueKind2 == JsonValueKind.False && valueKind1 == JsonValueKind.True)
                        {
                            jsonCollection1[i].SameField = true;
                            item2.SameField = true;

                            if (item1.ChildFields != null)
                                CompareJson(item1.ChildFields, item2.ChildFields);

                            break;
                        }
                    }
                }
                
            }
        }
    }
}
