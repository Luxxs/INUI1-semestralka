using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Logic
{
    public abstract class AbstractPath
    {
        public abstract LinkedList<Tuple<int, int>>  GetPathAsTupleSeries();
    }
}
