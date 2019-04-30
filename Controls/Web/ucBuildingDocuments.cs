using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astro.Library.Entities;
using Astrodon.ClientPortal;
using Astradon.Data.Utility;
using iTextSharp.text.pdf;
using System.IO;
using Astro.Library;

namespace Astrodon.Controls.Web
{
    public partial class ucBuildingDocuments : UserControl
    {
        private List<Building> _Buildings;
        private Building _SelectedBuilding = null;
        private List<FileDetail> _Data;
        private AstrodonClientPortal clientPortal = new AstrodonClientPortal(SqlDataHandler.GetClientPortalConnectionString());
        private List<DocumentCategoryListItem> _Categories;

        public ucBuildingDocuments()
        {
            InitializeComponent();
            LoadBuildings();
            LoadDocumentCategories();
        }

        private void LoadDocumentCategories()
        {
            _Categories = new List<DocumentCategoryListItem>();

            foreach (DocumentCategoryType cat in Enum.GetValues(typeof(DocumentCategoryType)))
            {
                if (cat != DocumentCategoryType.Letter)
                    _Categories.Add(new DocumentCategoryListItem() { Category = cat });
            }

            _Categories = _Categories.OrderBy(a => a.Display).ToList();
            cbDocumentCategory.DataSource = _Categories;
            cbDocumentCategory.ValueMember = "Category";
            cbDocumentCategory.DisplayMember = "Display";

        }

        private void LoadBuildings()
        {
            var userid = Controller.user.id;
            Buildings bManager = (userid == 0 || Controller.UserIsSheldon() ? new Buildings(false) : new Buildings(userid));

            _Buildings = bManager.buildings.ToList();
            _Buildings.Insert(0, new Building() { Name = "", ID = 0 });
            cmbBuilding.DataSource = _Buildings;
            cmbBuilding.ValueMember = "ID";
            cmbBuilding.DisplayMember = "Name";
            if (_Buildings.Count > 0)
                cmbBuilding.SelectedIndex = 0;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            var building = cmbBuilding.SelectedItem as Building;

            if (cmbBuilding.SelectedItem != null)
            {
                if (!Controller.VerifyBuildingDetailsEntered(building.ID))
                {
                    cmbBuilding.SelectedIndex = -1;
                    dgItems.DataSource = null;
                    picBuilding.Image = null;
                    return;
                }
            }

            if (cmbBuilding.SelectedIndex >= 0)
            {
                LoadSelectedBuilding(building);
                LoadBuildingImage(building);
            }
        }

        private void LoadSelectedBuilding(Building building)
        {
            _SelectedBuilding = building;
            if (_SelectedBuilding != null)
            {
                _Data = clientPortal.BuildingDocumentList(_SelectedBuilding.ID)
                                    .OrderByDescending(a => a.DocumentDate)
                                    .ThenBy(a => a.Title).ToList();

                BindDataGrid();

                tbMain.SelectedTab = tbDocuments;
            }
        }


        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";

            var periodStyle = new DataGridViewCellStyle();
            periodStyle.Format = "yyyy/MMM";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();

            dgItems.DataSource = bs;

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "View",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DocumentDateStr",
                HeaderText = "Uploaded",
                MinimumWidth = 50,
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "DocumentTypeStr",
                HeaderText = "Type",
                ReadOnly = true
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Title",
                HeaderText = "Title",
                ReadOnly = true
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "File",
                HeaderText = "File",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 30
            });


            dgItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgItems.AutoResizeColumns();
        }

        class DocumentCategoryListItem
        {
            public DocumentCategoryType Category { get; set; }

            public string Display
            {
                get
                {
                    return NameSplitting.SplitCamelCase(Category);
                }
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (cbDocumentCategory.SelectedValue == null)
            {
                Controller.HandleError("Please select a document cateogory.", "Validation Error");
                return;
            }

            if (_SelectedBuilding == null)
            {
                Controller.HandleError("Please select a building.", "Validation Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(tbTitle.Text))
            {
                Controller.HandleError("File title required", "Validation Error");
                return;
            }

            var documentType = (DocumentCategoryType)cbDocumentCategory.SelectedValue;
            if (fdOpen.ShowDialog() == DialogResult.OK)
            {
                var filePath = fdOpen.FileName;
                if (!IsValidPdf(filePath))
                {
                    Controller.HandleError("Not a valid PDF file");
                    return;
                }

                clientPortal.UploadBuildingDocument(documentType, DateTime.Now, _SelectedBuilding.ID, tbTitle.Text, filePath,string.Empty);

                Controller.ShowMessage("File Uploaded");

                LoadSelectedBuilding(_SelectedBuilding);
            }

        }

        private bool IsValidPdf(string filepath)
        {
            bool Ret = true;

            PdfReader reader = null;

            try
            {
                using (reader = new PdfReader(filepath))
                {
                    reader.Close();
                }
            }
            catch
            {
                Ret = false;
            }

            return Ret;
        }

        private string _TempPDFFile = string.Empty;

        private void DisplayPDF(byte[] pdfData)
        {
            if (pdfData == null)
            {
                this.axAcroPDF1.Visible = false;
                return;
            }
            if (!String.IsNullOrWhiteSpace(_TempPDFFile))
                File.Delete(_TempPDFFile);
            _TempPDFFile = Path.GetTempPath();
            if (!_TempPDFFile.EndsWith(@"\"))
                _TempPDFFile = _TempPDFFile + @"\";

            _TempPDFFile = _TempPDFFile + System.Guid.NewGuid().ToString("N") + ".pdf";
            File.WriteAllBytes(_TempPDFFile, pdfData);


            try
            {
                this.axAcroPDF1.Visible = true;
                this.axAcroPDF1.LoadFile(_TempPDFFile);
                this.axAcroPDF1.src = _TempPDFFile;
                this.axAcroPDF1.setShowToolbar(false);
                this.axAcroPDF1.setView("FitH");
                this.axAcroPDF1.setLayoutMode("SinglePage");
                this.axAcroPDF1.setShowToolbar(false);

                this.axAcroPDF1.Show();
                tbMain.SelectedTab = tbPreview;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            File.Delete(_TempPDFFile);
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            if(_SelectedBuilding == null)
            {
                Controller.HandleError("Please select a building.", "Validation Error");
                return;
            }
            if(openImage.ShowDialog() == DialogResult.OK)
            {
                byte[] jpg = File.ReadAllBytes(openImage.FileName);

                var img = ImageUtils.MaxSizeImage(jpg, 350, 150);
                clientPortal.SaveBuildingImage(_SelectedBuilding.ID, img);
                LoadBuildingImage(_SelectedBuilding);
            }
        }


        private void LoadBuildingImage(Building building)
        {
            var image = clientPortal.GetBuildingImage(building.ID);
            using (var mem = new MemoryStream(image))
            {
                picBuilding.Image = Image.FromStream(mem);
            }
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var doc = senderGrid.Rows[e.RowIndex].DataBoundItem as FileDetail;

                if (doc != null)
                {
                    if (e.ColumnIndex == 0)
                        DisplayCustomerDocument(doc);
                    else 
                        DeleteDocument(doc);
                }
            }
        }

        private void DeleteDocument(FileDetail doc)
        {
            if (_SelectedBuilding == null)
            {
                Controller.HandleError("Please select a building", "Validation Error");
                return;
            }

            if (Controller.AskQuestion("Are you sure you want to delete " + doc.File + "?"))
            {
                clientPortal.DeleteBuildingFiles(new List<Guid> { doc.Id });

                Controller.ShowMessage("Document deleted");

                LoadSelectedBuilding(_SelectedBuilding);
            }
        }

        private void DisplayCustomerDocument(FileDetail doc)
        {
            if(_SelectedBuilding == null)
            {
                Controller.HandleError("Please select a building", "Validation Error");
                return;
            }
            var filerData = clientPortal.GetBuildingFile(_SelectedBuilding.ID, doc.Id);
            DisplayPDF(filerData);
        }
    }
}
