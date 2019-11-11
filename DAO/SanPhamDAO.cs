using FonNature.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DAO
{
    class SanPhamDAO
    {
        private static SanPhamDAO instance;

        internal static SanPhamDAO Instance
        {
            get
            {
                if (instance == null) instance = new SanPhamDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private SanPhamDAO()
        { }

        public void LoadSanPhamlenCSDL(SanPham sp)
        {
            string query1 = "EXEC dbo.USB_GetListSanPhamByNameAndColor @name , @color ";
            DataTable dt = DataProvider.Instance.ExecureQuery(query1, new object[] { sp.Name, sp.Color });
            if(dt.Rows.Count  == 0)
            {
                string query = "EXEC dbo.USB_LoadSP @name , @color , @typesp , @price , @soluong";
                DataProvider.Instance.ExecureNonQuery(query, new object[] { sp.Name, sp.Color, sp.Loaisp, sp.Price, sp.Soluong });
            }

        }

        public bool checkSPCSDL(string name, string color)
        {
            string query = "EXEC dbo.USB_GetListSanPhamByNameAndColor @name , @color ";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { name, color });

            return dt.Rows.Count > 0;
        }

        public int getIdbyNameAndColor(string name, string color)
        {
            int id=0;
            string query = "EXEC dbo.USB_GetListSanPhamByNameAndColor @name , @color ";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { name, color });
            
            List<SanPham> listsp = new List<SanPham>();
            foreach(DataRow item in dt.Rows)
            {
                SanPham sp = new SanPham(item);
                listsp.Add(sp);
            }
           
            foreach(SanPham item in listsp)
            {
                if(item.Name  == name && item.Color == color)
                {
                    id = item.ID;
                }
            }
            return id;
        }
        
        public int CheckKhoHang(string name, string color)
        {
            int id = getIdbyNameAndColor(name, color);
            int soluong = 0;
            string query = "EXEC dbo.USB_GetSPbyID @id";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { id });
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    soluong = (int)row["soluong"];
                }
            }
            return soluong;          

        }

        public DataTable LoadSP()
        {
            string query = @"SELECT * FROM dbo.SanPham";
            DataTable dt = DAO.DataProvider.Instance.ExecureQuery(query);
            return dt;
        }

        public void DeleteSPbyID(int id)
        {
            string query = "EXEC dbo.USB_DeleteSPbyID @id";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { id });                    
        }

        public void UpdateSPCSDL(int id, string name, string color, string typesp, double giale, int soluong)
        {
            string query = "EXEC dbo.USB_UpdateSPCSDL @id , @name , @color , @typesp , @price , @soluong";
            DataProvider.Instance.ExecureNonQuery(query, new object[] {id, name, color, typesp, giale , soluong});
        }

        public DataTable GetDataSPbyName(string name)
        {
            string query = "EXEC dbo.USB_GetDataSPByName @name";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { name });
            return dt;
        }

        public void Updatekhohang(int id, int soluong)
        {
            string query = "EXEC dbo.USB_UpdateKho @id , @soluong";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { id, soluong });
        }


    }
}
