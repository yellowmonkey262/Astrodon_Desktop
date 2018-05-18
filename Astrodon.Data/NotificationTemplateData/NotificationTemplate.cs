using Astrodon.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Astrodon.Data.NotificationTemplateData
{
    [Table("NotificationTemplate")] 
    public class NotificationTemplate : DbEntity
    {
        [Index("IDX_NotificationTemplateType")]
        public NotificationTemplateType TemplateType { get; set; }

        [Required]
        [MaxLength(50)]
        public string TemplateName { get; set; }

        [Required]
        [MaxLength(500)]
        public string MessageText { get; set; }

        public static List<string> GetAllTags(string message)
        {
            string temp = message;
            List<string> tags = new List<string>();
            while(temp.Length > 0)
            {
                var start = temp.IndexOf("{");
                var end = temp.IndexOf("}");

                if (start < 0 || end < 0 || end < start)
                    temp = "";
                else
                {
                    var tag = temp.Substring(start, end - start + 1);
                    tags.Add(tag);

                    temp = temp.Remove(0, end + 1).Trim();
                }
            }

            return tags;
        }

        public static string GetTagName(NotificationTagType tagType)
        {
            return NotificationTypeTag.TagName(tagType);
        }

       
    }
}
