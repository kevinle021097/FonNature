using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonNature.DAO
{
    public class DataProvider
    {
        // Design patern Singleton
        // Mục tiêu: Nếu mỗi lần muốn sử dụng data provider , ta lại tạo ra 1 biến data => Tạo ra rất nhiều connect 1 lúc.
        // Cách thức: Tạo 1 biến static thể hiện của lớp đó, thông qua biến đó ta gọi các hàm bên dưới 
        // Biến đó không thể thay đổi được được trạng thái, chỉ có thể gọi ra để thực hiện các hàm bên dưới.
        // Vì là biến static, nên giá trị sử dụng hết vòng đời chương trình

        private static DataProvider instance; // Tạo 1 thể hiện của lớp data provider

        private string ConnectionSTR = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLySonMoi;Integrated Security=True";

        public static DataProvider Instance 
        {
            get
            {
                if (instance == null) instance = new DataProvider(); // Nếu instanr = null thì mới khơi tạo connect
                return instance;
            }

            private set // Xét private cho set, để bên ngoài k thể chính sửa được
            {
                instance = value;
            }
        }

        private DataProvider() // Xét private cho hàm khởi tạo mặc định, để bên ngoài khỏi truy xuất vào để sửa
        {

        }
        
        public DataTable ExecureQuery(string query, object[] parameter = null) // Mảng kiểu dữ liệu nào cũng đc object[]                                                                              parameter
                                                                               // object[] parameter có thể Null
                                                                               // Gán default parameter = null, để có thể khỏi                                                 cần đưa biến vô -> tiện lợi hơn
        {

            DataTable dt = new DataTable();
            // Vì lỡ như đang chạy code mà, chưa chạy tới dòng close -> dễ lỗi, nên recommend sử dụng using
            // Sử dụng using thì nó tự giải phòng, dù thế nào thì nó cũng đi hết
            using (SqlConnection conn = new SqlConnection(ConnectionSTR))
            {
                
                conn.Open();                
                SqlCommand cmd= new SqlCommand(query, conn);               
                if(parameter != null) // Nếu thêm tên biến vào
                {
                    string[] listpra = query.Split(' '); // Tách cái query ra từng phàn ví dụ Exec - bla bla - @paramater ,..
                    int i = 0;
                    foreach (string item in listpra) // Tìm trong cái đám đã tách, đứa nào là @paramater
                    {
                        if(item.Contains("@")) // Nghĩa là thằng này là Parameter
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]); // Gán cho nó bằng cái paramater[i] ( tham số thứ i đã truyền vào ở trên)
                            i++; // Tăng i lên , nếu có6
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
            }
            return dt;          
        }

        public int  ExecureNonQuery(string query, object[] parameter = null) // Mảng kiểu dữ liệu nào cũng đc object[] parameter
                                                                               // object[] parameter có thể Null
                                                                               // Gán default parameter = null, để có thể khỏi cần đưa biến vô -> tiện lợi hơn
        {

           
            int soluong=0;
            // Vì lỡ như đang chạy code mà, chưa chạy tới dòng close -> dễ lỗi, nên recommend sử dụng using
            // Sử dụng using thì nó tự giải phòng, dù thế nào thì nó cũng đi hết
            using (SqlConnection conn = new SqlConnection(ConnectionSTR))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                if (parameter != null) // Nếu thêm tên biến vào
                {
                    string[] listpra = query.Split(' '); // Tách cái query ra từng phàn ví dụ Exec - bla bla - @paramater ,..
                    int i = 0;
                    foreach (string item in listpra) // Tìm trong cái đám đã tách, đứa nào là @parmater
                    {
                        if (item.Contains("@")) // Nghĩa là thằng này là Parameter
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]); // Gán cho nó bằng cái paramater[i] ( tham số thứ i đã truyền vào ở trên)
                            i++; // Tăng i lên , nếu có6
                        }
                    }
                }
                
                soluong = cmd.ExecuteNonQuery(); // hàm trả về số dòng thành công, đồng thời xử lí query Update, instert,..
                conn.Close();
            }
            return soluong;
        }

        public object ExecureScalar(string query, object[] parameter = null) // Mảng kiểu dữ liệu nào cũng đc object[] parameter
                                                                            // object[] parameter có thể Null
                                                                            // Gán default parameter = null, để có thể khỏi cần đưa biến vô -> tiện lợi hơn
        {

            
            object a=0;
            // Vì lỡ như đang chạy code mà, chưa chạy tới dòng close -> dễ lỗi, nên recommend sử dụng using
            // Sử dụng using thì nó tự giải phòng, dù thế nào thì nó cũng đi hết
            using (SqlConnection conn = new SqlConnection(ConnectionSTR))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                if (parameter != null) // Nếu thêm tên biến vào
                {
                    string[] listpra = query.Split(' '); // Tách cái query ra từng phàn ví dụ Exec - bla bla - @paramater ,..
                    int i = 0;
                    foreach (string item in listpra) // Tìm trong cái đám đã tách, đứa nào là @parmater
                    {
                        if (item.Contains("@")) // Nghĩa là thằng này là Parameter
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]); // Gán cho nó bằng cái paramater[i] ( tham số thứ i đã truyền vào ở trên)
                            i++; // Tăng i lên , nếu có6
                        }
                    }
                }

                a = cmd.ExecuteScalar(); // Hàm trả về những query như count, sum, avg ,...
                conn.Close();
            }
            return a;
        }
    }
}
