using OrderManagement.ReportDesigner;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            report.Save(@"Reports\RptOrderDetail.mrt");
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            report.Dictionary.Databases.Clear();
            var orderdata = new List<CustomerOrder_OrderDetailDto>();
            //report.Load(@"Reports\RptOrderDetail.mrt");
            report.RegData("Table", orderdata);
            report.Design();
        }
    }
}
