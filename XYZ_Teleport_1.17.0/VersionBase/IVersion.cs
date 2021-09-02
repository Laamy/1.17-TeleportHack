using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZ_Teleport_1._17._0.VersionBase
{
    class IVersion // version template
    {
        public IVersion(string[] list)
        {
            name = list[0];
            sdk = new string[] { list[1], list[2] };
        }

        public string name = "0.0.0.0";
        public string[] sdk = { "", "" };
    }
}
