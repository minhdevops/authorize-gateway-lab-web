namespace AuthorizeGatewayLab.Web.Models.Entities;

public class ShareRequest
{
    public int Id { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string CitizenId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
