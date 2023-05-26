using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations.reports
{
    public class OperativeReportPdf : IOperativeReportPdf
    {
        private readonly IWebHostEnvironment _env;
        private readonly SpecialReportMaps _rm;
        private readonly DataContext _context;

        public OperativeReportPdf(IWebHostEnvironment env, SpecialReportMaps rm, DataContext context)
        {
            _env = env;
            _rm = rm;
            _context = context;
        }
        public async Task<int> getPdf(int report_code, Class_Final_operative_report fr)
        {
            var pathToFile = _env.ContentRootPath + "/assets/pdf/";
            var ps = fr.procedure_id.ToString();
            var file_name = pathToFile + ps + ".pdf";
            var currentProcedure = await _context.Procedures.FirstOrDefaultAsync(r => r.ProcedureId == fr.procedure_id);
            var soort = currentProcedure.fdType;


            using (var fs = new System.IO.FileStream(file_name, System.IO.FileMode.Create))
            {
                var doc = new iTextSharp.text.Document();
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
                doc.SetMargins(0.0F, 10.0F, 70.0F, 10.0F);
                compose_pdf(doc, writer, report_code, soort, fr);
            }

            return 1;

        }

        private void compose_pdf(iTextSharp.text.Document doc,
        iTextSharp.text.pdf.PdfWriter wri,
        int report_code,
        int soort,
        Class_Final_operative_report fr)
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
        }
    }
}