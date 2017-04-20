using Astro.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Astrodon.Classes
{
    public class BuildingComparer : IComparer<Building>
    {
        private string memberName = string.Empty; // specifies the member name to be sorted
        private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.

        /// <summary>
        /// constructor to set the sort column and sort order.
        /// </summary>
        /// <param name="strMemberName"></param>
        /// <param name="sortingOrder"></param>
        public BuildingComparer(string strMemberName, SortOrder sortingOrder)
        {
            memberName = strMemberName;
            sortOrder = sortingOrder;
        }

        /// <summary>
        /// Compares two Students based on member name and sort order
        /// and return the result.
        /// </summary>
        /// <param name="Student1"></param>
        /// <param name="Student2"></param>
        /// <returns></returns>
        public int Compare(Building trn1, Building trn2)
        {
            int returnValue = 1;
            switch (memberName)
            {
                case "Name":
                    if (sortOrder == SortOrder.Ascending)
                    {
                        returnValue = trn1.Name.CompareTo(trn2.Name);
                    }
                    else
                    {
                        returnValue = trn2.Name.CompareTo(trn1.Name);
                    }

                    break;
            }
            return returnValue;
        }
    }
}
