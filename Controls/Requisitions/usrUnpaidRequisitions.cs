using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Astrodon.Controls.Requisitions
{
    public partial class usrUnpaidRequisitions : UserControl
    {

        private List<RequistitionGridItem> _DataItems = null;

        public usrUnpaidRequisitions()
        {
            InitializeComponent();
            LoadUnpaidRequisitions();
        }

        private void LoadUnpaidRequisitions()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    DateTime startDate = DateTime.Today.AddDays(-7);

                    var q = from r in context.tblRequisitions
                            join u in context.tblUsers on r.userID equals u.id
                            join b in context.tblBuildings on r.building equals b.id
                            where r.paid == false && r.processed == true
                            && r.trnDate <= startDate
                            && b.BuildingDisabled == false
                            select new RequistitionGridItem
                            {
                                Id = r.id,
                                User = u.username,
                                TransactionDate = r.trnDate,
                                Building = b.Building,
                                Amount = r.amount,
                                Account = r.account,
                                Ledger = r.ledger,
                                Reference = r.reference,
                                Payreference = r.payreference,
                                Contractor = r.contractor,
                                Supplier = r.SupplierId == null ? string.Empty : r.Supplier.CompanyName,
                                InvoiceNumber = r.InvoiceNumber,
                                InvoiceDate = r.InvoiceDate,
                                BankName = r.BankName,
                                AccountNumber = r.AccountNumber,
                                BuildingCode = b.Code,
                                ReqBatchNumber = r.RequisitionBatchId == null ? 0 : r.RequisitionBatch.BatchNumber,
                                Paid = false
                            };

                    _DataItems = q.OrderBy(a => a.Building).ThenByDescending(a => a.TransactionDate).ToList();
                    LoadGrid();


                }

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LoadGrid()
        {
            dgItems.ClearSelection();
            dgItems.MultiSelect = false;
            dgItems.AutoGenerateColumns = false;

            var dateColumnStyle = new DataGridViewCellStyle();
            dateColumnStyle.Format = "yyyy/MM/dd";


            var currencyColumnStyle = new DataGridViewCellStyle();
            currencyColumnStyle.Format = "###,##0.00";
            currencyColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            BindingSource bs = new BindingSource();
            bs.DataSource = _DataItems;

            dgItems.Columns.Clear();
            dgItems.ReadOnly = true;

            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Paid",
                Text = "Set Paid",
                UseColumnTextForButtonValue = true,
            });
            dgItems.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TransactionDate",
                HeaderText = "Date",
                ReadOnly = true,
                DefaultCellStyle = dateColumnStyle,
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Building",
                HeaderText = "Building",
                ReadOnly = true,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ReqBatchNumber",
                DataPropertyName = "BatchNumber",
                HeaderText = "Batch Number",
                ReadOnly = true
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Account",
                HeaderText = "Ledger",
                ReadOnly = false,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "OwnTrust",
                DataPropertyName = "Account",
                HeaderText = "Account",
                ReadOnly = false
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Amount",
                HeaderText = "Amount",
                ReadOnly = false,
                DefaultCellStyle = currencyColumnStyle,
            });
          
            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Reference",
                HeaderText = "Reference",
                ReadOnly = false,
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Payreference",
                HeaderText = "Payreference",
                ReadOnly = false,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Supplier",
                HeaderText = "Supplier",
                ReadOnly = false,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "InvoiceNumber",
                DataPropertyName = "InvoiceNumber",
                HeaderText = "Invoice Number",
                ReadOnly = false,
            });

            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "InvoiceDateX",
                DataPropertyName = "InvoiceDate",
                HeaderText = "Invoice Date",
                ReadOnly = false,
                DefaultCellStyle = dateColumnStyle
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Bank",
                DataPropertyName = "BankName",
                HeaderText = "Bank",
                ReadOnly = false
            });


            dgItems.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "AccountNumber",
                DataPropertyName = "AccountNumber",
                HeaderText = "Account Number",
                ReadOnly = true
            });

            dgItems.DataSource = bs;

            dgItems.AutoResizeColumns();
        }

        private void usrUnpaidRequisitions_Load(object sender, EventArgs e)
        {

        }

        class RequistitionGridItem
        {
            public string Account { get; internal set; }
            public string AccountNumber { get; internal set; }
            public decimal Amount { get; internal set; }
            public string BankName { get; internal set; }
            public int ReqBatchNumber { get; internal set; }
            public string Building { get; internal set; }
            public string BuildingCode { get; internal set; }
            public string Contractor { get; internal set; }
            public DateTime? InvoiceDate { get; internal set; }
            public string InvoiceNumber { get; internal set; }
            public string Ledger { get; internal set; }
            public string Payreference { get; internal set; }
            public string Reference { get; internal set; }
            public string Supplier { get; internal set; }
            public DateTime TransactionDate { get; internal set; }
            public string User { get; internal set; }
            public bool Paid { get; internal set; }
            public string BatchNumber
            {
                get
                {
                    if (ReqBatchNumber > 0)
                    {
                        return BuildingCode + "-" + ReqBatchNumber.ToString().PadLeft(6, '0');
                    }
                    return string.Empty;
                }
            }

            public int Id { get; internal set; }

            public override string ToString()
            {
                return "Loaded :" + TransactionDate.ToString("yyyy/MM/dd") + Environment.NewLine
                       + "Supplier : " + Supplier + Environment.NewLine
                       + "Amount :R" + Amount.ToString("###,##0.00") + Environment.NewLine
                       + "Reference :" + Reference;
            }

        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var item = senderGrid.Rows[e.RowIndex].DataBoundItem as RequistitionGridItem;

                if (e.ColumnIndex == 0)
                {
                    MarkAsPaid(item);
                }
                else
                {
                    DeleteRequisition(item);
                }
            }
        }

        private void DeleteRequisition(RequistitionGridItem item)
        {
            if (item == null || item.Paid)
                return;

            if(Controller.AskQuestion("Are you sure you want to delete this requisition?"
                  + Environment.NewLine + item.ToString()))
            {
                using (var context = SqlDataHandler.GetDataContext())
                {
                    context.DeleteRequisition(item.Id);
                    _DataItems.Remove(item);
                    var bindingSource = new BindingSource();
                    bindingSource.DataSource = _DataItems;
                    dgItems.DataSource = bindingSource;
                }
            }
        }

        private void MarkAsPaid(RequistitionGridItem item)
        {
            if (item != null && item.Paid == false)
            {
                if (Controller.AskQuestion("Are you sure you want to mark this item as Paid?"
                                      + Environment.NewLine + item.ToString()))
                {
                    using (var context = SqlDataHandler.GetDataContext())
                    {
                        var req = context.tblRequisitions.Single(a => a.id == item.Id);
                        req.paid = true;
                        req.PaymentDataPath = "Manual-" + Controller.user.username + "-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                        context.SaveChanges();
                        _DataItems.Remove(item);
                        var bindingSource = new BindingSource();
                        bindingSource.DataSource = _DataItems;
                        dgItems.DataSource = bindingSource;
                    }
                }
            }
        }
    }
}
