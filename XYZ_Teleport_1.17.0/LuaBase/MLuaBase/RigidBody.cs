using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZ_Teleport_1._17._0.LuaBase.MLuaBase
{
    class RigidBody
    {
        private LocalPlayer lp;
        public RigidBody(LocalPlayer lp)
        {
            this.lp = lp;
        }

        public void applyForce(Vector3 v)
        {
            lp.velocity.x += v.x;
            lp.velocity.y += v.y;
            lp.velocity.z += v.z;
        }

        public void applyRealForce(Vector3 v) // Apply force based on transforms roation
        {
            /*
            lp.velocity.x += v.x;
            lp.velocity.y += v.y;
            lp.velocity.z += v.z;*/
        }
    }
}
