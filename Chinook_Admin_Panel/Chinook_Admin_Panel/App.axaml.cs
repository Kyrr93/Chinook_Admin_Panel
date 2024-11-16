using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Chinook_Admin_Panel.ViewModels;
using Chinook_Admin_Panel.Views;
using DataAccessLayer.Models;
using ImplementationLayer.Commands;
using ImplementationLayer.Queries;
using InfrastructureLayer.UseCases.Commands;
using InfrastructureLayer.UseCases.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chinook_Admin_Panel
{
    public partial class App : Application
    {
        public IServiceProvider _serviceProvider;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Configure DI container
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);

                // Use DI to resolve MainWindow with injected MainView and MainViewModel
                desktop.MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = _serviceProvider.GetRequiredService<MainView>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register view models
            services.AddSingleton<MainViewModel>();

            // Register views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainView>();

            // Register queries and context
            services.AddSingleton<IGetTracksQuery, EfGetTracks>();
            services.AddSingleton<IFindTrackQuery, EfFindTrack>();
            services.AddSingleton<IGetAlbumsQuery, EfGetAlbums>();
            services.AddSingleton<IGetGenresQuery, EfGetGenres>();
            services.AddSingleton<IGetMediaTypesQuery, EfGetMediaTypes>();

            services.AddSingleton<ICreateTrackCommand, EfCreateTrack>();
            services.AddSingleton<IUpdateTrackCommand, EfUpdateTrack>();
            services.AddSingleton<IDeleteTrackCommand, EfDeleteTrack>();
            services.AddSingleton<AppDbContext>();
        }

    }

}