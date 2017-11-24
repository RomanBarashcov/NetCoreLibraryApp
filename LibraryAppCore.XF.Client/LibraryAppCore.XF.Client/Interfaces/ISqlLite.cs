using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.XF.Client.Interfaces
{
    public interface ISqlLite
    {
        string GetDatabasePath(string filename);
    }
}
