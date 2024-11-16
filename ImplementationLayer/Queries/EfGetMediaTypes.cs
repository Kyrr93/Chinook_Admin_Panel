using DataAccessLayer.Models;
using InfrastructureLayer.UseCases.DTO;
using InfrastructureLayer.UseCases.Queries;
using InfrastructureLayer.UseCases.Searches;
using System.Collections.Generic;
using System.Linq;

namespace ImplementationLayer.Queries
{
    public class EfGetMediaTypes : EfUseCase, IGetMediaTypesQuery
    {
        public EfGetMediaTypes(AppDbContext context) : base(context) { }

        public int Id => 3;
        public string Name => "Get Media Types";
        public string Description => "Fetches a list of media types";

        public List<MediaTypesDTO> Execute(MediaTypesSearch search)
        {
            return Context.MediaTypes
                .Select(mt => new MediaTypesDTO
                {
                    MediaTypeId = mt.MediaTypeId,
                    Name = mt.Name
                })
                .ToList();
        }
    }
}
