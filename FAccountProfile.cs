using FonNature.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FonNature
{
    public partial class FAccountProfile : Form
    {
        public FAccountProfile()
        {
            InitializeComponent();
            LoadThongtin();
        }
        public void LoadThongtin()
        {
            txbusername.Text = fLogin.tendangnhap;
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btncapnhat_Click(object sender, EventArgs e)
        {
            if(txbtenhienthi.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên hiển thị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(txbmatkhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu hiện tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(txbmatkhaumoi.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txbnhaplaimatkhau.Text == "")
            {
                MessageBox.Show("Vui lòng Nhập lại mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(AccountDAO.Instance.CheckLogin(txbusername.Text,txbmatkhau.Text) == true)
                {
                    if(txbmatkhaumoi.Text == txbnhaplaimatkhau.Text)
                    {
                        AccountDAO.Instance.updateAccount(txbusername.Text, txbtenhienthi.Text, txbmatkhaumoi.Text);
                        MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Nhập lại mật khẩu sai !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
    }
}
