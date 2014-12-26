using Windows.ApplicationModel.Resources;

namespace wallabag.Common
{
    public static class Helpers
    {
        public static string LocalizedString(string resourceName)
        {
            return ResourceLoader.GetForCurrentView().GetString(resourceName);
        }
    }
}
