using System;

namespace SportPlanner.Models.Constants
{
    public static class UserConstants
    {
        public const string UserName = "Matte";
        public static Guid UserId => new Guid("0cafdfe9-614f-495b-b672-61508baa3dbc");
    }

    public static class IconConstants
    {
        public const string OkSymbol = "icon_oksymbol.png";
        public const string NotOkSymbol = "icon_notoksymbol.png";
        public const string NoReplySymbol = "icon_noreply.png";
    }

    public static class FileNameConstants
    {
        public const string UserInfoJson = "userInfo.json";
    }
}
