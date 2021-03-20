using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using PInvoke;

namespace AvaVulkan
{
    public class VulkanSurface : NativeControlHost
    {
        public event EventHandler<IntPtr> WindowCreated;

        public bool IsSecond { get; set; }

        private IntPtr win32Window;

        public VulkanSurface()
        {
        }

        IPlatformHandle CreateLinux(IPlatformHandle parent)
        {
            return null;
        }

        void DestroyLinux(IPlatformHandle handle)
        {
            base.DestroyNativeControlCore(handle);
        }

        IPlatformHandle CreateWin32(IPlatformHandle parent)
        {
            win32Window = parent.Handle;

            WindowCreated?.Invoke(this, win32Window);

            return new PlatformHandle(win32Window, "HWND");
        }

        public unsafe SurfaceKHR CreateWin32Surface(Device device, Instance instance, Vk vk)
        {
            vk.TryGetInstanceExtension(instance, out KhrWin32Surface khrSurface);

            var createInfo = new Win32SurfaceCreateInfoKHR()
            {
                SType = StructureType.Win32SurfaceCreateInfoKhr,
                Hinstance = 0,
                Hwnd = win32Window
            };

            khrSurface.CreateWin32Surface(instance, createInfo, null, out var surface);

            return surface;
        }

        void DestroyWin32(IPlatformHandle handle)
        {
            WinApi.DestroyWindow(handle.Handle);
        }

        IPlatformHandle CreateOSX(IPlatformHandle parent)
        {
            return null;
        }

        void DestroyOSX(IPlatformHandle handle)
        {
        }

        protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return CreateLinux(parent);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return CreateWin32(parent);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return CreateOSX(parent);
            return base.CreateNativeControlCore(parent);
        }

        protected override void DestroyNativeControlCore(IPlatformHandle control)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                DestroyLinux(control);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                DestroyWin32(control);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                DestroyOSX(control);
            else
                base.DestroyNativeControlCore(control);
        }
    }
}
