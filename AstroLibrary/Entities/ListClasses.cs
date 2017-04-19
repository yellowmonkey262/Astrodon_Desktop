using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class ListLetter : BuildingList
    {
        public bool Update { get; set; }

        public bool Age_Analysis { get; set; }

        private bool printemail = false;

        public ListLetter(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public void SetPrint(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public bool Print_Email { get { return printemail; } }

        public bool File { get; set; }
    }

    public class ListStmt : BuildingList
    {
        public bool Update { get; set; }

        public bool Interest { get; set; }

        private bool printemail = false;

        public ListStmt(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public void SetPrint(bool sentLetter)
        {
            printemail = sentLetter;
        }

        public bool Print_Email { get { return printemail; } }

        public bool File { get; set; }
    }

    public class ListMonthEnd : BuildingList
    {
        public bool Update { get; set; }

        public bool Invest_Acc { get; set; }

        public bool _9990 { get; set; }

        public bool _4000 { get; set; }

        public bool Petty_Cash { get; set; }
    }

    public class ListDaily : BuildingList
    {
        public bool Trust { get; set; }

        public bool Own { get; set; }

        public bool File { get; set; }
    }

    public class ListReport
    {
        public String Name { get; set; }

        public String Code { get; set; }

        public String Debtor { get; set; }

        public String Daily_trust { get; set; }

        public String Daily_own { get; set; }

        public String Daily_file { get; set; }

        public String Letters_updated { get; set; }

        public String Letters_ageanalysis { get; set; }

        public String Letters_printed { get; set; }

        public String Letters_filed { get; set; }

        public String Statements_updated { get; set; }

        public String Statements_interest { get; set; }

        public String Statements_printed { get; set; }

        public String Statements_filed { get; set; }

        public String Month_end_updated { get; set; }

        public String Month_end_invest_account { get; set; }

        public String Month_end_9990 { get; set; }

        public String Month_end_4000 { get; set; }

        public String Month_end_petty_cash { get; set; }
    }
}