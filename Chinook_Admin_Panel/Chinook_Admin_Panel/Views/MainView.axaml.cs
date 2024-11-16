using Avalonia;
using Avalonia.Controls;
using Chinook_Admin_Panel.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Chinook_Admin_Panel.Views
{
    public partial class MainView : UserControl
    {
        // Parameterless constructor required by Avalonia XAML loader
        public MainView()
        {
            InitializeComponent();

            // Access the DI container via Application.Current and resolve MainViewModel
            DataContext = ((App)Application.Current)._serviceProvider.GetRequiredService<MainViewModel>();
        }


    }
}
