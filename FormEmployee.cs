using BT05.Models;
using BT05.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT05
{
    public partial class FormEmployee : Form
    {
        DBModel db;

        Employee selectedItem;

        public FormEmployee()
        {
            InitializeComponent();
            db = new DBModel();
            LoadAllData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckTextBoxData()) return;
            try
            {
                SetDataFromTextBoxToSelectedItem();

                db.Employees.Add(selectedItem);

                db.SaveChanges();

                LoadDGV(db.Employees.ToList());

                ResetData();

                MessageBox.Show("Thêm nhân viên thành công?");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CheckTextBoxData()
        {
            string name = txtFullname.Text.Trim();
            string address = txtAddress.Text.Trim();
            string phoneNum = txtPhoneNum.Text.Trim();

            

            if (name == ""|| address == "" || phoneNum == "")
            {
                MessageBox.Show("Nhập đầy đủ thông tin");
                return false;
            }

            long sdt;

            bool success = Int64.TryParse(phoneNum, out sdt);

            if (!success || phoneNum.Length != 10)
            {
                MessageBox.Show("sdt không đúng định dạng! là chữ số và phải đủ 10 kí tự");
                return false;
            }

            

            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            

            if (selectedItem.Id.ToString() == "0")
            {
                MessageBox.Show("Vui lòng chọn nhân viên");
                return;
            }

            if (!CheckTextBoxData()) return;

            try
            {
                FindEmployeeById(selectedItem.Id);

                SetDataFromTextBoxToSelectedItem();

                db.SaveChanges();

                LoadDGV(db.Employees.ToList());

                ResetData();

                MessageBox.Show("Cập nhật nhân viên thành công?");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsSelectedItem()
        {
            if (selectedItem.Id.ToString() == "0")
            {
                MessageBox.Show("Vui lòng chọn nhân viên");
                return false;
            }
            return true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (!IsSelectedItem()) return;


            try
            {
                FindEmployeeById(selectedItem.Id);

                if (MessageBox.Show("Bạn thật sự muốn xoá? " + selectedItem.Name, "Confirm Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;

                Employee item = db.Employees.FirstOrDefault(p => p.Id == selectedItem.Id);
                db.Employees.Remove(item);

                db.SaveChanges();

                LoadDGV(db.Employees.ToList());

                ResetData();

                MessageBox.Show("Xoá nhân viên thành công?");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvEmployee.Rows[e.RowIndex].Cells[0].Value != null)//kiểm tra data có tồn tại chưa nhá
                {
                    var c = dgvEmployee.Rows[e.RowIndex].Cells[0].Value.ToString();
                    int id;
                    if(!int.TryParse(c, out id))
                    {
                        MessageBox.Show("Id không đúng định dạng");
                        return;
                    }

                    if(!db.Employees.Any(s => s.Id == id))
                    {
                        MessageBox.Show("Không tìm thấy nhân viên");
                        return;
                    }
                    FindEmployeeById(id);
                    FillSelectedItemDataToTextBox();
                }
            }
            catch
            {
                return;
            }
        }

        private void LoadAllData()
        {
            this.selectedItem = new Employee();
            LoadDGV(db.Employees.ToList());
            LoadCBBFaculty();
            LoadCBBLevel();
        }

        private void LoadCBBLevel()
        {
            cbbLevel.DataSource = db.Levels.ToList();
            cbbLevel.DisplayMember = "Name";
        }

        private void LoadCBBFaculty()
        {
            cbbFal.DataSource = db.Faculties.ToList();
            cbbFal.DisplayMember = "Name";
        }

        private void LoadDGV(List<Employee> ls)
        {
            var result = from s in ls
                         select new {Id = s.Id, Fullname = s.Name, Address = s.Address, PhoneNum = s.PhoneNum, Faculty = s.Faculty.Name, Date = s.DateOfBirth.ToString("dd/MM/yyyy"), Level = s.Level.Name };

            dgvEmployee.DataSource = result.ToList();
        }

        private void ResetData()
        {
            this.selectedItem = new Employee();
            txtFullname.Text = "";
            txtAddress.Text = "";
            txtPhoneNum.Text = "";
            cbbFal.SelectedIndex = 0;
            cbbLevel.SelectedIndex = 0;
            dtpDate.Value = DateTime.Now;
        }

        private void FillSelectedItemDataToTextBox()
        {
            txtFullname.Text = selectedItem.Name;
            txtAddress.Text = selectedItem.Address;
            txtPhoneNum.Text = selectedItem.PhoneNum;
            cbbFal.SelectedItem = selectedItem.Faculty;
            cbbLevel.SelectedItem = selectedItem.Level;
            dtpDate.Value = selectedItem.DateOfBirth;
        }

        private void SetDataFromTextBoxToSelectedItem()
        {
            selectedItem.Name = txtFullname.Text;
            selectedItem.Address = txtAddress.Text;
            selectedItem.PhoneNum = txtPhoneNum.Text;
            selectedItem.Faculty = (Faculty) cbbFal.SelectedValue;
            selectedItem.Level = (Level) cbbLevel.SelectedValue;
            selectedItem.DateOfBirth = dtpDate.Value;
        }

        private void FindEmployeeById(int id)
        {
            this.selectedItem = db.Employees.FirstOrDefault(s => s.Id == id);
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = dtpDate.Value;
            if(date.Date > DateTime.Now.Date)
            {
                dtpDate.Value = DateTime.Now.Date;
            }
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            FormReport f = new FormReport(db);
            f.Show();
        }
    }
}
