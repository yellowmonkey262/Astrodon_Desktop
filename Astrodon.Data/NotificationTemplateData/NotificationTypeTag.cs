using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.Data.NotificationTemplateData
{
    public enum NotificationTagType
    {
        CustomerFullName,
        Amount
    }

    public class NotificationTypeTag
    {
        public static string TagName(NotificationTagType tagType)
        {
            return "{" + tagType.ToString() + "}";
        }

        private static Dictionary<NotificationTemplateType, NotificationTagType[]> _NotificationTags = null;
        public static Dictionary<NotificationTemplateType, NotificationTagType[]> NotificationTags
        {
            get
            {
                if (_NotificationTags == null)
                {
                    _NotificationTags = new Dictionary<NotificationTemplateType, NotificationTagType[]>();

                    _NotificationTags.Add(NotificationTemplateType.Birthday,
                                          new NotificationTagType[]
                                          {
                                            NotificationTagType.CustomerFullName,
                                          });

                    _NotificationTags.Add(NotificationTemplateType.Reminder,
                                        new NotificationTagType[]
                                        {
                                            NotificationTagType.Amount,
                                        });

                    _NotificationTags.Add(NotificationTemplateType.FinalDemand,
                                      new NotificationTagType[]
                                      {
                                            NotificationTagType.Amount,
                                      });

                    _NotificationTags.Add(NotificationTemplateType.DisconnectionNotice,
                                    new NotificationTagType[]
                                    {
                                            NotificationTagType.Amount,
                                    });

                    _NotificationTags.Add(NotificationTemplateType.SummonsPending,
                                  new NotificationTagType[]
                                  {
                                            NotificationTagType.Amount,
                                  });

                    _NotificationTags.Add(NotificationTemplateType.Disconnection,
                           new NotificationTagType[]
                           {
                                            NotificationTagType.Amount,
                           });

                    _NotificationTags.Add(NotificationTemplateType.LegalHandover,
                      new NotificationTagType[]
                      {
                                            NotificationTagType.Amount,
                      });
                }
                return _NotificationTags;
            }
        }
    }
}


