using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.ReportService;

namespace Astrodon.Reports.TransactionSearch
{
    public partial class ucTransactionSearch : UserControl
    {
        public ucTransactionSearch()
        {
            InitializeComponent();
        }

        bool searchStopped = false;

        private void SearchTransactions()
        {
            searchStopped = false;
            using (var context = SqlDataHandler.GetDataContext())
            {
                var buildings = context.tblBuildings.ToList();

                List<TransactionDataItem> allResults = new List<TransactionDataItem>();
                foreach(var building in buildings)
                {
                    if (searchStopped)
                        break;


                    //Searching building x
                    //update label to display - building name
                    Application.DoEvents();

                   
                        using (var reportService = ReportServiceClient.CreateInstance())
                        {
                            try
                            {

                            //var buildingResult = reportService.SearchPastel(building.DataPath,);
                            //  allResults.AddRange(buildingResult);
                            //  UpdateDataGrid(allResults);
                            }
                            catch (Exception ex)
                            {
                                Controller.HandleError(ex);

                                Controller.ShowMessage(ex.GetType().ToString());
                            }
                        }
                 
                }
            }
        }

        private void UpdateDataGrid(List<TransactionDataItem> allResults)
        {
            throw new NotImplementedException();
        }
    }
}
