using FastReport;
using OrderManagement.ReportDesigner;
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

namespace OrderManagement.RptDesigner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private FastReport.Report ReportConfig(string reportName)
        {
            var report = new FastReport.Report();
            report.Dictionary.Clear();
            string fileName = Properties.Settings.Default.ReportPath + reportName + ".frx";
            if (File.Exists(fileName))
            {
                report.Load(fileName);
            }
            else
            {
                report.Report.FileName = fileName;
            }
            return report;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var report = ReportConfig("RptContractForm");
            var orderdata = new List<OrderDetailDto>()
            {
                new OrderDetailDto()
                {
                    BirthCertId = "123456",
                    NationalCode = "1092271600",
                    Address = "sdfsdfsfsdfsdfsdfsdfsdfsdfsdfsdfsd"

                }
            };
            report.Report.RegisterData(orderdata, "Table");
            report.Design();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var report = ReportConfig("RptFactor");
            var orderdata = new List<OrderDetailDto>()
            {
                new OrderDetailDto()
                {
                    BirthCertId = "123456",
                    CreationTime = DateTime.Now,
                    OrderId = 12345,
                    Name = "مظاهر",
                    SurName = "دیانی",
                    ProductTitle = "پژو 405",
                    TransactionCommitDate = DateTime.Now,
                    TransactionId = "45789632",
                    NationalCode = "1092271600",
                    Address = "sdfsdfsfsdfsdfsdfsdfsdfsdfsdfsdfsd",
                    PaymentPrice = 1500000000,

                }
            };
            report.Report.RegisterData(orderdata, "Table");
            report.Design();
        }
    }
}
