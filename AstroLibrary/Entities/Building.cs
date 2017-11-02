using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class Building
    {
        public int ID { get; set; }

        public String Name { get; set; }

        public bool BuildingDisabled { get; set; }

        public String Abbr { get; set; }

        public String Trust { get; set; }

        public String DataPath { get; set; }

        public int Period { get; set; }

        public String Cash_Book { get; set; }

        public String OwnBank { get; set; }

        public String Cashbook3 { get; set; }

        public int Payments { get; set; }

        public int Receipts { get; set; }

        public int Journal { get; set; }

        public String Centrec_Account { get; set; }

        public String Centrec_Building { get; set; }

        public String Business_Account { get; set; }

        public String Bank { get; set; }

        public String PM { get; set; }

        public String Debtor { get; set; }

        public String Bank_Name { get; set; }

        public String Acc_Name { get; set; }

        public String Bank_Acc_Number { get; set; }

        public String Branch_Code { get; set; }

        public bool Web_Building { get; set; }

        public String letterName { get; set; }

        public String webFolder { get; set; }

        public String pid { get; set; }

        public double reminderFee { get; set; }

        public double reminderSplit { get; set; }

        public double finalFee { get; set; }

        public double finalSplit { get; set; }

        public double disconnectionNoticefee { get; set; }

        public double disconnectionNoticeSplit { get; set; }

        public double summonsFee { get; set; }

        public double summonsSplit { get; set; }

        public double disconnectionFee { get; set; }

        public double disconnectionSplit { get; set; }

        public double handoverFee { get; set; }

        public double handoverSplit { get; set; }

        public decimal DebitOrderFee { get;set;}

        public String reminderTemplate { get; set; }

        public String finalTemplate { get; set; }

        public String diconnectionNoticeTemplate { get; set; }

        public String summonsTemplate { get; set; }

        public String reminderSMS { get; set; }

        public String finalSMS { get; set; }

        public String disconnectionNoticeSMS { get; set; }

        public String summonsSMS { get; set; }

        public String disconnectionSMS { get; set; }

        public String handoverSMS { get; set; }

        public String addy1 { get; set; }

        public String addy2 { get; set; }

        public String addy3 { get; set; }

        public String addy4 { get; set; }

        public String addy5 { get; set; }

        public bool isHOA { get; set; }
        public double limitM { get; set; }
        public double limitW { get; set; }
        public double limitD { get; set; }

        public Building()
        {
            reminderTemplate = "";
            finalTemplate = "";
            diconnectionNoticeTemplate = "";
            summonsTemplate = "";
            reminderSMS = "";
            finalSMS = "";
            disconnectionNoticeSMS = "";
            summonsSMS = "";
            disconnectionSMS = "";
            handoverSMS = "";
            BuildingDisabled = false;
        }
    }

    public class PMBuilding
    {
        public String Code { get; set; }

        public String Name { get; set; }

        public String Outstanding { get; set; }

        public String Bank_Balance { get; set; }

        public String Bank_Last_Transaction_Date { get; set; }

        public String Trust_Balance { get; set; }

        public String Trust_Last_Transaction_Date { get; set; }

        public String Own_Bank_Balance { get; set; }

        public String Own_Bank_Last_Transaction_Date { get; set; }

        public String Invest_Balance { get; set; }

        public String Invest_Last_Transaction_Date { get; set; }
    }

    public class BuildingList
    {
        public String Name { get; set; }

        public String Code { get; set; }

        public String Debtor { get; set; }
    }

    public class Building2
    {
        private int _id;
        private String _building;
        private String _code;
        private String _path;
        private int _period;
        private int _journal;
        private String _acc;
        private String _contra;
        private List<Customer> _customers;

        public Account buildCentrec;
        public Customer centrecBuild;
        private String _bc;
        private String _business;
        private String _bank;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String BuildingName
        {
            get { return _building; }
            set { _building = value; }
        }

        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public String Acc
        {
            get { return _acc; }
            set { _acc = value; }
        }

        public String Contra
        {
            get { return _contra; }
            set { _contra = value; }
        }

        public String BC
        {
            get { return _bc; }
            set { _bc = value; }
        }

        public String Business
        {
            get { return _business; }
            set { _business = value; }
        }

        public int Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public int Journal
        {
            get { return _journal; }
            set { _journal = value; }
        }

        public String Bank
        {
            get { return _bank; }
            set { _bank = value; }
        }

        public bool isHOA { get; set; }

        public Building2(int __id, String __building, String __code, String __path, int __period, int __journal, String __acc, String __contra, String bc, String business, Account _centrec, Customer _build, String __bank)
        {
            Id = __id;
            BuildingName = __building;
            Code = __code;
            Path = __path;
            Acc = __acc;
            Contra = __contra;
            Period = __period;
            Journal = __journal;
            BC = bc;
            Business = business;
            _customers = new List<Customer>();
            buildCentrec = _centrec;
            centrecBuild = _build;
            Bank = __bank;
        }

        public Building2(int __id, String __building, String __code, String __path, int __period, int __journal, String __acc, String __contra, String bc, String business, String __bank)
        {
            Id = __id;
            BuildingName = __building;
            Code = __code;
            Path = __path;
            Acc = __acc;
            Contra = __contra;
            Period = __period;
            Journal = __journal;
            BC = bc;
            Business = business;
            _customers = new List<Customer>();
            Bank = __bank;
        }

        public List<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }
    }
}