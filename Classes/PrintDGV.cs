using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Astrodon {

    internal class PrintDGV {
        private static StringFormat StrFormat;  // Holds content of a TextBox Cell to write by DrawString
        private static StringFormat StrFormatComboBox; // Holds content of a Boolean Cell to write by DrawImage
        private static Button CellButton;       // Holds the Contents of Button Cell
        private static CheckBox CellCheckBox;   // Holds the Contents of CheckBox Cell
        private static ComboBox CellComboBox;   // Holds the Contents of ComboBox Cell

        private static int TotalWidth;          // Summation of Columns widths
        private static int RowPos;              // Position of currently printing row
        private static bool NewPage;            // Indicates if a new page reached
        private static int PageNo;              // Number of pages to print
        private static ArrayList ColumnLefts = new ArrayList();  // Left Coordinate of Columns
        private static ArrayList ColumnWidths = new ArrayList(); // Width of Columns
        private static ArrayList ColumnTypes = new ArrayList();  // DataType of Columns
        private static int CellHeight;          // Height of DataGrid Cell
        private static int RowsPerPage;         // Number of Rows per Page

        private static System.Drawing.Printing.PrintDocument printDoc =
                       new System.Drawing.Printing.PrintDocument();  // PrintDocumnet Object used for printing

        private static string PrintTitle = "";  // Header of pages
        private static DataGridView dgv;        // Holds DataGridView Object to print its contents
        private static List<string> SelectedColumns = new List<string>();   // The Columns Selected by user to print.
        private static List<string> AvailableColumns = new List<string>();  // All Columns avaiable in DataGrid
        private static bool PrintAllRows = true;   // True = print all rows,  False = print selected rows
        private static bool FitToPageWidth = true; // True = Fits selected columns to page width ,  False = Print columns as showed
        private static int HeaderHeight = 0;

        public static void Print_DataGridView(DataGridView dgv1) {
            PrintPreviewDialog ppvw;
            try {
                // Getting DataGridView object to print
                dgv = dgv1;

                // Getting all Coulmns Names in the DataGridView
                AvailableColumns.Clear();
                foreach (DataGridViewColumn c in dgv.Columns) {
                    if (!c.Visible)
                        continue;
                    AvailableColumns.Add(c.HeaderText);
                }

                // Showing the PrintOption Form
                PrintOptions dlg = new PrintOptions(AvailableColumns);
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                PrintTitle = dlg.PrintTitle;
                PrintAllRows = dlg.PrintAllRows;
                FitToPageWidth = dlg.FitToPageWidth;
                SelectedColumns = dlg.GetSelectedColumns();

                RowsPerPage = 0;

                ppvw = new PrintPreviewDialog();
                ppvw.Document = printDoc;

                // Showing the Print Preview Page
                printDoc.BeginPrint += new System.Drawing.Printing.PrintEventHandler(PrintDoc_BeginPrint);
                printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintDoc_PrintPage);
                if (ppvw.ShowDialog() != DialogResult.OK) {
                    printDoc.BeginPrint -= new System.Drawing.Printing.PrintEventHandler(PrintDoc_BeginPrint);
                    printDoc.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(PrintDoc_PrintPage);
                    return;
                }

                // Printing the Documnet
                printDoc.Print();
                printDoc.BeginPrint -= new System.Drawing.Printing.PrintEventHandler(PrintDoc_BeginPrint);
                printDoc.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(PrintDoc_PrintPage);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
            }
        }

        private static void PrintDoc_BeginPrint(object sender,
                    System.Drawing.Printing.PrintEventArgs e) {
            try {
                // Formatting the Content of Text Cell to print
                StrFormat = new StringFormat();
                StrFormat.Alignment = StringAlignment.Near;
                StrFormat.LineAlignment = StringAlignment.Center;
                StrFormat.Trimming = StringTrimming.EllipsisCharacter;

                // Formatting the Content of Combo Cells to print
                StrFormatComboBox = new StringFormat();
                StrFormatComboBox.LineAlignment = StringAlignment.Center;
                StrFormatComboBox.FormatFlags = StringFormatFlags.NoWrap;
                StrFormatComboBox.Trimming = StringTrimming.EllipsisCharacter;

                ColumnLefts.Clear();
                ColumnWidths.Clear();
                ColumnTypes.Clear();
                CellHeight = 0;
                RowsPerPage = 0;

                // For various column types
                CellButton = new Button();
                CellCheckBox = new CheckBox();
                CellComboBox = new ComboBox();

                // Calculating Total Widths
                TotalWidth = 0;
                foreach (DataGridViewColumn GridCol in dgv.Columns) {
                    if (!GridCol.Visible)
                        continue;
                    if (!PrintDGV.SelectedColumns.Contains(GridCol.HeaderText))
                        continue;
                    TotalWidth += GridCol.Width;
                }
                PageNo = 1;
                NewPage = true;
                RowPos = 0;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void PrintDoc_PrintPage(object sender,
                    System.Drawing.Printing.PrintPageEventArgs e) {
            int tmpWidth, i;
            int tmpTop = e.MarginBounds.Top;
            int tmpLeft = e.MarginBounds.Left;

            try {
                // Before starting first page, it saves Width & Height of Headers and CoulmnType
                if (PageNo == 1) {
                    foreach (DataGridViewColumn GridCol in dgv.Columns) {
                        if (!GridCol.Visible)
                            continue;
                        // Skip if the current column not selected
                        if (!PrintDGV.SelectedColumns.Contains(GridCol.HeaderText))
                            continue;

                        // Detemining whether the columns are fitted to page or not.
                        if (FitToPageWidth)
                            tmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                       (double)TotalWidth * (double)TotalWidth *
                                       ((double)e.MarginBounds.Width / (double)TotalWidth))));
                        else
                            tmpWidth = GridCol.Width;

                        HeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                    GridCol.InheritedStyle.Font, tmpWidth).Height) + 11;

                        // Save width & height of headres and ColumnType
                        ColumnLefts.Add(tmpLeft);
                        ColumnWidths.Add(tmpWidth);
                        ColumnTypes.Add(GridCol.GetType());
                        tmpLeft += tmpWidth;
                    }
                }

                // Printing Current Page, Row by Row
                while (RowPos <= dgv.Rows.Count - 1) {
                    DataGridViewRow GridRow = dgv.Rows[RowPos];
                    if (GridRow.IsNewRow || (!PrintAllRows && !GridRow.Selected)) {
                        RowPos++;
                        continue;
                    }

                    CellHeight = GridRow.Height;

                    if (tmpTop + CellHeight >= e.MarginBounds.Height + e.MarginBounds.Top) {
                        DrawFooter(e, RowsPerPage);
                        NewPage = true;
                        PageNo++;
                        e.HasMorePages = true;
                        return;
                    } else {
                        if (NewPage) {
                            // Draw Header
                            e.Graphics.DrawString(PrintTitle, new Font(dgv.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                            e.Graphics.MeasureString(PrintTitle, new Font(dgv.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            String s = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();

                            e.Graphics.DrawString(s, new Font(dgv.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(s, new Font(dgv.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(PrintTitle, new Font(new Font(dgv.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            // Draw Columns
                            tmpTop = e.MarginBounds.Top;
                            i = 0;
                            foreach (DataGridViewColumn GridCol in dgv.Columns) {
                                if (!GridCol.Visible)
                                    continue;
                                if (!PrintDGV.SelectedColumns.Contains(GridCol.HeaderText))
                                    continue;

                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                    new Rectangle((int)ColumnLefts[i], tmpTop,
                                    (int)ColumnWidths[i], HeaderHeight));

                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle((int)ColumnLefts[i], tmpTop,
                                    (int)ColumnWidths[i], HeaderHeight));

                                e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                    new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                    new RectangleF((int)ColumnLefts[i], tmpTop,
                                    (int)ColumnWidths[i], HeaderHeight), StrFormat);
                                i++;
                            }
                            NewPage = false;
                            tmpTop += HeaderHeight;
                        }

                        // Draw Columns Contents
                        i = 0;
                        foreach (DataGridViewCell Cel in GridRow.Cells) {
                            if (!Cel.OwningColumn.Visible)
                                continue;
                            if (!SelectedColumns.Contains(Cel.OwningColumn.HeaderText))
                                continue;

                            // For the TextBox Column
                            if (((Type)ColumnTypes[i]).Name == "DataGridViewTextBoxColumn" ||
                                ((Type)ColumnTypes[i]).Name == "DataGridViewLinkColumn") {
                                e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                        new SolidBrush(Cel.InheritedStyle.ForeColor),
                                        new RectangleF((int)ColumnLefts[i], (float)tmpTop,
                                        (int)ColumnWidths[i], (float)CellHeight), StrFormat);
                            }
                                // For the Button Column
                            else if (((Type)ColumnTypes[i]).Name == "DataGridViewButtonColumn") {
                                CellButton.Text = Cel.Value.ToString();
                                CellButton.Size = new Size((int)ColumnWidths[i], CellHeight);
                                Bitmap bmp = new Bitmap(CellButton.Width, CellButton.Height);
                                CellButton.DrawToBitmap(bmp, new Rectangle(0, 0,
                                        bmp.Width, bmp.Height));
                                e.Graphics.DrawImage(bmp, new Point((int)ColumnLefts[i], tmpTop));
                            }
                                // For the CheckBox Column
                            else if (((Type)ColumnTypes[i]).Name == "DataGridViewCheckBoxColumn") {
                                CellCheckBox.Size = new Size(14, 14);
                                CellCheckBox.Checked = (bool)Cel.Value;
                                Bitmap bmp = new Bitmap((int)ColumnWidths[i], CellHeight);
                                Graphics tmpGraphics = Graphics.FromImage(bmp);
                                tmpGraphics.FillRectangle(Brushes.White, new Rectangle(0, 0,
                                        bmp.Width, bmp.Height));
                                CellCheckBox.DrawToBitmap(bmp,
                                        new Rectangle((int)((bmp.Width - CellCheckBox.Width) / 2),
                                        (int)((bmp.Height - CellCheckBox.Height) / 2),
                                        CellCheckBox.Width, CellCheckBox.Height));
                                e.Graphics.DrawImage(bmp, new Point((int)ColumnLefts[i], tmpTop));
                            }
                                // For the ComboBox Column
                            else if (((Type)ColumnTypes[i]).Name == "DataGridViewComboBoxColumn") {
                                CellComboBox.Size = new Size((int)ColumnWidths[i], CellHeight);
                                Bitmap bmp = new Bitmap(CellComboBox.Width, CellComboBox.Height);
                                CellComboBox.DrawToBitmap(bmp, new Rectangle(0, 0,
                                        bmp.Width, bmp.Height));
                                e.Graphics.DrawImage(bmp, new Point((int)ColumnLefts[i], tmpTop));
                                e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                        new SolidBrush(Cel.InheritedStyle.ForeColor),
                                        new RectangleF((int)ColumnLefts[i] + 1, tmpTop, (int)ColumnWidths[i]
                                        - 16, CellHeight), StrFormatComboBox);
                            }
                                // For the Image Column
                            else if (((Type)ColumnTypes[i]).Name == "DataGridViewImageColumn") {
                                Rectangle CelSize = new Rectangle((int)ColumnLefts[i],
                                        tmpTop, (int)ColumnWidths[i], CellHeight);
                                Size ImgSize = ((Image)(Cel.FormattedValue)).Size;
                                e.Graphics.DrawImage((Image)Cel.FormattedValue,
                                        new Rectangle((int)ColumnLefts[i] + (int)((CelSize.Width - ImgSize.Width) / 2),
                                        tmpTop + (int)((CelSize.Height - ImgSize.Height) / 2),
                                        ((Image)(Cel.FormattedValue)).Width, ((Image)(Cel.FormattedValue)).Height));
                            }

                            // Drawing Cells Borders
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)ColumnLefts[i],
                                    tmpTop, (int)ColumnWidths[i], CellHeight));

                            i++;
                        }
                        tmpTop += CellHeight;
                    }

                    RowPos++;
                    // For the first page it calculates Rows per Page
                    if (PageNo == 1)
                        RowsPerPage++;
                }

                if (RowsPerPage == 0)
                    return;

                // Write Footer (Page Number)
                DrawFooter(e, RowsPerPage);

                e.HasMorePages = false;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void DrawFooter(System.Drawing.Printing.PrintPageEventArgs e,
                    int RowsPerPage) {
            double cnt = 0;

            // Detemining rows number to print
            if (PrintAllRows) {
                if (dgv.Rows[dgv.Rows.Count - 1].IsNewRow)
                    cnt = dgv.Rows.Count - 2; // When the DataGridView doesn't allow adding rows
                else
                    cnt = dgv.Rows.Count - 1; // When the DataGridView allows adding rows
            } else
                cnt = dgv.SelectedRows.Count;

            // Writing the Page Number on the Bottom of Page
            string PageNum = PageNo.ToString() + " of " +
                Math.Ceiling((double)(cnt / RowsPerPage)).ToString();

            e.Graphics.DrawString(PageNum, dgv.Font, Brushes.Black,
                e.MarginBounds.Left + (e.MarginBounds.Width -
                e.Graphics.MeasureString(PageNum, dgv.Font,
                e.MarginBounds.Width).Width) / 2, e.MarginBounds.Top +
                e.MarginBounds.Height + 31);
        }
    }

    public class PrintHelper {

        #region Fields

        private readonly PrintDocument printDoc;

        private Image controlImage;

        private PrintDirection currentDir;

        private Point lastPrintPosition;

        private int nodeHeight;

        private int pageNumber;

        private int scrollBarHeight;

        private int scrollBarWidth;

        private string title = string.Empty;

        #endregion Fields

        #region Constructors and Destructors

        public PrintHelper() {
            this.lastPrintPosition = new Point(0, 0);
            this.printDoc = new PrintDocument();
            this.printDoc.BeginPrint += this.PrintDocBeginPrint;
            this.printDoc.PrintPage += this.PrintDocPrintPage;
            this.printDoc.EndPrint += this.PrintDocEndPrint;
        }

        #endregion Constructors and Destructors

        #region Enums

        private enum PrintDirection {
            Horizontal,

            Vertical
        }

        #endregion Enums

        #region Public Methods and Operators

        /// <summary>
        ///     Shows a PrintPreview dialog displaying the Tree control passed in.
        /// </summary>
        /// <param name="tree">TreeView to print preview</param>
        /// <param name="reportTitle"></param>
        public void PrintPreviewTree(TreeView tree, string reportTitle) {
            this.title = reportTitle;
            this.PrepareTreeImage(tree);
            var pp = new PrintPreviewDialog { Document = this.printDoc };
            pp.Show();
        }

        /// <summary>
        ///     Prints a tree
        /// </summary>
        /// <param name="tree">TreeView to print</param>
        /// <param name="reportTitle"></param>
        public void PrintTree(TreeView tree, string reportTitle) {
            this.title = reportTitle;
            this.PrepareTreeImage(tree);
            var pd = new PrintDialog { Document = this.printDoc };
            if (pd.ShowDialog() == DialogResult.OK) {
                this.printDoc.Print();
            }
        }

        #endregion Public Methods and Operators

        #region Methods

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        // Returns an image of the specified width and height, of a control represented by handle.
        private Image GetImage(IntPtr handle, int width, int height) {
            IntPtr screenDC = GetDC(IntPtr.Zero);
            IntPtr hbm = CreateCompatibleBitmap(screenDC, width, height);
            Image image = Image.FromHbitmap(hbm);
            Graphics g = Graphics.FromImage(image);
            IntPtr hdc = g.GetHdc();
            SendMessage(handle, 0x0318 /*WM_PRINTCLIENT*/, hdc, (0x00000010 | 0x00000004 | 0x00000002));
            g.ReleaseHdc(hdc);
            ReleaseDC(IntPtr.Zero, screenDC);
            return image;
        }

        /// <summary>
        ///     Gets an image that shows the entire tree, not just what is visible on the form
        /// </summary>
        /// <param name="tree"></param>
        ///

        private void ExpandNode(TreeNode t) {
            t.ExpandAll();
            for (int i = 0; i < t.Nodes.Count; i++) { ExpandNode(t.Nodes[i]); }
        }

        private void PrepareTreeImage(TreeView tree) {
            ExpandNode(tree.Nodes[0]);
            this.scrollBarWidth = tree.Width - tree.ClientSize.Width;
            this.scrollBarHeight = tree.Height - tree.ClientSize.Height;
            tree.Nodes[0].EnsureVisible();
            int height = tree.Nodes[0].Bounds.Height;
            this.nodeHeight = height;
            int width = tree.Nodes[0].Bounds.Right;
            TreeNode node = tree.Nodes[0].NextVisibleNode;
            while (node != null) {
                height += node.Bounds.Height;
                if (node.Bounds.Right > width) {
                    width = node.Bounds.Right;
                }
                node = node.NextVisibleNode;
            }
            //keep track of the original tree settings
            int tempHeight = tree.Height;
            int tempWidth = tree.Width;
            BorderStyle tempBorder = tree.BorderStyle;
            bool tempScrollable = tree.Scrollable;
            TreeNode selectedNode = tree.SelectedNode;
            //setup the tree to take the snapshot
            tree.SelectedNode = null;
            DockStyle tempDock = tree.Dock;
            tree.Height = height + this.scrollBarHeight;
            tree.Width = width + this.scrollBarWidth;
            tree.BorderStyle = BorderStyle.None;
            tree.Dock = DockStyle.None;
            //get the image of the tree

            // .Net 2.0 supports drawing controls onto bitmaps natively now
            // However, the full tree didn't get drawn when I tried it, so I am
            // sticking with the P/Invoke calls
            //_controlImage = new Bitmap(height, width);
            //Bitmap bmp = _controlImage as Bitmap;
            //tree.DrawToBitmap(bmp, tree.Bounds);

            this.controlImage = this.GetImage(tree.Handle, tree.Width, tree.Height);

            //reset the tree to its original settings
            tree.BorderStyle = tempBorder;
            tree.Width = tempWidth;
            tree.Height = tempHeight;
            tree.Dock = tempDock;
            tree.Scrollable = tempScrollable;
            tree.SelectedNode = selectedNode;
            //give the window time to update
            Application.DoEvents();
        }

        private void PrintDocEndPrint(object sender, PrintEventArgs e) {
            this.controlImage.Dispose();
        }

        private void PrintDocBeginPrint(object sender, PrintEventArgs e) {
            this.lastPrintPosition = new Point(0, 0);
            this.currentDir = PrintDirection.Horizontal;
            this.pageNumber = 0;
        }

        private void PrintDocPrintPage(object sender, PrintPageEventArgs e) {
            this.pageNumber++;
            Graphics g = e.Graphics;
            var sourceRect = new Rectangle(this.lastPrintPosition, e.MarginBounds.Size);
            Rectangle destRect = e.MarginBounds;

            if ((sourceRect.Height % this.nodeHeight) > 0) {
                sourceRect.Height -= (sourceRect.Height % this.nodeHeight);
            }
            g.DrawImage(this.controlImage, destRect, sourceRect, GraphicsUnit.Pixel);
            //check to see if we need more pages
            if ((this.controlImage.Height - this.scrollBarHeight) > sourceRect.Bottom || (this.controlImage.Width - this.scrollBarWidth) > sourceRect.Right) {
                //need more pages
                e.HasMorePages = true;
            }
            if (this.currentDir == PrintDirection.Horizontal) {
                if (sourceRect.Right < (this.controlImage.Width - this.scrollBarWidth)) {
                    //still need to print horizontally
                    this.lastPrintPosition.X += (sourceRect.Width + 1);
                } else {
                    this.lastPrintPosition.X = 0;
                    this.lastPrintPosition.Y += (sourceRect.Height + 1);
                    this.currentDir = PrintDirection.Vertical;
                }
            } else if (this.currentDir == PrintDirection.Vertical
                       && sourceRect.Right < (this.controlImage.Width - this.scrollBarWidth)) {
                this.currentDir = PrintDirection.Horizontal;
                this.lastPrintPosition.X += (sourceRect.Width + 1);
            } else {
                this.lastPrintPosition.Y += (sourceRect.Height + 1);
            }

            //print footer
            Brush brush = new SolidBrush(Color.Black);
            string footer = this.pageNumber.ToString(NumberFormatInfo.CurrentInfo);
            var f = new Font(FontFamily.GenericSansSerif, 10f);
            SizeF footerSize = g.MeasureString(footer, f);
            var pageBottomCenter = new PointF(x: e.PageBounds.Width / 2, y: e.MarginBounds.Bottom + ((e.PageBounds.Bottom - e.MarginBounds.Bottom) / 2));
            var footerLocation = new PointF(
                pageBottomCenter.X - (footerSize.Width / 2), pageBottomCenter.Y - (footerSize.Height / 2));
            g.DrawString(footer, f, brush, footerLocation);

            //print header
            if (this.pageNumber == 1 && this.title.Length > 0) {
                var headerFont = new Font(FontFamily.GenericSansSerif, 24f, FontStyle.Bold, GraphicsUnit.Point);
                SizeF headerSize = g.MeasureString(this.title, headerFont);
                var headerLocation = new PointF(x: e.MarginBounds.Left, y: ((e.MarginBounds.Top - e.PageBounds.Top) / 2) - (headerSize.Height / 2));
                g.DrawString(this.title, headerFont, brush, headerLocation);
            }
        }

        #endregion Methods

        //External function declarations
    }
}