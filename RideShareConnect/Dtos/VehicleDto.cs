namespace RideShareConnect.Models.DTOs
{
    public class VehicleDto
    {
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public string VehicleType { get; set; }
        public string LicensePlate { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime RegistrationExpiry { get; set; }
        public string RCDocumentBase64 { get; set; }
        public string InsuranceDocumentBase64 { get; set; }

        // Added from Vehicle model
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
