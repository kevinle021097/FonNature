using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DTO
{
    class KhachHang
    {
        public KhachHang(string name, string diachi, string sdt , string Linkfacebook)
        {
           
            this.name = name;
            this.diachi = diachi;
            this.sdt = sdt;
            this.Linkfacebook = Linkfacebook;
            
        }

        public KhachHang(DataRow row)
        {
           
            this.name = row["name"].ToString();
            this.diachi = row["diachi"].ToString();
            this.sdt = row["SDT"].ToString();
            this.Linkfacebook = row["LinkFaceBook"].ToString();
        }

        public KhachHang()
        {

        }
       
        private string name;
        private string diachi;
        private string sdt;
        private string Linkfacebook;
    

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Diachi
        {
            get
            {
                return diachi;
            }

            set
            {
                diachi = value;
            }
        }

        public string Sdt
        {
            get
            {
                return sdt;
            }

            set
            {
                sdt = value;
            }
        }

        public string Linkfacebook1
        {
            get
            {
                return Linkfacebook;
            }

            set
            {
                Linkfacebook = value;
            }
        }
    }
}
