using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DTO
{
    class Hoadon
    {
        private int id;
        private DateTime Ngayxuat;
        private string Trangthai; // 0 là chưa giao hàng, 1 là đã giao hàng
        private string sdtkh;
        private double price;
        public Hoadon()
        {

        }

        public Hoadon(int id, DateTime ngayxuat,string sdtkh, string trangthai, double price)
        {
            this.id = id;
            this.Ngayxuat = ngayxuat;
            this.Trangthai2 = trangthai;
            this.Sdtkh = sdtkh;
            this.price = price;
        }

        public Hoadon(DataRow row)
        {
            this.id = (int)row["id"];
            this.Ngayxuat = (DateTime)row["NgayXuat"];
            this.Sdtkh= row["SDTkh"].ToString();
            this.Trangthai2 = row["STATUS"].ToString();
            this.price = (double)row["price"];
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public DateTime Ngayxuat1
        {
            get
            {
                return Ngayxuat;
            }

            set
            {
                Ngayxuat = value;
            }
        }

       
   

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }



        public string Trangthai2
        {
            get
            {
                return Trangthai;
            }

            set
            {
                Trangthai = value;
            }
        }

        public string Sdtkh
        {
            get
            {
                return sdtkh;
            }

            set
            {
                sdtkh = value;
            }
        }
    }

}
