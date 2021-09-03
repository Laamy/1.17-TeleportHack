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
            }), // (Note: gamemode works like 1(Creative) * 4294967296, etc
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

        public static void setVersion(IVersion version) // Didnt realize I fucked this setVersion function up :cry:
        {
            Game.localPlayer = version.sdk[1];
            Game.localPlayer_XPosition = version.sdk[2];
            Game.localPlayer_Gamemode = version.sdk[3];
            Game.localPlayer_XVelocity = version.sdk[4];
        }
    }
}

/*

onGround - 1E0
onGround2 - 1E4
stepHeight - 240
worldAge - 2B0
gamemode - 1E08
isFlying - 9C0
blocksTraveled_Ex - 250
blocksTraveled - 250 + 16
helditemCount - 228A
holdingItem - 2274
holdingItemId - 2280 (I think..?)
selectedHotbarId - 22F8
viewCreativeItems - 9D8
viewCreativeItemsSelectedCategory - 2370
entityType - 410
inInventory - 11E0
username - 920
gameDim - 370 18
positionX - 4D0
hitbox - 4D0 + 28
velocity - 50C
swingAn - 7A0
lookingEntityId - 10B8
inWater - 265
bodyRots - 148

 */