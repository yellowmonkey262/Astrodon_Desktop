using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Classes
{
    public class ParentDetail
    {
        private String _trustCode, _buildName;
        private int _period;
        private double _total;
        private List<Detail> _transactions = new List<Detail>();

        public String TrustCode
        {
            get { return _trustCode; }
            set { _trustCode = value; }
        }

        public String BuildName
        {
            get { return _buildName; }
            set { _buildName = value; }
        }

        public int Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public void Total()
        {
            _total = 0;
            foreach (Detail transaction in _transactions)
            {
                double amt = transaction.Amt;
                _total += amt;
            }
            AddTransaction(new Detail("Total", "", DateTime.Now, _total));
        }

        public List<Detail> Transactions
        {
            get { return _transactions; }
        }

        public void AddTransaction(Detail transaction)
        {
            _transactions.Add(transaction);
        }

        public ParentDetail(String trustCode, String buildName, int period)
        {
            TrustCode = trustCode;
            BuildName = buildName;
            Period = period;
        }
    }
}