using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway
{
    public class GatewayConnectionProperties
    {
        public static GatewayConnectionProperties FromLibraryName(string libraryName)
            => new(
                browser:            libraryName,
                device:             libraryName,
                operatingSystem:    GetCurrentOperatingSystem());

        public GatewayConnectionProperties(
            string browser,
            string device,
            string operatingSystem)
        {
            Browser         = browser;
            Device          = device;
            OperatingSystem = operatingSystem;
        }

        [JsonPropertyName("$browser")]
        public string Browser { get; }

        [JsonPropertyName("$device")]
        public string Device { get; }

        [JsonPropertyName("$os")]
        public string OperatingSystem { get; }

        private static string GetCurrentOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "linux";

            #if NET5_0
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return "freebsd";
            #endif

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "osx";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "windows";

            return "unknown";
        }
    }
}
