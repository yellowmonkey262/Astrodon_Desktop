using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Astrodon.Email
{
    public class ResourceManager
    {
        //to read a file from the assembly - set the build action on the properties of the file to "Embedded Resource"
        public byte[] ReadResource(string path)
        {
            byte[] result = null;
            var assembly = Assembly.GetExecutingAssembly();
            using (var _stream = assembly.GetManifestResourceStream(path))
            {
                result = new byte[_stream.Length];
                _stream.Read(result, 0, result.Length);
            }
            return result;
        }

        public static string EmailTemplate(string templateName)
        {
            var template = new ResourceManager().ReadResource("Astrodon.Email.Templates." + templateName);
            return Encoding.UTF8.GetString(template);
        }

        public static string EmailLayout(string title)
        {
            var emailTemplate = new ResourceManager().ReadResource("Astrodon.Email.Templates.Template.html");
            string templateString = Encoding.UTF8.GetString(emailTemplate);

            templateString = templateString.Replace("{{TITLE}}", title);
            return templateString;
        }

        public static string ReadStatementTemplate()
        {
            return EmailTemplate("StatementEmailTemplate.html");
        }

        public static string ReadCustomerLetterTemplate()
        {
            return EmailTemplate("CustomerLetter.html");
        }

        public static string ReadRequisitionPaymentTemplate()
        {
            return EmailTemplate("RequisitionPayment.html");
        }

        public static string ReadAttachmentTemplate()
        {
            return EmailTemplate("Attachment.html");
        }
    }
}
