using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{
    public class EmailDto
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; } = true;
    }
}