using FonNature.DAO;
using FonNature.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FonNature
{
    public partial class FAdmin : Form
    {
        public FAdmin()
        {
            InitializeComponent();
            LOADAccount();
            LOADSanPham();
            LoadHoaDon();
            LoadKhachHang();
        }

       
        #region method
        private void LOADSanPham()
        {
            string query = @"SELECT * FROM dbo.SanPham";
            DataTable dt = DAO.DataProvider.Instance.ExecureQuery(query);
            dgvDSSanpham.DataSource = dt;
            dgvDSSanpham.Columns[4].DefaultCellStyle.Format = "N0";
            dgvDSSanpham.Columns[0].HeaderText = "ID";
            dgvDSSanpham.Columns[1].HeaderText = "Tên Sản Phẩm";
            dgvDSSanpham.Columns[2].HeaderText = "Màu Sản Phẩm";
            dgvDSSanpham.Columns[3].HeaderText = "Loại sản phẩm";
            dgvDSSanpham.Columns[4].HeaderText = "Giá sản phẩm";
            dgvDSSanpham.Columns[5].HeaderText = "Số lượng";
        }

        private void LOADAccount()
        {

            string query = "SELECT loginname,displayname,type FROM dbo.Account";
            DataTable dt = DataProvider.Instance.ExecureQuery(query);
            dgvAccount.DataSource = dt;
            dgvAccount.Columns[0].HeaderText = "Tên Đăng Nhập";
            dgvAccount.Columns[1].HeaderText = "Tên Hiển Thị";
            dgvAccount.Columns[2].HeaderText = "Loại Tài Khoản";

            cbbTypeACC.Items.Add("Nhân viên");
            cbbTypeACC.Items.Add("Admin");
            cbbTypeACC.SelectedIndex = 0;

            // Ví dụ : new object[]{"minhphuoc","thuyvan"}         
        }
        private void LoadHoaDon()
        {
                   
            if(dpendday.Value.Month == 1)
            {
                int monthstartday = 12;
                int daystartdayy = dpendday.Value.Day;
                int yearstartday = dpendday.Value.Year -1;
                DateTime starday = new DateTime(yearstartday, monthstartday, daystartdayy);
                dpStartday.Value = starday;
            }
            else
            {
                int monthstartday = dpendday.Value.Month - 1;
                int daystartdayy = dpendday.Value.Day;
                int yearstartday = dpendday.Value.Year;
                DateTime starday = new DateTime(yearstartday, monthstartday, daystartdayy);
                dpStartday.Value = starday; 
            }
            

            button1_Click(this, new EventArgs());



        }
        private void LoadKhachHang()
        {
            DataTable dt = KhachHangDAO.Instance.GetKhFromCSDL();
            dgvKhachHang.DataSource = dt;
            dgvKhachHang.Columns[0].HeaderText = "Số Điện Thoại";
            dgvKhachHang.Columns[1].HeaderText = "Tên Khách Hàng";
            dgvKhachHang.Columns[2].HeaderText = "Địa Chỉ Khách Hàng";
        }

        

        #endregion

        #region hoadon
        private void btngiaohang_Click(object sender, EventArgs e)
        {
            int rowindex = dgvHoaDon.CurrentRow.Index;
            if (dgvHoaDon.CurrentCell.ColumnIndex == 6)
            {
                if (dgvHoaDon.CurrentCell.Value.ToString() == "Chưa Giao Hàng")
                {
                    dgvHoaDon.CurrentCell.Value = "Đang Giao Hàng";
                    int idhd = int.Parse(dgvHoaDon.Rows[rowindex].Cells[0].Value.ToString());
                    DataTable dt = ThongTinHDDAO.Instance.GetTableforKho(idhd);
                    List<ThongTinHoaDon> listtthd = new List<ThongTinHoaDon>();


                    foreach (DataRow item in dt.Rows)
                    {
                        ThongTinHoaDon tthd = new ThongTinHoaDon(item);
                        listtthd.Add(tthd);
                    }

                    // Lấy ra các id sp và update
                    for (int i = 0; i < listtthd.Count; i++)
                    {
                        SanPhamDAO.Instance.Updatekhohang(listtthd[i].Idsp, listtthd[i].Soluong);
                    }

                    //Cập nhật lại dgvsanpham
                    


                }
                else
                {
                    dgvHoaDon.CurrentCell.Value = "Đã Giao Hàng";
                    
                }

            }
            int id = int.Parse(dgvHoaDon.Rows[rowindex].Cells[0].Value.ToString());
            HoaDonDAO.Instance.xacnhangiaohang(dgvHoaDon.CurrentCell.Value.ToString(), id);
        }

        private void dgvHoaDon_Click(object sender, EventArgs e)
        {
            int id = 0;

            int indexrows = dgvHoaDon.CurrentRow.Index;
            id = int.Parse(dgvHoaDon.Rows[indexrows].Cells[0].Value.ToString());


            if (id != 0)
            {
                DataTable dt = ThongTinHDDAO.Instance.GetTTHDbyID(id);
                dgvchitiethd.DataSource = dt;
            }
        }


        private void txbsearchhd_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbsearchhd.Text != "")
            {
                DataTable dt = HoaDonDAO.Instance.SearchHdBySDT("%" + txbsearchhd.Text + "%");
                dgvHoaDon.DataSource = dt;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DateTime starday = dpStartday.Value;
            DateTime endday = dpendday.Value;
            DataTable dt = HoaDonDAO.Instance.Gethdbydateandpage(starday, endday,int.Parse(txbpage.Text));

            dgvHoaDon.DataSource = dt;
            dgvHoaDon.Columns[4].DefaultCellStyle.Format = "N0";
        }

        private void btnDoanhthu_Click(object sender, EventArgs e)
        {
            double doanhthu = 0;
            for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
            {
                if(dgvHoaDon.Rows[i].Cells[6].Value.ToString() == "Đã Giao Hàng" || dgvHoaDon.Rows[i].Cells[6].Value.ToString() == "Đang Giao Hàng")
                {
                    double tien = double.Parse(dgvHoaDon.Rows[i].Cells[4].Value.ToString());
                    doanhthu += tien;
                }                            
            }
            MessageBox.Show("Doanh Thu = " + doanhthu.ToString("N0"));
        }

        private void dpendday_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(this, new EventArgs());
        }

        private void dpStartday_ValueChanged(object sender, EventArgs e)
        {
            button1_Click(this, new EventArgs());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            txbpage.Text = "1";
        }

        private void txbpage_TextChanged(object sender, EventArgs e)
        {
            button1_Click(this, new EventArgs());
        }

        private void btnLastpage_Click(object sender, EventArgs e)
        {
            int sumrecord = HoaDonDAO.Instance.GetCountHdByDate(dpStartday.Value, dpendday.Value);
            int lastpage = sumrecord / 20;
            if (sumrecord % 20 != 0)
                lastpage++;

            txbpage.Text = lastpage.ToString();

        }

        private void btnNextpage_Click(object sender, EventArgs e)
        {
            int sumrecord = HoaDonDAO.Instance.GetCountHdByDate(dpStartday.Value, dpendday.Value);
            int lastpage = sumrecord / 20;
            if (sumrecord % 20 != 0)
                lastpage++;
            int page = int.Parse(txbpage.Text);

            if (page < lastpage)
            {
                page++;
                txbpage.Text = page.ToString();
            }

        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {
            int page = int.Parse(txbpage.Text);
            if (page > 1)
            {
                page--;
                txbpage.Text = page.ToString();
            }
        }

        #endregion

        #region sanpham
        private void btnAddSP_Click(object sender, EventArgs e)
        {
            if (txbTenSanPham.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txbColorSP.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập màu sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txbloaisp.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập loại sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (nmsoluongsp.Value == 0)
            {
                MessageBox.Show("Bạn chưa nhập số lượng sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (SanPhamDAO.Instance.checkSPCSDL(txbTenSanPham.Text, txbColorSP.Text) == false)
                {
                    SanPham sp = new SanPham(0, txbTenSanPham.Text, txbColorSP.Text, txbloaisp.Text, double.Parse(nmgiale.Value.ToString()), int.Parse(nmsoluongsp.Value.ToString()));
                    SanPhamDAO.Instance.LoadSanPhamlenCSDL(sp);
                    LOADSanPham();
                    txbIDSP.Text = "";
                    txbTenSanPham.Text = "";
                    txbColorSP.Text = "";
                    txbloaisp.Text = "";
                    nmsoluongsp.Value = 0;
                    nmgiale.Value = 0;

                }
                else MessageBox.Show("Sản phẩm đã tồn tại trong cơ sở dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnDeleteSP_Click(object sender, EventArgs e)
        {
            if (dgvDSSanpham.Rows.Count > 0)
            {
                int index = dgvDSSanpham.CurrentRow.Index;
                int idsp = int.Parse(dgvDSSanpham.Rows[index].Cells[0].Value.ToString());
                string ten = dgvDSSanpham.Rows[index].Cells[1].Value.ToString();
                string color = dgvDSSanpham.Rows[index].Cells[2].Value.ToString();

                if (MessageBox.Show("Bạn có thực sự muốn xóa sản phẩm : " + ten + " , màu : " + color + " không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SanPhamDAO.Instance.DeleteSPbyID(idsp);
                    LOADSanPham();
                }
            }


        }

        private void btnupdateSP_Click(object sender, EventArgs e)
        {
            if (txbIDSP.Text != "")
            {
                SanPhamDAO.Instance.UpdateSPCSDL(int.Parse(txbIDSP.Text), txbTenSanPham.Text, txbColorSP.Text, txbloaisp.Text, double.Parse(nmgiale.Value.ToString()), int.Parse(nmsoluongsp.Value.ToString()));
                LOADSanPham();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvDSSanpham_Click(object sender, EventArgs e)
        {
            int index = dgvDSSanpham.CurrentRow.Index;

            txbIDSP.Text = dgvDSSanpham.Rows[index].Cells[0].Value.ToString();
            txbTenSanPham.Text = dgvDSSanpham.Rows[index].Cells[1].Value.ToString();
            txbColorSP.Text = dgvDSSanpham.Rows[index].Cells[2].Value.ToString();
            txbloaisp.Text = dgvDSSanpham.Rows[index].Cells[3].Value.ToString();
            nmgiale.Value = int.Parse(dgvDSSanpham.Rows[index].Cells[4].Value.ToString());
            nmsoluongsp.Value = int.Parse(dgvDSSanpham.Rows[index].Cells[5].Value.ToString());
        }

        private void btnResetSP_Click(object sender, EventArgs e)
        {
            txbIDSP.Text = "";
            txbTenSanPham.Text = "";
            txbColorSP.Text = "";
            txbloaisp.Text = "";
            nmgiale.Value = 0;
            nmsoluongsp.Value = 0;
        }


        private void txbSearchSP_KeyUp(object sender, KeyEventArgs e)
        {
            DataTable dt = SanPhamDAO.Instance.GetDataSPbyName("%" + txbSearchSP.Text + "%");
            dgvDSSanpham.DataSource = dt;
            dgvDSSanpham.Columns[4].DefaultCellStyle.Format = "N0";
            dgvDSSanpham.Columns[0].HeaderText = "ID";
            dgvDSSanpham.Columns[1].HeaderText = "Tên Sản Phẩm";
            dgvDSSanpham.Columns[2].HeaderText = "Màu Sản Phẩm";
            dgvDSSanpham.Columns[3].HeaderText = "Loại sản phẩm";
            dgvDSSanpham.Columns[4].HeaderText = "Giá sản phẩm";
            dgvDSSanpham.Columns[5].HeaderText = "Số lượng";
        }
        #endregion

        #region khachhang
        private void BtnThemKH_Click(object sender, EventArgs e)
        {


            if (txbTenKH.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txbDiaChiKH.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txbSDTKH.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (KhachHangDAO.Instance.checkKHCSDL(txbSDTKH.Text) == false && KhachHangDAO.Instance.checkNameKH(txbTenKH.Text) == false)
                {
                    KhachHang kh = new KhachHang(txbTenKH.Text, txbDiaChiKH.Text, txbSDTKH.Text,txblinkfacebook.Text);
                    KhachHangDAO.Instance.LoadKHLenCSDL(kh);

                    LoadKhachHang();

                    txbSDTKH.Text = "";
                    txbDiaChiKH.Text = "";
                    txbSDTKH.Text = "";


                }
                else MessageBox.Show("Khách Hàng đã tồn tại trong cơ sở dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void BtnXoaKh_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.Rows.Count > 0)
            {
                int index = dgvKhachHang.CurrentRow.Index;
                string sdt = dgvKhachHang.Rows[index].Cells[0].Value.ToString();
                string ten = dgvKhachHang.Rows[index].Cells[1].Value.ToString();


                if (MessageBox.Show("Bạn có thực sự muốn xóa khách hàng : " + ten + " , SĐT : " + sdt + " không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    KhachHangDAO.Instance.DeleteKhbySDT(sdt);
                    LoadKhachHang();
                }
            }
        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            if (txbSDTKH.Text != "")
            {
                // SDT đã có trong CSDL, Chỉnh sửa tên và địa chỉ
                if (KhachHangDAO.Instance.checkKHCSDL(txbSDTKH.Text))
                {
                    KhachHangDAO.Instance.updateKH(txbSDTKH.Text, txbTenKH.Text, txbDiaChiKH.Text, txblinkfacebook.Text);
                    LoadKhachHang();
                }
                // SDT không có trong CSDL
                else
                {
                    // Xem thử tên có trong CSDL không, có mới xử lí
                    if (KhachHangDAO.Instance.checkNameKH(txbTenKH.Text))
                    {
                        // Lấy sdt cũ trong CSDL
                        string sdtold = KhachHangDAO.Instance.getsdtByname(txbTenKH.Text);

                        // Thêm khách hàng gồm sdt mới vào CSDL
                        KhachHang kh = new KhachHang(txbTenKH.Text, txbDiaChiKH.Text, txbSDTKH.Text, txblinkfacebook.Text);
                        KhachHangDAO.Instance.LoadKHLenCSDL(kh);

                        //Chính sửa SDT mới vào Hóa đơn thay cho sdt cũ
                        HoaDonDAO.Instance.UpdateSDTHD(txbSDTKH.Text, sdtold);

                        //Xóa Khách hàng có SDT cũ
                        KhachHangDAO.Instance.DeleteKhbySDT(sdtold);

                        // Cập nhật lại DataGridView
                        LoadKhachHang();

                    }
                    // Tên không có thì yêu cầu nhập Khách Hàng mới
                    else
                    {
                        MessageBox.Show("Hãy nhập khách hàng mới, Vì SĐT và Tên Khách không tồn tại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }



                    

                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvKhachHang_Click(object sender, EventArgs e)
        {
            int index = dgvKhachHang.CurrentRow.Index;

            txbSDTKH.Text = dgvKhachHang.Rows[index].Cells[0].Value.ToString();
            txbTenKH.Text = dgvKhachHang.Rows[index].Cells[1].Value.ToString();
            txbDiaChiKH.Text = dgvKhachHang.Rows[index].Cells[2].Value.ToString();
         
        }

        private void btnRsKH_Click(object sender, EventArgs e)
        {
            txbTenKH.Text = "";
            txbSDTKH.Text = "";
            txbDiaChiKH.Text = "";
            txblinkfacebook.Text = "";
        }

        private void txbSearchKH_KeyUp(object sender, KeyEventArgs e)
        {
            DataTable dt = KhachHangDAO.Instance.GetDataKhbysdt("%" + txbSearchKH.Text + "%");
            dgvKhachHang.DataSource = dt;
            dgvKhachHang.Columns[0].HeaderText = "Số Điện thoại";
            dgvKhachHang.Columns[1].HeaderText = "Tên Khách Hàng";
            dgvKhachHang.Columns[2].HeaderText = "Địa chỉ Khách Hàng";
        }
        #endregion

        #region Account
        private void btnAddACC_Click(object sender, EventArgs e)
        {
            if(txbusername.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txbDisplayAcc.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên hiển thị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txbPassACC.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(AccountDAO.Instance.gettypeAccByUsername(txbusername.Text) == -1)
                {
                    int type;
                    if (cbbTypeACC.Text == "Admin") type = 1;
                    else type = 0;
                    AccountDAO.Instance.AddAccount(txbusername.Text, txbDisplayAcc.Text, txbPassACC.Text, type);
                    MessageBox.Show("Thêm Thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LOADAccount();
                }
            }
        }

        private void btnDeleteACC_Click(object sender, EventArgs e)
        {
            int index = dgvAccount.CurrentRow.Index;
            if(MessageBox.Show("Bạn có chắc muốn xóa không ?","Thông báo", MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (dgvAccount.Rows[index].Cells[0].Value.ToString() != "minhphuoc")
                {
                    AccountDAO.Instance.DeleteAccount(dgvAccount.Rows[index].Cells[0].Value.ToString());
                    LOADAccount();
                }
                else
                {
                    MessageBox.Show("Không được xóa tài khoản này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
               
        }

        private void btnResetACC_Click(object sender, EventArgs e)
        {
            txbusername.Text = "";
            txbPassACC.Text = "";
            txbDisplayAcc.Text = "";
            cbbTypeACC.SelectedIndex = 0;
        }




        #endregion

        private void FAdmin_Load(object sender, EventArgs e)
        {

        }

        private void FAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {

            //FMain fMain = new FMain();
            //fMain.FMain_Load(fMain.passSender, fMain.passE);
        }
    }
}
