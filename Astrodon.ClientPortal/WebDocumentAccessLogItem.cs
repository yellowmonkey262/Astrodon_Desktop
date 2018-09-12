using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Astrodon.ClientPortal
{
    public class WebDocumentAccessLogItem : DataItemBase
    {
        public WebDocumentAccessLogItem(DataRow row)
            :base(row)
        {

        }

        [FromDB]
        public DateTime AccessDate { get; set; }

        [FromDB]
        public string AccessToken { get; set; }

        [FromDB]
        public string EmailAddress { get; set; }

        public string AccessType
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(AccessToken))
                    return "Email Link";
                else
                    return "Web portal";
            }
        }

        public string AccessDateStr
        {
            get
            {
                return AccessDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }
    }
}
