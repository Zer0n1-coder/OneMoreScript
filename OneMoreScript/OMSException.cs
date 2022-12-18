using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMoreScript
{
    class OMSException : Exception
    {
        public OMSException(String m):base(m) {}
        //public OMSException(String m, ASTree t):Exception(m + " " + t.location()) { }
    }
}
