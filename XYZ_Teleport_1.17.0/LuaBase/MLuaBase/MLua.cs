using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYZ_Teleport_1._17._0.VersionBase;

namespace XYZ_Teleport_1._17._0.LuaBase.MLuaBase
{
    class MLua
    {
        public IVersion Version; // Wont be used its just so you have access to the required pointers & offsets
        public MLua(IVersion mcVersion)
        {
            Version = mcVersion;
        }

        public LocalPlayer getLocalPlayer()
        {
            return new LocalPlayer();
        }
    }
}
