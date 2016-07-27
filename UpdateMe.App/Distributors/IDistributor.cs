using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App
{
    public interface IDistributor
    {
        void DistributeRelease(List<string> files);
    }
}
