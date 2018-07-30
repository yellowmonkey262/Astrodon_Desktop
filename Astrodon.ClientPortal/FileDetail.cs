using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Astrodon.ClientPortal
{
    public class FileDetail: DataItemBase
    {
        public FileDetail(DataRow row)
            :base(row)
        {

        }

        [FromDB]
        public Guid Id { get; set; }

      
        public string DocumentTypeStr
        {
            get
            {
                return SplitCamelCase(DocumentType.ToString());
            }
        }

        [FromDB]
        public int DocumentCategory { get; set; }

        [FromDB]
        public DateTime DocumentDate { get; set; }

        [FromDB]
        public string Title { get; set; }

        [FromDB]
        public string File { get; set; }

        [FromDB]
        public string AccountNumber { get; set; }

        public override string ToString()
        {
            return File;
        }

        public DocumentCategoryType DocumentType
        {
            get { return (DocumentCategoryType)DocumentCategory; }
        }

        public string DocumentDateStr
        {
            get
            {
                return DocumentDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            }
        }

        private string SplitCamelCase(string value)
        {
            if (value == null)
                return null;
            return Regex.Replace(Regex.Replace(value, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

    }
}
