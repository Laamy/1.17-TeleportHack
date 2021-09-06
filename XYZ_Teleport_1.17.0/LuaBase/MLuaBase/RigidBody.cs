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

        // Also not required but fuck it
        public void applyForce(Vector3 v)
        {
            lp.velocity.x += v.x;
            lp.velocity.y += v.y;
            lp.velocity.z += v.z;
        }

        // Not required but ill keep it to make my life easier
        public void applyFakeForce(Vector3 v) // FAKE
        {
            lp.velocity.x = v.x;
            lp.velocity.y = v.y;
            lp.velocity.z = v.z;
        }

        /// <summary>
        /// Useful for real velocity modules
        /// </summary>
        public void applyRealForce(Vector3 v) // Apply force based on transforms roation
        {
            Vector3 DirVec = lp.dirVec;

            lp.velocity.x += v.x * DirVec.x;
            lp.velocity.y += v.y * DirVec.y;
            lp.velocity.z += v.z * DirVec.z;
        }

        /// <summary>
        /// Useful for flies
        /// </summary>
        public void applyRealFakeForce(Vector3 v) // Apply force based on transforms roation (FAKE)
        {
            Vector3 DirVec = lp.dirVec;

            lp.velocity.x = v.x * DirVec.x;
            lp.velocity.y = v.y * DirVec.y;
            lp.velocity.z = v.z * DirVec.z;
        }
    }
}
