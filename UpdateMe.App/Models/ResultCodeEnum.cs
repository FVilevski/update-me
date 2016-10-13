using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App
{
    public enum ResultCodeEnum : int
    {
        SUCCESS = 0,
        SUCCESS_HELP = 0,

        ERROR_INPUT = -10,
        ERROR_PECONDITION = -11,
        ERROR_UNEXPECTED = -12,
        ERROR_GET_VERSION = -13,
    }
}
