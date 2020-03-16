using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public sealed class Chat
    {
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
        public string? Text { get; set; }
        [JsonPropertyName("color")]
        public string? Color { get; set; }
        [JsonPropertyName("extra")]
        public List<Chat>? Subcomponents { get; set; }

        public Chat AddSubcomponent(Chat chat)
        {
            if (Subcomponents is null)
                Subcomponents = new List<Chat>(1);

            Subcomponents.Add(chat);
            return this;
        }
    }
}
