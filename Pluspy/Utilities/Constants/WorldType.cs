namespace Pluspy.Constants
{
    public sealed class WorldType
    {
        public static readonly WorldType Default = "default";
        public static readonly WorldType Flat = "flat";
        public static readonly WorldType LargeBiomes = "largeBiomes";
        public static readonly WorldType Amplified = "amplified";
        public static readonly WorldType Default_1_1 = "default_1_1";

        private readonly string _type;

        private WorldType(string type)
        {
            _type = type;
        }

        public override string ToString()
            => _type;

        public static implicit operator string(WorldType worldType)
            => worldType.ToString();

        public static implicit operator WorldType(string input)
            => new WorldType(input);
    }
}
