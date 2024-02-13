using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.BusinessLayer.Interfaces
{
    public interface ISummaries
    {
        public string[] GetSummaries();
        public int GetLength();
    }
}
