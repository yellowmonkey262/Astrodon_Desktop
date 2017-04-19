using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Astrodon.Classes
{
    public class TransComparer : IComparer<Trns>
    {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName">
        /// </param>
        /// <param name="sortingOrder">
        /// </param>
        public TransComparer(string strMemberName, SortOrder sortingOrder)
        {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order and return the result.
        /// </summary>
        /// <param name="Student1">
        /// </param>
        /// <param name="Student2">
        /// </param>
        /// <returns>
        /// </returns>
        public int Compare(Trns trn1, Trns trn2)
        {
            int returnValue = 1;
            switch (memberName)
            {
                case "Date":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.Date.CompareTo(trn2.Date);
                    }
                    else
                    {
                        returnValue = trn2.Date.CompareTo(trn1.Date);
                    }

                    break;

                case "Description":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.Description.CompareTo(trn2.Description);
                    }
                    else
                    {
                        returnValue = trn2.Description.CompareTo(trn1.Description);
                    }

                    break;

                case "Reference":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.Reference.CompareTo(trn2.Reference);
                    }
                    else
                    {
                        returnValue = trn2.Reference.CompareTo(trn1.Reference);
                    }

                    break;

                case "Amount":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.Amount.CompareTo(trn2.Amount);
                    }
                    else
                    {
                        returnValue = trn2.Amount.CompareTo(trn1.Amount);
                    }

                    break;
            }
            return returnValue;
        }
    }

    public class DocsComparer : IComparer<CustomerDocument>
    {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName">
        /// </param>
        /// <param name="sortingOrder">
        /// </param>
        public DocsComparer(string strMemberName, SortOrder sortingOrder)
        {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order and return the result.
        /// </summary>
        /// <param name="Student1">
        /// </param>
        /// <param name="Student2">
        /// </param>
        /// <returns>
        /// </returns>
        public int Compare(CustomerDocument trn1, CustomerDocument trn2)
        {
            int returnValue = 1;
            switch (memberName)
            {
                case "Date":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.tstamp.CompareTo(trn2.tstamp);
                    }
                    else
                    {
                        returnValue = trn2.tstamp.CompareTo(trn1.tstamp);
                    }

                    break;

                case "Subject":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.subject.CompareTo(trn2.subject);
                    }
                    else
                    {
                        returnValue = trn2.subject.CompareTo(trn1.subject);
                    }

                    break;

                case "Title":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.title.CompareTo(trn2.title);
                    }
                    else
                    {
                        returnValue = trn2.title.CompareTo(trn1.title);
                    }

                    break;

                case "File":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.file.CompareTo(trn2.file);
                    }
                    else
                    {
                        returnValue = trn2.file.CompareTo(trn1.file);
                    }

                    break;
            }
            return returnValue;
        }
    }
}