namespace MovieBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int NoOfTickets { get; set; }
        public string TheatreName { get; set; }
        public int SeatNumber { get; set; }
        public double Price { get; set; }
        public string Phone { get; set; }
        public DateTime BookingTime { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
    }
}
