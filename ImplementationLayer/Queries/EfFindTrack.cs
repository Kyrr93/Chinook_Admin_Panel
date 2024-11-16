using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.DTO;
using InfrastructureLayer.UseCases.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ImplementationLayer.Queries
{
    public class EfFindTrack : IFindTrackQuery
    {
        private readonly AppDbContext _context;

        public EfFindTrack(AppDbContext context)
        {
            _context = context;
        }

        public int Id => 2;

        public string Name => "Find track";

        public string Description => "Find a track by its ID.";

        public FindTrackDTO Execute(int search)
        {
            // Fetch the track from the database
            var track = _context.Tracks
                .FirstOrDefault(t => t.TrackId == search);

            // Throw exception if the track is not found
            if (track == null)
            {
                throw new Exception("Entity not found!");
            }

            // Map the entity to DTO
            return new FindTrackDTO
            {
                TrackId = track.TrackId,
                Name = track.Name,
                AlbumId = track.AlbumId,
                MediaTypeId = track.MediaTypeId,
                GenreId = track.GenreId,
                Composer = track.Composer,
                Milliseconds = track.Milliseconds,
                Bytes = track.Bytes,
                UnitPrice = track.UnitPrice
            };
        }
    }
}
