using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.DTO;
using InfrastructureLayer.UseCases.Queries;
using InfrastructureLayer.UseCases.Searches;
using System.Collections.Generic;
using System.Linq;

namespace ImplementationLayer.Queries
{
    public class EfGetGenres : EfUseCase, IGetGenresQuery
    {
        public EfGetGenres(AppDbContext context) : base(context) { }

        public int Id => 2;
        public string Name => "Get Genres";
        public string Description => "Fetches a list of genres";

        public List<GenresDTO> Execute(GenresSearch search)
        {
            return Context.Genres
                .Select(g => new GenresDTO
                {
                    GenreId = g.GenreId,
                    Name = g.Name
                })
                .ToList();
        }
    }
}
