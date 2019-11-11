using FonNature.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DAO
{
    class ThongTinHDDAO
    {
        private static ThongTinHDDAO instance;

        internal static ThongTinHDDAO Instance
        {
            get
            {
                if (instance == null) instance = new ThongTinHDDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private ThongTinHDDAO()
        {

        }

        public void LoadTTHDtoCSDL(ThongTinHoaDon tthd)
        {
            string query = "EXEC dbo.USB_LoadTTHD @idhd , @idsp , @soluong ";
            DataProvider.Instance.ExecureNonQuery(query, new object[] {tthd.Idhd , tthd.Idsp , tthd.Soluong });
        }

        public DataTable GetTTHDbyID(int id)
        {
            string query = "EXEC dbo.USB_GetTTHDbyID @id ";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { id });
            return dt;
        }

        public DataTable GetTableforKho(int id)
        {
            string query = "EXEC dbo.USB_GetTableForKho @id";
            DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { id });
            return dt;
        }

        
    }
}
