using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DTO
{
    class SanPham
    {
        public SanPham(int id, string name, string color, string loaisp, double price, int soluong)
        {
            this.iD = id;
            this.name = name;
            this.Color = color;
            this.loaisp = loaisp;
            this.price = price;
            this.soluong = soluong;          
        }

        public SanPham()
        {

        }

        public SanPham(DataRow row)
        {
            this.iD = (int)row["id"]; // kiểu dữ liệu của row trả về là object, nên ép kiểu nào cũng đc
            this.name = row["name"].ToString();
            this.Color = row["color"].ToString(); 
            this.loaisp = row["Typesp"].ToString();
            this.price = (double)row["Price"];
            
        }


        private int iD;

        public int ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
            }
        }

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

        public string Loaisp
        {
            get
            {
                return loaisp;
            }

            set
            {
                loaisp = value;
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

        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
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

        private string name;
        private string loaisp;
        private double price;
        private string color;
        private int soluong;
    }
}
