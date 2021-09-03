using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using XYZ_Teleport_1._17._0._Keymap;
using XYZ_Teleport_1._17._0.VersionBase;

namespace XYZ_Teleport_1._17._0
{
    public partial class Form1 : Form
    {
        public static Form1 handle;
        public Mem mem;
        public Form1()
        {
            InitializeComponent();

            VersionClass.setVersion(VersionClass.versions[0]); // Load latest version!

            handle = this;
            mem = new Mem();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    if (recallRecordingEnabled && mem != null)
                    {
                        Vector3 _Vec3 = Game.position;
                        recallList.Add(_Vec3);
                        Thread.Sleep(1);
                    }
                }
            }).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game.teleport(new Vector3(textBox1.Text)); // new Vec3("1, 1, 1") vector string parser inbuilt
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
                    try
                    {
                        textBox2.Text = Game.position.ToString();
                    }
                    catch { }
                    try
                    {
                        textBox7.Text = Game.velocity.ToString();
                    }
                    catch { }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox5.Text = new Vector3(textBox2.Text).DistanceTo(new Vector3(textBox3.Text)).ToString();
        }

        bool recallRecordingEnabled = false;
        List<Vector3> recallList = new List<Vector3>();

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
                foreach (Vector3 pos in recallList)
                {
                    Game.teleport(pos);
                    Thread.Sleep(1);
                }
            }).Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            recallList.Reverse();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            mem.WriteMemory(Game.localPlayer + HexHandler.addBytes(Game.localPlayer_XPosition, 16), "float", Game.position.y.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Game.teleport(new Vector3(Game.position.x, Game.position.y - 3.6f, Game.position.z));
            mem.WriteMemory(Game.localPlayer + HexHandler.addBytes(Game.localPlayer_XPosition, 4), "float", (Game.position.y + 3.6f).ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Game.teleport(Game.position);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (IVersion version in VersionClass.versions)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(version.name);

                if (version == VersionClass.versions[0])
                {
                    item.Text = item.Text + " (Current)";
                }

                item.Tag = version.name;
                item.Click += versionSwitched;

                VersionListItem.DropDownItems.Add(item);
            }

            foreach (Control con in Controls)
            {
                if (con.Tag == null) return; // Skip base controls that we dont want to disable (Exc, menuStrip1)

                parseControl(con);
            }

            Keymap km = new Keymap();
            Keymap.keyUp += gameKeyUp;
        }

        void parseControl(Control v) // Disable controls if no valid offset assigned to them
        {
            if (Game.localPlayer == "" && v.Tag != null)
            {
                v.Enabled = false;
                return;
            }

            if (v.Tag == null) return;

            if (v.Tag.ToString() == "t" && Game.localPlayer_XPosition == "")
            {
                v.Enabled = false;
                return;
            }
            else v.Enabled = true;

            if (v.Tag.ToString() == "g" && Game.localPlayer_Gamemode == "")
            {
                v.Enabled = false;
                return;
            }
            else v.Enabled = true;

            if (v.Tag.ToString() == "v" && Game.localPlayer_XVelocity == "")
            {
                v.Enabled = false;
                return;
            }
            else v.Enabled = true;
        }

        private void versionSwitched(object sender, EventArgs e)
        {
            ToolStripMenuItem tagValue = (ToolStripMenuItem)sender;

            foreach (IVersion v in VersionClass.versions)
            {
                if (v.name == tagValue.Tag.ToString())
                {
                    VersionClass.setVersion(v); // Load verison

                    foreach (ToolStripItem c in VersionListItem.DropDownItems)
                    {
                        c.Text = c.Tag.ToString();
                    }

                    tagValue.Text = tagValue.Tag.ToString() + " (Current)";
                }
            }

            foreach (Control con in Controls)
            {
                parseControl(con);
            }
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            TreeNode lab = new TreeNode();
            lab.Tag = Game.position.ToString();
            lab.Text = textBox6.Text;
            treeView1.Nodes.Add(lab);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) => Game.teleport(new Vector3(e.Node.Tag.ToString()));

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) => treeView1.Nodes.Remove(e.Node);

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void killGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process MinecraftProcess = Process.GetProcessesByName("Minecraft.Windows")[0];
            if (MinecraftProcess != null)
                MinecraftProcess.Kill();

            Process ApplicationHost = Process.GetProcessesByName("ApplicationFrameHost")[0]; // Kill main window aswell so it instantly closes
            if (ApplicationHost != null)
                ApplicationHost.Kill();
        }

        private void crashGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process MinecraftProcess = Process.GetProcessesByName("Minecraft.Windows")[0];
            if (MinecraftProcess != null)
                MinecraftProcess.Kill();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Game.velocity = new Vector3(textBox8.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox10.Text.ToLower() == "survival" || textBox10.Text.ToLower() == "s" || textBox10.Text == "0")
            {
                Game.gamemode = 0;
            }
            else if (textBox10.Text.ToLower() == "creative" || textBox10.Text.ToLower() == "c" || textBox10.Text == "1")
            {
                Game.gamemode = 1;
            }
            else if (textBox10.Text.ToLower() == "adventure" || textBox10.Text.ToLower() == "a" || textBox10.Text == "2")
            {
                Game.gamemode = 2;
            }
        }

        List<List<string>> _list = new List<List<string>> { /*
            new List<string>
            {
                "Velocity",
                "0,0,0",
                "c"
            }*/
        }; // Keybinds

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>();
            tempList.Add("Velocity");
            tempList.Add(keybind_Velocity.Text);
            tempList.Add(keybind_Keybind.Text);

            _list.Add(tempList);
        }

        private void gameKeyUp(object sender, KeyEvent e)
        {

        }
    }
}
