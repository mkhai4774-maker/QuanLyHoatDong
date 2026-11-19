namespace QuanLyHoatDong;

static class Program
{
    // Ghi lỗi khởi động ra 2 nơi: cạnh file exe và trong thư mục Temp của Windows
    static void Log(string s)
    {
        string dong = DateTime.Now + "  " + s + Environment.NewLine;
        try { File.AppendAllText(Path.Combine(AppContext.BaseDirectory, "loi.txt"), dong); } catch { }
        try { File.AppendAllText(Path.Combine(Path.GetTempPath(), "QuanLyHoatDong_loi.txt"), dong); } catch { }
    }

    [STAThread]
    static void Main()
    {
        Log("1. Main bat dau");
        AppDomain.CurrentDomain.UnhandledException += (s, e) => Log("UnhandledException: " + e.ExceptionObject);

        try
        {
            Chay();
        }
        catch (Exception ex)
        {
            Log("LOI: " + ex);
            MessageBox.Show(ex.ToString(), "Lỗi khởi động chương trình", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    static void Chay()
    {
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (s, e) =>
        {
            Log("ThreadException: " + e.Exception);
            UI.Error("Có lỗi xảy ra (có thể do mất kết nối SQL Server):\n" + e.Exception.Message);
        };

        try
        {
            TaiKhoanDAL.TaoAdminMacDinh();   // tạo tài khoản Admin nếu chưa có
        }
        catch (Exception ex)
        {
            Log("LOI SQL: " + ex);
            UI.Error("Không kết nối được SQL Server.\nHãy kiểm tra chuỗi kết nối trong Database/Database.cs " +
                     "và đã chạy file SQL/TaoDatabase.sql chưa.\n\n" + ex.Message);
            return;
        }

        Application.Run(new FrmDangNhap());
    }
}
