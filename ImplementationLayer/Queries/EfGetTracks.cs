using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.DTO;
using InfrastructureLayer.UseCases.Queries;
using InfrastructureLayer.UseCases.Searches;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ImplementationLayer.Queries
{
    public class EfGetTracks : EfUseCase, IGetTracksQuery
    {
        public EfGetTracks(AppDbContext context) : base(context)
        {
        }

        public int Id => 1;

        public string Name => "Get Tracks";

        public string Description => "Fetches tracks with optional filters and pagination.";

        public PagedResponse<TracksDTO> Execute(TracksSearch search)
        {
            IQueryable<Track> query = Context.Tracks;

            // Apply SearchString filter
            if (!string.IsNullOrWhiteSpace(search.SearchString))
            {
                string searchString = search.SearchString.ToLower();
                query = query.Where(t =>
                    t.Name.ToLower().Contains(searchString) ||
                    t.Album.Title.ToLower().Contains(searchString) ||
                    t.MediaType.Name.ToLower().Contains(searchString) ||
                    (t.Genre != null && t.Genre.Name.ToLower().Contains(searchString)) ||
                    (t.Composer != null && t.Composer.ToLower().Contains(searchString))
                );
            }

            // Total count for pagination
            int totalCount = query.Count();

            // Pagination logic
            var items = query
                .Skip((search.Page - 1) * search.PerPage)
                .Take(search.PerPage)
                .Select(t => new TracksDTO
                {
                    TrackId = t.TrackId,
                    Name = t.Name,
                    AlbumTitle = t.Album.Title,
                    MediaTypeName = t.MediaType.Name,
                    GenreName = t.Genre.Name,
                    Composer = t.Composer,
                    Duration = $"{t.Milliseconds / 60000:D2}:{(t.Milliseconds % 60000) / 1000:D2}",
                    SizeInMB = t.Bytes.HasValue ? $"{(t.Bytes.Value / 1048576.0):F2} MB" : "Unknown",
                    UnitPrice = t.UnitPrice
                })
                .ToList();

            return new PagedResponse<TracksDTO>
            {
                TotalCount = totalCount,
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                Items = items
            };
        }

    }
}
