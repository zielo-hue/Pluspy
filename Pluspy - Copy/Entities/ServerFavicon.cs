using System;
using System.IO;

namespace Pluspy.Entities
{
    public readonly struct Favicon
    {
        public string? FaviconString { get; }

        private Favicon(string base64String)
        {
            if (!base64String.StartsWith("data:image/png;base64,"))
                FaviconString = "data:image/png;base64," + base64String;

            FaviconString = base64String;
        }

        public override string? ToString()
            => FaviconString;
        public static implicit operator string?(Favicon favicon)
            => favicon.FaviconString;
        public static Favicon FromBase64String(string base64String)
            => new Favicon(base64String);
        public static Favicon FromFile(string fileUrl)
            => new Favicon(Convert.ToBase64String(File.ReadAllBytes(fileUrl)));
    }
}
