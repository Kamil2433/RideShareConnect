using System.ComponentModel.DataAnnotations;


namespace RideShareConnect.Dtos
{
    public class SendResetPasswordOtpDto
    {
        public string Email { get; set; }
    }

    public class VerifyResetPasswordOtpDto
    {
        public string Email { get; set; }
        public string OtpCode { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
