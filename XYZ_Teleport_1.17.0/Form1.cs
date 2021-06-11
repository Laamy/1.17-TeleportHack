using System;
using System.Collections.Generic;
using System.Threading;
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


            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    if (recallRecordingEnabled && mem != null)
                    {
                        Vec3 _Vec3 = Game.position;
                        recallList.Add(_Vec3);
                        Thread.Sleep(1);
                    }
                }
            }).Start();
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

        private void button2_Click(object sender, EventArgs e)
        {
            textBox5.Text = new Vec3(textBox2.Text).DistanceTo(new Vec3(textBox3.Text)).ToString();
        }

        bool recallRecordingEnabled = false;
        List<Vec3> recallList = new List<Vec3>();

        private void button3_Click(object sender, EventArgs e)
        {
            if (recallRecordingEnabled)
            {
                recallList.Reverse();
                button3.Text = "Start recording";
            }
            else
            {
                recallList.Clear();
                button3.Text = "Stop...";
            }
            recallRecordingEnabled = !recallRecordingEnabled;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                foreach (Vec3 pos in recallList)
                {
                    Game.teleport(pos);
                    Thread.Sleep(1);
                }
            }).Start();
        }
    }
}
