using Xamarin.Essentials;

namespace SportPlanner.Models.Constants
{
    internal static class UriConstants
    {
        //public const string BaseUri = "https://sp-wa-api.azurewebsites.net";
        public static string BaseUri
        {
            get
            {
                return DeviceInfo.Platform == DevicePlatform.Android
                    ? "http://10.0.2.2:7071"
                    : "http://localhost:7071";
            }
        }

        public const string EventUri = "api/event";

        public const string UserUri = "api/user";
    }
}
