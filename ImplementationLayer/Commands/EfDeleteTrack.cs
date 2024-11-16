using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.Commands;
using InfrastructureLayer.UseCases.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ImplementationLayer.Commands
{
    public class EfDeleteTrack : IDeleteTrackCommand
    {
        public int Id => 5;

        public string Name => "Delete Track";

        public string Description => "Delete a track and handle related entities using EF.";

        private readonly AppDbContext _appDbContext;

        public EfDeleteTrack(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Execute(DeleteTrackDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Track data is required.");

            // Fetch the track by ID
            var track = _appDbContext.Tracks
                .Include(t => t.InvoiceLines) // Load related InvoiceLines
                .Include(t => t.Playlists)   // Load related Playlists
                .FirstOrDefault(t => t.TrackId == request.TrackId);

            if (track == null)
                throw new InvalidOperationException($"Track with ID {request.TrackId} does not exist.");

            // Remove related InvoiceLines
            if (track.InvoiceLines.Any())
            {
                _appDbContext.InvoiceLines.RemoveRange(track.InvoiceLines);
            }

            // Remove relationships with Playlists (do not delete Playlists themselves)
            if (track.Playlists.Any())
            {
                foreach (var playlist in track.Playlists)
                {
                    playlist.Tracks.Remove(track);
                }
            }

            // Remove the track itself
            _appDbContext.Tracks.Remove(track);

            // Save changes
            _appDbContext.SaveChanges();
        }
    }
}
