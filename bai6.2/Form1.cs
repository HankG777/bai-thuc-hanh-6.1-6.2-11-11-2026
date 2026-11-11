
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace bai6_2
{
    public partial class Form1 : Form
    {
        TextBox txtName, txtPosition, txtSalary, txtDays;
        DataGridView dgv;
        Label lblTotal;

        List<Employee> employees = new List<Employee>();

        public Form1()
        {
            InitializeComponent();

            Text = "HE THONG QUAN LY NHAN SU";
            Size = new Size(950, 600);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label();
            title.Text = "QUAN LY NHAN SU VA TINH LUONG";
            title.Font = new Font("Arial", 19, FontStyle.Bold);
            title.Location = new Point(220, 20);
            title.AutoSize = true;
            Controls.Add(title);

            AddLabel("Ho va ten:", 40, 90);
            txtName = AddTextBox(170, 90);

            AddLabel("Chuc vu:", 40, 130);
            txtPosition = AddTextBox(170, 130);

            AddLabel("Luong co ban:", 40, 170);
            txtSalary = AddTextBox(170, 170);

            AddLabel("So ngay cong:", 40, 210);
            txtDays = AddTextBox(170, 210);

            Button btnAdd = new Button();
            btnAdd.Text = "Them nhan vien";
            btnAdd.Location = new Point(400, 90);
            btnAdd.Size = new Size(140, 35);
            btnAdd.Click += AddEmployee;
            Controls.Add(btnAdd);

            Button btnDelete = new Button();
            btnDelete.Text = "Xoa nhan vien";
            btnDelete.Location = new Point(400, 135);
            btnDelete.Size = new Size(140, 35);
            btnDelete.Click += DeleteEmployee;
            Controls.Add(btnDelete);

            Button btnCalculate = new Button();
            btnCalculate.Text = "Tinh luong";
            btnCalculate.Location = new Point(400, 180);
            btnCalculate.Size = new Size(140, 35);
            btnCalculate.Click += CalculateSalary;
            Controls.Add(btnCalculate);

            dgv = new DataGridView();
            dgv.Location = new Point(40, 280);
            dgv.Size = new Size(850, 190);
            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            Controls.Add(dgv);

            lblTotal = new Label();
            lblTotal.Location = new Point(40, 490);
            lblTotal.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTotal.AutoSize = true;
            Controls.Add(lblTotal);

            RefreshGrid();
        }

        void AddLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            Controls.Add(lbl);
        }

        TextBox AddTextBox(int x, int y)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Width = 180;
            Controls.Add(txt);
            return txt;
        }

        void AddEmployee(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string position = txtPosition.Text.Trim();

            decimal salary;
            int days;

            if (name == "" || position == "" ||
                !decimal.TryParse(txtSalary.Text, out salary) ||
                !int.TryParse(txtDays.Text, out days) ||
                salary <= 0 || days < 0 || days > 26)
            {
                MessageBox.Show("Vui long nhap thong tin hop le!");
                return;
            }

            employees.Add(new Employee
            {
                Id = employees.Count == 0 ? 1 :
                    employees.Max(x => x.Id) + 1,
                Name = name,
                Position = position,
                BasicSalary = salary,
                WorkDays = days,
                ActualSalary = 0
            });

            txtName.Clear();
            txtPosition.Clear();
            txtSalary.Clear();
            txtDays.Clear();

            RefreshGrid();
        }

        void DeleteEmployee(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Hay chon nhan vien can xoa!");
                return;
            }

            int id = Convert.ToInt32(
                dgv.CurrentRow.Cells["MaNV"].Value);

            Employee emp = employees.FirstOrDefault(
                x => x.Id == id);

            if (emp != null)
                employees.Remove(emp);

            RefreshGrid();
        }

        void CalculateSalary(object sender, EventArgs e)
        {
            if (employees.Count == 0)
            {
                MessageBox.Show("Chua co nhan vien!");
                return;
            }

            foreach (Employee emp in employees)
            {
                emp.ActualSalary =
                    emp.BasicSalary / 26 * emp.WorkDays;
            }

            RefreshGrid();

            decimal total = employees.Sum(
                x => x.ActualSalary);

            MessageBox.Show(
                "Tong luong phai tra: " +
                total.ToString("N0") + " VND");
        }

        void RefreshGrid()
        {
            dgv.DataSource = null;

            dgv.DataSource = employees.Select(emp => new
            {
                MaNV = emp.Id,
                HoTen = emp.Name,
                ChucVu = emp.Position,
                LuongCoBan = emp.BasicSalary,
                SoNgayCong = emp.WorkDays,
                LuongThucNhan = emp.ActualSalary
            }).ToList();

            decimal total = employees.Sum(
                x => x.ActualSalary);

            lblTotal.Text = "Tong luong: " +
                total.ToString("N0") + " VND";
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public decimal BasicSalary { get; set; }
        public int WorkDays { get; set; }
        public decimal ActualSalary { get; set; }
    }
}
