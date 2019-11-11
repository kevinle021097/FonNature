using FonNature.DAO;
using FonNature.DTO;
using FonNature.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FonNature
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
            loadlistview();
            CheckTK();
        }
        List<SanPham> listsp = new List<SanPham>();
        DataTable dt = DataProvider.Instance.ExecureQuery("EXEC dbo.USP_GetListSanPham");
        #region method
        private void loadlistview()
        {
            lsvhoadon.Columns.Add("Tên Khách Hàng", 100);
            lsvhoadon.Columns.Add("SDT", 90);
            lsvhoadon.Columns.Add("Địa Chỉ", 130);
            lsvhoadon.Columns.Add("Sản phẩm", 120);
            lsvhoadon.Columns.Add("Color", 60);
            lsvhoadon.Columns.Add("Số Lượng", 60);
            lsvhoadon.Columns.Add("Chiết Khấu", 70);
            lsvhoadon.Columns.Add("Tổng tiền", 100);
            // Load dữ liệu cho combobox sản phẩm



            foreach (DataRow item in dt.Rows)
            {
                SanPham sp = new SanPham(item);
                listsp.Add(sp);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (SanPham item in listsp)
                {
                    bool check = true;
                    bool check2 = true;

                    for (int i = 0; i < cbbsanpham.Items.Count; i++)
                    {
                        if (cbbsanpham.Items[i].ToString() == item.Name)
                        {
                            check2 = false;
                        }

                    }
                    for (int i = 0; i < cbbMauSP.Items.Count; i++)
                    {
                        if (cbbMauSP.Items[i].ToString() == item.Color)
                        {
                            check = false;
                        }

                    }
                    if (check == true)
                    {
                        cbbMauSP.Items.Add(item.Color);
                    }
                    if (check2 == true)
                    {
                        cbbsanpham.Items.Add(item.Name);
                    }

                }
                cbbMauSP.SelectedIndex = 0;
                cbbsanpham.SelectedIndex = 0;



                cbbmember.Items.Add("Bạc");
                cbbmember.Items.Add("Vàng");
                cbbmember.Items.Add("Kim Cương");
                cbbmember.SelectedIndex = 0;

            }



            cbbchietkhau.Items.Add("0%");
            cbbchietkhau.Items.Add("10%");
            cbbchietkhau.Items.Add("20%");
            cbbchietkhau.Items.Add("30%");
            cbbchietkhau.SelectedIndex = 0;
        }
        private void CheckTK()
        {

            int kiemtra = AccountDAO.Instance.gettypeAccByUsername(fLogin.tendangnhap);
            if (kiemtra != 1)
            {
                adminToolStripMenuItem.Visible = false;
            }

        }


        #endregion

        #region event


        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FAccountProfile frmthongtin = new FAccountProfile();

            frmthongtin.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FAdmin fadmin = new FAdmin();
            fadmin.ShowDialog();
        }



        private void btnclear_Click(object sender, EventArgs e)
        {
            txbName.Clear();
            txbdiachi.Clear();
            txbsdt.Clear();
            nmtongtien.Value = 0;
            txbsdt.Focus();
            cbbsanpham.SelectedIndex = 0;
            nmsanpham.Value = 1;
        }
        double tonghoadon = 0;

        private void btnthem_Click(object sender, EventArgs e)
        {

            if (txbName.Text == "")
            {

                MessageBox.Show("Vui lòng nhập Tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txbdiachi.Text == "")
            {

                MessageBox.Show("Vui lòng nhập Địa chỉ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txbsdt.Text == "")
            {

                MessageBox.Show("Vui lòng nhập SĐT!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (SanPhamDAO.Instance.checkSPCSDL(cbbsanpham.Text, cbbMauSP.Text) == false)
            {
                MessageBox.Show("Sản phẩm hiện tại không có Color đang chọn, Vui lòng chọn Color khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Trường hợp 1 : Listview đã có item
                if (lsvhoadon.Items.Count != 0)
                {

                    bool check = true;

                    foreach (ListViewItem item in lsvhoadon.Items)
                    {
                        if (item.SubItems[0].Text != txbName.Text)
                        {
                            check = false;
                            MessageBox.Show("Chỉ đc nhập tên một khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (item.SubItems[2].Text != txbdiachi.Text)
                        {
                            check = false;
                            MessageBox.Show("Chỉ đc nhập địa chỉ một khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (item.SubItems[1].Text != txbsdt.Text)
                        {
                            check = false;
                            MessageBox.Show("Chỉ được nhập một số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    for (int i = 0; i < lsvhoadon.Items.Count; i++)
                    {
                        if (cbbsanpham.Text == lsvhoadon.Items[i].SubItems[3].Text && cbbMauSP.Text == lsvhoadon.Items[i].SubItems[4].Text)
                        {
                            check = false;
                            MessageBox.Show("Sản phẩm và Color đã có trong bảng rồi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (int.Parse(nmsanpham.Value.ToString()) > SanPhamDAO.Instance.CheckKhoHang(cbbsanpham.Text, cbbMauSP.Text))
                    {
                        check = false;
                        MessageBox.Show("Kho đã hết hàng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (check == true)
                    {
                        ListViewItem item = new ListViewItem(txbName.Text);
                        item.SubItems.Add(txbsdt.Text);
                        item.SubItems.Add(txbdiachi.Text);
                        item.SubItems.Add(cbbsanpham.Text);
                        item.SubItems.Add(cbbMauSP.Text);
                        item.SubItems.Add(nmsanpham.Text);
                        item.SubItems.Add(cbbchietkhau.Text);
                        double tongtien = double.Parse(nmsanpham.Value.ToString()) * 0.01 * (100 - double.Parse(cbbchietkhau.Text.Remove(cbbchietkhau.Text.Length - 1))) * double.Parse(nmtongtien.Value.ToString());
                        item.SubItems.Add(tongtien.ToString("N0"));
                        lsvhoadon.Items.Add(item);
                        tonghoadon += tongtien;
                        lbltongcong.Text = tonghoadon.ToString("N0");



                    }

                }
                else
                {
                    if (int.Parse(nmsanpham.Value.ToString()) > SanPhamDAO.Instance.CheckKhoHang(cbbsanpham.Text, cbbMauSP.Text))
                    {
                        MessageBox.Show("Kho đã hết hàng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem(txbName.Text);
                        item.SubItems.Add(txbsdt.Text);
                        item.SubItems.Add(txbdiachi.Text);
                        item.SubItems.Add(cbbsanpham.Text);
                        item.SubItems.Add(cbbMauSP.Text);
                        item.SubItems.Add(nmsanpham.Text);
                        item.SubItems.Add(cbbchietkhau.Text);
                        double tongtien = double.Parse(nmsanpham.Value.ToString()) * 0.01 * (100 - double.Parse(cbbchietkhau.Text.Remove(cbbchietkhau.Text.Length - 1))) * double.Parse(nmtongtien.Value.ToString());
                        item.SubItems.Add(tongtien.ToString("N0"));
                        lsvhoadon.Items.Add(item);
                        tonghoadon += tongtien;
                        lbltongcong.Text = tonghoadon.ToString("N0");
                    }
                }

            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            for (int i = lsvhoadon.SelectedItems.Count - 1; i >= 0; i--)
            {
                lsvhoadon.Items.Remove(lsvhoadon.SelectedItems[i]);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (lbltongcong.Text == "")
            {
                MessageBox.Show("Bạn chưa bấm Tổng Cộng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                bool check = KhachHangDAO.Instance.checkKHCSDL(txbsdt.Text); // Check xem Kh da ton tai trong csdl chua
                if (check == false)
                {
                    //Tạo khách hàng, và thêm khách hàng vào CSDL
                    KhachHang kh = new KhachHang(txbName.Text, txbdiachi.Text, txbsdt.Text, txblinkfacebook.Text);
                    KhachHangDAO.Instance.LoadKHLenCSDL(kh);
                    //Tạo hóa đơn và lấy id hóa đơn, để gán vào thông tin hóa đơn, thêm hóa đơn vào CSDL
                    Hoadon hd = new Hoadon(0, DateTime.Now, txbsdt.Text, "Chưa Giao Hàng", double.Parse(lbltongcong.Text));
                    HoaDonDAO.Instance.LoadHoaDonLenCSDL(hd);
                    //Xử lý thêm thông tin hóa đơn 
                    foreach (ListViewItem item in lsvhoadon.Items)
                    {

                        // Bước 1: Lấy id sản phẩm
                        int idhd1 = HoaDonDAO.Instance.GetidhdbySDTanddate(txbsdt.Text, DateTime.Now);
                        int idsp = SanPhamDAO.Instance.getIdbyNameAndColor(item.SubItems[3].Text, item.SubItems[4].Text);

                        ThongTinHoaDon tthd = new ThongTinHoaDon(idhd1, idsp, int.Parse(item.SubItems[5].Text));
                        ThongTinHDDAO.Instance.LoadTTHDtoCSDL(tthd);


                    }
                    MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {


                    // Tạo hóa đơn và lấy id hóa đơn, để gán vào thông tin hóa đơn, thêm hóa đơn vào CSDL


                    Hoadon hd = new Hoadon(0, DateTime.Now, txbsdt.Text, "Chưa Giao Hàng", double.Parse(lbltongcong.Text));
                    HoaDonDAO.Instance.LoadHoaDonLenCSDL(hd);
                    // Xử lý thêm thông tin hóa đơn

                    foreach (ListViewItem item in lsvhoadon.Items)
                    {
                        // Bước 1: Lấy id sản phẩm
                        int idhd1 = HoaDonDAO.Instance.GetidhdbySDTanddate(txbsdt.Text, DateTime.Now);
                        int idsp = SanPhamDAO.Instance.getIdbyNameAndColor(item.SubItems[3].Text, item.SubItems[4].Text);

                        ThongTinHoaDon tthd = new ThongTinHoaDon(idhd1, idsp, int.Parse(item.SubItems[5].Text));
                        ThongTinHDDAO.Instance.LoadTTHDtoCSDL(tthd);
                    }
                    // Cuối cùng, xóa hết dữ liệu, để nhập dữ liệu mới
                    MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }

                txbName.Clear();
                txbdiachi.Clear();
                txbsdt.Clear();
                nmtongtien.Value = 0;
                txbName.Focus();
                cbbsanpham.SelectedIndex = 0;
                nmsanpham.Value = 1;
                lbltongcong.Text = "";
                lsvhoadon.Items.Clear();
            }

        }


        private void btnTongcong_Click(object sender, EventArgs e)
        {
            double sum = 0;
            foreach (ListViewItem item in lsvhoadon.Items)
            {
                sum += double.Parse(item.SubItems[6].Text);
            }

            lbltongcong.Text = sum.ToString("N0");
        }

        #endregion

        private void txbsdt_KeyUp(object sender, KeyEventArgs e)
        {
            DataTable dt = KhachHangDAO.Instance.getkhsdt(txbsdt.Text);
            KhachHang kh = new KhachHang();
            foreach (DataRow item in dt.Rows)
            {
                kh = new KhachHang(item);
            }
            txbName.Text = kh.Name;
            txbdiachi.Text = kh.Diachi;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnclear_Click(this, new EventArgs());
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image image = Resources._16aabf2401dfe581bcce;

            e.Graphics.DrawImage(image, 75, -30, 700, 450);
            e.Graphics.DrawString("Ngày mua hàng : " + DateTime.Now.ToString("dd/MM/yyyy"), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, 210));
            e.Graphics.DrawString("Khách hàng : " + txbName.Text.Trim(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, 240));
            e.Graphics.DrawString("Số điện thoại : " + txbsdt.Text.Trim(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, 270));
            e.Graphics.DrawString("Địa chỉ : " + txbdiachi.Text.Trim(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, 300));
            e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, 330));
            e.Graphics.DrawString("Sản Phẩm", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, 350));
            e.Graphics.DrawString("Màu Sắc", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(230, 350));
            e.Graphics.DrawString("Số Lượng", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(400, 350));
            e.Graphics.DrawString("Chiết Khấu", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(540, 350));
            e.Graphics.DrawString("Giá Tiền", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(700, 350));
            e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, 370));
            int x = 20;
            int y = 400;
            if (lsvhoadon.Items.Count > 0)
            {
                foreach (ListViewItem item in lsvhoadon.Items)
                {
                    e.Graphics.DrawString(item.SubItems[3].Text.ToString(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(x, y));
                    e.Graphics.DrawString(item.SubItems[4].Text.ToString(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(x + 220, y));
                    e.Graphics.DrawString(item.SubItems[5].Text.ToString(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(x + 410, y));
                    e.Graphics.DrawString(item.SubItems[6].Text.ToString(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(x + 545, y));
                    e.Graphics.DrawString(item.SubItems[7].Text.ToString(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(x + 680, y));
                    y += 30;
                }
            }
            e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(20, y));
            e.Graphics.DrawString("Tổng cộng : " + lbltongcong.Text, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(630, y + 30));

        }

        private void btnpreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        //internal  object passSender;
        //internal  EventArgs passE;

        internal void FMain_Load(object sender, EventArgs e)
        {
            //    Load += FMain_Load;
            //    List<SanPham> listsp1 = new List<SanPham>();
            //    DataTable dt1 = DataProvider.Instance.ExecureQuery("EXEC dbo.USP_GetListSanPham");
            //    foreach (DataRow item in dt1.Rows)
            //    {
            //        SanPham sp = new SanPham(item);
            //        listsp1.Add(sp);
            //    }
            //    passSender = sender;
            //    passE = e;
            //    cbbsanpham.Items.Clear();
            //    cbbMauSP.Items.Clear();
            //    foreach (SanPham item in listsp1)
            //    {
            //        bool check = true;
            //        bool check2 = true;
            //        for (int i = 0; i < cbbsanpham.Items.Count; i++)
            //        {
            //            if (cbbsanpham.Items[i].ToString() == item.Name)
            //            {
            //                check2 = false;
            //            }

            //        }
            //        for (int i = 0; i < cbbMauSP.Items.Count; i++)
            //        {
            //            if (cbbMauSP.Items[i].ToString() == item.Color)
            //            {
            //                check = false;
            //            }

            //        }
            //        if (check == true)
            //        {
            //            cbbMauSP.Items.Add(item.Color);
            //        }
            //        if (check2 == true)
            //        {
            //            cbbsanpham.Items.Add(item.Name);
            //        }

            //    }
            //    cbbMauSP.SelectedIndex = 0;
            //    cbbsanpham.SelectedIndex = 0;
            //}

            //private void FMain_Load1(object sender, EventArgs e)
            //{
            //    throw new NotImplementedException();
            //}
        }
    }
}
