using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Astrodon.Data;

namespace Astrodon.Reports.BuildingPMDebtor
{
    public partial class ucBuildingPMDebtorList : UserControl
    {
        private List<BuildingPMDebtorResult> _BuildingPMDebtorResultList { get; set; }

        public ucBuildingPMDebtorList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = SqlDataHandler.GetDataContext())
            {
                _BuildingPMDebtorResultList = (from b in context.tblBuildings
                                               join pmTemp in context.tblUsers on b.pm equals pmTemp.email into pmJoin
                                               from pm in pmJoin.DefaultIfEmpty()
                                               join ubTemp in context.tblUserBuildings on b.id equals ubTemp.buildingid into ubJoin
                                               from ub in ubJoin.DefaultIfEmpty()
                                               join debtorTemp in context.tblUsers on ub.userid equals debtorTemp.id into debtorJoin
                                               from debtor in debtorJoin.DefaultIfEmpty()
                                               where b.BuildingDisabled == false && debtor.usertype == 3
                                               orderby b.Building
                                               select new BuildingPMDebtorResult()
                                               {
                                                   BuildingId = b.id,
                                                   BuildingName = b.Building,
                                                   BuildingCode = b.Code,

                                                   PortfolioManager = (pm.name == null) ? "" : pm.name,
                                                   PortfolioManagerEmail = (pm.email == null) ? "" : pm.email,

                                                   Debtor = debtor.name,
                                                   DebtorEmail = debtor.email
                                               }).ToList();
            }

            buildingPMDebtorGridView.DataSource = _BuildingPMDebtorResultList;
            buildingPMDebtorGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
