using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Chinook_Admin_Panel.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Chinook_Admin_Panel;

public partial class DialogEditTrack : UserControl
{
    public DialogEditTrack()
    {
        InitializeComponent();

        DataContext = ((App)Application.Current)._serviceProvider.GetRequiredService<MainViewModel>();
    }
}