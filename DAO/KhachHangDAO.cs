using FonNature.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DAO
{
    class KhachHangDAO
    {
        private static KhachHangDAO instance;

        internal static KhachHangDAO Instance
        {
            get
            {
                if (instance == null) instance = new KhachHangDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private KhachHangDAO() { }


        public void LoadKHLenCSDL(KhachHang kh)
        {
            string queryKH = "EXEC dbo.USB_GetKhBySDT @sdt";

            DataTable dt = DataProvider.Instance.ExecureQuery(queryKH, new object[] { kh.Sdt });
            if (dt.Rows.Count == 0)
            {
                string query = "EXEC dbo.USB_LoadKH @ten , @diachi , @sdt , @LinkFaceBook";
                DataProvider.Instance.ExecureNonQuery(query, new object[] { kh.Name, kh.Diachi, kh.Sdt , kh.Linkfacebook1});

            }
      
        }

        public bool checkKHCSDL(string sdt)
        {
            string queryKH = "EXEC dbo.USB_GetKhBySDT @sdt";

            DataTable dt = DataProvider.Instance.ExecureQuery(queryKH, new object[] { sdt });

            return dt.Rows.Count > 0; 
        }              
        public List<KhachHang> LoadKHlist()
        {
            List<KhachHang> listkh = new List<KhachHang>();

            string query = "SELECT * FROM dbo.KhachHang";

            DataTable dt = DataProvider.Instance.ExecureQuery(query);

            foreach(DataRow item in dt.Rows)
            {
                KhachHang kh = new KhachHang(item);
                listkh.Add(kh);
            }
            return listkh;
        }

        public DataTable GetKhFromCSDL()
        {
            string query = "SELECT * FROM dbo.KhachHang";
            DataTable dt = DataProvider.Instance.ExecureQuery(query);
            return dt;
        }

        public void DeleteKhbySDT(string sdt)
        {
            string query = "EXEC dbo.USB_DeleteKHbySDT @sdt";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { sdt });
        }

        public void updateKH(string sdt, string name, string diachi, string LinkFaceBook)
        {
            string query = "EXEC dbo.USB_UpdateKH @sdt , @name , @diachi , @LinkFaceBook";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { sdt, name, diachi , LinkFaceBook});
        }

        public bool checkNameKH(string name)
        {
            string queryKH = "EXEC dbo.USB_GetKHByName @name";

            DataTable dt = DataProvider.Instance.ExecureQuery(queryKH, new object[] { name });

            return dt.Rows.Count > 0;
        }

        public string getsdtByname(string name)
        {
            string sdt = "";
            string queryKH = "EXEC dbo.USB_GetKHByName @name";

            DataTable dt = DataProvider.Instance.ExecureQuery(queryKH, new object[] { name });
            List<KhachHang> listkh = new List<KhachHang>();

            foreach(DataRow item in dt.Rows)
            {
                KhachHang kh = new KhachHang(item);
                listkh.Add(kh);
            }

            foreach(KhachHang item in listkh)
            {
                if(item.Name == name)
                {
                    sdt = item.Sdt;
                }
            }


            return sdt;
        }

        public bool checkDiachiKH(string diachi)
        {
            string query = "EXEC dbo.USB_GetKhByDiaChi @diachi";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { diachi });

            return dt.Rows.Count > 0;
        }

        public DataTable GetDataKhbysdt(string name)
        {
            string query = "EXEC dbo.USB_GetdataKHbySDT @name";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { name });
            return dt;
        }

        public DataTable getkhsdt(string sdt)
        {
            string query = "EXEC dbo.USB_GetKhBySDT @sdt";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { sdt });
            return dt;
        }


    }
}
