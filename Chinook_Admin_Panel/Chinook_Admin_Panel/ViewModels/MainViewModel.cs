using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using InfrastructureLayer.UseCases.Commands;
using InfrastructureLayer.UseCases.DTO;
using InfrastructureLayer.UseCases.Queries;
using InfrastructureLayer.UseCases.Searches;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook_Admin_Panel.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IGetTracksQuery _getTracksQuery;
    private readonly IGetAlbumsQuery _getAlbumsQuery;
    private readonly IGetGenresQuery _getGenresQuery;
    private readonly IGetMediaTypesQuery _getMediaTypesQuery;
    private readonly IFindTrackQuery _findTrackQuery;

    private readonly ICreateTrackCommand _createTrackCommand;
    private readonly IUpdateTrackCommand _updateTrackCommand;
    private readonly IDeleteTrackCommand _deleteTrackCommand;

    [ObservableProperty]
    private ObservableCollection<TracksDTO> tracksCollection = new();

    [ObservableProperty]
    private TracksDTO? selectedTrack;

    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalPages;

    [ObservableProperty]
    private string currentPageText = "Page 1 of 1";

    [ObservableProperty]
    private ObservableCollection<AlbumsDTO> albumsCollection = new();

    [ObservableProperty]
    private ObservableCollection<GenresDTO> genresCollection = new();

    [ObservableProperty]
    private ObservableCollection<MediaTypesDTO> mediaTypesCollection = new();

    [ObservableProperty]
    private AlbumsDTO? selectedAlbum;

    [ObservableProperty]
    private MediaTypesDTO? selectedMediaType;

    [ObservableProperty]
    private GenresDTO? selectedGenre;

    [ObservableProperty]
    private string? trackName;

    [ObservableProperty]
    private string? composer;

    [ObservableProperty]
    private int milliseconds;

    [ObservableProperty]
    private int? bytes;

    [ObservableProperty]
    private decimal unitPrice;

    public IRelayCommand NextPageCommand { get; }
    public IRelayCommand PreviousPageCommand { get; }
    public IRelayCommand SearchCommand { get; }
    public IRelayCommand ClearFiltersCommand { get; }
    public IRelayCommand OpenCreateDialogCommand { get; }
    public IRelayCommand OpenEditDialogCommand { get; }
    public IRelayCommand OpenDeleteDialogCommand { get; }
    public IRelayCommand AddTrackCommand { get; }
    public IRelayCommand EditTrackCommand { get; }
    public IRelayCommand DeleteTrackCommand { get; }
    public IRelayCommand ConfirmCommand { get; }
    public IRelayCommand CancelCommand { get; }
    public IRelayCommand CloseDialogCommand { get; }

    public MainViewModel(
        IGetTracksQuery getTracksQuery,
        IGetAlbumsQuery getAlbumsQuery,
        IGetGenresQuery getGenresQuery,
        IGetMediaTypesQuery getMediaTypesQuery,
        IFindTrackQuery findTrackQuery,
        ICreateTrackCommand createTrackCommand,
        IUpdateTrackCommand updateTrackCommand,
        IDeleteTrackCommand deleteTrackCommand)
    {
        _getTracksQuery = getTracksQuery;
        _getAlbumsQuery = getAlbumsQuery;
        _getGenresQuery = getGenresQuery;
        _getMediaTypesQuery = getMediaTypesQuery;
        _findTrackQuery = findTrackQuery;

        _createTrackCommand = createTrackCommand;
        _updateTrackCommand = updateTrackCommand;
        _deleteTrackCommand = deleteTrackCommand;

        SearchCommand = new RelayCommand(async () => await ExecuteSearchAsync());
        ClearFiltersCommand = new RelayCommand(async () => await ClearFiltersAsync());
        AddTrackCommand = new RelayCommand(AddTrack);
        OpenCreateDialogCommand = new RelayCommand(OpenCreateDialog);
        OpenEditDialogCommand = new RelayCommand(OpenEditDialog);
        OpenDeleteDialogCommand = new RelayCommand(OpenDeleteDialog);
        EditTrackCommand = new RelayCommand(EditTrack);
        DeleteTrackCommand = new RelayCommand(DeleteTrack);
        ConfirmCommand = new RelayCommand(() => DialogHost.Close("MainDialogHost", true));
        CancelCommand = new RelayCommand(() => DialogHost.Close("MainDialogHost", false));
        CloseDialogCommand = new RelayCommand(CloseDialog);
        NextPageCommand = new RelayCommand(async () => await GoToNextPageAsync());
        PreviousPageCommand = new RelayCommand(async () => await GoToPreviousPageAsync());

        Task.Run(async () =>
        {
            await LoadTracksAsync();
            await LoadComboBoxDataAsync();
        }).ConfigureAwait(false);
    }

    public bool CanGoToNextPage => CurrentPage < TotalPages && TotalPages > 1;
    public bool CanGoToPreviousPage => CurrentPage > 1;

    private async Task LoadComboBoxDataAsync()
    {
        // Load albums
        var albums = await Task.Run(() => _getAlbumsQuery.Execute(new()));
        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            albumsCollection.Clear();
            foreach (var album in albums)
            {
                albumsCollection.Add(album);
            }
        });

        // Load genres
        var genres = await Task.Run(() => _getGenresQuery.Execute(new()));
        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            genresCollection.Clear();
            foreach (var genre in genres)
            {
                genresCollection.Add(genre);
            }
        });

        // Load media types
        var mediaTypes = await Task.Run(() => _getMediaTypesQuery.Execute(new()));
        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            mediaTypesCollection.Clear();
            foreach (var mediaType in mediaTypes)
            {
                mediaTypesCollection.Add(mediaType);
            }
        });
    }

    private async Task LoadTracksAsync()
    {
        try
        {
            var search = new TracksSearch
            {
                SearchString = SearchQuery,
                Page = CurrentPage,
                PerPage = 13 // Number of tracks per page
            };

            var response = await Task.Run(() => _getTracksQuery.Execute(search));

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                TracksCollection.Clear();
                foreach (var track in response.Items)
                {
                    TracksCollection.Add(track);
                }

                TotalPages = Math.Max(1, (int)Math.Ceiling((double)response.TotalCount / search.PerPage));
                CurrentPage = Math.Min(CurrentPage, TotalPages);

                UpdatePaginationText();
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading tracks: {ex.Message}");
        }
    }

    private async Task ExecuteSearchAsync()
    {
        CurrentPage = 1; // Reset to the first page for new search
        await LoadTracksAsync();
    }

    private async Task ClearFiltersAsync()
    {
        SearchQuery = string.Empty;
        await LoadTracksAsync();
    }

    private async Task GoToNextPageAsync()
    {
        if (CanGoToNextPage)
        {
            CurrentPage++;
            await LoadTracksAsync();
        }
    }

    private async Task GoToPreviousPageAsync()
    {
        if (CanGoToPreviousPage)
        {
            CurrentPage--;
            await LoadTracksAsync();
        }
    }

    private async void OpenCreateDialog()
    {
        try
        {
            // Reset dialog properties for new track creation
            TrackName = string.Empty;
            SelectedAlbum = null;
            SelectedMediaType = null;
            SelectedGenre = null;
            Composer = string.Empty;
            Milliseconds = 0;
            Bytes = null;
            UnitPrice = 0;

            // Open the create dialog
            var createDialog = new DialogCreateTrack();
            await DialogHost.Show(createDialog, "MainDialogHost");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error opening create dialog: {ex.Message}");
        }
    }

    private void AddTrack()
    {
        try
        {
            var createTrackDto = new CreateTrackDTO
            {
                Name = TrackName,
                AlbumId = SelectedAlbum?.AlbumId,
                MediaTypeId = SelectedMediaType?.MediaTypeId ?? 0,
                GenreId = SelectedGenre?.GenreId,
                Composer = Composer,
                Milliseconds = Milliseconds,
                Bytes = Bytes,
                UnitPrice = UnitPrice
            };

            _createTrackCommand.Execute(createTrackDto);

            Task.Run(async () => await LoadTracksAsync());

            CloseDialog();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating track: {ex.Message}");
        }
    }
    private async void OpenEditDialog()
    {
        if (SelectedTrack == null)
            return;

        try
        {
            // Fetch track details using the FindTrackQuery
            var trackDetails = await Task.Run(() => _findTrackQuery.Execute(SelectedTrack.TrackId)); // No error now

            // Map track details to dialog properties
            TrackName = trackDetails.Name;
            SelectedAlbum = AlbumsCollection.FirstOrDefault(a => a.AlbumId == trackDetails.AlbumId);
            SelectedMediaType = MediaTypesCollection.FirstOrDefault(mt => mt.MediaTypeId == trackDetails.MediaTypeId);
            SelectedGenre = GenresCollection.FirstOrDefault(g => g.GenreId == trackDetails.GenreId);
            Composer = trackDetails.Composer;
            Milliseconds = trackDetails.Milliseconds;
            Bytes = trackDetails.Bytes;
            UnitPrice = trackDetails.UnitPrice;

            // Open the edit dialog
            var editDialog = new DialogEditTrack();
            await DialogHost.Show(editDialog, "MainDialogHost");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error opening edit dialog: {ex.Message}");
        }
    }


    private void EditTrack()
    {
        if (SelectedTrack == null)
            return;

        try
        {
            var updateTrackDto = new UpdateTrackDTO
            {
                TrackId = SelectedTrack.TrackId,
                Name = TrackName,
                AlbumId = SelectedAlbum?.AlbumId,
                MediaTypeId = SelectedMediaType.MediaTypeId,
                GenreId = SelectedGenre?.GenreId,
                Composer = Composer,
                Milliseconds = Milliseconds,
                Bytes = Bytes,
                UnitPrice = UnitPrice
            };

            // Avoid any indirect calls to EditTrack or property setters
            _updateTrackCommand.Execute(updateTrackDto);

            Task.Run(async () => await LoadTracksAsync());

            CloseDialog();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating track: {ex.Message}");
        }
    }

    private async void OpenDeleteDialog()
    {
        if (SelectedTrack == null)
            return;

        try
        {
            // Open confirmation dialog
            var deleteDialog = new DialogDeleteTrack();
            var result = await DialogHost.Show(deleteDialog, "MainDialogHost");

            if (result is bool confirm && confirm)
            {
                // Proceed to delete the track if confirmed
                DeleteTrack();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error opening delete dialog: {ex.Message}");
        }
    }


    private void DeleteTrack()
    {
        if (SelectedTrack == null)
            return;

        try
        {
            var deleteTrackDto = new DeleteTrackDTO
            {
                TrackId = SelectedTrack.TrackId
            };

            _deleteTrackCommand.Execute(deleteTrackDto);

            Task.Run(async () => await LoadTracksAsync());

            CloseDialog();
        }
        catch (DbUpdateException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deleting track: {ex.Message}");
            if (ex.InnerException != null)
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"General error deleting track: {ex.Message}");
        }
    }



    private void CloseDialog()
    {
        DialogHost.Close("MainDialogHost");
    }

    private void UpdatePaginationText()
    {
        CurrentPageText = $"Page {CurrentPage} of {TotalPages}";
        OnPropertyChanged(nameof(CanGoToNextPage));
        OnPropertyChanged(nameof(CanGoToPreviousPage));
    }
}
