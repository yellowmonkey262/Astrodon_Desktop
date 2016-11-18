using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Astrodon
{
    public class PDF
    {
        private SqlDataHandler dh;
        private PdfContentByte _pcb;
        private BaseFont bf;
        private BaseFont bf2;
        private iTextSharp.text.Font fontT = FontFactory.GetFont("Calibri", 6f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        private iTextSharp.text.Font font = FontFactory.GetFont("Calibri", 7f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        private iTextSharp.text.Font fontB = FontFactory.GetFont("Calibri", 7f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        private iTextSharp.text.Font fontB2 = FontFactory.GetFont("Helvetica", 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        private iTextSharp.text.Font fontBig = FontFactory.GetFont("Calibri", 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        private iTextSharp.text.Font fontBU = FontFactory.GetFont("Calibri", 8.5f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
        private iTextSharp.text.Font fontR = FontFactory.GetFont("Calibri", 8.5f, iTextSharp.text.Font.BOLD, BaseColor.RED);
        private iTextSharp.text.Font fontRI = FontFactory.GetFont("Calibri", 8.5f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC, BaseColor.RED);
        private iTextSharp.text.Font fontCertificate = FontFactory.GetFont("Monotype Corsiva", 20f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        private iTextSharp.text.Font fontCert = FontFactory.GetFont("Calibri", 11f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        private iTextSharp.text.Font fontCertB = FontFactory.GetFont("Calibri", 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        private iTextSharp.text.Font fontCertTB = FontFactory.GetFont("Times New Roman", 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        private iTextSharp.text.Font fontPA = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        private iTextSharp.text.Font fontE = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        private iTextSharp.text.Font fontPAI = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
        private iTextSharp.text.Font fontPAU = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
        private iTextSharp.text.Font fontPAIU = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.ITALIC | iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
        private iTextSharp.text.Font fontPAB = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        private iTextSharp.text.Font fontPABI = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
        private iTextSharp.text.Font fontPABU = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
        private iTextSharp.text.Font fontPABIU = FontFactory.GetFont("Arial Narrow", 11f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC | iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);

        //
        private ClearanceObject clr = null;

        private String folderPath = "";

        public PDF()
        {
            dh = new SqlDataHandler();
            bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
            String clearanceFolder = "Clearances";
            String appData = AppDomain.CurrentDomain.BaseDirectory;
            folderPath = Path.Combine(appData, clearanceFolder);
            if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        }

        private String statementFolder = String.Empty;

        public PDF(bool statementRun)
        {
            dh = new SqlDataHandler();
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            bf2 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
            statementFolder = "statements";
            String serverDrive = "K:\\Debtors System";
            //String serverDrive = AppDomain.CurrentDomain.BaseDirectory;
            folderPath = Path.Combine(serverDrive, statementFolder);
            if (!Directory.Exists(folderPath))
            {
                folderPath = Path.Combine("C:\\Pastel11\\Debtors System", statementFolder);
            }
        }

        private bool GetClearance(int id)
        {
            String query1 = "SELECT id, buildingCode, customerCode, preparedBy, trfAttorneys, attReference, fax, certDate, complex, unitNo, seller, purchaser, purchaserAddress, purchaserTel, ";
            query1 += " purchaserEmail, regDate, notes, clearanceFee, astrodonTotal, validDate, processed, registered, extClearance FROM tblClearances WHERE (id = " + id.ToString() + ")";
            String status = String.Empty;
            DataSet ds = dh.GetData(query1, null, out status);
            DataRow dr = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];
            }
            String query2 = "SELECT description, amount FROM tblClearanceTransactions WHERE clearanceID = " + id.ToString();
            DataSet ds2 = dh.GetData(query2, null, out status);
            if (dr != null)
            {
                clr = new ClearanceObject(id, dr, ds2);
                return true;
            }
            else
            {
                MessageBox.Show(query1);
                return false;
            }
        }

        public bool Create(int clearanceID, out String fName)
        {
            bool success = false;
            fName = String.Empty;
            try
            {
                fName = "";
                if (GetClearance(clearanceID))
                {
                    Document document = new Document();
                    PdfReader reader = null;
                    fName = Path.Combine(folderPath, String.Format("Clearance request - {0}.pdf", clr.customerCode));
                    if (File.Exists(fName)) { File.Delete(fName); }
                    FileStream stream = new FileStream(fName, FileMode.CreateNew);
                    try
                    {
                        PdfWriter writer = PdfWriter.GetInstance(document, stream);
                        document.Open();
                        _pcb = writer.DirectContent;
                        String directory = AppDomain.CurrentDomain.BaseDirectory;
                        Image img = Image.GetInstance(Path.Combine(directory, "astlogo.jpg"));
                        document.Add(img);
                        Paragraph paragraphSpacer = new Paragraph(" ");
                        document.Add(paragraphSpacer);

                        PdfPTable table = new PdfPTable(2);
                        table.TotalWidth = 510;
                        table.HorizontalAlignment = 1;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 255, 255 };
                        table.SetWidths(widths);
                        table.DefaultCell.Border = 1;

                        #region InvHeader

                        PdfPCell cell0 = new PdfPCell(new Paragraph("PREPARED BY: " + clr.preparedBy, fontB));
                        cell0.HorizontalAlignment = 1;
                        cell0.Colspan = 2;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("TRANSFERRING ATTORNEYS", fontB));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.trfAttorneys, font));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("ATTENTION YOUR REFERENCE", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.attReference, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("FACSIMILE NUMBER", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.fax, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("DATE", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.certDate.ToString("yyyy/MM/dd"), font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("CLEARANCE REQUIREMENT", fontB));
                        cell0.HorizontalAlignment = 1;
                        cell0.Colspan = 2;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", fontB));
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("COMPLEX", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.complex, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("UNIT NO", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.unitNo, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("NAME OF SELLER", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.seller, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", fontB));
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("NAME OF PURCHASER", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.purchaser, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("ADDRESS OF PURCHASER", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.purchaserAddress, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("TEL NO PURCHASER", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.purchaserTel, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("EMAIL ADDRESS OF PURCHASER", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.purchaserEmail, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("REGISTRATION DATE", fontB));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        if (!clr.registered)
                        {
                            cell0 = new PdfPCell(new Paragraph(clr.regDate.ToString("yyyy/MM/dd"), font));
                        }
                        else
                        {
                            cell0 = new PdfPCell(new Paragraph("NOT AVAILABLE", font));
                        }
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", fontB));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.notes, font));

                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("DETAILS OF CHARGES", fontBU));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("TOTAL", fontBU));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        #endregion InvHeader

                        foreach (ClearanceObjectTrans trn in clr.Trans)
                        {
                            cell0 = new PdfPCell(new Paragraph(trn.description, font));
                            cell0.HorizontalAlignment = 0;
                            table.AddCell(cell0);
                            cell0 = new PdfPCell(new Paragraph("R" + trn.amount.ToString("#,##0.00"), font));
                            cell0.HorizontalAlignment = 0;
                            table.AddCell(cell0);
                        }

                        #region Totals

                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("CLEARANCE FEES", font));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("R" + clr.clearanceFee.ToString("#,##0.00"), font));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("TOTAL DUE TO ASTRODON", fontB));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("R" + clr.astrodonTotal.ToString("#,##0.00"), fontB));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("VALID TO", fontB));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(clr.validTo.ToString("yyyy/MM/dd"), fontB));
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        cell0.Border = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("ALL CHARGES TO BE APPORTIONED BY THE TRANSFERRING ATTORNEY BETWEEN THE SELLER AND PURCHASER, REGARD TO DATES OF OCCUPATION!!!", fontB));
                        cell0.Border = 0;
                        cell0.Colspan = 2;
                        cell0.HorizontalAlignment = 1;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        cell0.Colspan = 2;
                        cell0.Border = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("BANKING DETAILS", fontB));
                        cell0.Border = 0;
                        cell0.Colspan = 2;
                        cell0.HorizontalAlignment = 1;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        cell0.Colspan = 2;
                        cell0.Border = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("COMPANY NAME", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("ASTRODON (PTY) LTD TRUST ACCOUNT", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("BANK", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("NEDBANK", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("BRANCH", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("ALBERTON", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("ACCOUNT NUMBER", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("1902226429", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("BRANCH CODE", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("190-242", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", font));
                        cell0.HorizontalAlignment = 0;
                        cell0.Colspan = 2;
                        cell0.Border = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("NB: PLEASE USE THE FOLLOWING REFERENCE ON YOUR DEPOSIT SLIP:", fontB));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 1;
                        cell0.Colspan = 2;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", fontBig));
                        cell0.Border = 0;
                        cell0.Colspan = 2;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(clr.customerCode, fontBig));
                        cell0.Border = 0;
                        cell0.Colspan = 2;
                        cell0.HorizontalAlignment = 1;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", fontBig));
                        cell0.Border = 0;
                        cell0.Colspan = 2;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph(" ", fontBig));
                        cell0.Border = 0;
                        cell0.Colspan = 2;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);

                        cell0 = new PdfPCell(new Paragraph("Company Registration: 2004-003502/07", fontT));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 0;
                        table.AddCell(cell0);
                        cell0 = new PdfPCell(new Paragraph("Director: Sheldon Terry", fontT));
                        cell0.Border = 0;
                        cell0.HorizontalAlignment = 2;
                        table.AddCell(cell0);

                        #endregion Totals

                        document.Add(table);

                        writer.Flush();
                        writer.Close();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                            reader.Dispose();
                        }

                        try
                        {
                            document.Close();
                        }
                        catch { }
                    }
                    if (document.IsOpen()) { document.Close(); }
                    //Process.Start(fName);
                    success = true;
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Producing: " + ex1.Message);
            }
            return success;
        }

        public void CreateBCCertificate(int clearanceID, out String fName)
        {
            fName = String.Empty;
            if (GetClearance(clearanceID))
            {
                Document document = new Document();
                PdfReader reader = null;
                fName = Path.Combine(folderPath, String.Format("Body Corporate Clearance certificate - {0}.pdf", clr.customerCode));
                if (File.Exists(fName)) { File.Delete(fName); }
                FileStream stream = new FileStream(fName, FileMode.CreateNew);
                String message = "creating";
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Get the top layer and write some text
                    _pcb = writer.DirectContent;
                    PdfContentByte under = writer.DirectContentUnder;
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                    under.BeginText();
                    String directory = AppDomain.CurrentDomain.BaseDirectory;
                    Image img = Image.GetInstance(Path.Combine(directory, "astbackground.png"));
                    img.ScaleToFit(PageSize.A4.Width - 10, PageSize.A4.Height - 10);
                    img.SetAbsolutePosition(5, 5);
                    under.AddImage(img);
                    under.EndText();

                    Paragraph paragraphSpacer = new Paragraph(" ");
                    iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 90.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1);
                    for (int i = 0; i < 2; i++) { document.Add(paragraphSpacer); }

                    document.Add(CreateParagraph("CERTIFICATE", fontCertificate));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("I, the undersigned, Sheldon Terry, in my capacity as duly authorized representative of the", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Managing Agents of the Body Corporate of", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(String.Format("{0} BODY CORPORATE", clr.complex.Replace("B/C", "").Replace(" BODY CORPORATE", "")), fontCertB));
                    document.Add(new Chunk(line));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("ASTRODON (PTY) LTD", fontCertTB));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Do hereby certify that all monies due to the Body Corporate have been paid in respect of ", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(clr.unitNo, fontCertB));
                    document.Add(new Chunk(line));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("in the scheme known as", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(String.Format("{0} BODY CORPORATE", clr.complex.Replace("B/C", "").Replace(" BODY CORPORATE", "")), fontCertB));
                    document.Add(new Chunk(line));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("This Certificate is valid to", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(clr.validTo.ToString("dd MMMM yyyy"), fontCertB));
                    document.Add(new Chunk(line));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("DATED at Brackenhurst this the ____________  day of _________________20___", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(new Chunk(line));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Sheldon Terry", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Astrodon as Managing Agent for", fontCert));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(String.Format("{0} BODY CORPORATE", clr.complex.Replace("B/C", "").Replace(" BODY CORPORATE", "")), fontCertB));
                    document.Add(new Chunk(line));

                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }

                    try
                    {
                        document.Close();
                    }
                    catch { }
                }
                if (document.IsOpen()) { document.Close(); }
            }
        }

        public void CreateBCLetter(int clearanceID, out String fName)
        {
            fName = String.Empty;
            if (GetClearance(clearanceID))
            {
                Document document = new Document();
                PdfReader reader = null;
                fName = Path.Combine(folderPath, String.Format("Body Corporate Clearance letter - {0}.pdf", clr.customerCode));
                if (File.Exists(fName)) { File.Delete(fName); }
                FileStream stream = new FileStream(fName, FileMode.CreateNew);
                String message = "creating";
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Get the top layer and write some text
                    _pcb = writer.DirectContent;
                    PdfContentByte under = writer.DirectContentUnder;
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                    under.BeginText();
                    String directory = AppDomain.CurrentDomain.BaseDirectory;
                    Image img = Image.GetInstance(Path.Combine(directory, "astbackground.png"));
                    img.ScaleToFit(PageSize.A4.Width - 10, PageSize.A4.Height - 10);
                    img.SetAbsolutePosition(5, 5);
                    under.AddImage(img);
                    under.EndText();

                    Paragraph paragraphSpacer = new Paragraph(" ");
                    iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 90.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1);
                    for (int i = 0; i < 3; i++) { document.Add(paragraphSpacer); }

                    PdfPTable table = new PdfPTable(3);
                    table.TotalWidth = 510;
                    table.HorizontalAlignment = 1;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 150, 10, 350 };
                    table.SetWidths(widths);
                    table.DefaultCell.Border = 0;

                    PdfPCell cell0 = new PdfPCell(CreateParagraph("DATE", fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(DateTime.Now.ToString("dd MMMM yyyy"), fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("ATTORNEY", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.trfAttorneys, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("ATTENTION", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.attReference, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("FAX NUMBER", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.fax, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph(" ", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(" ", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(" ", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("COMPLEX", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.complex.Replace("B/C", "").Replace(" BODY CORPORATE", ""), fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("TRANSFER UNIT", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.unitNo, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("FROM", fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.seller, fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("TO", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.purchaser, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    document.Add(table);

                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("As requested, we enclose herewith our Clearance Certificate as requested by you in terms of the Sectional titles Act 95 of 1986, as amended.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("It is a condition of the issuing of the Clearance Certificate that should transfer not have taken place during the validity of this Clearance Certificate, you undertake to request an updated Certificate, failing which we will look to you for the recovery of any amount whatsoever due to the Body Corporate at date of transfer, from your client.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("PLEASE NOTE:  THIS CERTIFICATE DOES NOT COVER ANY INCREASES IN THE MONTHLY LEVY NOR ANY SPECIAL LEVY, WHICH MAY HAVE BEEN IMPOSED IN THE INTERIM.", fontCertB, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("NO LEVY ADJUSTMENT ACCOUNT BETWEEN THE SELLER AND PURCHASER WILL BE PREPARED BY OUR OFFICE ON REGISTRATION OF TRANSFER.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("We further note that we have NOT received the Purchasers details which we shall be pleased if advise of the date the property will be registration.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Thanking you for your kind co-operation in this matter.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Yours faithfully", fontCertB, Element.ALIGN_LEFT));
                    document.Add(CreateParagraph("ASTRODON (PTY) LTD", fontCertB, Element.ALIGN_LEFT));
                    document.Add(CreateParagraph(String.Format("As Managing Agents for {0} Body Corporate", clr.complex.Replace("B/C", "").Replace(" BODY CORPORATE", "")), fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Sheldon Terry", fontCertB, Element.ALIGN_LEFT));
                    document.Add(CreateParagraph("Managing Director", fontCertB, Element.ALIGN_LEFT));

                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }

                    try
                    {
                        document.Close();
                    }
                    catch { }
                }
                if (document.IsOpen()) { document.Close(); }
            }
        }

        public void CreateHOACertificate(int clearanceID, out String fName)
        {
            fName = String.Empty;
            if (GetClearance(clearanceID))
            {
                Document document = new Document();
                PdfReader reader = null;
                fName = Path.Combine(folderPath, String.Format("HOA Clearance certificate - {0}.pdf", clr.customerCode));
                if (File.Exists(fName)) { File.Delete(fName); }
                FileStream stream = new FileStream(fName, FileMode.CreateNew);
                String message = "creating";
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Get the top layer and write some text
                    _pcb = writer.DirectContent;
                    PdfContentByte under = writer.DirectContentUnder;
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                    under.BeginText();
                    String directory = AppDomain.CurrentDomain.BaseDirectory;
                    Image img = Image.GetInstance(Path.Combine(directory, "astbackground.png"));
                    img.ScaleToFit(PageSize.A4.Width - 10, PageSize.A4.Height - 10);
                    img.SetAbsolutePosition(5, 5);
                    under.AddImage(img);
                    under.EndText();

                    Paragraph paragraphSpacer = new Paragraph(" ");
                    iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 90.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1);
                    for (int i = 0; i < 2; i++) { document.Add(paragraphSpacer); }

                    document.Add(CreateParagraph("CERTIFICATE", fontCertificate, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("I, the undersigned, Sheldon Terry, in my capacity as duly authorized representative of the", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(String.Format("{0} HOA (NPC)", clr.complex), fontCertTB, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("ASTRODON (PTY) LTD", fontCertTB, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Do hereby certify in respect of ", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(clr.unitNo, fontCertB, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("1. All monies due to the Home Owners Association have been paid.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("2. The purchaser,", fontCert, Element.ALIGN_LEFT));
                    document.Add(CreateParagraph("   " + clr.purchaser, fontCertB, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("3. , have bound here to the satisfaction of the Home Owners Association to become members of the Home Owners Association.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("4. The provisions if the Articles of Association of the Home Owners Association have been complied with.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("This Certificate is valid to", fontCertB, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(clr.validTo.ToString("dd MMMM yyyy"), fontCertB, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("DATED at Brackenhurst this the ____________  day of _________________20___", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("_________________________________", fontCert, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Sheldon Terry", fontCertB, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Astrodon as Managing Agent for", fontCert, Element.ALIGN_CENTER));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph(String.Format("{0} HOA (NPC)", clr.complex), fontCertB, Element.ALIGN_CENTER));
                    document.Add(new Chunk(line));

                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }

                    try
                    {
                        document.Close();
                    }
                    catch { }
                }
                if (document.IsOpen()) { document.Close(); }
            }
        }

        public void CreateHOALetter(int clearanceID, out String fName)
        {
            fName = String.Empty;
            if (GetClearance(clearanceID))
            {
                Document document = new Document();
                PdfReader reader = null;
                fName = Path.Combine(folderPath, String.Format("HOA Clearance letter - {0}.pdf", clr.customerCode));
                if (File.Exists(fName)) { File.Delete(fName); }
                FileStream stream = new FileStream(fName, FileMode.CreateNew);
                String message = "creating";
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Get the top layer and write some text
                    _pcb = writer.DirectContent;
                    PdfContentByte under = writer.DirectContentUnder;
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                    under.BeginText();
                    String directory = AppDomain.CurrentDomain.BaseDirectory;
                    Image img = Image.GetInstance(Path.Combine(directory, "astbackground.png"));
                    img.ScaleToFit(PageSize.A4.Width - 10, PageSize.A4.Height - 10);
                    img.SetAbsolutePosition(5, 5);
                    under.AddImage(img);
                    under.EndText();

                    Paragraph paragraphSpacer = new Paragraph(" ");
                    iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 90.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1);
                    for (int i = 0; i < 4; i++) { document.Add(paragraphSpacer); }

                    PdfPTable table = new PdfPTable(3);
                    table.TotalWidth = 510;
                    table.HorizontalAlignment = 1;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 150, 10, 350 };
                    table.SetWidths(widths);
                    table.DefaultCell.Border = 0;

                    PdfPCell cell0 = new PdfPCell(CreateParagraph("DATE", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(DateTime.Now.ToString("dd MMMM yyyy"), fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("ATTORNEY", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.trfAttorneys, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("ATTENTION", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.attReference, fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("FAX NUMBER", fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.fax, fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("NO. OF PAGES", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph("3", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("COMPLEX", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.complex, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    document.Add(table);

                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Dear Sirs", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);

                    table = new PdfPTable(3);
                    table.TotalWidth = 510;
                    table.HorizontalAlignment = 1;
                    table.LockedWidth = true;
                    table.SetWidths(widths);
                    table.DefaultCell.Border = 0;

                    cell0 = new PdfPCell(CreateParagraph("TRANSFER UNIT", fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.unitNo, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("FROM", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.seller, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    cell0 = new PdfPCell(CreateParagraph("TO", fontCertB, Element.ALIGN_LEFT));
                    cell0.HorizontalAlignment = 0;
                    cell0.Border = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(":", fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);
                    cell0 = new PdfPCell(CreateParagraph(clr.purchaser, fontCertB, Element.ALIGN_LEFT));
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 0;
                    table.AddCell(cell0);

                    document.Add(table);

                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("As requested, we enclose herewith our Clearance Certificate as requested by you in terms of the Articles of Association.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("It is a condition of the issuing of the Clearance Certificate that should transfer not have taken place during the validity of this Clearance Certificate, you undertake to request an updated Certificate, failing which we will look to you for the recovery of any amount whatsoever due to the HOA at date of transfer, from your client.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("PLEASE NOTE:  THIS CERTIFICATE DOES NOT COVER ANY INCREASES IN THE MONTHLY LEVY NOR ANY SPECIAL LEVY, WHICH MAY HAVE BEEN IMPOSED IN THE INTERIM.", fontCertB, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("NO LEVY ADJUSTMENT ACCOUNT BETWEEN THE SELLER AND PURCHASER WILL BE PREPARED BY OUR OFFICE ON REGISTRATION OF TRANSFER.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("We further note that we have NOT received the Purchasers details which we shall be pleased if advise of the date the property will be registration.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Thanking you for your kind co-operation in this matter.", fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Yours faithfully", fontCertB, Element.ALIGN_LEFT));
                    document.Add(CreateParagraph("ASTRODON (PTY) LTD", fontCertB, Element.ALIGN_LEFT));
                    document.Add(CreateParagraph(String.Format("As Managing Agents for {0} HOA", clr.complex), fontCert, Element.ALIGN_LEFT));
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(paragraphSpacer);
                    document.Add(CreateParagraph("Sheldon Terry", fontCertB, Element.ALIGN_LEFT));
                    document.Add(CreateParagraph("Managing Director", fontCertB, Element.ALIGN_LEFT));

                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }

                    try
                    {
                        document.Close();
                    }
                    catch { }
                }
                if (document.IsOpen()) { document.Close(); }
            }
        }

        public bool CreateStatement(Statement statement, bool isBuilding, out String fName, bool excludeStationery = false)
        {
            bool success = false;
            String message = "";
            message = "Starting";

            #region Create Document

            String templateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stmt_template.pdf");
            PdfReader reader = new PdfReader(templateFile);
            fName = String.Empty;
            Document document = new Document();
            //PdfReader reader = null;
            FileStream stream;
            try
            {
                fName = Path.Combine(folderPath, String.Format("{0} - statement - {1}_{2}.pdf", statement.AccNo.Replace(@"/", "-").Replace(@"\", "-"), DateTime.Now.ToString("dd-MMMM-yyyy"), (isBuilding ? "" : "R")));
                if (File.Exists(fName)) { File.Delete(fName); }
                stream = new FileStream(fName, FileMode.CreateNew);
            }
            catch
            {
                fName = String.Empty;
                return false;
            }
            message = "creating";

            #endregion Create Document

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                PdfTemplate background = null;
                background = writer.GetImportedPage(reader, 1);
                int transactionNumber = 0;
                while (transactionNumber < statement.Transactions.Count)
                {
                    int newTrnNumber = 0;
                    CreateStatements(statement, ref document, writer, background, transactionNumber, excludeStationery, out newTrnNumber);
                    transactionNumber = newTrnNumber;
                }

                document.Close();
                writer.Close();
                stream.Close();
                if (reader != null) { reader.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            success = true;

            return success;
        }

        public void CreateStatements(Statement statement, ref Document document, PdfWriter writer, PdfTemplate background, int transNumber, bool stdBank, out int newTrnNumber)
        {
            // Create a page in the document and add it to the bottom layer
            document.NewPage();
            if (background != null)
            {
                _pcb = writer.DirectContentUnder;
                _pcb.AddTemplate(background, 0, 0);
            }
            _pcb = writer.DirectContent;
            SetFont7();

            _pcb.BeginText();

            _pcb.ShowTextAligned(0, statement.StmtDate.ToString("yyyy/MM/dd"), 350, 780, 0);
            _pcb.EndText();

            _pcb.BeginText();
            _pcb.ShowTextAligned(0, statement.AccNo, 350, 740, 0);
            _pcb.EndText();

            _pcb.BeginText();
            _pcb.ShowTextAligned(0, statement.pm, 350, 700, 0);
            _pcb.EndText();

            _pcb.BeginText();
            _pcb.ShowTextAligned(0, statement.DebtorEmail, 350, 660, 0);
            _pcb.EndText();

            int startY = 700;
            foreach (String addyLine in statement.Address)
            {
                _pcb.BeginText();
                _pcb.ShowTextAligned(0, addyLine, 50, startY, 0);
                _pcb.EndText();
                startY -= 15;
            }

            #region table

            Paragraph paragraphSpacer = new Paragraph(" ");
            for (int i = 0; i < 10; i++)
            {
                document.Add(paragraphSpacer);
            }

            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 220;
            table.HorizontalAlignment = 2;
            table.LockedWidth = true;
            float[] widths = new float[] { 220 };
            table.SetWidths(widths);
            table.DefaultCell.Border = 0;

            //#region InvHeader
            PdfPCell cell0 = new PdfPCell(new Paragraph(statement.LevyMessage1, font));
            cell0.HorizontalAlignment = 0;
            cell0.Colspan = 1;
            cell0.Border = 0;
            table.AddCell(cell0);
            cell0 = new PdfPCell(new Paragraph(statement.LevyMessage2, fontB));
            cell0.HorizontalAlignment = 0;
            cell0.Colspan = 1;
            cell0.Border = 0;
            table.AddCell(cell0);
            document.Add(table);

            document.Add(paragraphSpacer);
            document.Add(paragraphSpacer);

            table = new PdfPTable(7);
            table.TotalWidth = 510;
            table.HorizontalAlignment = 0;
            table.LockedWidth = true;
            widths = new float[] { 50, 50, 50, 50, 200, 100, 110 };
            table.SetWidths(widths);

            int topleftbottom = 0;
            int topbottom = 0;
            int toprightbottom = 0;
            int left = 0;
            int right = 0;
            int all = 0;
            int topleft = 0;
            int top = 0;
            int topright = 0;
            int bottom = 0;
            int bottomleft = 0;
            int bottomright = 0;
            if (background != null)
            {
                topleftbottom = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
                toprightbottom = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                topbottom = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                top = Rectangle.TOP_BORDER;
                bottom = Rectangle.BOTTOM_BORDER;
                left = Rectangle.LEFT_BORDER;
                right = Rectangle.RIGHT_BORDER;
                topleft = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
                topright = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                bottomleft = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                bottomright = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                all = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            }

            PdfPCell cell1 = new PdfPCell(new Paragraph("Date", fontB));
            cell1.HorizontalAlignment = 0;
            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell1.Border = topleftbottom;
            cell1.BorderColorTop = BaseColor.BLACK;
            cell1.BorderColorLeft = BaseColor.BLACK;
            cell1.BorderColorBottom = BaseColor.DARK_GRAY;
            cell1.BorderColorRight = BaseColor.WHITE;
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Paragraph("Reference", fontB));
            cell2.HorizontalAlignment = 0;
            cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell2.Border = topbottom;
            cell2.BorderColorTop = BaseColor.BLACK;
            cell2.BorderColorLeft = BaseColor.WHITE;
            cell2.BorderColorBottom = BaseColor.DARK_GRAY;
            cell2.BorderColorRight = BaseColor.WHITE;
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Paragraph("Description", fontB));
            cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell3.Colspan = 3;
            cell3.Border = topbottom;
            cell3.BorderColorTop = BaseColor.BLACK;
            cell3.BorderColorLeft = BaseColor.WHITE;
            cell3.BorderColorBottom = BaseColor.DARK_GRAY;
            cell3.BorderColorRight = BaseColor.WHITE;
            cell3.HorizontalAlignment = 0;
            table.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Paragraph("Transaction Amount", fontB));
            cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell4.HorizontalAlignment = 1;
            cell4.Border = topbottom;
            cell4.BorderColorTop = BaseColor.BLACK;
            cell4.BorderColorLeft = BaseColor.WHITE;
            cell4.BorderColorBottom = BaseColor.DARK_GRAY;
            cell4.BorderColorRight = BaseColor.WHITE;
            table.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Paragraph("Accumulated Balance", fontB));
            cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell5.Border = toprightbottom;
            cell5.BorderColorTop = BaseColor.BLACK;
            cell5.BorderColorLeft = BaseColor.WHITE;
            cell5.BorderColorBottom = BaseColor.DARK_GRAY;
            cell5.BorderColorRight = BaseColor.BLACK;
            cell5.HorizontalAlignment = 1;
            table.AddCell(cell5);
            int transCount = (stdBank ? 25 : 30);
            int transMax = transNumber + (stdBank ? 25 : 30);
            if (transMax > statement.Transactions.Count) { transMax = statement.Transactions.Count; }
            for (int i = transNumber; i < transMax; i++)
            {
                Transaction trn = statement.Transactions[i];
                cell0 = new PdfPCell(new Paragraph(trn.TrnDate.ToString("yyyy/MM/dd"), font));
                cell0.Border = left;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(trn.Reference, font));
                cell0.Border = Rectangle.NO_BORDER;
                cell0.HorizontalAlignment = 0;
                cell0.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(trn.Description, font));
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = 3;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(trn.TrnAmt.ToString("#,##0.00"), font));
                cell0.HorizontalAlignment = 2;
                cell0.Border = Rectangle.NO_BORDER;
                cell0.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(trn.AccAmt.ToString("#,##0.00"), font));
                cell0.Border = Rectangle.RIGHT_BORDER;
                cell0.BorderColorRight = BaseColor.BLACK;
                cell0.HorizontalAlignment = 2;
                table.AddCell(cell0);
                transCount--;
                transNumber++;
            }
            newTrnNumber = transNumber;
            for (int i = 0; i < transCount; i++)
            {
                cell0 = new PdfPCell(new Paragraph(" ", font));
                cell0.Border = left;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(" ", font));
                cell0.Border = Rectangle.NO_BORDER;
                cell0.HorizontalAlignment = 0;
                cell0.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(" ", font));
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = 3;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(" ", font));
                cell0.HorizontalAlignment = 2;
                cell0.Border = Rectangle.NO_BORDER;
                cell0.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(" ", font));
                cell0.Border = right;
                cell0.BorderColorRight = BaseColor.BLACK;
                cell0.HorizontalAlignment = 2;
                table.AddCell(cell0);
            }
            //#region Totals

            cell0 = new PdfPCell(new Paragraph("***PLEASE USE THE FOLLOWING", font));
            cell0.Border = topleftbottom;
            cell0.BorderColorTop = BaseColor.BLACK;
            cell0.BorderColorBottom = BaseColor.BLACK;
            cell0.BorderColorLeft = BaseColor.BLACK;
            cell0.Colspan = 3;
            cell0.HorizontalAlignment = 0;
            table.AddCell(cell0);

            cell0 = new PdfPCell(new Paragraph(statement.AccNo, fontB));
            cell0.HorizontalAlignment = 0;
            cell0.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell0.Border = topbottom;
            cell0.BorderColorTop = BaseColor.BLACK;
            cell0.BorderColorBottom = BaseColor.BLACK;
            table.AddCell(cell0);

            cell0 = new PdfPCell(new Paragraph("AS REFERENCE***", font));
            cell0.HorizontalAlignment = 0;
            cell0.Border = toprightbottom;
            cell0.BorderColorTop = BaseColor.BLACK;
            cell0.BorderColorBottom = BaseColor.BLACK;
            cell0.BorderColorRight = BaseColor.BLACK;
            table.AddCell(cell0);

            cell0 = new PdfPCell(new Paragraph("TOTAL DUE", font));
            cell0.HorizontalAlignment = 0;
            cell0.Border = topleftbottom;
            cell0.BorderColorTop = BaseColor.BLACK;
            cell0.BorderColorBottom = BaseColor.BLACK;
            cell0.BorderColorLeft = BaseColor.BLACK;
            table.AddCell(cell0);

            cell0 = new PdfPCell(new Paragraph(statement.totalDue.ToString("#,##0.00"), fontB));
            cell0.HorizontalAlignment = 2;
            cell0.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell0.Border = toprightbottom;
            cell0.BorderColorTop = BaseColor.BLACK;
            cell0.BorderColorBottom = BaseColor.BLACK;
            cell0.BorderColorRight = BaseColor.BLACK;
            table.AddCell(cell0);

            document.Add(table);

            if (!String.IsNullOrEmpty(statement.Message))
            {
                //yAdjustment = 0;
                document.Add(paragraphSpacer);

                table = new PdfPTable(1);
                table.TotalWidth = 510;
                table.HorizontalAlignment = 0;
                table.LockedWidth = true;
                widths = new float[] { 510 };
                table.SetWidths(widths);
                table.DefaultCell.Border = 1;

                cell0 = new PdfPCell(new Paragraph(statement.Message, font));
                cell0.Border = all;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorBottom = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.BorderColorRight = BaseColor.BLACK;
                cell0.HorizontalAlignment = 1;
                table.AddCell(cell0);

                document.Add(table);
            }

            if (!stdBank)
            {
                document.Add(paragraphSpacer);
                table = new PdfPTable(2);
                table.TotalWidth = 510;
                table.HorizontalAlignment = 0;
                table.LockedWidth = true;
                widths = new float[] { 125, 385 };
                table.SetWidths(widths);

                cell0 = new PdfPCell(new Paragraph("BANKING DETAILS:", font));
                cell0.Border = topleftbottom;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorBottom = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(statement.BankDetails, font));
                cell0.HorizontalAlignment = 0;
                cell0.Border = toprightbottom;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorBottom = BaseColor.BLACK;
                cell0.BorderColorRight = BaseColor.BLACK;
                table.AddCell(cell0);

                document.Add(table);
                int yAdjustment = 50;

                #region deposit slip

                SetFont(24);
                _pcb.BeginText();
                _pcb.SetColorFill(BaseColor.LIGHT_GRAY);
                _pcb.ShowTextAligned(0, "SAMPLE DEPOSIT SLIP", 150, 20 + yAdjustment, 10);
                _pcb.EndText();

                table = new PdfPTable(4);
                table.TotalWidth = 510;
                table.HorizontalAlignment = 0;
                table.LockedWidth = true;
                widths = new float[] { 127.5f, 127.5f, 127.5f, 127.5f };
                table.SetWidths(widths);

                cell0 = new PdfPCell(new Paragraph(statement.bankName, fontB));
                cell0.Border = topleft;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph("DEPOSIT SLIP", fontB));
                cell0.Border = top;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph("DATE", fontB));
                cell0.Border = top;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(statement.StmtDate.ToString("yyyy/MM/dd"), fontB));
                cell0.Border = topright;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorRight = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph("Deposit To", fontB));
                cell0.Border = left;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(statement.accName, fontB));
                cell0.Border = Rectangle.NO_BORDER;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph("Branch Code", fontB));
                cell0.Border = Rectangle.NO_BORDER;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(statement.branch, fontB));
                cell0.Border = right;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorRight = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph("Account Number", fontB));
                cell0.Border = left;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(statement.accNumber, fontB));
                cell0.HorizontalAlignment = 0;
                cell0.Border = Rectangle.NO_BORDER;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(" ", fontB));
                cell0.Border = Rectangle.NO_BORDER;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(" ", fontB));
                cell0.Border = right;
                cell0.BorderColorTop = BaseColor.BLACK;
                cell0.BorderColorRight = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph("Reference", fontB));
                cell0.Border = bottomleft;
                cell0.BorderColorBottom = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(statement.AccNo, fontB));
                cell0.Border = bottom;
                cell0.BorderColorBottom = BaseColor.BLACK;
                cell0.BorderColorLeft = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph("Total", fontB));
                cell0.Border = bottom;
                cell0.BorderColorBottom = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                cell0 = new PdfPCell(new Paragraph(statement.totalDue.ToString("#,##0.00"), fontB));
                cell0.Border = bottomright;
                cell0.BorderColorBottom = BaseColor.BLACK;
                cell0.BorderColorRight = BaseColor.BLACK;
                cell0.HorizontalAlignment = 0;
                table.AddCell(cell0);

                document.Add(table);

                #endregion deposit slip
            }
            else
            {
                try
                {
                    CustomDashedLineSeparator separator = new CustomDashedLineSeparator();
                    separator.Dash = 10;
                    separator.Gap = 7;
                    separator.LineWidth = 1;
                    Chunk linebreak = new Chunk(separator);
                    document.Add(linebreak);

                    System.Drawing.Image myMd5 = Properties.Resources.stdmd5;
                    //System.Drawing.Image.FromFile(filePath);
                    int myHeight = myMd5.Height;
                    int myWidth = myMd5.Width;
                    iTextSharp.text.Image md5 = iTextSharp.text.Image.GetInstance(myMd5, System.Drawing.Imaging.ImageFormat.Png);
                    //MessageBox.Show(PageSize.A4.Width.ToString());
                    float x = 555;
                    float ratio = x / md5.Width;
                    float newHeight = md5.Height * ratio;
                    md5.ScaleAbsoluteWidth(x);
                    md5.ScaleAbsoluteHeight(newHeight);
                    float leftM = (PageSize.A4.Width - x) / 2;
                    md5.SetAbsolutePosition(leftM, 0);
                    _pcb = writer.DirectContentUnder;
                    _pcb.BeginText();
                    _pcb.AddImage(md5);
                    _pcb.EndText();
                    SetFont(8, false);

                    _pcb = writer.DirectContent;
                    _pcb.BeginText();
                    _pcb.SetTextMatrix(leftM + 90, 165);
                    _pcb.ShowText(statement.accName);
                    _pcb.SetTextMatrix(leftM + 90, 150);
                    _pcb.ShowText(statement.accNumber);
                    _pcb.SetTextMatrix(leftM + 90, 130);
                    _pcb.ShowText(statement.branch);
                    _pcb.SetTextMatrix(leftM + 90, 115);
                    _pcb.ShowText(statement.AccNo);
                    _pcb.EndText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("MD5" + ex.Message);
                }
            }

            #endregion table
        }

        public bool CreateReminderLetter(String accNo, String letterDate, String customerName, String[] address, String amtDue, String amtAdmin, String faxNumber, String dName, String dTelephone, bool isHOA, out String fName)
        {
            Font normalFont = FontFactory.GetFont(FontFactory.TIMES, 11);
            Font smallFont = FontFactory.GetFont(FontFactory.TIMES, 8);
            Font boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 11);
            bool success = false;

            #region Create Document

            String templateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, isHOA ? "rem_HOA_template.pdf" : "rem_template.pdf");
            PdfReader reader = new PdfReader(templateFile);
            fName = String.Empty;
            Document document = new Document();
            //PdfReader reader = null;
            FileStream stream;
            try
            {
                //folderPath = AppDomain.CurrentDomain.BaseDirectory;
                folderPath = (Directory.Exists("K:\\Debtors System\\Letters") ? "K:\\Debtors System\\Letters" : "C:\\Pastel11\\Debtors System\\Letters");
                fName = Path.Combine(folderPath, String.Format("{0}_{1}_REMINDER.pdf", letterDate.Replace(@"/", ""), accNo.Replace(@"/", "-").Replace(@"\", "-")));
                if (File.Exists(fName)) { File.Delete(fName); }
                stream = new FileStream(fName, FileMode.CreateNew);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                fName = String.Empty;
                return false;
            }

            #endregion Create Document

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                PdfTemplate background = writer.GetImportedPage(reader, 1);

                // Create a page in the document and add it to the bottom layer
                document.NewPage();

                _pcb = writer.DirectContentUnder;
                _pcb.AddTemplate(background, 0, 0);
                _pcb = writer.DirectContent;
                SetFont(11);

                for (int i = 0; i < 4; i++)
                {
                    Paragraph p1 = new Paragraph(" ", normalFont);
                    document.Add(p1);
                }

                Paragraph p = new Paragraph(letterDate, normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);
                p = new Paragraph(customerName, normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);

                for (int i = 0; i < 5; i++)
                {
                    String aLine = "   ";
                    try
                    {
                        aLine = address[i];
                    }
                    catch
                    {
                        aLine = "   ";
                    }
                    p = new Paragraph(aLine, normalFont);
                    p.IndentationLeft = 40f;
                    document.Add(p);
                }
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("OUTSTANDING CHARGES IN RESPECT OF PROPERTY:  Account No.:  " + accNo, boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("Dear Sir/Madam", boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("REMINDER LETTER", boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("We act upon instructions of the " + (isHOA ? " Directors of the Homeowners Association" : " trustees of the Body Corporate") + ": ", normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                Phrase phrase = new Phrase();
                phrase.Add(new Chunk("We draw your attention to the amount of, ", normalFont));
                phrase.Add(new Chunk("R" + amtDue, boldFont));
                phrase.Add(new Chunk(" which is reflected as outstanding according to our records. Levies are due and payable on or before the first day of each month, in advance as prescribed by the Sectional Titles Act 95, of 1986 as amended.", normalFont));
                p.Add(phrase);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph((isHOA ? "The Homeowners Association" : "The Body Corporate") + " is reliant on the abovementioned levy to fund expenses such as electricity, water, insurance and general maintenance items.", normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);

                p = new Paragraph();
                p.IndentationLeft = 40f;
                phrase = new Phrase();
                phrase.Add(new Chunk("An amount of ", normalFont));
                phrase.Add(new Chunk("R" + amtAdmin, boldFont));
                phrase.Add(new Chunk(" for administration fees has been debited to your account in respect of this letter.", normalFont));
                p.Add(phrase);
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                p.IndentationLeft = 40f;
                phrase = new Phrase();
                phrase.Add(new Chunk("Should payment have been made, please contact our office immediately to resolve this matter alternatively fax proof of payment to ", normalFont));
                phrase.Add(new Chunk(faxNumber, boldFont));
                phrase.Add(new Chunk(". Your urgent attention to this matter is required to prevent any additional amounts for final demands and summons being charged to your account.", normalFont));
                p.Add(phrase);
                document.Add(p);

                for (int pi = 0; pi < 10; pi++)
                {
                    p = new Paragraph(" ", normalFont);
                    document.Add(p);
                }

                p = new Paragraph("Enquiries  Please contact " + dName + " on " + dTelephone, smallFont);
                p.IndentationLeft = 40f;
                document.Add(p);

                document.Close();
                writer.Close();
                stream.Close();
                if (reader != null) { reader.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            success = true;
            return success;
        }

        public bool CreateSVTReminderLetter(String accNo, String letterDate, String customerName, String[] address, String amtDue, String amtAdmin, String faxNumber, out String fName)
        {
            Font normalFont = FontFactory.GetFont(FontFactory.TIMES, 10);
            Font boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);
            bool success = false;

            #region Create Document

            String templateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rem_template.pdf");
            PdfReader reader = new PdfReader(templateFile);
            fName = String.Empty;
            Document document = new Document();
            //PdfReader reader = null;
            FileStream stream;
            try
            {
                folderPath = (Directory.Exists("K:\\Debtors System\\Letters") ? "K:\\Debtors System\\Letters" : "C:\\Pastel11\\Debtors System\\Letters");
                fName = Path.Combine(folderPath, String.Format("{0}_{1}_REMINDER.pdf", letterDate.Replace(@"/", ""), accNo.Replace(@"/", "-").Replace(@"\", "-")));
                if (File.Exists(fName)) { File.Delete(fName); }
                stream = new FileStream(fName, FileMode.CreateNew);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                fName = String.Empty;
                return false;
            }

            #endregion Create Document

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                PdfTemplate background = writer.GetImportedPage(reader, 1);

                // Create a page in the document and add it to the bottom layer
                document.NewPage();

                _pcb = writer.DirectContentUnder;
                _pcb.AddTemplate(background, 0, 0);
                _pcb = writer.DirectContent;
                SetFont(11);

                for (int i = 0; i < 4; i++)
                {
                    Paragraph p1 = new Paragraph(" ", normalFont);
                    document.Add(p1);
                }

                Paragraph p = new Paragraph(letterDate, normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);
                p = new Paragraph(customerName, normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);

                for (int i = 0; i < 5; i++)
                {
                    String aLine = "";
                    try
                    {
                        aLine = address[i];
                    }
                    catch { }
                    p = new Paragraph(aLine, normalFont);
                    p.IndentationLeft = 40f;
                    document.Add(p);
                }
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("OUTSTANDING CHARGES IN RESPECT OF PROPERTY:  Account No.:  " + accNo, boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("Dear Sir/Madam", boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("REMINDER LETTER", boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("We act upon instructions of the trustees of the Body Corporate: ", normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                Phrase phrase = new Phrase();
                phrase.Add(new Chunk("We draw your attention to the amount of, ", normalFont));
                phrase.Add(new Chunk("R" + amtDue, boldFont));
                phrase.Add(new Chunk(" which is reflected as outstanding according to our records. Levies are due and payable on or before the first day of each month, in advance as prescribed by the Sectional Titles Act 95, of 1986 as amended.", normalFont));
                p.Add(phrase);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("The Body Corporate is reliant on the abovementioned levy to fund expenses such as electricity, water, insurance and general maintenance items.", normalFont);
                p.IndentationLeft = 40f;
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                p.IndentationLeft = 40f;
                phrase = new Phrase();
                phrase.Add(new Chunk("An amount of ", normalFont));
                phrase.Add(new Chunk("R" + amtAdmin, boldFont));
                phrase.Add(new Chunk(" for administration fees has been debited to your account in respect of this letter.", normalFont));
                p.Add(phrase);
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                p.IndentationLeft = 40f;
                phrase = new Phrase();
                phrase.Add(new Chunk("Should payment have been made, please contact our office immediately to resolve this matter alternatively fax proof of payment to ", normalFont));
                phrase.Add(new Chunk("086 695 5503", boldFont));
                phrase.Add(new Chunk(". Your urgent attention to this matter is required to prevent any additional amounts for final demands and summons being charged to your account.", normalFont));
                p.Add(phrase);
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                for (int i = 0; i < 9; i++)
                {
                    p = new Paragraph(" ", normalFont);
                    document.Add(p);
                }
                p = new Paragraph(accNo, normalFont);
                p.IndentationLeft = 60f;
                document.Add(p);

                document.Close();
                writer.Close();
                stream.Close();
                if (reader != null) { reader.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            success = true;
            return success;
        }

        public bool CreateWBGReminderLetter(String accNo, String letterDate, String customerName, String[] address, String amtDue, String amtAdmin, String faxNumber, out String fName)
        {
            Font normalFont = FontFactory.GetFont(FontFactory.TIMES, 11);
            Font boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 11);
            bool success = false;

            #region Create Document

            String templateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rem_template.pdf");
            PdfReader reader = new PdfReader(templateFile);
            fName = String.Empty;
            Document document = new Document();
            //PdfReader reader = null;
            FileStream stream;
            try
            {
                folderPath = (Directory.Exists("K:\\Debtors System\\Letters") ? "K:\\Debtors System\\Letters" : "C:\\Pastel11\\Debtors System\\Letters");
                fName = Path.Combine(folderPath, String.Format("{0}_{1}_REMINDER.pdf", letterDate.Replace(@"/", ""), accNo.Replace(@"/", "-").Replace(@"\", "-")));
                if (File.Exists(fName)) { File.Delete(fName); }
                stream = new FileStream(fName, FileMode.CreateNew);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                fName = String.Empty;
                return false;
            }

            #endregion Create Document

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                PdfTemplate background = writer.GetImportedPage(reader, 1);

                // Create a page in the document and add it to the bottom layer
                document.NewPage();

                _pcb = writer.DirectContentUnder;
                _pcb.AddTemplate(background, 0, 0);
                _pcb = writer.DirectContent;
                SetFont(11);

                for (int i = 0; i < 4; i++)
                {
                    Paragraph p1 = new Paragraph(" ", normalFont);
                    document.Add(p1);
                }

                Paragraph p = new Paragraph(letterDate, normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);
                p = new Paragraph(customerName, normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);

                for (int i = 0; i < 5; i++)
                {
                    String aLine = "";
                    try
                    {
                        aLine = address[i];
                    }
                    catch { }
                    p = new Paragraph(aLine, normalFont);
                    p.IndentationLeft = 40f;
                    document.Add(p);
                }
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("OUTSTANDING CHARGES IN RESPECT OF PROPERTY:  Account No.:  " + accNo, boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("Dear Sir/Madam", boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("REMINDER LETTER", boldFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("We act upon instructions of the trustees of the Body Corporate: ", normalFont);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                Phrase phrase = new Phrase();
                phrase.Add(new Chunk("We draw your attention to the amount of, ", normalFont));
                phrase.Add(new Chunk("R" + amtDue, boldFont));
                phrase.Add(new Chunk(" which is reflected as outstanding according to our records. Levies are due and payable on or before the first day of each month, in advance as prescribed by the Sectional Titles Act 95, of 1986 as amended.", normalFont));
                p.Add(phrase);
                p.IndentationLeft = 40f;
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph("The Body Corporate is reliant on the abovementioned levy to fund expenses such as electricity, water, insurance and general maintenance items.", normalFont);
                p.IndentationLeft = 40f;
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                p.IndentationLeft = 40f;
                phrase = new Phrase();
                phrase.Add(new Chunk("An amount of ", normalFont));
                phrase.Add(new Chunk("R" + amtAdmin, boldFont));
                phrase.Add(new Chunk(" for administration fees has been debited to your account in respect of this letter.", normalFont));
                p.Add(phrase);
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                p = new Paragraph();
                p.IndentationLeft = 40f;
                phrase = new Phrase();
                phrase.Add(new Chunk("Should payment have been made, please contact our office immediately to resolve this matter alternatively fax proof of payment to ", normalFont));
                phrase.Add(new Chunk("086 695 5503", boldFont));
                phrase.Add(new Chunk(". Your urgent attention to this matter is required to prevent any additional amounts for final demands and summons being charged to your account.", normalFont));
                p.Add(phrase);
                document.Add(p);
                p = new Paragraph(" ", normalFont);
                document.Add(p);

                document.Close();
                writer.Close();
                stream.Close();
                if (reader != null) { reader.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            success = true;
            return success;
        }

        public void CreatePALetter(Customer customer, Building building, User pm, String subject, DateTime letterDate, String html, List<byte[]> images, bool justify, bool letterhead, out Byte[] fStream)
        {
            int myJustify = (justify ? Element.ALIGN_JUSTIFIED : Element.ALIGN_LEFT);
            String fName = String.Empty;
            Document document = new Document(PageSize.A4);
            PdfReader reader = null;
            MemoryStream stream = new MemoryStream();
            String directory = AppDomain.CurrentDomain.BaseDirectory;
            //FileStream stream = new FileStream(Path.Combine(directory, "test.pdf"), FileMode.Create);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                // Get the top layer and write some text
                _pcb = writer.DirectContent;
                PdfContentByte under = writer.DirectContentUnder;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);

                Paragraph pDummy = new Paragraph(" ", fontPA);
                pDummy.SetLeading(0, 1);

                if (letterhead)
                {
                    under.BeginText();
                    //595
                    Image img = Image.GetInstance(Path.Combine(directory, "astroheader.png"));
                    img.SetAbsolutePosition(document.PageSize.GetLeft(0), document.PageSize.GetTop(0) - img.Height);
                    under.AddImage(img);
                    under.EndText();
                }
                for (int i = 0; i < 3; i++) { document.Add(new Paragraph(" ")); }
                Paragraph p = new Paragraph(letterDate.ToString("dd MMMM yyyy"), fontPA);
                p.SetLeading(0, 1);

                document.Add(p);
                document.Add(pDummy);

                if (customer != null)
                {
                    document.Add(new Paragraph(customer.description, fontPAB));
                    for (int i = 0; i < customer.address.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(customer.address[i]))
                        {
                            p = new Paragraph(customer.address[i], fontPA);
                            p.SetLeading(0, 1);
                            document.Add(p);
                        }
                    }
                    document.Add(pDummy);
                    try
                    {
                        if (!String.IsNullOrEmpty(customer.Email[0]))
                        {
                            String emailString = "Email: ";
                            int actCount = 0;
                            for (int i = 0; i < customer.Email.Length; i++) { if (!String.IsNullOrEmpty(customer.Email[i])) { actCount++; } }
                            String[] actualEmails = new String[actCount];
                            actCount = 0;
                            for (int i = 0; i < customer.Email.Length; i++)
                            {
                                if (!String.IsNullOrEmpty(customer.Email[i]))
                                {
                                    actualEmails[actCount] = customer.Email[i];
                                    actCount++;
                                }
                            }
                            for (int i = 0; i < actualEmails.Length; i++) { emailString += actualEmails[i] + (actualEmails.Length > 1 && i < actualEmails.Length - 1 ? "; " : ""); }
                            p = new Paragraph(emailString, fontPAB);
                            p.SetLeading(0, 1);
                            document.Add(p);
                            document.Add(pDummy);
                        }
                    }
                    catch { }
                    document.Add(pDummy);
                    p = new Paragraph("Dear Sir / Madam", fontPA);
                    p.SetLeading(0, 1);
                    document.Add(p);
                    document.Add(pDummy);
                }

                p = new Paragraph(subject.ToUpper(), fontPAB);
                p.Alignment = myJustify;
                p.SetLeading(0, 1);
                document.Add(p);
                document.Add(pDummy);

                if (html.Contains("<body>"))
                {
                    int bodyIdx = html.IndexOf("<body>");
                    html = html.Remove(0, bodyIdx + 6);
                }
                if (html.Contains("</body")) { html = html.Remove(html.IndexOf("</body>")); }
                html = html.Replace("<DIV STYLE=\"text-align:Left;font-family:Segoe UI;font-style:normal;font-weight:normal;font-size:12;color:#000000;\">", "");
                html = html.Replace("</DIV>", "");
                html = html.Replace("<span style=\"background-color:#FFFFFF;font-family:Arial Narrow;font-size:11pt;\">", "<span>");

                String[] pBits = html.Split(new String[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);
                Phrase phrase = new Phrase();
                int attachCount = 0;
                //MessageBox.Show(pBits.Length.ToString());
                foreach (String pBit in pBits)
                {
                    String phraseBit = pBit.Replace("</p>", "").Trim();
                    if (phraseBit == "") { continue; }
                    if (phraseBit == "&nbsp;")
                    {
                        document.Add(pDummy);
                    }
                    else if (phraseBit.Contains("<span>"))
                    {
                        try
                        {
                            String[] spanBits = phraseBit.Split(new String[] { "</span>" }, StringSplitOptions.None);
                            String orgFont = "";

                            foreach (String spanBit in spanBits)
                            {
                                Chunk chunk = new Chunk();
                                String sBit = spanBit.Replace("<span>", "");

                                if (!sBit.Contains("[pagebreak]"))
                                {
                                    String fontIndicator = "";
                                    if (sBit.Contains("<b>"))
                                    {
                                        sBit = sBit.Replace("<b>", "").Replace("</b>", "");
                                        fontIndicator = "E";
                                    }
                                    else
                                    {
                                        fontIndicator = "A";
                                    }
                                    if (sBit.Contains("<i>"))
                                    {
                                        sBit = sBit.Replace("<i>", "").Replace("</i>", "");
                                        if (fontIndicator == "A")
                                        {
                                            fontIndicator = "B";
                                        }
                                        else
                                        {
                                            fontIndicator = "F";
                                        }
                                    }
                                    if (sBit.Contains("<u>"))
                                    {
                                        sBit = sBit.Replace("<u>", "").Replace("</u>", "");
                                        switch (fontIndicator)
                                        {
                                            case "A":
                                                fontIndicator = "C";
                                                break;

                                            case "B":
                                                fontIndicator = "D";
                                                break;

                                            case "E":
                                                fontIndicator = "G";
                                                break;

                                            case "F":
                                                fontIndicator = "H";
                                                break;
                                        }
                                    }
                                    String text = sBit.Replace("</b>", "").Replace("</u>", "").Replace("</i>", "");
                                    text = text.Replace("&#39;", "'");
                                    text = text.Replace("&quot;", @"""");
                                    text = text.Replace("&amp;", "&");
                                    if (orgFont != fontIndicator)
                                    {
                                        orgFont = fontIndicator;
                                    }
                                    else
                                    {
                                        Chunk cDummy = new Chunk("", fontT);
                                        phrase.Add(cDummy);
                                    }
                                    switch (fontIndicator)
                                    {
                                        case "A":
                                            chunk = new Chunk(text, fontPA);
                                            break;

                                        case "B":
                                            chunk = new Chunk(text, fontPAI);
                                            break;

                                        case "C":
                                            chunk = new Chunk(text, fontPAU);
                                            break;

                                        case "D":
                                            chunk = new Chunk(text, fontPAIU);
                                            break;

                                        case "E":
                                            chunk = new Chunk(text, fontPAB);
                                            break;

                                        case "F":
                                            chunk = new Chunk(text, fontPABI);
                                            break;

                                        case "G":
                                            chunk = new Chunk(text, fontPABU);
                                            break;

                                        case "H":
                                            chunk = new Chunk(text, fontPABIU);
                                            break;
                                    }
                                    phrase.Add(chunk);
                                    // MessageBox.Show("added chunk");
                                }
                                else if (sBit.Contains("[pagebreak]"))
                                {
                                    p = new Paragraph(phrase);
                                    p.SetLeading(0, 1);
                                    p.Alignment = myJustify;
                                    //MessageBox.Show("added page");
                                    document.Add(p);
                                    document.NewPage();
                                }
                            }
                            p = new Paragraph(phrase);
                            //MessageBox.Show("added para");
                            p.SetLeading(0, 1);
                            p.Alignment = myJustify;
                            document.Add(p);
                            phrase = new Phrase();
                        }
                        catch { }
                    }
                    else if (pBit.Contains("img"))
                    {
                        document.Add(phrase);
                        document.Add(pDummy);

                        phrase = new Phrase();
                        int sourceStart = pBit.IndexOf("src=");
                        String source = pBit.Remove(0, sourceStart + 5);
                        int sourceEnd = source.IndexOf("\"");
                        source = source.Remove(sourceEnd);
                        System.Drawing.Image fImage = System.Drawing.Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, source));
                        Image attImage = Image.GetInstance(fImage, BaseColor.BLACK);
                        if (attImage.Width > 300)
                        {
                            float ratio = attImage.Width / attImage.Height;
                            attImage.ScaleToFit(300, attImage.Height / ratio);
                        }
                        document.Add(attImage);
                        //MessageBox.Show("added image");
                    }
                }
                if (phrase.Count > 0)
                {
                    document.Add(phrase);
                }
                //MessageBox.Show("end of body");

                document.Add(pDummy);

                if (letterhead)
                {
                    float sigY = writer.GetVerticalPosition(true);
                    if (sigY < 175) { document.NewPage(); }

                    p = new Paragraph("Yours faithfully", fontPA);
                    p.SetLeading(0, 1);
                    document.Add(p);
                    p = new Paragraph("For and on behalf of " + building.letterName, fontPAB);
                    p.SetLeading(0, 1);
                    document.Add(p);
                    float curY = writer.GetVerticalPosition(true);
                    float curX;
                    float actY = curY;

                    float picY;
                    curX = document.LeftMargin;
                    curY = writer.GetVerticalPosition(true);
                    Image pmSig = Image.GetInstance(pm.signature, BaseColor.BLACK);
                    pmSig.ScaleToFit(105, 70);

                    picY = curY - pmSig.ScaledHeight;
                    pmSig.SetAbsolutePosition(curX, picY);

                    writer.DirectContent.SetRGBColorStroke(0, 0, 0);
                    writer.DirectContent.SetLineWidth(1);
                    writer.DirectContent.MoveTo(curX, picY + 20);
                    writer.DirectContent.LineTo(curX + 150, picY + 20);
                    writer.DirectContent.Stroke();
                    actY = picY + 15;

                    for (int i = 0; i < 6; i++)
                    {
                        document.Add(pDummy);
                    }
                    p = new Paragraph(pm.name, fontPAB);
                    p.SetLeading(0, 1);
                    document.Add(p);

                    p = new Paragraph((pm.id == 17 ? "Senior " : "") + "Portfolio Manager", fontPAB);
                    p.SetLeading(0, 1);
                    document.Add(p);
                    p = new Paragraph("Tel: " + pm.phone, fontPAB);
                    p.SetLeading(0, 1);
                    document.Add(p);
                    p = new Paragraph("Fax No: " + pm.fax, fontPAB);
                    p.SetLeading(0, 1);
                    document.Add(p);
                    p = new Paragraph("Email: " + pm.email, fontPAB);
                    p.SetLeading(0, 1);
                    document.Add(p);

                    float afterSig = writer.GetVerticalPosition(true);
                    //MessageBox.Show("before = " + sigY.ToString() + " - after = " + afterSig.ToString());
                    under.BeginText();
                    //595
                    Image img = Image.GetInstance(Path.Combine(directory, "astrofooter.png"));
                    img.SetAbsolutePosition(document.PageSize.GetBottom(0), document.PageSize.GetLeft(0));
                    under.AddImage(img);
                    under.AddImage(pmSig);
                    under.EndText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                try
                {
                    document.Close();
                }
                catch { }
            }
            if (document.IsOpen()) { document.Close(); }
            try
            {
                stream.Flush();
                stream.Close();
            }
            catch { }
            fStream = stream.ToArray();
        }

        public void CreatePALetter(Building building, User pm, String html, out Byte[] fStream)
        {
            String fName = String.Empty;
            Document document = new Document(PageSize.A4);
            PdfReader reader = null;
            MemoryStream stream = new MemoryStream();
            String directory = AppDomain.CurrentDomain.BaseDirectory;
            //FileStream stream = new FileStream(Path.Combine(directory, "test.pdf"), FileMode.Create);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                // Get the top layer and write some text
                _pcb = writer.DirectContent;
                PdfContentByte under = writer.DirectContentUnder;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);

                Paragraph pDummy = new Paragraph(" ", fontPA);
                pDummy.SetLeading(0, 1);

                if (html.Contains("<body>"))
                {
                    int bodyIdx = html.IndexOf("<body>");
                    html = html.Remove(0, bodyIdx + 6);
                }
                if (html.Contains("</body")) { html = html.Remove(html.IndexOf("</body>")); }
                html = html.Replace("<DIV STYLE=\"text-align:Left;font-family:Segoe UI;font-style:normal;font-weight:normal;font-size:12;color:#000000;\">", "");
                html = html.Replace("</DIV>", "");
                html = html.Replace("<span style=\"background-color:#FFFFFF;font-family:Arial Narrow;font-size:11pt;\">", "<span>");

                String[] pBits = html.Split(new String[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);
                Phrase phrase = new Phrase();
                Paragraph p;
                foreach (String pBit in pBits)
                {
                    String phraseBit = pBit.Replace("</p>", "").Trim();
                    if (phraseBit == "") { continue; }
                    if (phraseBit == "&nbsp;")
                    {
                        document.Add(pDummy);
                    }
                    else if (phraseBit.Contains("<span>"))
                    {
                        try
                        {
                            String[] spanBits = phraseBit.Split(new String[] { "</span>" }, StringSplitOptions.None);
                            String orgFont = "";

                            foreach (String spanBit in spanBits)
                            {
                                Chunk chunk = new Chunk();
                                String sBit = spanBit.Replace("<span>", "");

                                if (!sBit.Contains("[pagebreak]"))
                                {
                                    String fontIndicator = "";
                                    if (sBit.Contains("<b>"))
                                    {
                                        sBit = sBit.Replace("<b>", "").Replace("</b>", "");
                                        fontIndicator = "E";
                                    }
                                    else
                                    {
                                        fontIndicator = "A";
                                    }
                                    if (sBit.Contains("<i>"))
                                    {
                                        sBit = sBit.Replace("<i>", "").Replace("</i>", "");
                                        if (fontIndicator == "A")
                                        {
                                            fontIndicator = "B";
                                        }
                                        else
                                        {
                                            fontIndicator = "F";
                                        }
                                    }
                                    if (sBit.Contains("<u>"))
                                    {
                                        sBit = sBit.Replace("<u>", "").Replace("</u>", "");
                                        switch (fontIndicator)
                                        {
                                            case "A":
                                                fontIndicator = "C";
                                                break;

                                            case "B":
                                                fontIndicator = "D";
                                                break;

                                            case "E":
                                                fontIndicator = "G";
                                                break;

                                            case "F":
                                                fontIndicator = "H";
                                                break;
                                        }
                                    }
                                    String text = sBit.Replace("</b>", "").Replace("</u>", "").Replace("</i>", "");
                                    text = text.Replace("&#39;", "'");

                                    if (orgFont != fontIndicator)
                                    {
                                        orgFont = fontIndicator;
                                    }
                                    else
                                    {
                                        Chunk cDummy = new Chunk("", fontT);
                                        phrase.Add(cDummy);
                                    }
                                    switch (fontIndicator)
                                    {
                                        case "A":
                                            chunk = new Chunk(text, fontPA);
                                            break;

                                        case "B":
                                            chunk = new Chunk(text, fontPAI);
                                            break;

                                        case "C":
                                            chunk = new Chunk(text, fontPAU);
                                            break;

                                        case "D":
                                            chunk = new Chunk(text, fontPAIU);
                                            break;

                                        case "E":
                                            chunk = new Chunk(text, fontPAB);
                                            break;

                                        case "F":
                                            chunk = new Chunk(text, fontPABI);
                                            break;

                                        case "G":
                                            chunk = new Chunk(text, fontPABU);
                                            break;

                                        case "H":
                                            chunk = new Chunk(text, fontPABIU);
                                            break;
                                    }
                                    phrase.Add(chunk);
                                    // MessageBox.Show("added chunk");
                                }
                                else if (sBit.Contains("[pagebreak]"))
                                {
                                    p = new Paragraph(phrase);
                                    p.SetLeading(0, 1);
                                    //MessageBox.Show("added page");
                                    document.Add(p);
                                    document.NewPage();
                                }
                            }
                            p = new Paragraph(phrase);
                            //MessageBox.Show("added para");
                            p.SetLeading(0, 1);
                            document.Add(p);
                            phrase = new Phrase();
                        }
                        catch { }
                    }
                }
                if (phrase.Count > 0)
                {
                    document.Add(phrase);
                }
                //MessageBox.Show("end of body");

                document.Add(pDummy);
                //p = new Paragraph("Should you have any queries, please do not hesitate to contact writer hereof.", fontPA);
                //p.SetLeading(0, 1)
                //document.Add(p);
                //document.Add(pDummy);
                float sigY = writer.GetVerticalPosition(true);
                if (sigY < 175) { document.NewPage(); }

                p = new Paragraph("Yours faithfully", fontPA);
                p.SetLeading(0, 1);
                document.Add(p);
                p = new Paragraph("For and on behalf of " + building.letterName, fontPAB);
                p.SetLeading(0, 1);
                document.Add(p);
                float curY = writer.GetVerticalPosition(true);
                float curX;
                float actY = curY;

                float picY;
                curX = document.LeftMargin;
                curY = writer.GetVerticalPosition(true);
                Image pmSig = Image.GetInstance(pm.signature, BaseColor.BLACK);
                pmSig.ScaleToFit(105, 70);

                picY = curY - pmSig.ScaledHeight;
                pmSig.SetAbsolutePosition(curX, picY);

                writer.DirectContent.SetRGBColorStroke(0, 0, 0);
                writer.DirectContent.SetLineWidth(1);
                writer.DirectContent.MoveTo(curX, picY + 20);
                writer.DirectContent.LineTo(curX + 150, picY + 20);
                writer.DirectContent.Stroke();
                actY = picY + 15;

                for (int i = 0; i < 6; i++)
                {
                    document.Add(pDummy);
                }
                p = new Paragraph(pm.name, fontPAB);
                p.SetLeading(0, 1);
                document.Add(p);

                p = new Paragraph((pm.id == 17 ? "Senior " : "") + "Portfolio Manager", fontPAB);
                p.SetLeading(0, 1);
                document.Add(p);
                p = new Paragraph("Tel: " + pm.phone, fontPAB);
                p.SetLeading(0, 1);
                document.Add(p);
                p = new Paragraph("Fax No: " + pm.fax, fontPAB);
                p.SetLeading(0, 1);
                document.Add(p);
                p = new Paragraph("Email: " + pm.email, fontPAB);
                p.SetLeading(0, 1);
                document.Add(p);

                float afterSig = writer.GetVerticalPosition(true);
                //MessageBox.Show("before = " + sigY.ToString() + " - after = " + afterSig.ToString());
                under.BeginText();
                under.AddImage(pmSig);
                under.EndText();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                try
                {
                    document.Close();
                }
                catch { }
            }
            if (document.IsOpen()) { document.Close(); }
            try
            {
                stream.Flush();
                stream.Close();
            }
            catch { }
            fStream = stream.ToArray();
        }

        public String CreateEnvelope(List<Customer> customers, int envelopeSize)
        {
            String directory = AppDomain.CurrentDomain.BaseDirectory;
            Rectangle pageSize;
            if (!Directory.Exists(Path.Combine(directory, "envelopes"))) { Directory.CreateDirectory(Path.Combine(directory, "envelopes")); }
            String envDirectory = Path.Combine(directory, "envelopes");
            Image img = Image.GetInstance(Path.Combine(directory, "envfooter.jpg"));
            int emptyLines = 0;
            switch (envelopeSize)
            {
                case 1:

                    pageSize = PageSize.A5.Rotate();
                    Rectangle actC5 = new Rectangle(iTextSharp.text.Utilities.MillimetersToPoints(229f), iTextSharp.text.Utilities.MillimetersToPoints(162f));
                    emptyLines = 6;
                    break;

                case 2:
                    pageSize = PageSize.A4;
                    emptyLines = 15;
                    break;

                default:
                    float ux = PageSize.A5.Rotate().Width;
                    float uy = PageSize.A5.Rotate().Height * 0.66f;
                    pageSize = new Rectangle(ux, uy);
                    //iTextSharp.text.Utilities.MillimetersToPoints(220f), iTextSharp.text.Utilities.MillimetersToPoints(110f));
                    emptyLines = 2;
                    break;
            }
            List<String> files = new List<string>();
            foreach (Customer c in customers)
            {
                try
                {
                    Document document = new Document(pageSize);
                    if (File.Exists(Path.Combine(envDirectory, c.accNumber + "_env.pdf"))) { File.Delete(Path.Combine(envDirectory, c.accNumber + "_env.pdf")); }
                    FileStream stream = new FileStream(Path.Combine(envDirectory, c.accNumber + "_env.pdf"), FileMode.CreateNew);
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    PdfContentByte under = writer.DirectContentUnder;
                    under.BeginText();
                    //img.ScaleAbsoluteWidth(pageSize.Width);
                    //img.ScaleAbsoluteWidth(pageSize.Width);
                    float margin = (pageSize.Width - img.Width) / 2;
                    img.SetAbsolutePosition(margin, 25);
                    under.AddImage(img);
                    under.EndText();

                    PdfContentByte cb = writer.DirectContent;
                    Chunk c00 = new Chunk((!String.IsNullOrEmpty(c.description) ? c.description : ""), fontE);
                    Chunk c0 = new Chunk((!String.IsNullOrEmpty(c.address[0]) ? c.address[0] : ""), fontE);
                    Chunk c1 = new Chunk((!String.IsNullOrEmpty(c.address[1]) ? c.address[1] : ""), fontE);
                    Chunk c2 = new Chunk((!String.IsNullOrEmpty(c.address[2]) ? c.address[2] : ""), fontE);
                    Chunk c3 = new Chunk((!String.IsNullOrEmpty(c.address[3]) ? c.address[3] : ""), fontE);
                    Chunk c4 = new Chunk((!String.IsNullOrEmpty(c.address[4]) ? c.address[4] : ""), fontE);

                    float maxWidth = 0;
                    if (c00.GetWidthPoint() > maxWidth) { maxWidth = c00.GetWidthPoint(); }
                    if (c0.GetWidthPoint() > maxWidth) { maxWidth = c0.GetWidthPoint(); }
                    if (c1.GetWidthPoint() > maxWidth) { maxWidth = c1.GetWidthPoint(); }
                    if (c2.GetWidthPoint() > maxWidth) { maxWidth = c2.GetWidthPoint(); }
                    if (c3.GetWidthPoint() > maxWidth) { maxWidth = c3.GetWidthPoint(); }
                    if (c4.GetWidthPoint() > maxWidth) { maxWidth = c4.GetWidthPoint(); }

                    float leading = (pageSize.Width - maxWidth) / 2;
                    for (int f = 0; f < emptyLines; f++) { document.Add(new Paragraph(" ", fontE)); }

                    Paragraph p00 = new Paragraph(c00);
                    Paragraph p0 = new Paragraph(c0);
                    Paragraph p1 = new Paragraph(c1);
                    Paragraph p2 = new Paragraph(c2);
                    Paragraph p3 = new Paragraph(c3);
                    Paragraph p4 = new Paragraph(c4);
                    p00.IndentationLeft = leading;
                    p0.IndentationLeft = leading;
                    p1.IndentationLeft = leading;
                    p2.IndentationLeft = leading;
                    p3.IndentationLeft = leading;
                    p4.IndentationLeft = leading;

                    document.Add(p00);
                    document.Add(p0);
                    document.Add(p1);
                    document.Add(p2);
                    document.Add(p3);
                    document.Add(p4);

                    document.Close();
                    files.Add(Path.Combine(envDirectory, c.accNumber + "_env.pdf"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            // step 1: creation of a document-object
            Document conDocument = new Document();

            // step 2: we create a writer that listens to the document
            String outFile = Path.Combine(envDirectory, DateTime.Now.ToString("yyyy-MM-dd") + "_env.pdf");
            PdfCopy conWriter = new PdfCopy(conDocument, new FileStream(outFile, FileMode.Create));
            if (conWriter != null)
            {
                // step 3: we open the document
                conDocument.Open();

                foreach (string fileName in files)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = conWriter.GetImportedPage(reader, i);
                        conWriter.AddPage(page);
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        conWriter.CopyAcroForm(reader);
                    }
                    reader.Close();
                    File.Delete(fileName);
                }
                conWriter.Close();
                conDocument.Close();
            }
            return outFile;
        }

        public String TrustMovement(Dictionary<String, List<Trns>> bTrans)
        {
            Document document = new Document();
            String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TRUSTMOVEMENT");
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            String fileName = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmtt") + ".pdf");
            try
            {
                if (File.Exists(fileName)) { File.Delete(fileName); }
            }
            catch
            {
                MessageBox.Show("File " + fileName + " is in use. Please close and try again.");
                return String.Empty;
            }
            FileStream stream = new FileStream(fileName, FileMode.CreateNew);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();
            _pcb = writer.DirectContent;
            foreach (KeyValuePair<String, List<Trns>> bTran in bTrans)
            {
                try
                {
                    SetFont7();

                    #region table

                    Paragraph paragraphSpacer = new Paragraph(" ");

                    PdfPTable table = new PdfPTable(4);
                    table.TotalWidth = 510;
                    table.HorizontalAlignment = 2;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 127.5f, 127.5f, 127.5f, 127.5f };
                    table.SetWidths(widths);
                    table.DefaultCell.Border = 1;
                    PdfPCell cell0 = new PdfPCell(new Paragraph(bTran.Key, fontB));
                    cell0.HorizontalAlignment = 0;
                    cell0.Colspan = 4;
                    cell0.Border = 1;
                    table.AddCell(cell0);

                    foreach (Trns t in bTran.Value)
                    {
                        try
                        {
                            //#region InvHeader
                            cell0 = new PdfPCell(new Paragraph(t.Date, font));
                            cell0.HorizontalAlignment = 0;
                            cell0.Colspan = 1;
                            cell0.Border = 1;
                            table.AddCell(cell0);
                            cell0 = new PdfPCell(new Paragraph(t.Description, font));
                            cell0.HorizontalAlignment = 0;
                            cell0.Colspan = 1;
                            cell0.Border = 1;
                            table.AddCell(cell0);
                            cell0 = new PdfPCell(new Paragraph(t.Reference, font));
                            cell0.HorizontalAlignment = 0;
                            cell0.Colspan = 1;
                            cell0.Border = 1;
                            table.AddCell(cell0);
                            cell0 = new PdfPCell(new Paragraph(t.Amount, font));
                            cell0.HorizontalAlignment = 2;
                            cell0.Colspan = 1;
                            cell0.Border = 1;
                            table.AddCell(cell0);
                        }
                        catch
                        {
                            MessageBox.Show("Tran");
                        }
                    }
                    document.Add(table);
                    document.NewPage();

                    #endregion table
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            document.Close();
            writer.Close();
            stream.Close();

            return fileName;
        }

        private Paragraph CreateParagraph(String text, Font f)
        {
            Paragraph p = new Paragraph(text, f);
            p.Alignment = Element.ALIGN_CENTER;
            return p;
        }

        private Paragraph CreateParagraph(String text, Font f, int alignment)
        {
            Paragraph p = new Paragraph(text, f);
            p.Alignment = alignment;
            return p;
        }

        #region Content Writing

        private void PrintXAxis(int y)
        {
            SetFont7();
            int x = 600;
            while (x >= 0)
            {
                if (x % 20 == 0)
                {
                    PrintTextCentered("" + x, x, y + 8);
                    PrintTextCentered("|", x, y);
                }
                else
                {
                    PrintTextCentered(".", x, y);
                }
                x -= 5;
            }
        }

        private void PrintYAxis(int x)
        {
            SetFont7();
            int y = 800;
            while (y >= 0)
            {
                if (y % 20 == 0)
                {
                    PrintText("__ " + y, x, y);
                }
                else
                {
                    PrintText("_", x, y);
                }
                y = y - 5;
            }
        }

        private void SetFont7()
        {
            _pcb.SetFontAndSize(bf, 7);
        }

        private void SetFont(float size)
        {
            _pcb.SetFontAndSize(bf, size);
        }

        private void SetFont(float size, bool isBold)
        {
            _pcb.SetFontAndSize((isBold ? bf2 : bf), size);
        }

        private void SetFont18()
        {
            _pcb.SetFontAndSize(bf, 18);
        }

        private void SetFont36()
        {
            _pcb.SetFontAndSize(bf, 36);
        }

        private void PrintText(string text, int x, int y)
        {
            _pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, x, y, 0);
        }

        private void PrintTextRight(string text, int x, int y)
        {
            _pcb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, x, y, 0);
        }

        private void PrintTextCentered(string text, int x, int y)
        {
            _pcb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, x, y, 0);
        }

        #endregion Content Writing
    }

    public class ClearanceObject
    {
        public String preparedBy { get; set; }

        public String buildingCode { get; set; }

        public String customerCode { get; set; }

        public String trfAttorneys { get; set; }

        public String attReference { get; set; }

        public String fax { get; set; }

        public String complex { get; set; }

        public String unitNo { get; set; }

        public String seller { get; set; }

        public String purchaser { get; set; }

        public String purchaserAddress { get; set; }

        public String purchaserTel { get; set; }

        public String purchaserEmail { get; set; }

        public String notes { get; set; }

        public double clearanceFee { get; set; }

        public double astrodonTotal { get; set; }

        public DateTime certDate { get; set; }

        public bool registered { get; set; }

        public DateTime regDate { get; set; }

        public DateTime validTo { get; set; }

        public List<ClearanceObjectTrans> Trans { get; set; }

        public bool extClearance { get; set; }

        public ClearanceObject(int clearanceID, DataRow dr, DataSet ds)
        {
            try
            {
                preparedBy = dr["preparedBy"].ToString();
                buildingCode = dr["buildingCode"].ToString();
                customerCode = dr["customerCode"].ToString();
                trfAttorneys = dr["trfAttorneys"].ToString();
                attReference = dr["attReference"].ToString();
                fax = dr["fax"].ToString();
                complex = dr["complex"].ToString();
                unitNo = dr["unitNo"].ToString();
                seller = dr["seller"].ToString();
                purchaser = dr["purchaser"].ToString();
                purchaserAddress = dr["purchaserAddress"].ToString();
                purchaserTel = dr["purchaserTel"].ToString();
                purchaserEmail = dr["purchaserEmail"].ToString();
                notes = dr["notes"].ToString();
                clearanceFee = double.Parse(dr["clearanceFee"].ToString());
                astrodonTotal = double.Parse(dr["astrodonTotal"].ToString());
                certDate = DateTime.Parse(dr["certDate"].ToString());
                regDate = DateTime.Parse(dr["regDate"].ToString());
                validTo = DateTime.Parse(dr["validDate"].ToString());
                registered = bool.Parse(dr["registered"].ToString());
                extClearance = bool.Parse(dr["extClearance"].ToString());
                Trans = new List<ClearanceObjectTrans>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr2 in ds.Tables[0].Rows)
                    {
                        Trans.Add(new ClearanceObjectTrans(dr2));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ": " + clearanceID.ToString());
            }
        }
    }

    public class ClearanceObjectTrans
    {
        public String description { get; set; }

        public double amount { get; set; }

        public ClearanceObjectTrans(DataRow dr)
        {
            description = dr["description"].ToString();
            amount = double.Parse(dr["amount"].ToString());
        }
    }

    public class CustomDashedLineSeparator : iTextSharp.text.pdf.draw.DottedLineSeparator
    {
        public Single dash = 5;
        public Single phase = 2.5F;

        public Single Dash
        {
            get
            {
                return dash;
            }
            set
            {
                dash = value;
            }
        }

        public Single Phase
        {
            get
            {
                return phase;
            }
            set
            {
                phase = value;
            }
        }

        public void draw(PdfContentByte canvas, Single llx, Single lly, Single urx, Single ury, Single y)
        {
            canvas.SaveState();
            //canvas.li = LineWidth;
            canvas.SetLineDash(dash, Gap, phase);
            DrawLine(canvas, llx, urx, y);
            canvas.RestoreState();
        }
    }
}