using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                "end",
                "true",
                "false",
                "local",
                "return"
            });
            luaTextbox.Settings.Keyvoid.AddRange(new string[]{ // Add lua statements
                "print"
            });

            luaTextbox.Settings.Comment = "--";

            luaTextbox.Settings.KeywordColor = Color.Blue;
            luaTextbox.Settings.KeyvoidColor = Color.DeepPink;
            luaTextbox.Settings.CommentColor = Color.Green;
            luaTextbox.Settings.StringColor = Color.IndianRed;
            luaTextbox.Settings.IntegerColor = Color.Red;

            luaTextbox.Settings.EnableStrings = true;
            luaTextbox.Settings.EnableIntegers = true;

            luaTextbox.CompileKeywords();
            luaTextbox.CompileKeyvoid();

            luaTextbox.TextChanged += ValidateLuaFormat;
            luaTextbox.KeyDown += CustomKeys;

            luaTextbox.Font = new Font(luaTextbox.SelectionFont.FontFamily, 12.0f);

            float selectionTabSize = 12f; // Pretty nice size if i do say so myself :thinking:

            selectionTabSize = selectionTabSize * (luaTextbox.Font.Size/6);
            luaTextbox.SelectionTabs = new int[] { (int)selectionTabSize, (int)(selectionTabSize * 2), (int)(selectionTabSize * 3), (int)(selectionTabSize * 4) };

            luaTextbox.AcceptsTab = true;

            Controls.Add(luaTextbox);
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
    }
}
