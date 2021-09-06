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

        // Would make a transform class but its pointless with what i have currently
        private static LocalPlayer lp = new LocalPlayer();
        public RigidBody rigidbody = new RigidBody(lp);

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

        public Vector2 rotation
        {
            get => Game.rotation;
            // set => Game.rotation = value;
        }

        public Vector3 lVector(float x, float y)
        {
            Vector3 tempVec = Base.Vec3(); // create empty vector

            tempVec.x = (float)Math.Cos(x) * (float)Math.Cos(y);
            tempVec.y = (float)Math.Sin(y);
            tempVec.z = (float)Math.Sin(x) * (float)Math.Sin(y);

            return tempVec;
        }

        public Vector3 dirVec
        {
            get
            {
                Vector3 tempVec;

                float cYaw = rotation.x + 89.9f * (float)Math.PI / 178f;
                float cPitch = rotation.y * (float)Math.PI / 178f;

                tempVec = lVector(cYaw, cPitch);

                return tempVec;
            }
            // set { }
        }
    }
}
