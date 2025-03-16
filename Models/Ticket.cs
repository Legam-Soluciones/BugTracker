namespace BugTracker.Models

{
    public class Ticket
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; } // Add this property
        // Add other properties as needed
    }
}
