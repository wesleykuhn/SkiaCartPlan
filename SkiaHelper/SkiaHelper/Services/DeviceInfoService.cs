using Xamarin.Essentials;

namespace SkiaHelper.Services;

public static class DeviceInfoService
{
    private static DisplayInfo DisplayInfo => DeviceDisplay.MainDisplayInfo;
    public static double XamarinSizeToScreenSize(double xSize) =>
        xSize * DisplayInfo.Density;

    public static double ScreenWidthToXamarinWidth() =>
            DisplayInfo.Width / DisplayInfo.Density;

    public static double ScreenHeightToXamarinHeight() =>
        DisplayInfo.Height / DisplayInfo.Density;

    public static string GetDeviceInformation() =>
        $"{DeviceInfo.Platform} {DeviceInfo.DeviceType} | " +
        $"Language:{DeviceInfo.Idiom} | " +
        $"Version:{DeviceInfo.VersionString} | " +
        $"{DeviceInfo.Manufacturer} {DeviceInfo.Model} | " +
        $"{DeviceInfo.Name}";
}
