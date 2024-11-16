using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.DTO;
using InfrastructureLayer.UseCases.Queries;
using InfrastructureLayer.UseCases.Searches;
using System.Collections.Generic;
using System.Linq;

namespace ImplementationLayer.Queries
{
    public class EfGetAlbums : EfUseCase, IGetAlbumsQuery
    {
        public EfGetAlbums(AppDbContext context) : base(context) { }

        public int Id => 1;
        public string Name => "Get Albums";
        public string Description => "Fetches a list of albums";

        public List<AlbumsDTO> Execute(AlbumsSearch search)
        {
            return Context.Albums
                .Select(a => new AlbumsDTO
                {
                    AlbumId = a.AlbumId,
                    Title = a.Title
                })
                .ToList();
        }
    }
}
