using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.OpenGL;

namespace AvaVulkan
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new X11PlatformOptions
                {
                    EnableMultiTouch = true,
                    UseDBusMenu = true,
                    EnableIme = true,
                    UseEGL = false,
                    GlProfiles = new List<GlVersion>
                    {
                        new GlVersion(GlProfileType.OpenGL, 4 , 2)
                    }
                })
                .With(new Win32PlatformOptions
                {
                    EnableMultitouch = true,
                    UseWgl = true,
                    WglProfiles = new List<GlVersion> 
                    { 
                        new GlVersion(GlProfileType.OpenGL, 4 , 2)
                    },
                    UseWindowsUIComposition = true
                })
                .UseSkia()
                .LogToTrace();
        
        

        internal static byte[] LoadEmbeddedResourceBytes(string path)
        {
            using (var s = typeof(Program).Assembly.GetManifestResourceStream(path))
            {
                using (var ms = new MemoryStream())
                {
                    s.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
