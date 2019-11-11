using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private AccountDAO() { }

        public bool CheckLogin(string username, string password)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password); // Lấy ra 1 mảng kiểu byte từ chuỗi
            byte[] hasdata = new MD5CryptoServiceProvider().ComputeHash(temp); // Hash theo mã của máy tính, băm cái pass ra 1 đống phần tử          
            // Chuyển cái chuỗi byte[] băm vừa được thành chuỗi, để chuyển xuống CSDL
            string haspass = ""; // string này là password sau khi mã hóa
            foreach(byte item in hasdata)
            {
                haspass += item;
            }

            // Nếu muốn tăng độ bảo mật, thì đảo ngươc chuỗi bằng cách
            var list = hasdata.ToString(); // Chuyển thành chuỗi
            list.Reverse(); // Đảo ngược lại ví dụ ABC => CBA . Xong sau đó đưa cái list vừa đảo ngược cho vào passworđ
           
            string query = "EXEC dbo.USP_Login @username , @password"; // Sử dụng store procedure để tránh sql injection
            
            DataTable kiemtra = DataProvider.Instance.ExecureQuery(query, new object[]{ username, haspass });
            
            return kiemtra.Rows.Count > 0;
        }

        public void updateAccount(string username ,string displayname, string password)
        {
            // Mã hóa mật khẩu
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password); // Lấy ra 1 mảng kiểu byte từ chuỗi
            byte[] hasdata = new MD5CryptoServiceProvider().ComputeHash(temp);
            // Chuyển cái chuỗi byte[] băm vừa được thành chuỗi, để chuyển xuống CSDL
            string haspass = "";
            foreach (byte item in hasdata)
            {
                haspass += item;
            }
            string query = "EXEC dbo.USB_UpdateAccount @username , @displayname , @password ";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { username, displayname, haspass });

        }

        public int gettypeAccByUsername(string username)
        {
            int type = -1;
            if(username != "")
            {
                string query = "EXEC dbo.USB_GetTypeAccByUsername @username ";
                DataTable dt = DataProvider.Instance.ExecureQuery(query, new object[] { username });
                if(dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        type = (int)item["type"];
                    }
                }     
            }
            return type;
        }

        public void AddAccount(string username, string displayname, string password , int type)
        {
            // Mã hóa mật khẩu
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password); // Lấy ra 1 mảng kiểu byte từ chuỗi
            byte[] hasdata = new MD5CryptoServiceProvider().ComputeHash(temp);
            // Chuyển cái chuỗi byte[] băm vừa được thành chuỗi, để chuyển xuống CSDL
            string haspass = "";
            foreach (byte item in hasdata)
            {
                haspass += item;
            }
            string query = "EXEC dbo.USB_AddCount @username , @displayname , @password , @type ";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { username, displayname, haspass, type });
        }

        public void DeleteAccount(string username)
        {
            string query = "EXEC dbo.USB_DeleteAccountByUsername @username";
            DataProvider.Instance.ExecureNonQuery(query, new object[] { username });
        }
    }
}
