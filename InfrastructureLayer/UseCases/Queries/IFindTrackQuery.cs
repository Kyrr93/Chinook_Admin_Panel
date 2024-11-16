using InfrastructureLayer.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.UseCases.Queries
{
    public interface IFindTrackQuery : IQuery<int, FindTrackDTO>
    {
    }
}
