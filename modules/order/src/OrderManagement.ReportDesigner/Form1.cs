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

            report.Save(Properties.Settings.Default.ReportPath + "RptContractForm.mrt");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            report.Dictionary.Databases.Clear();
            var orderdata = new List<CustomerOrder_OrderDetailDto>();
            string fileName = Properties.Settings.Default.ReportPath + "RptContractForm.mrt";
            if (File.Exists(fileName))
            {
                report.Load(fileName);
            }
            report.RegData("Table", orderdata);
            report.Design();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.button1_Click(button1, null);
        }
    }
}
