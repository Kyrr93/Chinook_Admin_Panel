using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationLayer
{
    public abstract class EfUseCase
    {
        protected AppDbContext Context { get; }

        protected EfUseCase(AppDbContext context)
        {
            Context = context;
        }
    }
}
