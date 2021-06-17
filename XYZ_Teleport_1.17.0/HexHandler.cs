using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZ_Teleport_1._17._0
{
    class HexHandler
    {
        public static string toHex(int inst) => inst.ToString("X");
        public static int toInt(string inst) => int.Parse(inst, System.Globalization.NumberStyles.HexNumber);
        public static string ath(string hex, int inst) => toHex(toInt(hex) + inst);
    }
}
