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
        public LocalPlayer localPlayer = new LocalPlayer(); // Should be able to apply force ingame via lua now using the local player rigid body class

        public Vector3 Vec3(int v, int c, int _) => Base.Vec3(v, c, _);
        public Vector3 Vec3(int v, int c) => Base.Vec3(v, c, 0);
        public Vector3 Vec3(int v) => Base.Vec3(v, 0, 0);
        public Vector3 Vec3() => Base.Vec3(0, 0, 0);

        public Vector2 Vec2(int v, int c) => Base.Vec2(v, c); // Give lua env access to vector2 without needing to call camera rotations by hand
        public Vector2 Vec2(int v) => Base.Vec2(v, 0);
        public Vector2 Vec2() => Base.Vec2(0, 0);

        public MLua(IVersion mcVersion)
        {
            Version = mcVersion;
        }
    }
}
