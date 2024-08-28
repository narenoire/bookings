using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ismat_36_proj.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public string? From { get; set; }  // This corresponds to the "From" input in both forms
        public string? Destination { get; set; }  // This corresponds to the "To" select dropdown (either hotel or holiday destination)
        public string? StartDate { get; set; }  // This corresponds to the "Start" input
        public string? ReturnDate { get; set; }  // This corresponds to the "Return" input
        public int Adults { get; set; }  // This corresponds to the "Adults" input
        public int Child { get; set; }  // This corresponds to the "Child" input
    }
}
