using Astro.Library.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Astrodon
{
    public class WordProcessor
    {
        #region Variables

        private Word.Application wordApp;
        private String templateDir;
        private String letterDir;
        private object missing = Missing.Value;
        private object readOnly = false;
        private object isVisible = false;

        #endregion Variables

        #region Constructor

        public WordProcessor()
        {
            killprocess("winword");
            templateDir = (!Directory.Exists("K:\\Debtors System\\") ? "C:\\Pastel11\\Debtors System\\" : "K:\\Debtors System\\");
            letterDir = (Directory.Exists("K:\\Debtors System\\Letters\\") ? "K:\\Debtors System\\Letters\\" : "C:\\Pastel11\\Debtors System\\Letters\\");
            if (!Directory.Exists(letterDir)) { Directory.CreateDirectory(letterDir); }
            wordApp = new Word.Application
            {
                Visible = false
            };
            if (wordApp == null) { MessageBox.Show("Cannot start word"); }
        }

        public void killprocess(String procName)
        {
            try
            {
                Process[] procs = Process.GetProcessesByName(procName);
                foreach (Process proc in procs)
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #endregion Constructor

        #region Templates

        public String[] templates
        {
            get
            {
                String[] template = new String[4];
                template[0] = templateDir + "Discon_template.doc";
                template[1] = templateDir + "fd_template.doc";
                template[2] = templateDir + "rem_template.doc";
                template[3] = templateDir + "sum_template.doc";
                return template;
            }
        }

        public String[] HOAtemplates
        {
            get
            {
                String[] template = new String[4];
                template[0] = templateDir + "Discon_template.doc";
                template[1] = templateDir + "fd_HOA_template.doc";
                template[2] = templateDir + "rem_HOA_template.doc";
                template[3] = templateDir + "sum_HOA_template.doc";
                return template;
            }
        }

        public String[] FileNames(String accNo, String fDate)
        {
            String[] files = new String[5];
            files[0] = letterDir + fDate + "_" + accNo + "_RESTRICTION.doc";
            files[1] = letterDir + fDate + "_" + accNo + "_FINALDEMAND.doc";
            files[2] = letterDir + fDate + "_" + accNo + "_REMINDER.doc";
            files[3] = letterDir + fDate + "_" + accNo + "_SUMMONS.doc";
            files[4] = letterDir + fDate + "_" + accNo + "_LPP.doc";
            return files;
        }

        #endregion Templates

        #region Processing

        public String disconGen(Customer customer, DateTime letterDate, DateTime disconDate, double reconFee, double disconfee, String username, String telephone, String fax, bool docStatement, String buildingCode)
        {
            String newFileName = FileNames(customer.accNumber, letterDate.ToString("yyyyMMdd"))[0];
            bool saved = false;
            try
            {
                Word.Document aDoc = null;
                object filename = newFileName;
                try
                {
                    String fName = "";
                    switch (buildingCode)
                    {
                        case "SVT":
                            fName = templateDir + "svt_disconnotice.doc";
                            break;

                        case "LR":
                            fName = templateDir + "lr_disconnotice_template.doc";
                            break;

                        case "WFM":
                            fName = templateDir + "wfm_disconnotice_template.doc";
                            break;

                        case "OVB":
                        case "OVE":
                        case "OVW":
                            fName = templateDir + "ormond_disconnotice_template.doc";
                            break;

                        case "WBG":
                            fName = templateDir + "wbg_discnotice_template.doc";
                            break;

                        default:
                            fName = templates[0];
                            break;
                    }
                    File.Copy(fName, newFileName, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error copying: " + ex.Message);
                }
                if (File.Exists((string)filename))
                {
                    try
                    {
                        aDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                        aDoc.Activate();
                    }
                    catch
                    {
                        MessageBox.Show("Error opening doc");
                    }
                    try
                    {
                        this.FindAndReplace(wordApp, "«Dos_Date»", letterDate.ToString("yyyy/MM/dd"));
                        this.FindAndReplace(wordApp, "«Name»", customer.description);
                        this.FindAndReplace(wordApp, "«Address_line_1»", customer.address[0]);
                        this.FindAndReplace(wordApp, "«Address_line_2»", customer.address[1]);
                        this.FindAndReplace(wordApp, "«Address_line_3»", customer.address[2]);
                        this.FindAndReplace(wordApp, "«Address_line_4»", customer.address[3]);
                        this.FindAndReplace(wordApp, "«Address_line_5»", customer.address[4]);
                        this.FindAndReplace(wordApp, "«Account_No»", customer.accNumber);
                        this.FindAndReplace(wordApp, "«Total_Amount_Due»", customer.ageing[0].ToString("#,##0.00"));
                        this.FindAndReplace(wordApp, "«Dis_Date»", disconDate.ToString("D"));
                        this.FindAndReplace(wordApp, "«reconfee»", reconFee.ToString("#,##0.00"));
                        this.FindAndReplace(wordApp, "«disconfee»", disconfee.ToString("#,##0.00"));
                        this.FindAndReplace(wordApp, "«username»", username);
                        this.FindAndReplace(wordApp, "«telephone»", telephone);
                        this.FindAndReplace(wordApp, "«fax»", fax);
                    }
                    catch
                    {
                        MessageBox.Show("Error replacing fields");
                    }
                    try
                    {
                        aDoc.Save();
                        SaveAsPDF(aDoc, newFileName);
                        aDoc.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Error Saving Doc");
                    }
                    saved = true;
                }
                else
                {
                    MessageBox.Show("File does not exist.", "No File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in process: " + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (saved)
            {
                return newFileName.Replace(".doc", ".pdf");
            }
            else
            {
                return string.Empty;
            }
        }

        public String FinalGen(Customer customer, DateTime letterDate, double adminfee, String username, String telephone, String fax, bool docStatement, String buildingCode, bool isHOA)
        {
            String newFileName = FileNames(customer.accNumber, letterDate.ToString("yyyyMMdd"))[1];
            bool saved = false;
            try
            {
                try
                {
                    String fName = "";
                    switch (buildingCode)
                    {
                        case "SVT":
                            fName = templateDir + "svt_fd_template.doc";
                            break;

                        case "WFM":
                            fName = templateDir + "wfm_fd_template.doc";
                            break;

                        case "WBG":
                            fName = templateDir + "wbg_final_template.doc";
                            break;

                        default:
                            fName = isHOA ? HOAtemplates[1] : templates[1];
                            break;
                    }
                    File.Copy(fName, newFileName, true);
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("Error copying");
                }
                Word.Document aDoc = null;
                object filename = newFileName;
                if (File.Exists((string)filename))
                {
                    aDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Activate();
                    this.FindAndReplace(wordApp, "«Dos_Date»", letterDate.ToString("yyyy/MM/dd"));
                    this.FindAndReplace(wordApp, "«Name»", customer.description);
                    this.FindAndReplace(wordApp, "«Address_line_1»", customer.address[0]);
                    this.FindAndReplace(wordApp, "«Address_line_2»", customer.address[1]);
                    this.FindAndReplace(wordApp, "«Address_line_3»", customer.address[2]);
                    this.FindAndReplace(wordApp, "«Address_line_4»", customer.address[3]);
                    this.FindAndReplace(wordApp, "«Address_line_5»", customer.address[4]);
                    this.FindAndReplace(wordApp, "«Account_No»", customer.accNumber);
                    this.FindAndReplace(wordApp, "«Total_Amount_Due»", customer.ageing[0].ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«adminfee»", adminfee.ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«username»", username);
                    this.FindAndReplace(wordApp, "«telephone»", telephone);
                    this.FindAndReplace(wordApp, "«fax»", fax);
                    aDoc.Save();
                    SaveAsPDF(aDoc, newFileName);
                    aDoc.Close();
                    if (!docStatement) { try { File.Delete(newFileName); } catch { } }
                    saved = true;
                }
                else
                {
                    MessageBox.Show("File does not exist.", "No File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in process:" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (saved)
            {
                return newFileName.Replace(".doc", ".pdf");
            }
            else
            {
                return string.Empty;
            }
        }

        public String LPPGen(Customer customer, DateTime letterDate, double adminfee, String username, String telephone, String fax, bool docStatement, String buildingCode)
        {
            String newFileName = FileNames(customer.accNumber.Replace("/", "_"), letterDate.ToString("yyyyMMdd"))[4];
            bool saved = false;
            try
            {
                try
                {
                    String fName = templateDir + "Rental LPP_template.doc";
                    File.Copy(fName, newFileName, true);
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("Error copying: " + ex1.Message);
                }
                Word.Document aDoc = null;
                object filename = newFileName;
                if (File.Exists((string)filename))
                {
                    aDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Activate();
                    this.FindAndReplace(wordApp, "«Dos_Date»", letterDate.ToString("yyyy/MM/dd"));
                    this.FindAndReplace(wordApp, "«Name»", customer.description);
                    this.FindAndReplace(wordApp, "«Address_line_1»", customer.address[0]);
                    this.FindAndReplace(wordApp, "«Address_line_2»", customer.address[1]);
                    this.FindAndReplace(wordApp, "«Address_line_3»", customer.address[2]);
                    this.FindAndReplace(wordApp, "«Address_line_4»", customer.address[3]);
                    this.FindAndReplace(wordApp, "«Address_line_5»", customer.address[4]);
                    this.FindAndReplace(wordApp, "«Account_No»", customer.accNumber);
                    this.FindAndReplace(wordApp, "«Total_Amount_Due»", customer.ageing[0].ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«adminfee»", adminfee.ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«username»", username);
                    this.FindAndReplace(wordApp, "«telephone»", telephone);
                    this.FindAndReplace(wordApp, "«fax»", fax);
                    aDoc.Save();
                    SaveAsPDF(aDoc, newFileName);
                    aDoc.Close();
                    if (!docStatement) { try { File.Delete(newFileName); } catch { } }
                    saved = true;
                }
                else
                {
                    MessageBox.Show("File does not exist.", "No File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in process:" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (saved)
            {
                return newFileName.Replace(".doc", ".pdf");
            }
            else
            {
                return string.Empty;
            }
        }

        public String ReminderGen(Customer customer, DateTime letterDate, double adminfee, String username, String telephone, String fax, bool docStatement, String buildingCode, bool isHOA)
        {
            String newFileName = FileNames(customer.accNumber, letterDate.ToString("yyyyMMdd"))[2];
            bool saved = false;
            try
            {
                String fName = "";
                switch (buildingCode)
                {
                    case "SVT":
                        fName = templateDir + "svt_remind_template.doc";
                        break;

                    case "WFM":
                        fName = templateDir + "wfm_rem_template.doc";
                        break;

                    case "WBG":
                        fName = templateDir + "wbg_reminder_template.doc";
                        break;

                    default:
                        fName = isHOA ? HOAtemplates[2] : templates[2];
                        break;
                }
                File.Copy(fName, newFileName, true);
                Word.Document aDoc = null;
                object filename = newFileName;
                if (File.Exists((string)filename))
                {
                    aDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Activate();
                    // Call FindAndReplace()function for each change
                    this.FindAndReplace(wordApp, "«Dos_Date»", letterDate.ToString("yyyy/MM/dd"));
                    this.FindAndReplace(wordApp, "«Name»", customer.description);
                    this.FindAndReplace(wordApp, "«Address_line_1»", customer.address[0]);
                    this.FindAndReplace(wordApp, "«Address_line_2»", customer.address[1]);
                    this.FindAndReplace(wordApp, "«Address_line_3»", customer.address[2]);
                    this.FindAndReplace(wordApp, "«Address_line_4»", customer.address[3]);
                    this.FindAndReplace(wordApp, "«Address_line_5»", customer.address[4]);
                    this.FindAndReplace(wordApp, "«Account_No»", customer.accNumber);
                    this.FindAndReplace(wordApp, "«Total_Amount_Due»", customer.ageing[0].ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«adminfee»", adminfee.ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«username»", username);
                    this.FindAndReplace(wordApp, "«telephone»", telephone);
                    this.FindAndReplace(wordApp, "«fax»", fax);
                    aDoc.Save();
                    SaveAsPDF(aDoc, newFileName);
                    aDoc.Close();
                    if (!docStatement) { try { File.Delete(newFileName); } catch { } }
                    saved = true;
                }
                else
                {
                    MessageBox.Show("File does not exist.", "No File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in process:" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (saved)
            {
                return newFileName.Replace(".doc", ".pdf");
            }
            else
            {
                return string.Empty;
            }
        }

        public String SummonsGen(Customer customer, DateTime letterDate, double adminfee, String username, String telephone, String fax, bool docStatement, String buildingCode, bool isHOA)
        {
            String newFileName = FileNames(customer.accNumber, letterDate.ToString("yyyyMMdd"))[3];
            bool saved = false;
            String fName = "";
            try
            {
                switch (buildingCode)
                {
                    case "WFM":
                        fName = templateDir + "wfm_sumpend_template.doc";
                        break;

                    default:
                        fName = isHOA ? HOAtemplates[3] : templates[3];
                        break;
                }
                File.Copy(fName, newFileName, true);
                Word.Document aDoc = null;
                object filename = newFileName;
                if (File.Exists((string)filename))
                {
                    aDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Activate();
                    this.FindAndReplace(wordApp, "«Dos_Date»", letterDate.ToString("yyyy/MM/dd"));
                    this.FindAndReplace(wordApp, "«Name»", customer.description);
                    this.FindAndReplace(wordApp, "«Address_line_1»", customer.address[0]);
                    this.FindAndReplace(wordApp, "«Address_line_2»", customer.address[1]);
                    this.FindAndReplace(wordApp, "«Address_line_3»", customer.address[2]);
                    this.FindAndReplace(wordApp, "«Address_line_4»", customer.address[3]);
                    this.FindAndReplace(wordApp, "«Address_line_5»", customer.address[4]);
                    this.FindAndReplace(wordApp, "«Account_No»", customer.accNumber);
                    this.FindAndReplace(wordApp, "«Total_Amount_Due»", customer.ageing[0].ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«adminfee»", adminfee.ToString("#,##0.00"));
                    this.FindAndReplace(wordApp, "«username»", username);
                    this.FindAndReplace(wordApp, "«telephone»", telephone);
                    this.FindAndReplace(wordApp, "«fax»", fax);

                    aDoc.Save();
                    SaveAsPDF(aDoc, newFileName);
                    aDoc.Close();
                    if (!docStatement) { try { File.Delete(newFileName); } catch (Exception ex) { MessageBox.Show(ex.Message); } }

                    saved = true;
                }
                else
                {
                    MessageBox.Show("File does not exist.", "No File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in process:" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (saved)
            {
                return newFileName.Replace(".doc", ".pdf");
            }
            else
            {
                return string.Empty;
            }
        }

        private void FindAndReplace(Word.Application wordApp, object findText, object replaceText)
        {
            try
            {
                Word.Find myFind = wordApp.Selection.Find;
                object typeMissing = System.Reflection.Missing.Value;
                object wdReplaceAll = Word.WdReplace.wdReplaceAll;
                myFind.Execute(ref findText, ref typeMissing, ref typeMissing, ref typeMissing, ref typeMissing, ref typeMissing, ref typeMissing, ref typeMissing, ref typeMissing, ref replaceText, ref wdReplaceAll, ref typeMissing, ref typeMissing, ref typeMissing, ref typeMissing);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveAsPDF(Word.Document doc, String wordFile)
        {
            object outputFileName = wordFile.Replace(".doc", ".pdf");
            object fileFormat = Word.WdSaveFormat.wdFormatPDF;
            doc.SaveAs(ref outputFileName, ref fileFormat, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
        }

        #endregion Processing

        public bool CloseWord()
        {
            try
            {
                if (wordApp != null)
                {
                    wordApp.Quit(missing, missing, missing);
                }
                wordApp = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}