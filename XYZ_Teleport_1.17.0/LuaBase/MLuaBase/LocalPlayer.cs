using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYZ_Teleport_1._17._0.VersionBase;

namespace XYZ_Teleport_1._17._0.LuaBase.MLuaBase
{
    class LocalPlayer // Class created for MLua
    {
        public IVersion version // Debug
        {
            get
            {
                IVersion tempVersion = new IVersion(new string[] {
                    "0.0.0.0",
                    Game.localPlayer,
                    Game.localPlayer_XPosition,
                    Game.localPlayer_Gamemode,
                    Game.localPlayer_XVelocity
                });
                return tempVersion;
            }
        }

        public int gamemode
        {
            get => Game.gamemode;
            set => Game.gamemode = value;
        }

        public Vector3 position
        {
            get => Game.position;
            set => Game.teleport(value);
        }

        public Vector3 velocity
        {
            get => Game.velocity;
            set => Game.velocity = value;
        }
    }
}
