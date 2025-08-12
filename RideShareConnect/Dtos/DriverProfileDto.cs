namespace RideShareConnect.Dtos
{
    public class DriverProfileDto
    {
        // Personal Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; } // Base64 string
        public bool IsNewProfile { get; set; }


        // Driver Information
        public string LicenseNumber { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string LicenseImageUrl { get; set; } // Base64 string
        public int YearsOfExperience { get; set; }

        // Emergency Contact
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
    }
}