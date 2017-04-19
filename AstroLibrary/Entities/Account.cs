using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class Account
    {
        private int _finCat;
        private String _accNumber;
        private String _description;
        private int _cat;
        private String _linkCode;
        private int _subAcc;
        private double[] _thisBal = new double[13];
        private double[] _lastBal = new double[13];
        private double[] _thisBudget = new double[13];
        private double[] _nextBudget = new double[13];
        private double[] _lastBudget = new double[13];
        private String _blocked;
        private int _tax;
        private String _defTax;
        private String _gaap;

        public int finCat { get { return _finCat; } set { _finCat = value; } }

        public String accNumber { get { return _accNumber; } set { _accNumber = value; } }

        public String description { get { return _description; } set { _description = value; } }

        public int cat { get { return _cat; } set { _cat = value; } }

        public String linkCode { get { return _linkCode; } set { _linkCode = value; } }

        public int subAcc { get { return _subAcc; } set { _subAcc = value; } }

        public double[] thisBal { get { return _thisBal; } set { _thisBal = value; } }

        public double[] lastBal { get { return _lastBal; } set { _lastBal = value; } }

        public double[] thisBudget { get { return _thisBudget; } set { _thisBudget = value; } }

        public double[] nextBudget { get { return _nextBudget; } set { _nextBudget = value; } }

        public double[] lastBudget { get { return _lastBudget; } set { _lastBudget = value; } }

        public String blocked { get { return _blocked; } set { _blocked = value; } }

        public int tax { get { return _tax; } set { _tax = value; } }

        public String defTax { get { return _defTax; } set { _defTax = value; } }

        public String gaap { get { return _gaap; } set { _gaap = value; } }

        public Account()
        {
        }

        public Account(String accString)
        {
            try
            {
                String[] splitter = new String[] { "|" };
                String[] contents = accString.Split(splitter, StringSplitOptions.None);
                //if (contents.Length == 75) {
                finCat = int.Parse(contents[1]);
                accNumber = contents[2];
                description = contents[3];
                cat = int.Parse(contents[4]);
                linkCode = contents[5];
                double[] balThis = new double[13];
                subAcc = int.Parse(contents[6]);

                #region Balances

                _thisBal[0] = double.Parse(contents[7]);
                _thisBal[1] = double.Parse(contents[8]);
                _thisBal[2] = double.Parse(contents[9]);
                _thisBal[3] = double.Parse(contents[10]);
                _thisBal[4] = double.Parse(contents[11]);
                _thisBal[5] = double.Parse(contents[12]);
                _thisBal[6] = double.Parse(contents[13]);
                _thisBal[7] = double.Parse(contents[14]);
                _thisBal[8] = double.Parse(contents[15]);
                _thisBal[9] = double.Parse(contents[16]);
                _thisBal[10] = double.Parse(contents[17]);
                _thisBal[11] = double.Parse(contents[18]);
                _thisBal[12] = double.Parse(contents[19]);
                //thisBal = balThis;
                _lastBal[0] = double.Parse(contents[20]);
                _lastBal[1] = double.Parse(contents[21]);
                _lastBal[2] = double.Parse(contents[22]);
                _lastBal[3] = double.Parse(contents[23]);
                _lastBal[4] = double.Parse(contents[24]);
                _lastBal[5] = double.Parse(contents[25]);
                _lastBal[6] = double.Parse(contents[26]);
                _lastBal[7] = double.Parse(contents[27]);
                _lastBal[8] = double.Parse(contents[28]);
                _lastBal[9] = double.Parse(contents[29]);
                _lastBal[10] = double.Parse(contents[30]);
                _lastBal[11] = double.Parse(contents[31]);
                _lastBal[12] = double.Parse(contents[32]);
                //lastBal = balThis;

                #endregion Balances

                #region Budgets

                balThis[0] = double.Parse(contents[33]);
                balThis[1] = double.Parse(contents[34]);
                balThis[2] = double.Parse(contents[35]);
                balThis[3] = double.Parse(contents[36]);
                balThis[4] = double.Parse(contents[37]);
                balThis[5] = double.Parse(contents[38]);
                balThis[6] = double.Parse(contents[39]);
                balThis[7] = double.Parse(contents[40]);
                balThis[8] = double.Parse(contents[41]);
                balThis[9] = double.Parse(contents[42]);
                balThis[10] = double.Parse(contents[43]);
                balThis[11] = double.Parse(contents[44]);
                balThis[12] = double.Parse(contents[45]);
                thisBudget = balThis;
                balThis[0] = double.Parse(contents[46]);
                balThis[1] = double.Parse(contents[47]);
                balThis[2] = double.Parse(contents[48]);
                balThis[3] = double.Parse(contents[49]);
                balThis[4] = double.Parse(contents[50]);
                balThis[5] = double.Parse(contents[51]);
                balThis[6] = double.Parse(contents[52]);
                balThis[7] = double.Parse(contents[53]);
                balThis[8] = double.Parse(contents[54]);
                balThis[9] = double.Parse(contents[55]);
                balThis[10] = double.Parse(contents[56]);
                balThis[11] = double.Parse(contents[57]);
                balThis[12] = double.Parse(contents[58]);
                nextBudget = balThis;
                balThis[0] = double.Parse(contents[59]);
                balThis[1] = double.Parse(contents[60]);
                balThis[2] = double.Parse(contents[61]);
                balThis[3] = double.Parse(contents[62]);
                balThis[4] = double.Parse(contents[63]);
                balThis[5] = double.Parse(contents[64]);
                balThis[6] = double.Parse(contents[65]);
                balThis[7] = double.Parse(contents[66]);
                balThis[8] = double.Parse(contents[67]);
                balThis[9] = double.Parse(contents[68]);
                balThis[10] = double.Parse(contents[69]);
                balThis[11] = double.Parse(contents[70]);
                balThis[12] = double.Parse(contents[71]);
                lastBudget = balThis;

                #endregion Budgets

                blocked = contents[72];
                tax = int.Parse(contents[73]);
                gaap = contents[74];
            }
            catch { }
        }
    }
}