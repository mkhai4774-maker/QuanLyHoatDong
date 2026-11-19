namespace QuanLyHoatDong;

// Dùng chung Admin và Sinh viên.
// Sinh viên: xem hồ sơ, sửa họ tên, email, SĐT + đổi mật khẩu. Admin: chỉ đổi mật khẩu (Admin không có hồ sơ sinh viên).
public partial class FrmTaiKhoan : Form
{
    public FrmTaiKhoan()
    {
        InitializeComponent();
        Theme.Nut(btnLuuThongTin, NutKieu.Chinh);
        Theme.Nut(btnDoiMatKhau, NutKieu.Chinh);

        var tk = Session.TaiKhoan;
        infoTenDN.Value = tk.TenDangNhap;

        if (tk.VaiTro == "SinhVien")
        {
            var sv = SinhVienDAL.Get(tk.MaSV);
            if (sv != null)
            {
                txtTen.Text = sv.HoTen;
                txtEmail.Text = sv.Email;
                txtSdt.Text = sv.SoDienThoai;

                avatarLon.Initials = Theme.VietTat(sv.HoTen);
                lblHoTen.Text = sv.HoTen;
                infoMa.Value = sv.MaSV;
                infoLop.Value = sv.Lop;
                infoKhoa.Value = sv.Khoa;
            }
            lblVaiTro.Text = "Sinh viên";
        }
        else
        {
            // Admin chỉ đổi mật khẩu: ẩn hồ sơ bên trái và thẻ thông tin cá nhân.
            // Thẻ đổi mật khẩu được thiết kế rộng 606 px nên giữ nguyên chiều rộng đó (không cho dãn
            // hết màn hình, nếu không các ô nhập neo hai bên sẽ giãn ra đè lên nhau và che nút).
            pnlTrai.Visible = false;
            gapTrai.Visible = false;
            cardThongTin.Visible = false;
            gapPhai.Visible = false;
            lblTieuDe.Visible = false;   // khung Admin đã có tiêu đề trang ở phía trên
            pnlPhai.Dock = DockStyle.Left;
            pnlPhai.Width = 606;
        }
    }

    void btnLuuThongTin_Click(object sender, EventArgs e)
    {
        string ten = txtTen.Text.Trim(), email = txtEmail.Text.Trim(), sdt = txtSdt.Text.Trim();
        if (ten == "") { UI.Error("Họ tên không được để trống."); return; }
        if (email != "" && !UI.EmailOK(email)) { UI.Error("Email không đúng định dạng."); return; }
        if (sdt != "" && !UI.SdtOK(sdt)) { UI.Error("Số điện thoại phải bắt đầu bằng 0 và có 10-11 chữ số."); return; }

        SinhVienDAL.CapNhatCaNhan(Session.TaiKhoan.MaSV, ten, email, sdt);
        lblHoTen.Text = ten;
        avatarLon.Initials = Theme.VietTat(ten);
        UI.Info("Đã cập nhật thông tin cá nhân.");
    }

    void btnDoiMatKhau_Click(object sender, EventArgs e)
    {
        if (txtCu.Text == "" || txtMoi.Text == "" || txtXN.Text == "")
        { UI.Error("Vui lòng nhập đầy đủ các ô mật khẩu."); return; }
        if (!TaiKhoanDAL.KiemTraMatKhau(Session.TaiKhoan.MaTK, txtCu.Text))
        { UI.Error("Mật khẩu hiện tại không đúng."); return; }
        if (txtMoi.Text.Length < 6)
        { UI.Error("Mật khẩu mới phải có ít nhất 6 ký tự."); return; }
        if (txtMoi.Text != txtXN.Text)
        { UI.Error("Mật khẩu mới và xác nhận không khớp."); return; }

        TaiKhoanDAL.DoiMatKhau(Session.TaiKhoan.MaTK, txtMoi.Text);
        txtCu.Clear(); txtMoi.Clear(); txtXN.Clear();
        UI.Info("Đổi mật khẩu thành công.");
    }
}
