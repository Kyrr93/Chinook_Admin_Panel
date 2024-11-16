using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.Commands;
using InfrastructureLayer.UseCases.DTO;
using Microsoft.EntityFrameworkCore;
using System;

namespace ImplementationLayer.Commands
{
    public class EfCreateTrack : ICreateTrackCommand
    {
        public int Id => 4;

        public string Name => "Create Track";

        public string Description => "Create Track using EF framework.";

        private readonly AppDbContext _appDbContext;

        public EfCreateTrack(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Execute(CreateTrackDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Track data is required.");

            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Track name is required.");

            if (request.MediaTypeId == 0)
                throw new ArgumentException("MediaTypeId is required.");

            if (request.AlbumId == null && request.GenreId == null && string.IsNullOrWhiteSpace(request.Composer))
                throw new ArgumentException("At least one additional field (Album, Genre, or Composer) is required.");

            // Create new Track entity
            var newTrack = new Track
            {
                Name = request.Name,
                AlbumId = request.AlbumId,
                MediaTypeId = request.MediaTypeId,
                GenreId = request.GenreId,
                Composer = request.Composer,
                Milliseconds = request.Milliseconds,
                Bytes = request.Bytes,
                UnitPrice = request.UnitPrice
            };

            // Add to database
            _appDbContext.Tracks.Add(newTrack);

            try
            {
                _appDbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                throw; // Optionally rethrow the exception after logging
            }

        }
    }
}
