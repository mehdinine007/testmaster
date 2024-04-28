using FastReport;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using OrderManagement.Application.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.OrderManagement
{
    public class ReportService : ApplicationService,IReportService
    {
        public ReportService()
        {
                
        }
        public async Task<string> Execute(string reportName, List<OrderDetailDto> data)
        {
            //StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkcgIvwL0jnpsDqRpWg5FI5kt2G7A0tYIcUygBh1sPs7koivWV0htru4Pn2682yhdY3+9jxMCVTKcKAjiEjgJzqXgLFCpe62hxJ7/VJZ9Hq5l39md0pyydqd5Dc1fSWhCtYqC042BVmGNkukYJQN0ufCozjA/qsNxzNMyEql26oHE6wWE77pHutroj+tKfOO1skJ52cbZklqPm8OiH/9mfU4rrkLffOhDQFnIxxhzhr2BL5pDFFCZ7axXX12y/4qzn5QLPBn1AVLo3NVrSmJB2KiwGwR4RL4RsYVxGScsYoCZbwqK2YrdbPHP0t5vOiLjBQ+Oy6F4rNtDYHn7SNMpthfkYiRoOibqDkPaX+RyCany0Z+uz8bzAg0oprJEn6qpkQ56WMEppdMJ9/CBnEbTFwn1s/9s8kYsmXCvtI4iQcz+RkUWspLcBzlmj0lJXWjTKMRZz+e9PmY11Au16wOnBU3NHvRc9T/Zk0YFh439GKd/fRwQrk8nJevYU65ENdAOqiP5po7Vnhif5FCiHRpxgF";
            //var report = new StiReport();
            //report.Load(@"Reports\"+ reportName + ".mrt");
            //report.RegData("Table", data);
            //report.Render();
            //MemoryStream strm = new MemoryStream();
            //report.ExportDocument(StiExportFormat.Pdf, strm);
            //strm.Position = 0;
            //byte[] pdfBytes = strm.ToArray();
            //return Convert.ToBase64String(pdfBytes);
            WebReport webReport = new WebReport(); // Create a Web Report Object
            webReport.Report.RegisterData(data, "Table");
            webReport.Report.Load(@"Reports\" + reportName + ".frx");
            webReport.Report.Prepare();
            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                byte[] pdfBytes = ms.ToArray();
                return Convert.ToBase64String(pdfBytes);
            }
        }
    }
}
