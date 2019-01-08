// Copyright © 2008 by Gert van 't Slot
// License: Microsoft Public License (Ms-PL)
// Full license text at: http://www.codeplex.com/WinformHtmlTextbox/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace GvS.Controls {
    internal partial class FrmToolbar : Form {

        private HtmlTextbox _htmlTextBox;
        private WebBrowser _theBrowser;

        const double DEFAULT_OPACITY = 0.7;
        const double OPACITY_STEP = 0.1;
        
        public FrmToolbar(HtmlTextbox parent) {
            InitializeComponent();
            this.HtmlTextBox = parent;
            this._theBrowser = parent.theBrowser;

            // Copy font items from parent
            this.tsbFont.Items.Clear();
            this.tsbFont.Items.AddRange(this.HtmlTextBox.Fonts);

            this.AttachMouseOnChildren();
        }

        /// <summary>
        /// A pointer back to the HtmlTextBox that is the parent of this form
        /// </summary>
        public HtmlTextbox HtmlTextBox {
            get { return this._htmlTextBox; }
            private set { 
                this._htmlTextBox = value;
                if (value == null) {
                    return;
                }
                this._htmlTextBox.Move += new EventHandler(HtmlBox_Move);
                if (_htmlTextBox.FindForm() == null) {
                    this._htmlTextBox.ParentChanged += new EventHandler(HtmlTextBox_ParentChanged);
                } else {
                    this._htmlTextBox.FindForm().Move += new EventHandler(HtmlBox_Move);
                }
            }
        }

        #region Events attached to the parent HtmlTextBox

        void HtmlTextBox_ParentChanged(object sender, EventArgs e) {
            // This is watched to react to movements of the form
            if (this._htmlTextBox.FindForm() != null) {
                this._htmlTextBox.FindForm().Move += new EventHandler(HtmlBox_Move);
            }
        }

        void HtmlBox_Move(object sender, EventArgs e) {
            this.FindGoodSpot();
        }

        #endregion

        #region Show / Hide / Location

        /// <summary>
        /// If the form is not visible, make it appear
        /// </summary>
        public void Appear() {
            if (this.Visible) {
                return;
            }
            this.FindGoodSpot();
            this.Opacity = OPACITY_STEP;
            this.tmrAppear.Enabled = true;
            this.Visible = true;
            // Give focus back to the calling form, when made visible, focus is changed
            this.HtmlTextBox.GetFocusBack();
        }

        /// <summary>
        /// Hide the toolbar form
        /// </summary>
        public void Disappear() {
            this.Visible = false;
        }

        private void tmrAppear_Tick(object sender, EventArgs e) {
            double newOpacity = this.Opacity + OPACITY_STEP;
            if (newOpacity > DEFAULT_OPACITY) {
                this.Opacity = DEFAULT_OPACITY;
                this.tmrAppear.Enabled = false;
                return;
            }
            this.Opacity = newOpacity;
        }

        /// <summary>
        /// <para>Find a good location to hover</para>
        /// <para>If possible, try to locate the form on the top left border</para>
        /// </summary>
        public void FindGoodSpot()
        {
            if (this.IsDisposed)
            {
                return;
            }

            HtmlTextbox htmlTextBox = this.HtmlTextBox;
            if (htmlTextBox == null || htmlTextBox.FindForm() == null)
            {
                return;
            }

            Rectangle bounds = htmlTextBox.Bounds;
            Rectangle rctParent = htmlTextBox.Parent.RectangleToScreen(bounds);

            bool onTop = true;
            if (this.Height > rctParent.Top)
            {
                onTop = false;
            }

            this.Left = rctParent.Left;
            this.Top = onTop ? rctParent.Top - this.Height : rctParent.Bottom;
        }

        #endregion

        #region MouseEnter & Leave

        private bool _childControlsAttached = false;

        /// <summary>
        /// Attach enter & leave events to child controls (recursive), this is needed for the ContainerEnter & 
        /// ContainerLeave methods.
        /// </summary>
        private void AttachMouseOnChildren() {
            if (_childControlsAttached) {
                return;
            }
            this.AttachMouseOnChildren(this.Controls);
            _childControlsAttached = true;
        }

        /// <summary>
        /// Attach the enter & leave events on a specific controls collection. The attachment
        /// is recursive.
        /// </summary>
        /// <param name="controls">The collection of child controls</param>
        private void AttachMouseOnChildren(System.Collections.IEnumerable controls) {
            foreach (Control item in controls) {
                item.MouseLeave += new EventHandler(item_MouseLeave);
                item.MouseEnter += new EventHandler(item_MouseEnter);
                this.AttachMouseOnChildren(item.Controls);
            }
        }

        /// <summary>
        /// Will be called by a MouseEnter event, with any of the controls within this
        /// </summary>
        void item_MouseEnter(object sender, EventArgs e) {
            this.OnMouseEnter(e);
        }

        /// <summary>
        /// Will be called by a MouseLeave event, with any of the controls within this
        /// </summary>
        void item_MouseLeave(object sender, EventArgs e) {
            this.OnMouseLeave(e);
        }

        /// <summary>
        /// Flag if the mouse is "entered" in this control, or any of its children
        /// </summary>
        private bool _containsMouse = false;

        /// <summary>
        /// Is called when the mouse entered the Form, or any of its children without entering
        /// the form itself first.
        /// </summary>
        protected void OnContainerEnter(EventArgs e) {
            // No longer transparent
            this.Opacity = 1;
            this.tmrAppear.Enabled = false;
            // Focus this control, this will let the toolbar act on first click
            this.Focus();
        }

        /// <summary>
        /// Is called when the mouse leaves the form. When the mouse leaves the form via one of
        /// its children, this will also call OnContainerLeave
        /// </summary>
        /// <param name="e"></param>
        protected void OnContainerLeave(EventArgs e) {
            this.Opacity = DEFAULT_OPACITY;
            // Focus the Textbox
            this.HtmlTextBox.GetFocusBack();
        }

        /// <summary>
        /// <para>Is called when a MouseLeave occurs on this form, or any of its children</para>
        /// <para>Calculates if OnContainerLeave should be called</para>
        /// </summary>
        protected override void OnMouseLeave(EventArgs e) {
            Point clientMouse = PointToClient(Control.MousePosition);
            if (!ClientRectangle.Contains(clientMouse)) {
                this._containsMouse = false;
                OnContainerLeave(e);
            }
        }
        /// <summary>
        /// <para>Is called when a MouseEnter occurs on this form, or any of its children</para>
        /// <para>Calculates if OnContainerEnter should be called</para>
        /// </summary>
        protected override void OnMouseEnter(EventArgs e) {
            if (!this._containsMouse) {
                _containsMouse = true;
                OnContainerEnter(e);
            }
        }

        #endregion

        #region Toolbar management

        /// <summary>
        /// Update the toolbar according to the selected text in the webbrowser
        /// </summary>
        /// <param name="theBrowser">The browser control in the textbox</param>
        public void UpdateControl(WebBrowser theBrowser) {
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
            // Synchronize the buttons
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

        private void tsbBold_Click(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("Bold", false, null);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbItalic_Click(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("Italic", false, null);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbOrderedList_Click(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("InsertOrderedList", false, null);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbBulletList_Click(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("InsertUnOrderedList", false, null);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbUnIndent_Click(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("Outdent", false, null);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbIndent_Click(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("Indent", false, null);
            this.HtmlTextBox.GetFocusBack();
        }

        #region Cut / Copy / Paste

        bool IsSourceFocused {
            get {
                return this.HtmlTextBox.IsSourceFocused;
            }
        }

        TextBox txtSource {
            get {
                return this.HtmlTextBox.txtSource;
            }
        }

        private void tsbCut_Click(object sender, EventArgs e) {
            if (IsSourceFocused) {
                this.txtSource.Cut();
            } else {
                this._theBrowser.Document.ExecCommand("Cut", false, null);
            }
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbPaste_Click(object sender, EventArgs e) {
            if (IsSourceFocused) {
                this.txtSource.Paste();
            } else {
                this._theBrowser.Document.ExecCommand("Paste", false, null);
            }
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbCopy_Click(object sender, EventArgs e) {
            if (IsSourceFocused) {
                this.txtSource.Copy();
            } else {
                this._theBrowser.Document.ExecCommand("Copy", false, null);
            }
            this.HtmlTextBox.GetFocusBack();
        }

        #endregion

        private void tsbViewSource_CheckedChanged(object sender, EventArgs e) {
            this.HtmlTextBox.ShowHtmlSource = this.tsbViewSource.Checked;
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbFont_Leave(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("FontName", false, this.tsbFont.Text);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbFont_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                this._theBrowser.Document.ExecCommand("FontName", false, this.tsbFont.Text);
                this.HtmlTextBox.GetFocusBack(); 
                e.Handled = true;
            }
        }

        private void tsbFont_SelectedIndexChanged(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("FontName", false, this.tsbFont.Text);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbFontSize_SelectedIndexChanged(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("FontSize", false, this.tsbFontSize.Text);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbFontSize_Leave(object sender, EventArgs e) {
            this._theBrowser.Document.ExecCommand("FontSize", false, this.tsbFontSize.Text);
            this.HtmlTextBox.GetFocusBack();
        }

        private void tsbFontSize_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                this._theBrowser.Document.ExecCommand("FontSize", false, this.tsbFontSize.Text);
                this.HtmlTextBox.GetFocusBack();
                e.Handled = true;
            }
        }

        #endregion

    }
}
