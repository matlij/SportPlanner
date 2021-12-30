using Xamarin.Essentials;

namespace SportPlanner.Models.Constants
{
    internal static class UriConstants
    {
        //public static string BaseUri
        //{
        //    get
        //    {
        //        return DeviceInfo.Platform == DevicePlatform.Android
        //            ? "http://10.0.2.2:7071"
        //            : "http://localhost:7071";
        //    }
        //}

        public const string BaseUri = "https://sportplannerapi.azurewebsites.net";

        public const string EventUri = "api/event";

        public const string UserUri = "api/user";

        public const string Apikey = "UQzBb8e8VrUeBrMpshlzdVXlLYfaZaaePOfhIMKH5srGk6J1HkW9CQ==";
    }
}
