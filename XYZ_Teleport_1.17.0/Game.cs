using System;
using System.Collections.Generic;

namespace XYZ_Teleport_1._17._0
{
    class Game
    {
        public static string localPlayer = "base+041457D8,8,20,C8,";
        public static string localPlayer_XPosition = "4D0";
        public static string localPlayer_Gamemode = "1E08";
        public static string localPlayer_XVelocity = "50C";

        public static void teleport(AABB advancedAxis) // More advanced axis teleportation
        {
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 0), "float", advancedAxis.x.x.ToString());
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 12), "float", advancedAxis.y.x.ToString());

            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 4), "float", advancedAxis.x.y.ToString());
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 16), "float", advancedAxis.y.y.ToString());

            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 8), "float", advancedAxis.x.z.ToString());
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 20), "float", advancedAxis.y.z.ToString());
        }

        public static void teleport(float x, float y, float z)
        {
            teleport(new AABB($"{x},{y},{z}:{x + .6f},{y + 1.8f},{z + .6f}"));
        }

        public static void teleport(Vector3 _Vec3)
        {
            teleport(_Vec3.x, _Vec3.y, _Vec3.z);
        }

        public static Vector3 position
        {
            get
            {
                return Base.Vec3(Form1.handle.mem.ReadFloat(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 0)) + "," +
                    Form1.handle.mem.ReadFloat(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 4)) + "," +
                    Form1.handle.mem.ReadFloat(localPlayer + HexHandler.addBytes(localPlayer_XPosition, 8)));
            }
        }

        public static int gamemode
        {
            get
            {
                return (int)(Form1.handle.mem.ReadLong(localPlayer + localPlayer_Gamemode) / 4294967296);
            }
            set
            {
                Form1.handle.mem.WriteMemory(localPlayer + localPlayer_Gamemode, "long", ((ulong)value * 4294967296).ToString());
            }
        }

        public static Vector3 velocity
        {
            get
            {
                return Base.Vec3(Form1.handle.mem.ReadFloat(localPlayer + HexHandler.addBytes(localPlayer_XVelocity, 0)) + "," +
                    Form1.handle.mem.ReadFloat(localPlayer + HexHandler.addBytes(localPlayer_XVelocity, 4)) + "," +
                    Form1.handle.mem.ReadFloat(localPlayer + HexHandler.addBytes(localPlayer_XVelocity, 8)));
            }
            set
            {
                Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XVelocity, 0), "float", value.x.ToString());
                Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XVelocity, 4), "float", value.y.ToString());
                Form1.handle.mem.WriteMemory(localPlayer + HexHandler.addBytes(localPlayer_XVelocity, 8), "float", value.z.ToString());
            }
        }
    }

    public class GamemodeRegistery
    {
        private List<List<string>> registery = new List<List<string>> {
            new List<string> // Survival
            {
                "0",
                "s",
                "survival"
            },
            new List<string> // Creative
            {
                "1",
                "c",
                "creative"
            },
            new List<string> // Adventure
            {
                "2",
                "a",
                "adventure"
            }
        }; // Gamemode Registery

        public GamemodeRegistery(out List<List<string>> list)
        {
            list = registery;
        }
    }

    /*public class ItemRegistery // Not used nor will be just thinking of an example :thinking:
    {
        private List<List<object>> registery = new List<List<object>> {
            new List<object> // Air
            {
                true, // isValid
                new List<object> // Names
                {
                    "0",
                    "air",
                    "minecraft:air"
                },
                new List<object> // Variants
                {
                    new string[] { // air { ID : 0 }
                        "0",
                        "air"
                    }
                }
            }, // Id 0
            new List<object> // Stone
            {
                true, // isValid
                new List<object> // Names
                {
                    "1",
                    "stone",
                    "minecraft:stone"
                },
                new List<object> // Variants
                {
                    new string[] { // stone { ID : 0 }
                        "0",
                        "stone"
                    },
                    new string[] { // stone { ID : 1 }
                        "1",
                        "granite"
                    },
                    new string[] { // stone { ID : 2 }
                        "2",
                        "polished_granite",
                        "polishedgranite"
                    },
                    new string[] { // stone { ID : 3 }
                        "3",
                        "polished_granite",
                        "polishedgranite"
                    },
                }
            } // Id 1
        }; // item Registery

         public ItemRegistery(out List<List<object>> list)
        {
            list = registery;
        }
    }*/

    public class Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3(string position)
        {
            try
            {
                string[] parsedStr = position.Replace(" ", "").Split(',');
                this.x = Convert.ToSingle(parsedStr[0]);
                this.y = Convert.ToSingle(parsedStr[1]);
                this.z = Convert.ToSingle(parsedStr[2]);
            }
            catch
            {
                string[] parsedStr = position.Replace(" ", "").Split(',');
                this.x = HexHandler.toLong(parsedStr[0]);
                this.y = HexHandler.toLong(parsedStr[1]);
                this.z = HexHandler.toLong(parsedStr[2]);
            }
        }
        public float DistanceTo(Vector3 _Vec3)
        {
            float diff_x = x - _Vec3.x, diff_y = y - _Vec3.y, diff_z = z - _Vec3.z;
            float output = (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y + diff_z * diff_z);
            if ((int)output == 0) output = _Vec3.Distance(this);
            return output;
        }
        public float Distance(Vector3 _Vec3)
        {
            float diff_x = x - _Vec3.x, diff_y = y - _Vec3.y, diff_z = z - _Vec3.z;
            return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y + diff_z * diff_z);
        }
        public override string ToString()
        {
            return x + "," + y + "," + z;
        }
    }
    public class AABB
    {
        public Vector3 x;
        public Vector3 y;
        public AABB(Vector3 x, Vector3 y)
        {
            this.x = x;
            this.y = y;
        }

        /// <param name="type">Types: (Advanced, Prehandled)</param>
        public AABB(string position, string type = "Advanced")
        {
            if (type == "Advanced") // Was gonna use a switch here but i might aswell go basic
            {
                string[] parsedStr = position.Replace(" ", "").Split(':');
                this.x = Base.Vec3(parsedStr[0]);
                this.y = Base.Vec3(parsedStr[1]);
            }
            else if (type == "Prehandled")
            {
                try
                {
                    string[] parsedStr = position.Replace(" ", "").Split(',');
                    this.x.x = Convert.ToSingle(parsedStr[0]);
                    this.x.x = Convert.ToSingle(parsedStr[1]);
                    this.x.x = Convert.ToSingle(parsedStr[2]);

                    this.y.x = Convert.ToSingle(parsedStr[0]) + 0.6f;
                    this.y.x = Convert.ToSingle(parsedStr[1]) + 1.8f;
                    this.y.x = Convert.ToSingle(parsedStr[2]) + 0.6f;
                }
                catch
                {
                    string[] parsedStr = position.Replace(" ", "").Split(',');
                    this.x.x = HexHandler.toLong(parsedStr[0]);
                    this.x.x = HexHandler.toLong(parsedStr[1]);
                    this.x.x = HexHandler.toLong(parsedStr[2]);

                    this.y.x = HexHandler.toLong(parsedStr[0]) + 0.6f;
                    this.y.x = HexHandler.toLong(parsedStr[1]) + 1.8f;
                    this.y.x = HexHandler.toLong(parsedStr[2]) + 0.6f;
                }
            }
        }
        public override string ToString()
        {
            return x.ToString() + ":" + y.ToString();
        }
    }
}
