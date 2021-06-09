using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XYZ_Teleport_1._17._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Mem mem;

        private void button1_Click(object sender, EventArgs e)
        {
            mem = new Mem();
            mem.OpenProcess(mem.GetProcIdFromName("Minecraft.Windows.exe"));
            teleport(
                (float)numericUpDown1.Value,
                (float)numericUpDown2.Value,
                (float)numericUpDown3.Value);
        }

        string localPlayer = "Minecraft.Windows.exe+03FFFA98,0,50,138,";
        int XHex = 0x4D0;

        public void teleport(float x, float y, float z)
        {
            if (checkBox1.Checked)
            {
                mem.WriteMemory(localPlayer + XHex.ToString("X"), "float", (0 - x).ToString());
                mem.WriteMemory(localPlayer + (XHex + 12).ToString("X"), "float", (0 - x + 0.6f).ToString());
            }
            else
            {
                mem.WriteMemory(localPlayer + XHex.ToString("X"), "float", x.ToString());
                mem.WriteMemory(localPlayer + (XHex + 12).ToString("X"), "float", (x + 0.6f).ToString());
            }

            if (checkBox2.Checked)
            {
                mem.WriteMemory(localPlayer + (XHex + 4).ToString("X"), "float", (0 - y).ToString());
                mem.WriteMemory(localPlayer + (XHex + 16).ToString("X"), "float", (0 - y + 0.6f).ToString());
            }
            else
            {
                mem.WriteMemory(localPlayer + (XHex + 4).ToString("X"), "float", y.ToString());
                mem.WriteMemory(localPlayer + (XHex + 16).ToString("X"), "float", (y + 1.8f).ToString());
            }

            if (checkBox3.Checked)
            {
                mem.WriteMemory(localPlayer + (XHex + 8).ToString("X"), "float", (0 - z).ToString());
                mem.WriteMemory(localPlayer + (XHex + 20).ToString("X"), "float", (0 - z + 0.6f).ToString());
            }
            else
            {
                mem.WriteMemory(localPlayer + (XHex + 8).ToString("X"), "float", z.ToString());
                mem.WriteMemory(localPlayer + (XHex + 20).ToString("X"), "float", (z + 0.6f).ToString());
            }
        }
    }
}
