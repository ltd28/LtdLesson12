using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace LtdTH1.Models
{
    public class Student
    {
        public int Id { get; set; } //ma sinh vien

        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100, MinimumLength = 4,
            ErrorMessage = "Họ tên phải từ 4 đến 100 ký tự")]
        public string ? Name { get; set; }

        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@gmail\.com$",
            ErrorMessage = "Email phải có đuôi gmail.com")]
        public string ? Email { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [RegularExpression( @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s])\S{8,}$",
            ErrorMessage = "Mật khẩu cần ít nhất 8 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt")]
        public string ? Password { get; set; } //mat khau
        [Required]
        public Branch ? Branch { get; set; } //nganh hoc
        [Required]
        public Gender ? Gender { get; set; } //gioi tinh
        public bool IsRegular { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        public string ? Address { get; set; } //dia chi

        [DisplayName("Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Student), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBorth { get; set; }

        public static ValidationResult? ValidateDateOfBirth(
            DateTime date, ValidationContext context)
        {
            if (date < new DateTime(1963, 1, 1) ||
                date > new DateTime(2005, 12, 31))
            {
                return new ValidationResult(
                    "Ngày sinh phải từ 01/01/1963 đến 31/12/2005");
            }

            return ValidationResult.Success;
        }

        [DisplayName("Ảnh")]
        public string ? Image { get; set; }

        [DisplayName("Điểm")]
        [Required(ErrorMessage = "Vui lòng nhập điểm")]
        [Range(typeof(decimal), "0.0", "10.0",
            ErrorMessage = "Điểm phải nằm trong khoảng từ 0.0 đến 10.0")]
        public decimal ? Diem { get; set; }
    }
}
