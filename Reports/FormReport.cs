using BT05.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT05.Reports
{
    public partial class FormReport : Form
    {
        DBModel db;
        public FormReport(DBModel db)
        {
            InitializeComponent();
            this.db = db;
        }

        private void FormReport_Load(object sender, EventArgs e)
        {

            ReportParameter[] param = new ReportParameter[] {
                new ReportParameter("txtDate", "Ngày " + DateTime.Now.ToString("dd") + " tháng " + DateTime.Now.ToString("MM") + " năm " + DateTime.Now.ToString("yyyy") ),
            };
            this.reportViewer1.LocalReport.SetParameters(param);

            var result = from s in db.Employees.ToList()
            select new { Id = s.Id, HoTen = s.Name, DiaChi = s.Address, SDT = s.PhoneNum, PhongBan = s.Faculty.Name, NgaySinh = s.DateOfBirth.ToString("dd/MM/yyyy"), TrinhDo = s.Level.Name  };

            var reportResource = new ReportDataSource("DataSetEmployee", result.ToList());
            this.reportViewer1.LocalReport.DataSources.Add(reportResource);
            reportViewer1.RefreshReport();
        }
    }
}
