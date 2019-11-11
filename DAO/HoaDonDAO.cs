using FonNature.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DAO
{
    class HoaDonDAO
    {
        private static HoaDonDAO instance;

        public static HoaDonDAO Instance
        {
            get
            {
                if (instance == null) instance = new HoaDonDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private HoaDonDAO()
        {
        }

        public void LoadHoaDonLenCSDL(Hoadon hd)
        {

            string queryhd = "EXEC dbo.USB_LoadDataHoaDon @idkh , @price";
            DataProvider.Instance.ExecureNonQuery(queryhd, new object[] { hd.Sdtkh, hd.Price });
        }

        public void xacnhangiaohang(string tinhtrang, int id)
        {
            string query = "EXEC dbo.USB_UpdateStatusHD @tinhtrang , @id";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { tinhtrang, id });
        }

        public DataTable gethdbydate(DateTime starday, DateTime endday)
        {
            string query = "EXEC dbo.USB_GetHoaDonTheoNgay @startday , @endday";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { starday, endday });
            return dt;
        }

        public int GetidhdbySDTanddate(string sdt, DateTime date)
        {
            int id = 0;
            string query = "EXEC dbo.USB_GetIDHDbyDateAndSDT @sdt , @date";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { sdt, date });
            if (dt.Rows.Count == 0)
            {
                return -1;
            }
            else
            {
                List<Hoadon> listhd = new List<Hoadon>();

                foreach (DataRow item in dt.Rows)
                {
                    Hoadon hd = new Hoadon(item);
                    listhd.Add(hd);
                }

                id = listhd[0].Id;

                for (int i = 0; i < listhd.Count; i++)
                {
                    if (listhd[i].Id > id)
                    {
                        id = listhd[i].Id;
                    }
                }

            }


            return id;
        }
        public int getIDHDLastest()
        {
            int id = 0;

            string query = "SELECT * FROM dbo.HoaDon";
            DataTable dt = DataProvider.Instance.ExecureQuery(query);

            List<Hoadon> listhd = new List<Hoadon>();

            if (dt.Rows.Count == 0)
            {
                return 1;
            }


            foreach (DataRow item in dt.Rows)
            {
                Hoadon hd = new Hoadon(item);
                listhd.Add(hd);
            }



            id = listhd[listhd.Count - 1].Id;

            return id;
        }

        public void UpdateSDTHD(string sdtnew, string sdtold)
        {
            if (KhachHangDAO.Instance.checkKHCSDL(sdtnew) == true)
            {
                string query = "EXEC dbo.USB_UpdateSDTHd @sdtnew , @sdtold";
                DataProvider.Instance.ExecureNonQuery(query, new object[] { sdtnew, sdtold });
            }

        }

        public DataTable SearchHdBySDT(string sdt)
        {
            string query = "EXEC dbo.USB_GetListHoaDonbySDT @sdt";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { sdt });
            return dt;
        }

        public DataTable Gethdbydateandpage(DateTime starday, DateTime endday, int page)
        {
            string query = "EXEC dbo.USB_GetHoaDonTheoNgayAndPage @startday , @endday , @page";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { starday, endday, page });
            return dt;
        }
        
        public int GetCountHdByDate(DateTime starday, DateTime endday)
        {
            string query = "EXEC dbo.USB_GetcountHoaDonTheoNgay @startday , @endday";
            return (int)DataProvider.Instance.ExecureScalar(query, new object[] { starday, endday});           
        }

    }
}
