using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public sealed class Text
    {
        public static Text Default { get; } = new Text  
        {
            Color = "white",
            Subcomponents = new List<Text>()
            {
                new Text
                {
                    Content = "Default Server",
                    IsBold = true,
                    Color = "aqua"
                },
                new Text
                {
                    Content = "Server",
                    Color = "white"
                }
            }
        };

        [JsonPropertyName("bold")]
        public bool IsBold { get; set; }
        [JsonPropertyName("italic")]
        public bool IsItalic { get; set; }
        [JsonPropertyName("obfuscated")]
        public bool IsObfuscated { get; set; }
        [JsonPropertyName("strikethrough")]
        public bool IsStrikethrough { get; set; }
        [JsonPropertyName("underlined")]
        public bool IsUnderlined { get; set; }
        [JsonPropertyName("text")]
        public string? Content { get; set; }
        [JsonPropertyName("color")]
        public string? Color { get; set; }
        [JsonPropertyName("extra")]
        public List<Text>? Subcomponents { get; set; }

        public Text AddSubcomponent(Text chat)
        {
            if (Subcomponents is null)
                Subcomponents = new List<Text>(1);

            Subcomponents.Add(chat);
            return this;
        }
    }
}
