using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaVulkan
{
    public class MainWindow : Window
    {
        private bool _isInit;

        public  ContentControl View { get; set; }
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.Activated+= OnActivated;
        }

        private void OnActivated(object? sender, EventArgs e)
        {
            if(_isInit)
            {
                return;
            }    

            _isInit = true;

            Initialize();
        }

        public void Initialize()
        {
            var vulkan = new VulkanSurface();

            vulkan.WindowCreated += Vulkan_WindowCreated;
            View.Content = vulkan;
        }

        private void Vulkan_WindowCreated(object? sender, IntPtr e)
        {
            var app = new HelloTriangleApplication();
            app.Initialize(sender as VulkanSurface);
            new Thread(app.Run).Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            View = this.FindControl<ContentControl>("View");
        }
    }
}