using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;
using Astrodon.Data.BankData;
using System.Data.Entity.Infrastructure;
using Astrodon.Data.NotificationTemplateData;

namespace Astrodon.Controls.SystemConfig
{
    public partial class ucNotificationTemplate : UserControl
    {
        private DataContext _Context;
        private NotificationTemplate _Item;
        private List<NotificationTemplate> _Data;
        private List<NotificationTemplateType> _TemplateTypes;

        public ucNotificationTemplate(DataContext context)
        {
            InitializeComponent();

            _Context = context;
            _TemplateTypes = new List<NotificationTemplateType>();

            foreach (NotificationTemplateType x in Enum.GetValues(typeof(NotificationTemplateType)))
                _TemplateTypes.Add(x);

            cbTemplateType.DataSource = _TemplateTypes;
            cbTemplateType.SelectedItem = _TemplateTypes.First();


            LoadData();
            GotoReadOnly();
        }

        private void LoadData()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DateTime startDate = DateTime.Today.AddMonths(-3);

                _Data = _Context.NotificationTemplateSet.OrderBy(a => a.TemplateType).ToList();

                BindDataGrid();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BindDataGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            BindingSource bs = new BindingSource();
            bs.DataSource = _Data;

            dgItems.Columns.Clear();

            dgItems.DataSource = bs;

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Action",
                Text = "Select",
                UseColumnTextForButtonValue = true,
                Width = 50
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TemplateType",
                HeaderText = "Template Type",
                ReadOnly = true,
                Width = 100
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TemplateName",
                HeaderText = "Name",
                ReadOnly = true,
                Width = 100
            });

        }

        private void GotoReadOnly()
        {
            txTemplateName.Text = "";
            txTemplateName.ReadOnly = true;

            txTemplateText.Text = "";
            txTemplateText.ReadOnly = true;
            cbTemplateType.Enabled = false;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnNew.Visible = true;
            dgItems.Enabled = true;
            label4.Visible = false;
            lbAllowedTags.Visible = false;
        }


        private void GotoEditable()
        {
            txTemplateName.ReadOnly = false;
            txTemplateText.ReadOnly = false;
            cbTemplateType.Enabled = true;

            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnNew.Visible = false;
            dgItems.Enabled = false;
            lbAllowedTags.Visible = true;
            label4.Visible = true;
            UpdateAllowedTags();

        }

        private void UpdateAllowedTags()
        {
            lbAllowedTags.DataSource = null;
            lbAllowedTags.SelectedItem = null;

            NotificationTemplateType templateType = (NotificationTemplateType)cbTemplateType.SelectedItem;
            if (NotificationTypeTag.NotificationTags.ContainsKey(templateType))
            {

                var allowedTags = NotificationTypeTag.NotificationTags[templateType];
                var allowedTagnames = allowedTags.Select(a => NotificationTypeTag.TagName(a)).OrderBy(a => a).ToArray();
                lbAllowedTags.DataSource = allowedTagnames;
                lbAllowedTags.SelectedItem = null;
            }

        } 

        private void btnNew_Click(object sender, EventArgs e)
        {
            _Item = null;
            GotoEditable();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _Context.ClearChanges();
            GotoReadOnly();
            LoadData();
        }


        private void EditItem()
        {
            txTemplateName.Text = _Item.TemplateName;
            txTemplateText.Text = _Item.MessageText;
            cbTemplateType.SelectedItem = _Item.TemplateType;
            GotoEditable();
            cbTemplateType.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txTemplateName.Text))
            {
                Controller.HandleError("Name is required", "Validation Error");
                return;
            }

            if (String.IsNullOrWhiteSpace(txTemplateText.Text))
            {
                Controller.HandleError("Text is required", "Validation Error");
                return;
            }

            //if not General then can only have one

            NotificationTemplateType templateType = (NotificationTemplateType)cbTemplateType.SelectedItem;

            if (templateType != NotificationTemplateType.General)
            {
                var existing = _Context.NotificationTemplateSet.Where(a => a.TemplateType == templateType).FirstOrDefault();
                if (existing != null)
                {
                    if (_Item == null || _Item.id != existing.id)
                    {
                        Controller.HandleError("A template is already defined for " + templateType.ToString() + " please edit the existing tempalte", "Validation Error");
                        return;
                    }
                }

                //validate that all tags are valid

                var tags = NotificationTemplate.GetAllTags(txTemplateText.Text);
                var allowedTags = NotificationTypeTag.NotificationTags[templateType];

                var allowedTagnames = allowedTags.Select(a => NotificationTypeTag.TagName(a)).ToArray();

                var tagsNotAllowed = tags.Where(a => !allowedTagnames.Contains(a)).ToList();
                if (tagsNotAllowed.Count() > 0)
                {
                    string errText = string.Empty;
                    foreach (var s in tagsNotAllowed)
                        errText += s + Environment.NewLine;

                    Controller.HandleError("The following tags are invalid for this message type " + Environment.NewLine +
                        errText, "Validation Error");
                    return;
                }
            }

            if (_Item == null)
            {
                _Item = new NotificationTemplate();
                _Context.NotificationTemplateSet.Add(_Item);
            }

            _Item.TemplateName = txTemplateName.Text;
            _Item.MessageText = txTemplateText.Text;
            _Item.TemplateType = (NotificationTemplateType)cbTemplateType.SelectedItem;

            try
            {
                bool isNew = _Item.id == 0;
                _Context.SaveChanges();

                if (isNew)
                    _Data.Insert(0, _Item);
                BindDataGrid();
                GotoReadOnly();
            }
            catch (DbUpdateException)
            {
                Controller.HandleError("Possible duplicate record detected", "Database Error");
            }
            catch (Exception ex2)
            {
                Controller.HandleError(ex2.Message);
            }
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _Item = senderGrid.Rows[e.RowIndex].DataBoundItem as NotificationTemplate;

                if (_Item != null)
                {
                    EditItem();
                }
            }
        }

        private void cbTemplateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAllowedTags();
        }

        private void lbAllowedTags_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(lbAllowedTags.SelectedItem != null)
            {
                var s = lbAllowedTags.SelectedItem as string;
                if(txTemplateText.Text.EndsWith(" "))
                  txTemplateText.Text += s;
                else
                    txTemplateText.Text += " " + s;
            }
        }
    }
}
