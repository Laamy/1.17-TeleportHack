// using KeraLua;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using XYZ_Teleport_1._17._0._Keymap;
using XYZ_Teleport_1._17._0.LuaBase;
using XYZ_Teleport_1._17._0.LuaBase.MLuaBase;
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

            new Keymap(); // Start key handle

            Keymap.keyEvent += gameKey_Clicked;
            Keymap.keyEvent += luaKeyEvents;

            handle = this;
            mem = new Mem();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (!Program.quit)
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
            // Text = Keymap.e.ToString();
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
            LuaEditor t = new LuaEditor();
            t.Show();

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

        List<List<string>> _list = new List<List<string>>
        { /*
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
            tempList.Add(keybind_Keybind.Text.ToUpper());

            _list.Add(tempList);
        }

        private void gameKey_Clicked(object sender, KeyEvent e) // Untested code
        {
            /*if (e.vkey == vKeyCodes.KeyUp) // KeyUp event
            {
                if (e.key == Keys.C)
                    MessageBox.Show("Worked!");
            }*/
            if (e.vkey == vKeyCodes.KeyUp) // KeyUp event
            {
                //if (_list == null || _list.Count == 0) return;

                foreach (List<string> list in _list)
                {
                    //MessageBox.Show(list[0]);
                    if (list[0] == "Velocity") // Velocity keybind
                    {
                        // ++Keymap.e;
                        if (e.key == (Keys)list[2].ToCharArray()[0])
                        {
                            Game.velocity = Base.Vec3(list[1]);
                        }
                    }
                    else if (list[0] == "Teleportation") // Teleportation keybind
                    {
                        if (e.key == (Keys)list[2].ToCharArray()[0])
                        {
                            Game.teleport(Base.Vec3(list[1]));
                        }
                    }
                    else if (list[0] == "Gamemode") // Gamemode keybind
                    {
                        if (e.key == (Keys)list[2].ToCharArray()[0])
                        {
                            List<List<string>> registery;
                            new GamemodeRegistery(out registery);
                            for (int i = 0; i < registery.Count;) // Loop through gamemode registery
                            {
                                foreach (string str in registery[i])
                                {
                                    if (list[1].ToLower() == str)
                                        Game.gamemode = i;
                                }
                                ++i;
                            }
                        }
                    }
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>();
            tempList.Add("Teleportation");
            tempList.Add(toolStripTextBox1.Text);
            tempList.Add(toolStripTextBox2.Text.ToUpper());

            _list.Add(tempList);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>();
            tempList.Add("Gamemode");
            tempList.Add(toolStripTextBox3.Text.ToLower());
            tempList.Add(toolStripTextBox4.Text.ToUpper());

            _list.Add(tempList);
        }

        public static List<Lua> luaEnvs = new List<Lua>(); // unsure if this is fully embedded ;-; might be broken due to NLua being shitty
        public static List<List<object>> luaEvents = new List<List<object>>
        {
            /*new List<object>
            {
                // EventKind
                // LuaFunction
            }*/
        };

        public static Lua executeLua(string v)
        {
            Lua newEnv = new Lua();

            newEnv["mlua"] = new MLua(VersionClass.currentVersion);

            newEnv.DoString(v);

            luaEvents.Add(new List<object>
            {
                "KeyPress",
                newEnv.GetFunction("on_KeyPress") // I have a new idea for these events
            });

            luaEnvs.Add(newEnv); // List the envs so we can call event functions

            return newEnv;
        }

        private void luaKeyEvents(object sender, KeyEvent e)
        {
            if (e.vkey == vKeyCodes.KeyDown)
            {
                foreach (var v in luaEvents)
                {
                    if (v[0].ToString() == "KeyPress")
                    {
                        LuaFunction func = (LuaFunction)v[1];
                        func.Call(e.vkey, e.key);
                    }
                }
            }
        }

        private void loadLuaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Text File";
            dlg.Filter = "MCLua files (*.mclua)|*.mclua|All files (*.*)|*.*";
            dlg.InitialDirectory = @"C:\";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    executeLua(File.ReadAllText(dlg.FileName));
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.quit = true;
        }
    }
}
