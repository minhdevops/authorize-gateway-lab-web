namespace AuthorizeGatewayLab.Web.Models.Entities;

public class CitizenData
{
    public string CitizenId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public string IssuePlace { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
}
