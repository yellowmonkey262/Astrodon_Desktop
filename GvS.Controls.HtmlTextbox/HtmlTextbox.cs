// Copyright © 2008 by Gert van 't Slot
// License: Microsoft Public License (Ms-PL)
// Full license text at: http://www.codeplex.com/WinformHtmlTextbox/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace GvS.Controls {
    [ToolboxBitmap(typeof(HtmlTextbox),"WinformHtmlTextboxIco.bmp")]
    public partial class HtmlTextbox : UserControl {

        private const string EDITOR_ID = "theTextbox";

        private bool _browserReady = false;
        private bool _focusHandled = false;
        private Control _lastControlFocused;

        private FrmToolbar _toolbar;

        public HtmlTextbox() {
            InitializeComponent();

            // Call OnPaint when resized
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            // Initialize the state of the toolbar
            this.ShowHideToolbar(); 

            // Load the initial code
            this.theBrowser.DocumentText = Properties.Resources.HtmlContent;
        }

        #region Text

        /// <summary>
        /// Returns the text the user edited in Html format
        /// </summary>
        [Category("Appearance")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(BindableSupport.Yes)]
        public override string Text {
            get {
                return this.SourceText;
            }
            set {
                this.BrowserText = this.SourceText = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new event EventHandler TextChanged {
            add {
                base.TextChanged += value;
            }
            remove {
                base.TextChanged -= value;
            }
        }

        [Category("Appearance")]
        public string PlainText {
            get {
                this.WaitUntilBrowserReady();
                return this.theBrowser.Document.All[EDITOR_ID].InnerText;
            }
        }

        private string BrowserText {
            get {
                this.WaitUntilBrowserReady();
                return this.FilterHtml(this.theBrowser.Document.All[EDITOR_ID].InnerHtml);
            }
            set {
                if (!this._browserReady) {
                    return;
                }
                this.theBrowser.Document.All[EDITOR_ID].InnerHtml = value;
            }
        }

        private string SourceText {
            get {
                return this.FilterHtml(this.txtSource.Text);
            }
            set {
                if (value != this.txtSource.Text) {
                    this.txtSource.Text = value;
                }
            }
        }

        #endregion

        #region ToolbarStyle

        private ToolbarStyles _toolbarStyle = ToolbarStyles.External;
        [DefaultValue(ToolbarStyles.External)]
        [Category("Appearance")]
        public ToolbarStyles ToolbarStyle {
            get { return this._toolbarStyle; }
            set {
                if (value != this._toolbarStyle) {
                    this._toolbarStyle = value;
                    this.OnToolbarStyleChanged(EventArgs.Empty);
                    this.ShowHideAll();
                }
            }
        }

        private object ToolbarStyleChangedEventKey = new object();
        public event EventHandler ToolbarStyleChanged {
            add {
                this.Events.AddHandler(ToolbarStyleChangedEventKey, value);
            }
            remove {
                this.Events.RemoveHandler(ToolbarStyleChangedEventKey, value);
            }
        }
        protected void OnToolbarStyleChanged(EventArgs e) {
            EventHandler evnt;
            if (null != (evnt = (EventHandler)this.Events[ToolbarStyleChangedEventKey])) {
                evnt(this, e);
            }
        }
        
        #endregion

        #region Showing, hiding syncing source <--> html

        private bool _showHtmlSource;
        [Category("Appearance")]
        public bool ShowHtmlSource {
            get {
                return this._showHtmlSource;
            }
            set {
                this._showHtmlSource = value;
                this.tsbViewSource.Checked = value;
                this.ShowHideSourcePanel();
                if (value == false && this.ContainsFocusExtra) {
                    this.WaitUntilBrowserReady();
                    this.theBrowser.Focus();
                }
            }
        }

        private void HandleFocusChanged() {
            if (this._focusHandled == this.ContainsFocus) {
                return;
            }
            this.ShowHideAll();
            this.Refresh();
            if (this.theBrowser.Focused) {
                this.WaitUntilBrowserReady();
                this.theBrowser.Document.InvokeScript("InitFocus");
            }
            this._focusHandled = this.ContainsFocus;
        }

        private void ShowHideAll() {
            this.ShowHideToolbar();
            this.ShowHideSourcePanel();
        }

        /// <summary>
        /// Show the source panel, when
        /// <list type="bullet">
        /// <item>ShowHtmlSource is true</item>
        /// <item>this control has the focus</item>
        /// </list>
        /// </summary>
        private void ShowHideSourcePanel() {
            if (this.ShowHtmlSource && this.ContainsFocusExtra) {
                this.editSplit.Panel2Collapsed = false;
                if (editSplit.SplitterDistance > this.ClientSize.Height) {
                    editSplit.SplitterDistance = editSplit.Height / 2;
                }
            } else {
                this.editSplit.Panel2Collapsed = true;
            }
        }

        /// <summary>
        /// Returns true, if the HtmlTextBox, or the corresponding (external) toolbar contains focus
        /// </summary>
        private bool ContainsFocusExtra {
            get {
                if (this.ContainsFocus) return true;
                if (this._toolbar != null && this._toolbar.ContainsFocus) return true;
                return false;
            }
        }

        internal void ShowHideToolbar() {
            if (ToolbarStyle == ToolbarStyles.External) {
                // External toolbar
                this.toolBar.Visible = false;
                if (this._toolbar == null) {
                    this._toolbar = new FrmToolbar(this);
                    this.components.Add(this._toolbar);
                }
                if (this.ContainsFocusExtra) {
                    this._toolbar.Appear();
                } else {
                    this._toolbar.Disappear();
                }
            } else {
                // No external toolbar
                if (this._toolbar != null) {
                    this._toolbar.Dispose();
                    this._toolbar = null;
                }
                switch (this.ToolbarStyle) {
                    case ToolbarStyles.Internal:
                        if (this.ContainsFocusExtra) {
                            this.toolBar.Visible = true;
                        } else {
                            this.toolBar.Visible = false;
                        }
                        break;
                    case ToolbarStyles.AlwaysInternal:
                        this.toolBar.Visible = true;
                        break;
                    case ToolbarStyles.Hide:
                        this.toolBar.Visible = false;
                        break;
                } // switch
            } // else
        }

        /// <summary>
        /// Returns true, if the browser is focused, or the browser was the last
        /// control focused, before the external toolbar was focused
        /// </summary>
        internal bool IsBrowserFocused {
            get {
                if (this.theBrowser.Focused) {
                    this._lastControlFocused = this.theBrowser;
                    return true;
                }
                if (this.ContainsFocusExtra) {
                    return this._lastControlFocused == this.theBrowser;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns true, if the source view is focused, or the source view was the
        /// last control focused before the external toolbar got the focus.
        /// </summary>
        internal bool IsSourceFocused {
            get {
                return this.txtSource.Focused || this._lastControlFocused == this.txtSource;
            }
        }

        /// <summary>
        /// Call this to give the focus back to the HtmlTextbox from the external toolbar
        /// </summary>
        internal void GetFocusBack() {
            this.Focus();
            if (_lastControlFocused != null) {
                if (this._lastControlFocused == theBrowser) {
                    this.WaitUntilBrowserReady();
                    this.theBrowser.Document.All[EDITOR_ID].Focus();
                } else {
                    this._lastControlFocused.Focus();
                }
            }
        }

        private void tmrSourceSync_Tick(object sender, EventArgs e) {
            if (!this._browserReady) {
                return;
            }
            // Only use CPU cycles if there is work to be performed
            if (this._focusHandled != this.ContainsFocusExtra) {
                this.HandleFocusChanged();
            }

            if (!this.ContainsFocusExtra) {
                this._lastControlFocused = null;
                return;
            }

            // Synchronize the both views            
            if (this.IsBrowserFocused) {
                this.SyncSource();
            } else if (this.IsSourceFocused) {
                this.SyncBrowser();
            }

            if (this._toolbar != null) {
                this._toolbar.UpdateControl(this.theBrowser);
            }

            
            // Synchronize the buttons
            if (IsSourceFocused) {
                this.tsbBold.Enabled = false;
                this.tsbItalic.Enabled = false;
                this.tsbOrderedList.Enabled = false;
                this.tsbBulletList.Enabled = false;
                this.tsbFont.Enabled = false;
                this.tsbFontSize.Enabled = false;
                this.tsbIndent.Enabled = false;
                this.tsbUnIndent.Enabled = false;
            } else {
                this.tsbBold.Enabled = true;
                this.tsbItalic.Enabled = true;
                this.tsbOrderedList.Enabled = true;
                this.tsbBulletList.Enabled = true;
                this.tsbFont.Enabled = true;
                this.tsbFontSize.Enabled = true;
                this.tsbIndent.Enabled = true;
                this.tsbUnIndent.Enabled = true;
            }
            this.tsbBold.Checked = (bool)theBrowser.Document.InvokeScript("IsBold");
            this.tsbItalic.Checked = (bool)theBrowser.Document.InvokeScript("IsItalic");
            this.tsbOrderedList.Checked = (bool)theBrowser.Document.InvokeScript("IsOrderedList");
            this.tsbBulletList.Checked = (bool)theBrowser.Document.InvokeScript("IsBulletList");

            if (!this.tsbFont.Focused) {
                this.tsbFont.Text = (theBrowser.Document.InvokeScript("GetFont") ?? string.Empty).ToString();
            }

            if (!this.tsbFontSize.Focused) {
                this.tsbFontSize.Text = theBrowser.Document.InvokeScript("GetFontSize").ToString();
            }

        }

        /// <summary>
        /// Synchronizes the content of the browser, to the content of the source
        /// textbox.
        /// </summary>
        private void SyncBrowser() {
            this.BrowserText = this.SourceText;
        }

        /// <summary>
        /// Synchronizes the content of the txtSource to the browser
        /// </summary>
        private void SyncSource() {
            this.SourceText = this.BrowserText;
        }

        private void tsbViewSource_CheckedChanged(object sender, EventArgs e) {
            this.ShowHtmlSource = this.tsbViewSource.Checked;
        }

        #endregion

        #region Cut / Copy / Paste

        private void tsbCut_Click(object sender, EventArgs e) {
            if (IsSourceFocused) {
                this.txtSource.Cut();
            } else {
                this.WaitUntilBrowserReady();
                this.theBrowser.Document.ExecCommand("Cut", false, null);
            }
        }

        private void tsbPaste_Click(object sender, EventArgs e) {
            if (IsSourceFocused) {
                this.txtSource.Paste();
            } else {
                this.WaitUntilBrowserReady();
                this.theBrowser.Document.ExecCommand("Paste", false, null);
            }
        }

        private void tsbCopy_Click(object sender, EventArgs e) {
            if (IsSourceFocused) {
                this.txtSource.Copy();
            } else {
                this.WaitUntilBrowserReady();
                this.theBrowser.Document.ExecCommand("Copy", false, null);
            }
        }

        #endregion

        #region Simple style toggles : Bold, Italic, Indent, UnIndent, Lists

        private void tsbBold_Click(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("Bold", false, null);
        }

        private void tsbItalic_Click(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("Italic", false, null);
        }

        private void tsbOrderedList_Click(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("InsertOrderedList", false, null);
        }

        private void tsbBulletList_Click(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("InsertUnOrderedList", false, null);
        }

        private void tsbUnIndent_Click(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("Outdent", false, null);
        }

        private void tsbIndent_Click(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("Indent", false, null);
        }

        #endregion

        #region Handle Font

        private void tsbFont_Leave(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("FontName", false, this.tsbFont.Text);
        }

        private void tsbFont_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                this.WaitUntilBrowserReady();
                this.theBrowser.Document.ExecCommand("FontName", false, this.tsbFont.Text);
                this.theBrowser.Focus();
                e.Handled = true;
            }
        }

        private void tsbFont_SelectedIndexChanged(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("FontName", false, this.tsbFont.Text);
        }

        private void tsbFontSize_SelectedIndexChanged(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("FontSize", false, this.tsbFontSize.Text);
        }

        private void tsbFontSize_Leave(object sender, EventArgs e) {
            this.WaitUntilBrowserReady();
            this.theBrowser.Document.ExecCommand("FontSize", false, this.tsbFontSize.Text);
        }

        private void tsbFontSize_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                this.WaitUntilBrowserReady();
                this.theBrowser.Document.ExecCommand("FontSize", false, this.tsbFontSize.Text);
                this.theBrowser.Focus();
                e.Handled = true;
            }
        }

        #endregion

        #region Misc events: DocumentCompleted, OnPaint, TextChanged

        private void theBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            this._browserReady = true;
            this.SyncBrowser();
            this.SetFontInBrowser();
        }

        private void WaitUntilBrowserReady() {
            if (this._browserReady) {
                return;
            }
            for (int i = 0; i < 60 && !this._browserReady; i++) {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
        }

        private Color _borderColorFocused = SystemColors.ActiveCaption;
        private Color _borderColorNonFocused = SystemColors.ControlDark;

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ActiveCaption")]
        public Color BorderColorFocused {
            get { return this._borderColorFocused; }
            set { this._borderColorFocused = value; }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color BorderColorNonFocused {
            get { return this._borderColorNonFocused; }
            set { this._borderColorNonFocused = value; }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (this.ContainsFocus) {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, this.BorderColorFocused, ButtonBorderStyle.Solid);
            } else {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, this.BorderColorNonFocused, ButtonBorderStyle.Solid);
            }
        }

        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);
            this.HandleFocusChanged();
        }

        private void txtSource_TextChanged(object sender, EventArgs e) {
            this.OnTextChanged(EventArgs.Empty);
        }

        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);
            this.SetFontInBrowser();
        }

        private void SetFontInBrowser() {
            if (this._browserReady) {
                this.theBrowser.Document.InvokeScript("SetFont",
                    new object[] {
                    this.Font.Name,
                    this.Font.Size.ToString(System.Globalization.CultureInfo.InvariantCulture) + "pt"});
            }
        }

        #endregion

        #region Focus management
        protected override void OnEnter(EventArgs e) {
            base.OnEnter(e);
            this.HandleFocusChanged();
        }

        protected override void OnLeave(EventArgs e) {
            base.OnLeave(e);
            this.HandleFocusChanged();
        }
        #endregion

        private string[] _fonts = null;
        private string[] _defaultFonts = new string[] { 
            "Corbel",
            "Corbel, Verdana, Arial, Helvetica, sans-serif",
            "Georgia, Times New Roman, Times, serif",
            "Consolas, Courier New, Courier, monospace" };
        [Category("Behavior")]
        public string[] Fonts {
            get { return _fonts == null ? _defaultFonts : _fonts; }
            set {
                if (value == null || value.Length == 0) {
                    _fonts = null;
                }
                this._fonts = value;

                this.tsbFont.Items.Clear();
                this.tsbFont.Items.AddRange(this._fonts);
                if (this._toolbar != null) {
                    this._toolbar.tsbFont.Items.Clear();
                    this._toolbar.tsbFont.Items.AddRange(this._fonts);
                }
            }
        }

        private static string[] _illegalPatternsDefault = new string[] {
                @"<script.*?>",                            // all <script >
                @"<\w+\s+.*?(j|java|vb|ecma)script:.*?>",  // any tag containing *script:
                @"<\w+(\s+|\s+.*?\s+)on\w+\s*=.+?>",       // any tag containing an attribute starting with "on"
                @"</?input.*?>"                            // <input> and </input>
            };
        private string[] _illegalPatterns = _illegalPatternsDefault;
        /// <summary>
        /// Contains a list of regular expression that are cleared from the html.
        /// Like script, of event handlers
        /// </summary>
        [Category("Behavior")]
        [Description(@"A list of regular expressions that are removed from the html.
To reset, set to single line with *.")]
        public string[] IllegalPatterns {
            get {
                if (this._illegalPatterns == null) {
                    return new string[0];
                }
                return this._illegalPatterns;
            }
            set {
                // When zero length then store a null
                if (value == null || value.Length == 0) {
                    this._illegalPatterns = null;
                    return;
                }
                if (value.Length == 1 && value[0] == "*") {
                    this._illegalPatterns = _illegalPatternsDefault;
                    return;
                }
                // Remove empty & duplicate strings
                List<string> buf = new List<string>();
                foreach (var item in value) {
                    if (!string.IsNullOrEmpty(item) && !buf.Contains(item)) {
                        buf.Add(item);
                    }
                }                
                this._illegalPatterns = buf.Count == 0 ? null : buf.ToArray();
            }
        }

        private string FilterHtml(string original) {
            if (string.IsNullOrEmpty(original) || this.IllegalPatterns.Length == 0) {
                return original;
            }
            string buf = original;
            foreach (var pattern in this.IllegalPatterns) {
                Regex reg = new Regex(pattern,
                    RegexOptions.IgnoreCase |
                    RegexOptions.Multiline |
                    RegexOptions.Singleline);
                buf = reg.Replace(buf, string.Empty);
            }
            System.Diagnostics.Debug.WriteLineIf(buf != original, "Filtered: " + buf);
            return buf;
        }

        private void txtSource_Enter(object sender, EventArgs e) {
            this._lastControlFocused = this.txtSource;
        }

        protected override bool ProcessDialogKey(Keys keyData) {
            // If Enter is pressed, this key does not need to be handled by the dialog,
            if (keyData == Keys.Return) {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
