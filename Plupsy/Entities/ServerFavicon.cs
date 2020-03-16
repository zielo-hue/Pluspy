using System;
using System.IO;

namespace Pluspy.Entities
{
    public readonly struct ServerFavicon
    {
        public string FaviconString { get; }

        private ServerFavicon(string base64String)
        {
            if (!base64String.StartsWith("data:image/png;base64,"))
                FaviconString = "data:image/png;base64," + base64String;

            FaviconString = base64String;
        }

        public override string ToString()
            => FaviconString;
        public static implicit operator string(ServerFavicon favicon)
            => favicon.FaviconString;
        public static ServerFavicon FromBase64String(string base64String)
            => new ServerFavicon(base64String);
        public static ServerFavicon FromFile(string fileUrl)
            => new ServerFavicon(Convert.ToBase64String(File.ReadAllBytes(fileUrl)));
    }
}
