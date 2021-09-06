using System;
using System.Globalization;

namespace XYZ_Teleport_1._17._0
{
    public class HexHandler
    {
        public static string toHex(int value)
        {
            string outp = value.ToString("X");
            return outp;
        }

        public static int toInt(string value)
        {
            int hexString = int.Parse(value, NumberStyles.HexNumber);
            return hexString;
        }

        public static long toLong(string value)
        {
            long outp = Convert.ToInt64(value, 16);
            return outp;
        }

        public static string addBytes(string hexString, int bytes)
        {
            var f = toInt(hexString) + bytes; // Convert hexString to int then plus x amount of bytes to it

            string outp = toHex(f); // Convert back to hexString

            return outp;
        }
    }

    class Base
    {
        public static Vector3 Vec3(float _, float v, float c)
        {
            Vector3 tempVec = new Vector3(_, v, c);
            return tempVec;
        }
        public static Vector3 Vec3(string v)
        {
            Vector3 tempVec = new Vector3(v);
            return tempVec;
        }
        public static Vector3 Vec3()
        {
            Vector3 tempVec = new Vector3(0, 0, 0);
            return tempVec;
        }

        public static Vector2 Vec2(float _, float v)
        {
            Vector2 tempVec = new Vector2(_, v);
            return tempVec;
        }
        public static Vector2 Vec2(string v)
        {
            Vector2 tempVec = new Vector2(v);
            return tempVec;
        }
        public static Vector2 Vec2()
        {
            Vector2 tempVec = new Vector2(0, 0);
            return tempVec;
        }
    }
}
