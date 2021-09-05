using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace XYZ_Teleport_1._17._0.LuaBase
{
	public class SyntaxRichTextBox : System.Windows.Forms.RichTextBox
	{
		private SyntaxSettings m_settings = new SyntaxSettings();
		private static bool m_bPaint = true;
		private string m_strLine = "";
		private int m_nContentLength = 0;
		private int m_nLineLength = 0;
		private int m_nLineStart = 0;
		private int m_nLineEnd = 0;
		private string m_strKeywords = "";
		private string m_strkeyvoid = "";
		private string m_strkeyc = "";
		private int m_nCurSelection = 0;

		/// <summary>
		/// The settings.
		/// </summary>
		public SyntaxSettings Settings
		{
			get { return m_settings; }
		}

		/// <summary>
		/// WndProc
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if (m.Msg == 0x00f)
			{
				if (m_bPaint)
					base.WndProc(ref m);
				else
					m.Result = IntPtr.Zero;
			}
			else
				base.WndProc(ref m);
		}
		/// <summary>
		/// OnTextChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Calculate shit here.
			m_nContentLength = this.TextLength;

			int nCurrentSelectionStart = SelectionStart;
			int nCurrentSelectionLength = SelectionLength;

			m_bPaint = false;

			// Find the start of the current line.
			m_nLineStart = nCurrentSelectionStart;
			while ((m_nLineStart > 0) && (Text[m_nLineStart - 1] != '\n'))
				m_nLineStart--;
			// Find the end of the current line.
			m_nLineEnd = nCurrentSelectionStart;
			while ((m_nLineEnd < Text.Length) && (Text[m_nLineEnd] != '\n'))
				m_nLineEnd++;
			// Calculate the length of the line.
			m_nLineLength = m_nLineEnd - m_nLineStart;
			// Get the current line.
			m_strLine = Text.Substring(m_nLineStart, m_nLineLength);

			// Process this line.
			ProcessLine();

			m_bPaint = true;
		}
		/// <summary>
		/// Process a line.
		/// </summary>
		private void ProcessLine()
		{
			// Save the position and make the whole line black
			int nPosition = SelectionStart;
			SelectionStart = m_nLineStart;
			SelectionLength = m_nLineLength;
			SelectionColor = Color.Black;

			// Process the keywords
			ProcessRegex(m_strKeywords, Settings.KeywordColor);
			ProcessRegex(m_strkeyvoid, Settings.KeyvoidColor);
			ProcessRegex(m_strkeyc, Settings.KeycColor);
			// Process numbers
			if (Settings.EnableIntegers)
				ProcessRegex("\\b(?:[0-9]*\\.)?[0-9]+\\b", Settings.IntegerColor);
			// Process strings
			if (Settings.EnableStrings)
				ProcessRegex("\"[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*\"", Settings.StringColor);
			// Process comments
			if (Settings.EnableComments && !string.IsNullOrEmpty(Settings.Comment))
				ProcessRegex(Settings.Comment + ".*$", Settings.CommentColor);

			SelectionStart = nPosition;
			SelectionLength = 0;
			SelectionColor = Color.Black;

			m_nCurSelection = nPosition;
		}
		private void ProcessRegex(string strRegex, Color color)
		{
			Regex regKeywords = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Match regMatch;

			for (regMatch = regKeywords.Match(m_strLine); regMatch.Success; regMatch = regMatch.NextMatch())
			{
				// Process the words
				int nStart = m_nLineStart + regMatch.Index;
				int nLenght = regMatch.Length;
				SelectionStart = nStart;
				SelectionLength = nLenght;
				SelectionColor = color;
			}
		}
		public void Compile()
		{
			for (int i = 0; i < Settings.Keywords.Count; i++)
			{
				string strKeyword = Settings.Keywords[i];

				if (i == Settings.Keywords.Count - 1)
					m_strKeywords += "\\b" + strKeyword + "\\b";
				else
					m_strKeywords += "\\b" + strKeyword + "\\b|";
			}

			for (int i = 0; i < Settings.Keyvoid.Count; i++)
			{
				string strKeyword = Settings.Keyvoid[i];

				if (i == Settings.Keyvoid.Count - 1)
					m_strkeyvoid += "\\b" + strKeyword + "\\b";
				else
					m_strkeyvoid += "\\b" + strKeyword + "\\b|";
			}

			for (int i = 0; i < Settings.Keyc.Count; i++)
			{
				string strKeyword = Settings.Keyc[i];

				if (i == Settings.Keyc.Count - 1)
					m_strkeyc += "\\b" + strKeyword + "\\b";
				else
					m_strkeyc += "\\b" + strKeyword + "\\b|";
			}
		}

		public void ProcessAllLines()
		{
			m_bPaint = false;

			int nStartPos = 0;
			int i = 0;
			int nOriginalPos = SelectionStart;
			while (i < Lines.Length)
			{
				m_strLine = Lines[i];
				m_nLineStart = nStartPos;
				m_nLineEnd = m_nLineStart + m_strLine.Length;

				ProcessLine();
				i++;

				nStartPos += m_strLine.Length + 1;
			}

			m_bPaint = true;
		}
	}

	/// <summary>
	/// Class to store syntax objects in.
	/// </summary>
	public class SyntaxList
	{
		public List<string> keywordList = new List<string>();
		public Color keywordColor = new Color();

		public List<string> keyvoidList = new List<string>();
		public Color keyvoidColor = new Color();

		public List<string> keycList = new List<string>();
		public Color keycColor = new Color();
	}

	/// <summary>
	/// Settings for the keywords and colors.
	/// </summary>
	public class SyntaxSettings
	{
		SyntaxList m_KeyWords = new SyntaxList();
		string m_strComment = "";
		Color m_colorComment = Color.Green;
		Color m_colorString = Color.Gray;
		Color m_colorInteger = Color.Red;
		bool m_bEnableComments = true;
		bool m_bEnableIntegers = true;
		bool m_bEnableStrings = true;

		#region Properties
		public List<string> Keyc
		{
			get { return m_KeyWords.keycList; }
		}
		public Color KeycColor
		{
			get { return m_KeyWords.keycColor; }
			set { m_KeyWords.keycColor = value; }
		}
		public List<string> Keyvoid
		{
			get { return m_KeyWords.keyvoidList; }
		}
		public Color KeyvoidColor
		{
			get { return m_KeyWords.keyvoidColor; }
			set { m_KeyWords.keyvoidColor = value; }
		}
		public List<string> Keywords
		{
			get { return m_KeyWords.keywordList; }
		}
		public Color KeywordColor
		{
			get { return m_KeyWords.keywordColor; }
			set { m_KeyWords.keywordColor = value; }
		}
		public string Comment
		{
			get { return m_strComment; }
			set { m_strComment = value; }
		}
		public Color CommentColor
		{
			get { return m_colorComment; }
			set { m_colorComment = value; }
		}
		public bool EnableComments
		{
			get { return m_bEnableComments; }
			set { m_bEnableComments = value; }
		}
		public bool EnableIntegers
		{
			get { return m_bEnableIntegers; }
			set { m_bEnableIntegers = value; }
		}
		public bool EnableStrings
		{
			get { return m_bEnableStrings; }
			set { m_bEnableStrings = value; }
		}
		public Color StringColor
		{
			get { return m_colorString; }
			set { m_colorString = value; }
		}
		public Color IntegerColor
		{
			get { return m_colorInteger; }
			set { m_colorInteger = value; }
		}
		#endregion
	}
}
