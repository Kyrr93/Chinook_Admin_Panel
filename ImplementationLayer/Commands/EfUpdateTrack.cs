using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.Commands;
using InfrastructureLayer.UseCases.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ImplementationLayer.Commands
{
    public class EfUpdateTrack : IUpdateTrackCommand
    {
        public int Id => 3;

        public string Name => "Update Track";

        public string Description => "Update Track using EF framework.";

        private readonly AppDbContext _appDbContext;

        public EfUpdateTrack(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Execute(UpdateTrackDTO request)
        {
            // Validate input
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Update request cannot be null.");
            }

            // Find the track in the database
            var track = _appDbContext.Tracks
                .FirstOrDefault(t => t.TrackId == request.TrackId);

            if (track == null)
            {
                throw new InvalidOperationException($"Track with ID {request.TrackId} not found.");
            }

            // Update properties
            track.Name = request.Name;
            track.AlbumId = request.AlbumId;
            track.MediaTypeId = request.MediaTypeId;
            track.GenreId = request.GenreId;
            track.Composer = request.Composer;
            track.Milliseconds = request.Milliseconds;
            track.Bytes = request.Bytes;
            track.UnitPrice = request.UnitPrice;

            // Save changes to the database
            _appDbContext.SaveChanges();
        }
    }
}
