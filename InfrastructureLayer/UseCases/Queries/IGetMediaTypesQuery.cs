using InfrastructureLayer.UseCases.DTO;
using InfrastructureLayer.UseCases.Searches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.UseCases.Queries
{
    public interface IGetMediaTypesQuery : IQuery<MediaTypesSearch, List<MediaTypesDTO>>
    {
    }
}
