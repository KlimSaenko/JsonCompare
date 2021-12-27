using System.Text.Json;

namespace TestTask.JsonUtility
{
    public interface IJsonField
    {
        bool SameField { get; set; }

        string ValueString { get; }

        IJsonField[] ChildFields { get; set; }

        JsonProperty JsonProperty { get; set; }
    }
}
