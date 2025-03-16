namespace BugTracker.Models

{
 public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;  // Evitar valores nulos
    public string Description { get; set; } = string.Empty;  // Evitar valores nulos
    public string Status { get; set; } = "Open";  // Valor predeterminado para Status
    public required User User { get; set; }  // Asegúrate de que el modelo 'User' exista
    public int UserId { get; set; }
}

}
