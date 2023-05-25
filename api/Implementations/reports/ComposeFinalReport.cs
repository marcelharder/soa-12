using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace api.Implementations.reports
{
    public class ComposeFinalReport : IComposeFinalReport
    {
        private readonly IWebHostEnvironment _env;
        private readonly SpecialReportMaps _rm;
        private DataContext _context;



        public ComposeFinalReport(IWebHostEnvironment env, SpecialReportMaps rm, DataContext context)
        {
            _env = env;
            _rm = rm;
            _context = context;
         
        }
        iTextSharp.text.Font smallfont = FontFactory.GetFont("Arial", 8);
        iTextSharp.text.Font boldfont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font bigboldfont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
        CmykColor header_background_color = new CmykColor(0, 0, 70, 0);
        CmykColor footer_background_color = new CmykColor(0, 0, 0, 14);
        PdfPCell cell;



        public async Task composeAsync(int procedure_id)
        {
            var _frs = await _context.finalReports.FirstOrDefaultAsync(r => r.procedure_id == procedure_id);
           
            
            var currentProcedure = await _context.Procedures.FirstOrDefaultAsync(r => r.ProcedureId == procedure_id);
            var report_code = Convert.ToInt32(_rm.getReportCode(currentProcedure.fdType));
            var soort = currentProcedure.fdType;


            var pathToFile = _env.ContentRootPath + "/assets/pdf/";
            var ps = procedure_id.ToString();
            var file_name = pathToFile + ps + ".pdf";

            using (var fs = new FileStream(file_name, FileMode.Create))
            {
                var doc = new iTextSharp.text.Document();
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
                doc.SetMargins(0.0F, 10.0F, 70.0F, 10.0F);
                compose_pdf(doc, writer, report_code, soort, _frs);
            }
            // remove the final report now in the database once the pdf is made
             _context.finalReports.Remove(_frs);
             await _context.SaveChangesAsync();
        }



        private int compose_pdf(iTextSharp.text.Document doc,iTextSharp.text.pdf.PdfWriter wri,int report_code,int soort,Class_Final_operative_report fr)
        {
            switch (report_code)
            {
                case 1:
                    doc.Open();
                    doc.Add(getHeader(fr));
                    doc.Add(getEmployees(fr));
                    doc.Add(getBlockTable_1(fr));
                    doc.Add(getCabg_Details(fr));
                    doc.Add(getBlockTable_2(fr));
                    doc.Add(getCommentTable(fr));
                    doc.Add(getFooter(fr));
                    PdfContentByte cb = wri.DirectContent;
                    cb.SetLineWidth(2.0F);
                    cb.SetGrayStroke(0.75F);
                    cb.MoveTo(40, 575);
                    cb.LineTo(540, 575);
                    cb.Stroke();
                    doc.Close();
                    break;
                case 2:
                    doc.Open();
                    doc.Add(getHeader(fr));
                    doc.Add(getEmployees(fr));
                    doc.Add(getBlockTable_1(fr));
                    doc.Add(getCabg_Details(fr));
                    doc.Add(getBlockTable_2(fr));
                    doc.Add(getCommentTable(fr));
                    doc.Add(getFooter(fr));
                    PdfContentByte cb1 = wri.DirectContent;
                    cb1.SetLineWidth(2.0F);
                    cb1.SetGrayStroke(0.75F);
                    cb1.MoveTo(40, 575);
                    cb1.LineTo(540, 575);
                    cb1.Stroke();
                    doc.Close();
                    break;
                case 3:
                    doc.Open();
                    doc.Add(getHeader(fr));
                    doc.Add(getEmployees(fr));
                    doc.Add(getBlockTable_1(fr));
                    doc.Add(getAortic_Valve_Details(fr));
                    doc.Add(getBlockTable_2(fr));
                    doc.Add(getCommentTable(fr));
                    doc.Add(getFooter(fr));
                    PdfContentByte cb3 = wri.DirectContent;
                    cb3.SetLineWidth(2.0F);
                    cb3.SetGrayStroke(0.75F);
                    cb3.MoveTo(40, 575);
                    cb3.LineTo(540, 575);
                    cb3.Stroke();
                    doc.Close();
                    break;

                case 4:
                    doc.Open();
                    doc.Add(getHeader(fr));
                    doc.Add(getEmployees(fr));
                    doc.Add(getBlockTable_1(fr));
                    doc.Add(getMitral_Valve_Details(fr));
                    doc.Add(getBlockTable_2(fr));
                    doc.Add(getCommentTable(fr));
                    doc.Add(getFooter(fr));
                    PdfContentByte cb6 = wri.DirectContent;
                    cb6.SetLineWidth(2.0F);
                    cb6.SetGrayStroke(0.75F);
                    cb6.MoveTo(40, 575);
                    cb6.LineTo(540, 575);
                    cb6.Stroke();
                    doc.Close();
                    break;

                case 5:
                    doc.Open();
                    doc.Add(getHeader(fr));
                    doc.Add(getEmployees(fr));

                    doc.Add(getBlockTable_1(fr));
                    doc.Add(getMitral_Valve_Details(fr));
                    doc.Add(getBlockAVR_MVR_2(fr));
                    doc.Add(getAortic_Valve_Details(fr));
                    doc.Add(getBlockAVR_MVR_3(fr));

                    doc.Add(getCommentTable(fr));
                    doc.Add(getFooter(fr));

                    PdfContentByte cb4 = wri.DirectContent;
                    cb4.SetLineWidth(2.0F);
                    cb4.SetGrayStroke(0.75F);
                    cb4.MoveTo(40, 575);
                    cb4.LineTo(540, 575);
                    cb4.Stroke();
                    doc.Close();
                    break;

                case 6:
                    doc.Open();
                    doc.Add(getHeader(fr));
                    doc.Add(getEmployees(fr));
                    doc.Add(getGeneralReport(fr));
                    doc.Add(getCommentTable(fr));
                    doc.Add(getFooter(fr));
                    PdfContentByte cb5 = wri.DirectContent;
                    cb5.SetLineWidth(2.0F);
                    cb5.SetGrayStroke(0.75F);
                    cb5.MoveTo(40, 600);
                    cb5.LineTo(540, 600);
                    cb5.Stroke();
                    doc.Close();
                    break;


            }
            return 1;
        }
        private PdfPTable getHeader(Class_Final_operative_report _frs)
        {
           
            var header = new PdfPTable(3);
            header.TotalWidth = 500.0F;
            header.LockedWidth = true;


            Image test;
            if (_frs.HospitalUrl == null)
            {

                try
                {
                    test = Image.GetInstance(new Uri("https://res.cloudinary.com/marcelcloud/image/upload/v1668181198/zbzftf4medwmdyqnuobi.jpg"));

                }
                catch (System.Exception)
                {

                    throw;
                }
            }
            else
            {
                test = Image.GetInstance(new Uri(_frs.HospitalUrl)); // hospital url is eigenlijk de foto van de chirurg
            }




            test.ScaleToFit(80.0F, 85.0F);


            var my_picture = new PdfPCell(test);
            my_picture.BackgroundColor = header_background_color;

            var cell_2 = new PdfPCell(getHeaderTable_1(_frs));
            cell_2.BackgroundColor = header_background_color;

            var cell_3 = new PdfPCell(getHeaderTable_2(_frs));
            cell_3.BackgroundColor = header_background_color;

            var cell_4 = new PdfPCell(new Paragraph("Operative report", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14.0F, iTextSharp.text.Font.BOLD)));
            cell_4.FixedHeight = 50.0F;
            cell_4.Border = 0;
            cell_4.Colspan = 3;
            cell_4.HorizontalAlignment = 1;

            header.AddCell(my_picture);
            header.AddCell(cell_2);
            header.AddCell(cell_3);
            header.AddCell(cell_4);

            return header;
        }
        private PdfPTable getGeneralHeader(Class_Final_operative_report _frs)
        {

            iTextSharp.text.Image i = iTextSharp.text.Image.GetInstance(_frs.HospitalUrl);
            i.ScaleToFit(85.0F, 100.0F);
            var header = new PdfPTable(3);
            header.TotalWidth = 500.0F;
            header.LockedWidth = true;

            var my_picture = new PdfPCell(i);
            my_picture.BackgroundColor = header_background_color;

            var cell_2 = new PdfPCell(getHeaderTable_1(_frs));
            cell_2.BackgroundColor = header_background_color;

            var cell_3 = new PdfPCell(getHeaderTable_2(_frs));
            cell_3.BackgroundColor = header_background_color;




            header.AddCell(my_picture);
            header.AddCell(cell_2);
            header.AddCell(cell_3);

            return header;
        }
        private PdfPTable getHeaderTable_1(Class_Final_operative_report _frs)
        {
            var help = new PdfPTable(1);

            var cell_1 = new PdfPCell(new Paragraph(_frs.HeaderText1, boldfont)); cell_1.Border = 0; cell_1.HorizontalAlignment = 1;
            var cell_2 = new PdfPCell(new Paragraph(_frs.HeaderText2, smallfont)); cell_2.Border = 0; cell_2.HorizontalAlignment = 1;
            var cell_3 = new PdfPCell(new Paragraph(_frs.HeaderText3, smallfont)); cell_3.Border = 0; cell_3.HorizontalAlignment = 1;
            var cell_4 = new PdfPCell(new Paragraph(_frs.HeaderText4, smallfont)); cell_4.Border = 0; cell_4.HorizontalAlignment = 1;


            help.AddCell(cell_1);
            help.AddCell(cell_2);
            help.AddCell(cell_3);
            help.AddCell(cell_4);
            return help;
        }
        private PdfPTable getHeaderTable_2(Class_Final_operative_report _frs)
        {
            var help = new PdfPTable(1);
            var cell_1 = new PdfPCell(new Paragraph(_frs.HeaderText5, smallfont)); cell_1.Border = 0;
            var cell_2 = new PdfPCell(new Paragraph(_frs.HeaderText6, smallfont)); cell_2.Border = 0;
            var cell_3 = new PdfPCell(new Paragraph(_frs.HeaderText7, smallfont)); cell_3.Border = 0;
            var cell_5 = new PdfPCell(new Paragraph(_frs.HeaderText8, smallfont)); cell_5.Border = 0;
            var cell_4 = new PdfPCell(new Paragraph(_frs.HeaderText9, smallfont)); cell_4.Border = 0;
            help.AddCell(cell_1);
            help.AddCell(cell_5);
            help.AddCell(cell_2);
            help.AddCell(cell_3);
            help.AddCell(cell_4);
            return help;
        }
        private PdfPTable getEmployees(Class_Final_operative_report _frs)
        {
            var help = new PdfPTable(4);
            help.SpacingBefore = 5.0F;
            help.SpacingAfter = 20.0F;
            help.TotalWidth = 500.0F;
            help.LockedWidth = true;

            var cell_1 = new PdfPCell(new Paragraph("Pre-operative diagnosis", boldfont)); cell_1.Border = 0;
            var cell_2 = new PdfPCell(new Paragraph(_frs.Regel10, smallfont)); cell_2.Border = 0;
            var cell_3 = new PdfPCell(new Paragraph("Surgeon", boldfont)); cell_3.Border = 0;
            var cell_4 = new PdfPCell(new Paragraph(_frs.Regel13, smallfont)); cell_4.Border = 0;

            var cell_5 = new PdfPCell(new Paragraph("Operation", boldfont)); cell_5.Border = 0;
            var cell_6 = new PdfPCell(new Paragraph(_frs.Regel11, smallfont)); cell_6.Border = 0;
            var cell_7 = new PdfPCell(new Paragraph("Assistant", boldfont)); cell_7.Border = 0;
            var cell_8 = new PdfPCell(new Paragraph(_frs.Regel14, smallfont)); cell_8.Border = 0;


            var cell_9 = new PdfPCell(new Paragraph("Operation date", boldfont)); cell_9.Border = 0;
            var cell_10 = new PdfPCell(new Paragraph(_frs.Regel12, smallfont)); cell_10.Border = 0;
            var cell_11 = new PdfPCell(new Paragraph("Anaesthesiologist", boldfont)); cell_11.Border = 0;
            var cell_12 = new PdfPCell(new Paragraph(_frs.Regel15, smallfont)); cell_12.Border = 0;


            var cell_13 = new PdfPCell(new Paragraph("", boldfont)); cell_13.Border = 0;
            var cell_14 = new PdfPCell(new Paragraph("", boldfont)); cell_14.Border = 0;
            var cell_15 = new PdfPCell(new Paragraph("Perfusionist", boldfont)); cell_15.Border = 0;
            var cell_16 = new PdfPCell(new Paragraph(_frs.Regel16, smallfont)); cell_16.Border = 0;


            help.AddCell(cell_1);
            help.AddCell(cell_2);
            help.AddCell(cell_3);
            help.AddCell(cell_4);
            help.AddCell(cell_5);
            help.AddCell(cell_6);
            help.AddCell(cell_7);
            help.AddCell(cell_8);
            help.AddCell(cell_9);
            help.AddCell(cell_10);
            help.AddCell(cell_11);
            help.AddCell(cell_12);
            help.AddCell(cell_13);
            help.AddCell(cell_14);
            help.AddCell(cell_15);
            help.AddCell(cell_16);
            return help;
        }
        private PdfPTable getOffPumpEmployees(Class_Final_operative_report _frs)
        {
            var help = new PdfPTable(4);
            help.SpacingBefore = 5.0F;
            help.SpacingAfter = 20.0F;
            help.TotalWidth = 500.0F;
            help.LockedWidth = true;

            var cell_1 = new PdfPCell(new Paragraph("Pre-operative diagnosis", boldfont)); cell_1.Border = 0;
            var cell_2 = new PdfPCell(new Paragraph(_frs.Regel10, smallfont)); cell_2.Border = 0;
            var cell_3 = new PdfPCell(new Paragraph("Surgeon", boldfont)); cell_3.Border = 0;
            var cell_4 = new PdfPCell(new Paragraph(_frs.Regel13, smallfont)); cell_4.Border = 0;

            var cell_5 = new PdfPCell(new Paragraph("Operation", boldfont)); cell_5.Border = 0;
            var cell_6 = new PdfPCell(new Paragraph(_frs.Regel11, smallfont)); cell_6.Border = 0;
            var cell_7 = new PdfPCell(new Paragraph("Assistant", boldfont)); cell_7.Border = 0;
            var cell_8 = new PdfPCell(new Paragraph(_frs.Regel14, smallfont)); cell_8.Border = 0;


            var cell_9 = new PdfPCell(new Paragraph("Operation date", boldfont)); cell_9.Border = 0;
            var cell_10 = new PdfPCell(new Paragraph(_frs.Regel12, smallfont)); cell_10.Border = 0;
            var cell_11 = new PdfPCell(new Paragraph("Anaesthesiologist", boldfont)); cell_11.Border = 0;
            var cell_12 = new PdfPCell(new Paragraph(_frs.Regel15, smallfont)); cell_12.Border = 0;





            help.AddCell(cell_1);
            help.AddCell(cell_2);
            help.AddCell(cell_3);
            help.AddCell(cell_4);
            help.AddCell(cell_5);
            help.AddCell(cell_6);
            help.AddCell(cell_7);
            help.AddCell(cell_8);
            help.AddCell(cell_9);
            help.AddCell(cell_10);
            help.AddCell(cell_11);
            help.AddCell(cell_12);

            return help;
        }
        private PdfPTable getBlockTable_1(Class_Final_operative_report _frs)
        {
            var pdftable_a = new PdfPTable(1);
            pdftable_a.TotalWidth = 500.0F;
            pdftable_a.LockedWidth = true;


            if (_frs.Regel17 != "") { var cell_1 = new PdfPCell(new Paragraph(_frs.Regel17, smallfont)); cell_1.Border = 0; pdftable_a.AddCell(cell_1); }
            if (_frs.Regel18 != "") { var cell_2 = new PdfPCell(new Paragraph(_frs.Regel18, smallfont)); cell_2.Border = 0; pdftable_a.AddCell(cell_2); }

            if (_frs.Regel19 != "") { var cell_3 = new PdfPCell(new Paragraph(_frs.Regel19, smallfont)); cell_3.Border = 0; pdftable_a.AddCell(cell_3); }
            if (_frs.Regel20 != "") { var cell_4 = new PdfPCell(new Paragraph(_frs.Regel20, smallfont)); cell_4.Border = 0; pdftable_a.AddCell(cell_4); }

            if (_frs.Regel21 != "") { var cell_5 = new PdfPCell(new Paragraph(_frs.Regel21, smallfont)); cell_5.Border = 0; pdftable_a.AddCell(cell_5); }
            if (_frs.Regel22 != "") { var cell_6 = new PdfPCell(new Paragraph(_frs.Regel22, smallfont)); cell_6.Border = 0; pdftable_a.AddCell(cell_6); }

            if (_frs.Regel23 != "") { var cell_7 = new PdfPCell(new Paragraph(_frs.Regel23, smallfont)); cell_7.Border = 0; pdftable_a.AddCell(cell_7); }
            if (_frs.Regel24 != "") { var cell_8 = new PdfPCell(new Paragraph(_frs.Regel24, smallfont)); cell_8.Border = 0; pdftable_a.AddCell(cell_8); }

            if (_frs.Regel58 != "") { var cell_9 = new PdfPCell(new Paragraph(_frs.Regel58, smallfont)); cell_9.Border = 0; pdftable_a.AddCell(cell_9); }
            if (_frs.Regel59 != "") { var cell_10 = new PdfPCell(new Paragraph(_frs.Regel59, smallfont)); cell_10.Border = 0; pdftable_a.AddCell(cell_10); }

            if (_frs.Regel60 != "") { var cell_11 = new PdfPCell(new Paragraph(_frs.Regel60, smallfont)); cell_11.Border = 0; pdftable_a.AddCell(cell_11); }
            if (_frs.Regel61 != "") { var cell_12 = new PdfPCell(new Paragraph(_frs.Regel61, smallfont)); cell_12.Border = 0; pdftable_a.AddCell(cell_12); }

            if (_frs.Regel62 != "") { var cell_13 = new PdfPCell(new Paragraph(_frs.Regel62, smallfont)); cell_13.Border = 0; pdftable_a.AddCell(cell_13); }
            if (_frs.Regel63 != "") { var cell_14 = new PdfPCell(new Paragraph(_frs.Regel63, smallfont)); cell_14.Border = 0; pdftable_a.AddCell(cell_14); }

            return pdftable_a;




        }
        private PdfPTable getCabg_Details(Class_Final_operative_report _frs)
        {
            var cabg_details = new PdfPTable(4);
            cabg_details.TotalWidth = 400.0F;
            cabg_details.LockedWidth = true;


            cell = new PdfPCell(new Paragraph("Artery", boldfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Quality", boldfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Angle", boldfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Diameter", boldfont)); cell.Border = 0; cabg_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel25, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel26, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel27, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel28, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel29, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel30, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel31, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel32, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel33, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel34, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel35, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel36, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel37, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel38, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel39, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel40, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel41, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel42, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel43, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel44, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel1, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel2, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel3, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel4, smallfont)); cell.Border = 0; cabg_details.AddCell(cell);

            return cabg_details;
        }
        private PdfPTable getAortic_Valve_Details(Class_Final_operative_report _frs)
        {
            var valve_details = new PdfPTable(3);
            valve_details.TotalWidth = 400.0F;
            valve_details.LockedWidth = true;


            cell = new PdfPCell(new Paragraph("Valve", boldfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Size", boldfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Serial number", boldfont)); cell.Border = 0; valve_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel25, smallfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel26, smallfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel27, smallfont)); cell.Border = 0; valve_details.AddCell(cell);

            return valve_details;
        }
        private PdfPTable getMitral_Valve_Details(Class_Final_operative_report _frs)
        {
            var valve_details = new PdfPTable(3);
            valve_details.TotalWidth = 400.0F;
            valve_details.LockedWidth = true;


            cell = new PdfPCell(new Paragraph("Valve", boldfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Size", boldfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Serial number", boldfont)); cell.Border = 0; valve_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel28, smallfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel29, smallfont)); cell.Border = 0; valve_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel30, smallfont)); cell.Border = 0; valve_details.AddCell(cell);

            return valve_details;
        }
        private PdfPTable getGeneralReport(Class_Final_operative_report _frs)
        {

            var gen_details = new PdfPTable(1);
            gen_details.TotalWidth = 500.0F;
            gen_details.LockedWidth = true;


            cell = new PdfPCell(new Paragraph("Report", bigboldfont));
            cell.Border = 0;
            cell.HorizontalAlignment = 0;
            cell.PaddingBottom = 10.0F;
            gen_details.AddCell(cell);


            cell = new PdfPCell(new Paragraph(_frs.Regel17, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel18, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel19, smallfont)); cell.Border = 0; gen_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel20, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel21, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel22, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel23, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel24, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel25, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel26, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel27, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel28, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel29, smallfont)); cell.Border = 0; gen_details.AddCell(cell);

            cell = new PdfPCell(new Paragraph(_frs.Regel30, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel31, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel32, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel33, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel34, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel35, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel36, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel37, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel38, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Regel39, smallfont)); cell.Border = 0; gen_details.AddCell(cell);
            return gen_details;

        }
        private PdfPTable getBlockTable_2(Class_Final_operative_report _frs)
        {

            var pdftable_a = new PdfPTable(1);
            pdftable_a.TotalWidth = 500.0F;
            pdftable_a.LockedWidth = true;


            if (_frs.Regel45 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel45, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel46 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel46, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel47 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel47, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel48 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel48, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel49 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel49, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel50 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel50, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel51 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel51, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel52 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel52, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel53 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel53, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel54 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel54, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }

            return pdftable_a;
        }
        private PdfPTable getBlockAVR_MVR_2(Class_Final_operative_report _frs)
        {

            var pdftable_a = new PdfPTable(1);
            pdftable_a.TotalWidth = 500.0F;
            pdftable_a.LockedWidth = true;


            if (_frs.Regel31 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel31, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel32 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel32, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel33 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel33, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel34 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel34, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel35 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel35, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel36 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel36, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel37 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel37, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }


            return pdftable_a;
        }
        private PdfPTable getBlockAVR_MVR_3(Class_Final_operative_report _frs)
        {

            var pdftable_a = new PdfPTable(1);
            pdftable_a.TotalWidth = 500.0F;
            pdftable_a.LockedWidth = true;


            if (_frs.Regel38 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel38, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel39 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel39, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel40 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel40, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel41 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel41, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel42 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel42, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel43 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel43, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel44 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel44, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel45 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel45, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }
            if (_frs.Regel46 != "") { cell = new PdfPCell(new Paragraph(_frs.Regel46, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell); }


            return pdftable_a;
        }
        private PdfPTable getCommentTable(Class_Final_operative_report _frs)
        {
            var pdftable_a = new PdfPTable(1);
            pdftable_a.TotalWidth = 500.0F;
            pdftable_a.LockedWidth = true;


            cell = new PdfPCell(new Paragraph("Comments", boldfont)); cell.Border = 0; pdftable_a.AddCell(cell);


            cell = new PdfPCell(new Paragraph(_frs.Comment1, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Comment2, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell);
            cell = new PdfPCell(new Paragraph(_frs.Comment3, smallfont)); cell.Border = 0; pdftable_a.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Signature", boldfont));
            cell.Border = 0;
            cell.HorizontalAlignment = 2;
            pdftable_a.AddCell(cell);

            return pdftable_a;
        }
        private PdfPTable getFooter(Class_Final_operative_report _frs)
        {
            var footer = new PdfPTable(2);
            footer.TotalWidth = 500.0F;
            footer.LockedWidth = true;
            footer.DefaultCell.BackgroundColor = footer_background_color;


            cell = new PdfPCell(new Paragraph("CRD 2943.78545", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 5.0F, iTextSharp.text.Font.NORMAL)));
            cell.Border = 0;
            cell.BackgroundColor = footer_background_color;
            cell.Colspan = 2;



            footer.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Printed at: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), smallfont));
            cell.Border = 0;
            cell.BackgroundColor = footer_background_color;
            cell.Colspan = 2;
            footer.AddCell(cell);

            cell = new PdfPCell(new Paragraph("by: " + _frs.Regel13, smallfont));
            cell.Border = 0;
            cell.BackgroundColor = footer_background_color;
            cell.Colspan = 2;
            footer.AddCell(cell);


            return footer;
        }
        private PdfPTable getGeneralFooter(Class_Final_operative_report _frs)
        {
            var footer = new PdfPTable(2);
            footer.TotalWidth = 500.0F;
            footer.LockedWidth = true;
            footer.DefaultCell.BackgroundColor = footer_background_color;

            cell = new PdfPCell(new Paragraph("Signature", bigboldfont));
            cell.Border = 0;
            cell.HorizontalAlignment = 2;
            cell.Colspan = 2;
            footer.AddCell(cell);


            cell = new PdfPCell(new Paragraph("CRD 2943.78545", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 5.0F, iTextSharp.text.Font.NORMAL)));
            cell.Border = 0;
            cell.BackgroundColor = footer_background_color;
            cell.Colspan = 2;
            footer.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Printed at: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), smallfont));
            cell.Border = 0;
            cell.BackgroundColor = footer_background_color;
            cell.Colspan = 2;
            footer.AddCell(cell);

            cell = new PdfPCell(new Paragraph("by: " + _frs.UserName, smallfont));
            cell.Border = 0;
            cell.BackgroundColor = footer_background_color;
            cell.Colspan = 2;
            footer.AddCell(cell);


            return footer;
        }



        public int deletePDF(int id)
        {
            var id_string = id.ToString();
            var pathToFile = _env.ContentRootPath + "/assets/pdf/";
            var file_name = pathToFile + id_string + ".pdf";

            if (System.IO.File.Exists(file_name))
            {
                System.IO.File.Delete(file_name);
                System.Threading.Thread.Sleep(20);
            }
            return 1;
        }
        public async Task<int> getReportCode(int procedure_id)
        {
            var currentProcedure = await _context.Procedures.FirstOrDefaultAsync(r => r.ProcedureId == procedure_id);
            var report_code = Convert.ToInt32(_rm.getReportCode(currentProcedure.fdType));
            return report_code;
        }
        public int addToExpiredReports(ReportTiming rt)
        {
            var l = GetXmlDetails();
            bool changesMade = false;

            if (!l.Any(r => r.id == rt.id))
            {
                l.Add(rt);
                changesMade = true;
            }

            if (changesMade)
            {
                SaveXmlDetails(l);
            }

            return 3;
        }


        public async Task<bool> isReportExpired(int id)
        {
            var result = false;
            // find out if this report is expired
            await Task.Run(() =>
            {
                var l = new List<ReportTiming>();
                l = GetXmlDetails();  // load the xml file im a list of ReportTimings
                foreach (ReportTiming rep in l)
                {
                    if (rep.id == id)
                    {
                        var currentTicks = DateTime.Now.Ticks;
                        var interval = 2592000000000; // interval is set to 3 days for now
                        if ((rep.publishTime.Ticks + interval) < currentTicks)
                        {
                            // report is expired
                            result = true;
                        }
                        else { result = false; }
                    }
                }
            });
            return result;
        }
        public int deleteExpiredReports()
        {
            // this is called by a CRON job and checks if there are expired reports, which are then deleted
            var currentTicks = DateTime.UtcNow.Ticks; // use UTC time instead of local time
            var interval = TimeSpan.FromDays(3).Ticks; // use TimeSpan to define interval

            try
            {
                // load the xml file im a list of ReportTimings
                var reportTimings = GetXmlDetails();

                // filter on expired report timings
                var expiredReportTimings = reportTimings.Where(rt => (rt.publishTime.Ticks + interval) < currentTicks).ToList();
                foreach (ReportTiming rt in expiredReportTimings) { deletePDF(rt.id); } // delete expired pdf's

                // filter out expired report timings
                var newReportTimings = reportTimings.Where(rt => (rt.publishTime.Ticks + interval) >= currentTicks).ToList();

                // write new report timings to file
                SaveXmlDetails(newReportTimings);

                return 1;
            }
            catch (Exception ex)
            {
                // log the exception instead of returning ret value
                Console.WriteLine("Failed to delete expired reports: " + ex.Message);
                return 2;
            }
        }
        private List<ReportTiming> GetXmlDetails()
        {
            // load the xml file into a list of ReportTimings
            var pathToFile = Path.Combine(_env.ContentRootPath, "conf", "timingsRefReport.xml");

            if (!File.Exists(pathToFile))
            {
                return new List<ReportTiming>();
            }

            using (var stream = File.Open(pathToFile, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<ReportTiming>));
                return (List<ReportTiming>)serializer.Deserialize(stream);
            }
        }
        private void SaveXmlDetails(List<ReportTiming> reportTimings)
        {
            // save the list of ReportTimings to an xml file
            var pathToFile = Path.Combine(_env.ContentRootPath, "conf", "timingsRefReport.xml");

            using (var stream = File.Open(pathToFile, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<ReportTiming>));
                serializer.Serialize(stream, reportTimings);
            }
        }
    }
}
