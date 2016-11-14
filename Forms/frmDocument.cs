using NetSpell.SpellChecker;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Astrodon.Forms {

    public partial class frmDocument : Form {
        public String header = String.Empty;
        public String footer = String.Empty;
        public String rtf = String.Empty;
        private String content = String.Empty;
        private Building building;
        private NetSpell.SpellChecker.Spelling spellCheck;
        private NetSpell.SpellChecker.Dictionary.WordDictionary dictionary;

        public frmDocument(String template, Building build) {
            building = build;
            InitializeComponent();
            if (!String.IsNullOrEmpty(template)) { content = template; }
            spellCheck = new Spelling();
            dictionary = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            dictionary.DictionaryFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dic");
            spellCheck.Dictionary = dictionary;
            spellCheck.EndOfText += spellCheck_EndOfText;
            spellCheck.DeletedWord += spellCheck_DeletedWord;
            spellCheck.ReplacedWord += spellCheck_ReplacedWord;
        }

        private void spellCheck_ReplacedWord(object sender, ReplaceWordEventArgs e) {
            int start = this.rtfEditor.SelectionStart;
            int length = this.rtfEditor.SelectionLength;

            this.rtfEditor.Select(e.TextIndex, e.Word.Length);
            this.rtfEditor.SelectedText = e.ReplacementWord;

            if (start > this.rtfEditor.Text.Length) { start = this.rtfEditor.Text.Length; }

            if ((start + length) > this.rtfEditor.Text.Length) { length = 0; }

            this.rtfEditor.Select(start, length);
        }

        private void spellCheck_DeletedWord(object sender, SpellingEventArgs e) {
            int start = this.rtfEditor.SelectionStart;
            int length = this.rtfEditor.SelectionLength;

            this.rtfEditor.Select(e.TextIndex, e.Word.Length);
            this.rtfEditor.SelectedText = "";

            if (start > this.rtfEditor.Text.Length) { start = this.rtfEditor.Text.Length; }
            if ((start + length) > this.rtfEditor.Text.Length) { length = 0; }

            this.rtfEditor.Select(start, length);
        }

        private void spellCheck_EndOfText(object sender, EventArgs e) {
            MessageBox.Show("End of text");
        }

        private String[] AddHeaderLine() {
            String headerLine = "We write in our capacity as the Managing Agent and on behalf of the " + (building.Name.ToLower().Contains("hoa") ? "Directors" : "Trustees") + " of " + building.letterName + ".";
            String footerLine = "Should you have any queries, please do not hesitate to contact writer hereof.";
            String[] lines = new string[] { headerLine, footerLine };
            return lines;
        }

        private void frmDocument_Load(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(content)) {
                rtfEditor.Rtf = content;
            } else if (building != null) {
                rtfEditor.AppendText(AddHeaderLine()[0]);
                rtfEditor.AppendText("\r\n\r\n");
                rtfEditor.AppendText(AddHeaderLine()[1]);
            }
        }

        private void btnBold_Click(object sender, EventArgs e) {
            SetFontStyle(rtfEditor, FontStyle.Bold);
        }

        private void btnUnder_Click(object sender, EventArgs e) {
            SetFontStyle(rtfEditor, FontStyle.Underline);
        }

        private void btnItalic_Click(object sender, EventArgs e) {
            SetFontStyle(rtfEditor, FontStyle.Italic);
        }

        private void SetFontStyle(RichTextBox rtb, FontStyle style) {
            rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style ^ style);
        }

        private void btnLeft_Click(object sender, EventArgs e) {
            rtfEditor.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void btnMid_Click(object sender, EventArgs e) {
            rtfEditor.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void btnRight_Click(object sender, EventArgs e) {
            rtfEditor.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            rtf = rtfEditor.Rtf;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private int selectionPos;

        private void rtfEditor_TextChanged(object sender, EventArgs e) {
        }

        private void btnPaste_Click(object sender, EventArgs e) {
            IDataObject iData = Clipboard.GetDataObject();
            // Is Data Text?
            if (iData.GetDataPresent(DataFormats.Rtf)) {
                selectionPos = rtfEditor.Text.Length;
                rtfEditor.Text += (String)iData.GetData(DataFormats.Text);
                rtfEditor.SelectionStart = selectionPos;
                rtfEditor.SelectionLength = 1;
                while (rtfEditor.SelectionStart < rtfEditor.Text.Length) {
                    if (rtfEditor.SelectionFont.FontFamily != rtfEditor.Font.FontFamily || rtfEditor.SelectionFont.Size != rtfEditor.Font.Size) {
                        rtfEditor.SelectionFont = new Font(rtfEditor.Font.FontFamily, rtfEditor.Font.Size, rtfEditor.SelectionFont.Style);
                    }
                    rtfEditor.SelectionStart = rtfEditor.SelectionStart + 1;
                    rtfEditor.SelectionLength = 1;
                }
            }
        }

        private void btnAttach_Click(object sender, EventArgs e) {
            rtfEditor.AppendText(Environment.NewLine + "[attachment]" + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e) {
            rtfEditor.AppendText(Environment.NewLine + "[pagebreak]" + Environment.NewLine);
        }

        private void btnSpell_Click(object sender, EventArgs e) {
            this.spellCheck.Text = this.rtfEditor.Text;
            this.spellCheck.SpellCheck();
        }
    }
}