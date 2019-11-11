using FonNature.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FonNature
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private bool Kiemtra(string username, string password)
        {
            return AccountDAO.Instance.CheckLogin(username,password);
        }
        public static string tendangnhap ;
        public static string matkhau;
        private void btnlogin_Click(object sender, EventArgs e)
        {
            // Không được sử dụng kí tự đặc biệt trong username
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (regexItem.IsMatch(txbusername.Text))
            {
                
                string username = txbusername.Text;
                string password = txbpass.Text;
                tendangnhap = username;
                matkhau = password;

                if (Kiemtra(username, password))
                {
                    FMain fmain = new FMain();
                    this.Hide(); // Ẩn flogin đi
                    fmain.ShowDialog(); // top mode, xử lí xong thằng fmain, fmain tắt đi, thì flogin thì code mới chạy xuống dòng this.show
                    this.Show(); // Tắt Fmain thì flogin hiển thị lại
                }
                else MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không được sử dụng kí tự đặc biệt trong username", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

          
         
        }

        private void btnquit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có thật sự muốn thoát chương trình","Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) != DialogResult.OK)
            {
                e.Cancel = true; // Không cho event này thực thi
            }
        }
    }
}
