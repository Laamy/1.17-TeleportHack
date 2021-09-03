using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZ_Teleport_1._17._0.VersionBase
{
    class IVersion // version template
    {
        public string name;
        public string[] sdk;

        public IVersion(string[] list) // fixed
        {
            name = list[0];
            sdk = list.Skip(1).ToArray();
        }
    }
}
