namespace RideShareConnect.Models.DTOs
{
    public class VehicleUpdateDto
    {
        public int VehicleId { get; set; }
        public string VehicleType { get; set; }
        public string LicensePlate { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime? RegistrationExpiry { get; set; }
        public string RCDocumentBase64 { get; set; }
        public string InsuranceDocumentBase64 { get; set; }
    }
}