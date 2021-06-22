using System;

namespace XYZ_Teleport_1._17._0
{
    class Game
    {
        public static string localPlayer = "base+04020228,0,18,B8,";
        public static string localPlayer_XPosition = "4D0";

        public static void teleport(float x, float y, float z)
        {
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.ath(localPlayer_XPosition, 0), "float", (x).ToString());
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.ath(localPlayer_XPosition, 12), "float", (x + 0.6f).ToString());

            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.ath(localPlayer_XPosition, 4), "float", (y).ToString());
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.ath(localPlayer_XPosition, 16), "float", (y + 1.8f).ToString());

            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.ath(localPlayer_XPosition, 8), "float", (z).ToString());
            Form1.handle.mem.WriteMemory(localPlayer + HexHandler.ath(localPlayer_XPosition, 20), "float", (z + 0.6f).ToString());
        }

        public static void teleport(Vec3 _Vec3) => teleport(_Vec3.x, _Vec3.y, _Vec3.z);

        public static Vec3 position
        {
            get
            {
                return new Vec3(Form1.handle.mem.ReadFloat(localPlayer + HexHandler.ath(localPlayer_XPosition, 0)) + "," +
                    Form1.handle.mem.ReadFloat(localPlayer + HexHandler.ath(localPlayer_XPosition, 4)) + "," +
                    Form1.handle.mem.ReadFloat(localPlayer + HexHandler.ath(localPlayer_XPosition, 8)));
            }
        }
    }

    public class Vec3
    {
        public float x;
        public float y;
        public float z;
        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vec3(string position)
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
                this.x = HexHandler._toInt(parsedStr[0]);
                this.y = HexHandler._toInt(parsedStr[1]);
                this.z = HexHandler._toInt(parsedStr[2]);
            }
        }
        public float DistanceTo(Vec3 _Vec3)
        {
            float diff_x = x - _Vec3.x, diff_y = y - _Vec3.y, diff_z = z - _Vec3.z;
            float output = (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y + diff_z * diff_z);
            if ((int)output == 0) output = _Vec3.Distance(this);
            return output;
        }
        public float Distance(Vec3 _Vec3)
        {
            float diff_x = x - _Vec3.x, diff_y = y - _Vec3.y, diff_z = z - _Vec3.z;
            return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y + diff_z * diff_z);
        }
        public override string ToString()
        {
            return x + "," + y + "," + z;
        }
    }
}
