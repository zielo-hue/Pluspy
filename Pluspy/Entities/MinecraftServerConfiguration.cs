using Pluspy.Attributes;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Pluspy.Entities
{
#nullable disable
    public sealed class MinecraftServerConfiguration
    {
        [Property("allow-flight")]
        public bool AllowFlight { get; }

        [Property("allow-nether")]
        public bool AllowNether { get; }

        [Property("broadcast-console-to-ops")]
        public bool BroadcastConsoleToOps { get; }

        [Property("broadcast-rcon-to-ops")]
        public bool BroadcastRconToOps { get; }

        [Property("enable-command-block")]
        public bool EnableCommandBlock { get; }

        [Property("enable-query")]
        public bool EnableQuery { get; }

        [Property("enable-rcon")]
        public bool EnableRcon { get; }

        [Property("enforce-whitelist")]
        public bool EnforceWhitelist { get; }

        [Property("force-gamemode")]
        public bool ForceGamemode { get; }

        [Property("generate-structures")]
        public bool GenerateStructures { get; }

        [Property("hardcore")]
        public bool Hardcore { get; }

        [Property("online-mode")]
        public bool OnlineMode { get; }

        [Property("prevent-proxy-connections")]
        public bool PreventProxyConnections { get; }

        [Property("pvp")]
        public bool Pvp { get; }

        [Property("snooper-enabled")]
        public bool SnooperEnabled { get; }

        [Property("spawn-animals")]
        public bool SpawnAnimals { get; }

        [Property("spawn-monsters")]
        public bool SpawnMonsters { get; }

        [Property("spawn-npcs")]
        public bool SpawnNpcs { get; }

        [Property("use-native-transport")]
        public bool UseNativeTransport { get; }

        [Property("white-list")]
        public bool WhiteList { get; }

        [Property("function-permission-level")]
        public int FunctionPermissionLevel { get; }

        [Property("max-build-height")]
        public int MaxBuildHeight { get; }

        [Property("max-players")]
        public int MaxPlayers { get; }

        [Property("max-world-size")]
        public int MaxWorldSize { get; }

        [Property("network-compression-threshold")]
        public int NetworkCompressionThreshold { get; }

        [Property("op-permission-level")]
        public int OpPermissionLevel { get; }

        [Property("player-idle-timeout")]
        public int PlayerIdleTimeout { get; }

        [Property("spawn-protection")]
        public int SpawnProtection { get; }

        [Property("view-distance")]
        public int ViewDistance { get; }

        [Property("difficulty")]
        public string Difficulty { get; }

        [Property("gamemode")]
        public string Gamemode { get; }

        [Property("generator-settings")]
        public string GeneratorSettings { get; }

        [Property("level-name")]
        public string LevelName { get; }

        [Property("level-seed")]
        public string LevelSeed { get; }

        [Property("level-type")]
        public string LevelType { get; }

        [Property("motd")]
        public string Motd { get; }

        [Property("rcon.password")]
        public string RconPassword { get; }

        [Property("resource-pack")]
        public string ResourcePack { get; }

        [Property("resource-pack-sha1")]
        public string ResourcePackSha1 { get; }

        [Property("server-ip")]
        public string ServerIp { get; }

        [Property("max-tick-time")]
        public ulong MaxTickTime { get; }

        [Property("query.port")]
        public ushort QueryPort { get; }

        [Property("rcon.port")]
        public ushort RconPort { get; }

        [Property("server-port")]
        public ushort ServerPort { get; }
        public bool EnableEncryption { get; internal set; }

        public static MinecraftServerConfiguration FromFile(string path)
        {
            var config = new MinecraftServerConfiguration();
            var dictionary = File.ReadAllLines(path)
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.TrimStart()[0] != '#')
                .Select(x => x.Split('='))
                .ToDictionary(x => x[0].Trim(), x => x[1].Trim());
            var propertiesMetadata = typeof(MinecraftServerConfiguration)
                .GetProperties()
                .Select(x => (x.Name, SettingName: x.GetCustomAttribute<PropertyAttribute>()?.Name))
                .Where(x => x.SettingName is object)
                .ToArray();

            foreach (var (propertyName, name) in propertiesMetadata)
            {
                if (!dictionary.TryGetValue(name, out var value))
                    continue;

                var backingField = typeof(MinecraftServerConfiguration).GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
                var type = backingField.FieldType;

                if (type == typeof(string))
                    backingField.SetValue(config, value);
                else if (type == typeof(int))
                    backingField.SetValue(config, int.Parse(value));
                else if (type == typeof(ushort))
                    backingField.SetValue(config, ushort.Parse(value));
                else if (type == typeof(bool))
                    backingField.SetValue(config, bool.Parse(value));
                else if (type == typeof(ulong))
                    backingField.SetValue(config, ulong.Parse(value));
            }

            return config;
        }
    }
}