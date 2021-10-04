using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Interface.Date
{
    public interface IDateBuilder<TResult>
    {
        public TResult Create();
    }

}
