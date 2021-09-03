using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZ_Teleport_1._17._0.VersionBase
{
    class VersionClass // versions
    {
        public static IVersion[] versions = new IVersion[] { // Version pointers (Feel free to add your versions >~<)
            /*new IVersion(new string[]
            {
                "1.",
                "base+",
                "" ,
                "" 
            }),*/
            new IVersion(new string[] // might add Beta versions later :thinking: nah
            {
                "1.17.11", // Version
                "base+041457D8,8,20,C8,", // LocalPlayer
                "4D0", // PositionX-1 offset
                "1E08", // Gamemode offset,
                "50C" // VelocityX offset
            }),
            new IVersion(new string[]
            {
                "1.17.2",
                "base+04020228,0,18,B8,",
                "4D0" ,
                "1E08",
                "50C"
            }),
            new IVersion(new string[]
            {
                "1.17",
                "base+03FFFA98,0,50,138,",
                "4D0",
                "1E08",
                "50C"
            }),
            new IVersion(new string[] // Leave a slot blank if you dont have the offset/dont know how to get it (I dont want to downgrade just to find the gamemode offset :p
            {
                "1.16.221",
                "base+03CDE520,38,50,140,",
                "4A0" ,
                "",
                "4E0"
            })
        };

        public static void setVersion(IVersion version)
        {
            Game.localPlayer = version.sdk[0];
            Game.localPlayer_XPosition = version.sdk[1];
        }
    }
}
