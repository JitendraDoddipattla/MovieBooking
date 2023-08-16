using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBooking.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string  Duration { get; set; }
        public string? Language { get; set; }
        public DateTime PlayingTime { get; set; }
        public DateTime PlayingDate { get; set; }
        public double TicketPrice { get; set; }
        public double Rating { get; set; }
        public string Genre { get; set; }
        public int TicketsAllotted { get; set; }
        public string TheatreName { get; set; }
        
        //public string ImageUrl { get; set; }
       

        //[NotMapped]
        //public IFormFile Image { get; set; }
        //public   ICollection<Booking> Bookings { get; set; }
    }
}
