using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace XYZ_Teleport_1._17._0.LuaBase
{
    public partial class LuaEditor : Form
    {
        SyntaxRichTextBox luaTextbox;
        public LuaEditor()
        {
            InitializeComponent();

            luaTextbox = new SyntaxRichTextBox();
            luaTextbox.Dock = DockStyle.Fill;

            luaTextbox.Settings.Keywords.AddRange(new string[]{ // Add lua statements
                "function",
                "if",
                "then",
                "else",
                "elseif",
                "do",
                "end",
                "while",
                "true",
                "repeat",
                "break",
                "for",
                "until",
                "false",
                "local",
                "return",
                "nil"
            });
            luaTextbox.Settings.Keyvoid.AddRange(new string[]{
                "print",
                //"mlua:exit()",
                //"mlua:Base()", // local vec3 = mlua:Base().Vec3(0,0,0) ; create a vector3 in lua
                "mlua:getLocalPlayer()",
            });
            luaTextbox.Settings.Keyc.AddRange(new string[]{
                "mlua"
            });

            luaTextbox.Settings.Comment = "--";

            luaTextbox.Settings.KeywordColor = Color.Blue;
            luaTextbox.Settings.KeyvoidColor = Color.LightSkyBlue;
            luaTextbox.Settings.KeycColor = Color.BlueViolet;
            luaTextbox.Settings.CommentColor = Color.Green;
            luaTextbox.Settings.StringColor = Color.IndianRed;
            luaTextbox.Settings.IntegerColor = Color.Red;

            luaTextbox.Settings.EnableStrings = true;
            luaTextbox.Settings.EnableIntegers = true;

            luaTextbox.Compile();

            luaTextbox.TextChanged += ValidateLuaFormat;
            luaTextbox.KeyDown += CustomKeys;
            luaTextbox.FontChanged += RecalculateTab;

            luaTextbox.Font = new Font(luaTextbox.SelectionFont.FontFamily, 16f);

            luaTextbox.AcceptsTab = true;

            Controls.Add(luaTextbox);

            luaTextbox.BringToFront();
        }

        float vSelectionTabSize = 12f;

        private void RecalculateTab(object sender, EventArgs e)
        {
            float selectionTabSize = vSelectionTabSize / 6;

            selectionTabSize = selectionTabSize * (luaTextbox.Font.Size);
            luaTextbox.SelectionTabs = new int[] { (int)selectionTabSize, (int)(selectionTabSize * 2), (int)(selectionTabSize * 3), (int)(selectionTabSize * 4) };
        }

        private void CustomKeys(object sender, KeyEventArgs e)
        {
            // MessageBox.Show(((int)e.KeyCode).ToString());

            if (e.KeyCode == Keys.OemOpenBrackets && e.Shift) // Add BracketsAutocomplete
            {
                e.Handled = true;
                luaTextbox.Text = luaTextbox.Text.Insert(luaTextbox.SelectionStart, ")");
            }
        }

        private void ValidateLuaFormat(object sender, EventArgs e)
        {
            luaTextbox.ProcessAllLines();
        }

        private void LuaEditor_Load(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            luaTextbox.Clear();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            luaTextbox.Undo();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            luaTextbox.Redo();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "script.mclua";
            savefile.Filter = "MCLua files (*.mclua)|*.mclua|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                {
                    sw.Write(luaTextbox.Text);
                }
            }
        }

        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Text File";
            dlg.Filter = "MCLua files (*.mclua)|*.mclua|All files (*.*)|*.*";
            dlg.InitialDirectory = @"C:\";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    luaTextbox.Text = File.ReadAllText(dlg.FileName);
                }
            }
            luaTextbox.ProcessAllLines();
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            luaTextbox.ProcessAllLines();
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                luaTextbox.Font = new Font(luaTextbox.SelectionFont.FontFamily, Convert.ToSingle(FontSize.Text));
                luaTextbox.ProcessAllLines();
            }
            catch { }
        }

        private void TabSizeBox_TextChanged(object sender, EventArgs e)
        {
            try // Wont function
            {
                vSelectionTabSize = Convert.ToSingle(TabSizeBox.Text);
                luaTextbox.ProcessAllLines();
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Program.quit)
                Application.Exit();
        }
    }
}
