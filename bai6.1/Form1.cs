
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RetailSalesSystem
{
    public partial class Form1 : Form
    {
        TextBox txtName, txtPrice, txtQuantity;
        DataGridView dgv;
        Label lblTotal;

        List<Product> products = new List<Product>();

        public Form1()
        {
            InitializeComponent();
            Text = "HE THONG QUAN LY BAN HANG";
            Size = new Size(850, 550);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label();
            title.Text = "QUAN LY BAN HANG";
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.Location = new Point(250, 20);
            title.AutoSize = true;
            Controls.Add(title);

            AddLabel("Ten san pham:", 40, 90);
            txtName = AddTextBox(170, 90);

            AddLabel("Don gia:", 40, 130);
            txtPrice = AddTextBox(170, 130);

            AddLabel("So luong:", 40, 170);
            txtQuantity = AddTextBox(170, 170);

            Button btnAdd = new Button();
            btnAdd.Text = "Them san pham";
            btnAdd.Location = new Point(40, 220);
            btnAdd.Size = new Size(130, 35);
            btnAdd.Click += AddProduct;
            Controls.Add(btnAdd);

            Button btnDelete = new Button();
            btnDelete.Text = "Xoa san pham";
            btnDelete.Location = new Point(180, 220);
            btnDelete.Size = new Size(130, 35);
            btnDelete.Click += DeleteProduct;
            Controls.Add(btnDelete);

            dgv = new DataGridView();
            dgv.Location = new Point(40, 280);
            dgv.Size = new Size(750, 160);
            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            Controls.Add(dgv);

            lblTotal = new Label();
            lblTotal.Location = new Point(40, 460);
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

        void AddProduct(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();

            decimal price;
            int quantity;

            if (name == "" ||
                !decimal.TryParse(txtPrice.Text, out price) ||
                !int.TryParse(txtQuantity.Text, out quantity) ||
                price <= 0 || quantity <= 0)
            {
                MessageBox.Show("Vui long nhap thong tin hop le!");
                return;
            }

            products.Add(new Product
            {
                Name = name,
                Price = price,
                Quantity = quantity
            });

            txtName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();

            RefreshGrid();
        }

        void DeleteProduct(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null ||
                dgv.CurrentRow.Index >= products.Count)
                return;

            products.RemoveAt(dgv.CurrentRow.Index);
            RefreshGrid();
        }

        void RefreshGrid()
        {
            dgv.DataSource = null;
            dgv.DataSource = products.Select(p => new
            {
                TenSanPham = p.Name,
                DonGia = p.Price,
                SoLuong = p.Quantity,
                ThanhTien = p.Price * p.Quantity
            }).ToList();

            decimal total = products.Sum(
                p => p.Price * p.Quantity);

            lblTotal.Text = "Tong gia tri hang: " +
                total.ToString("N0") + " VND";
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
