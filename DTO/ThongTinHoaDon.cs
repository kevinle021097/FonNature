using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DTO
{
    class ThongTinHoaDon
    {
        private int idhd;
        private int soluong;
        private int idsp;

        public ThongTinHoaDon(int idhd, int idsp1, int soluong1)
        {
            this.Idhd = idhd;
            this.idsp = idsp1;
            this.soluong = soluong1;
        }

        public ThongTinHoaDon(DataRow row)
        {
            this.idhd = (int)row["idHoaDon"];
            this.idsp = (int)row["idSanpham"];
            this.soluong = (int)row["count"];
        }

       

        public int Soluong
        {
            get
            {
                return soluong;
            }

            set
            {
                soluong = value;
            }
        }

        public int Idsp
        {
            get
            {
                return idsp;
            }

            set
            {
                idsp = value;
            }
        }

        public int Idhd
        {
            get
            {
                return idhd;
            }

            set
            {
                idhd = value;
            }
        }
    }
}
