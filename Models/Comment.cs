namespace BugTracker.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public int TicketId { get; set; }  // Relación con Ticket
        public required Ticket Ticket { get; set; }
    }
}
