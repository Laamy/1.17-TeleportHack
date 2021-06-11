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
        public static Form1 handle;
        public Mem mem;
        public Form1()
        {
            InitializeComponent();
            handle = this;
            mem = new Mem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game.teleport(new Vec3(textBox1.Text)); // new Vec3("1, 1, 1") vector string parser inbuilt
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mem != null) // check if mc is open then attach if its not already
            {
                if (mem.theProc == null || mem.theProc.HasExited)
                {
                    try
                    {
                        mem.OpenProcess(mem.GetProcIdFromName("Minecraft.Windows.exe"));
                    }
                    catch { }
                }
                else
                {
                    textBox2.Text = Game.position.ToString();
                }
            }
        }
    }
}
