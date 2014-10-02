using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository
{
    public abstract class RepositoryWithContext
    {
        protected readonly RepositoryContext Context;

        protected RepositoryWithContext(RepositoryContext context)
        {
            this.Context = context;
        }

    }
}
