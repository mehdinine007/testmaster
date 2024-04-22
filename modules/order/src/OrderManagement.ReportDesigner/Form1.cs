using OrderManagement.ReportDesigner;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManagement.ReportDesigner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StiDesigner.SavingReport += new StiSavingObjectEventHandler(GlobalEvents_SaveReport);

        }

        private void GlobalEvents_SaveReport(object sender, StiSavingObjectEventArgs e)
        {
            if (e.EventSource == StiSaveEventSource.SaveAs)
            {
                e.Processed = false;
                return;
            }

            var report = ((IStiDesignerBase)sender).Report;
            report.Save(report.ReportFile);

        }
        private StiReport ReportConfig(string reportName)
        {
            var report = new StiReport();
            report.Dictionary.Databases.Clear();
            string fileName = Properties.Settings.Default.ReportPath + reportName + ".mrt";
            report.ReportFile = fileName;
            if (File.Exists(fileName))
            {
                report.Load(fileName);
            }
            return report;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            var report = ReportConfig("RptContractForm");
            var orderdata = new List<CustomerOrder_OrderDetailDto>();
            report.RegData("Table", orderdata);
            report.Design();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.button1_Click(button1, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var report = ReportConfig("RptFactor");
            var orderdata = new List<CustomerOrder_OrderDetailDto>
            {
                new CustomerOrder_OrderDetailDto
                {
                    CreationTime = DateTime.Now,
                    SurName = "حسنی",
                    Name = "مصطفی",
                    OrderId = 0943870293,
                    PaymentPrice = 90_000_000,
                    ProductTitle = "بنز در عقب صندلی جلو",
                    TransactionId = "qdd1eddsd211dda",
                    TransactionCommitDate = DateTime.Now
                }
            };
            report.RegData("Table", orderdata);
            report.Design();
        }
    }
}
