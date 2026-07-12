using AuthorizeGatewayLab.Web.Models.Entities;

namespace AuthorizeGatewayLab.Web.Services;

public class MockShareRequestService : IShareRequestService
{
    private readonly List<ShareRequest> _requests;
    private readonly List<CitizenData> _citizens;

    public MockShareRequestService()
    {
        _requests = new List<ShareRequest>
        {
            new() { Id = 1, TransactionId = "TXN001", CitizenId = "012345678901", FullName = "Nguyễn Văn An", PhoneNumber = "0912345678", Status = "Pending", CreatedAt = DateTime.Now.AddDays(-2) },
            new() { Id = 2, TransactionId = "TXN002", CitizenId = "012345678902", FullName = "Trần Thị Bình", PhoneNumber = "0923456789", Status = "Pending", CreatedAt = DateTime.Now.AddDays(-1) },
            new() { Id = 3, TransactionId = "TXN003", CitizenId = "012345678903", FullName = "Lê Văn Cường", PhoneNumber = "0934567890", Status = "Approved", CreatedAt = DateTime.Now.AddHours(-8) },
            new() { Id = 4, TransactionId = "TXN004", CitizenId = "012345678904", FullName = "Phạm Thị Dung", PhoneNumber = "0945678901", Status = "Rejected", CreatedAt = DateTime.Now.AddHours(-5) },
            new() { Id = 5, TransactionId = "TXN005", CitizenId = "012345678905", FullName = "Hoàng Văn Em", PhoneNumber = "0956789012", Status = "Pending", CreatedAt = DateTime.Now.AddHours(-1) }
        };

        _citizens = new List<CitizenData>
        {
            new() { CitizenId = "012345678901", FullName = "Nguyễn Văn An", DateOfBirth = new DateTime(1990, 1, 12), Gender = "Nam", Address = "Cầu Giấy, Hà Nội", IssueDate = new DateTime(2021, 5, 10), IssuePlace = "Cục CSQLHC về TTXH", Nationality = "Việt Nam" },
            new() { CitizenId = "012345678902", FullName = "Trần Thị Bình", DateOfBirth = new DateTime(1992, 3, 20), Gender = "Nữ", Address = "Hoàng Mai, Hà Nội", IssueDate = new DateTime(2020, 8, 15), IssuePlace = "Cục CSQLHC về TTXH", Nationality = "Việt Nam" },
            new() { CitizenId = "012345678903", FullName = "Lê Văn Cường", DateOfBirth = new DateTime(1988, 7, 5), Gender = "Nam", Address = "Quận 1, TP Hồ Chí Minh", IssueDate = new DateTime(2019, 11, 2), IssuePlace = "Cục CSQLHC về TTXH", Nationality = "Việt Nam" },
            new() { CitizenId = "012345678904", FullName = "Phạm Thị Dung", DateOfBirth = new DateTime(1995, 12, 1), Gender = "Nữ", Address = "Hải Châu, Đà Nẵng", IssueDate = new DateTime(2022, 2, 18), IssuePlace = "Cục CSQLHC về TTXH", Nationality = "Việt Nam" },
            new() { CitizenId = "012345678905", FullName = "Hoàng Văn Em", DateOfBirth = new DateTime(1991, 9, 9), Gender = "Nam", Address = "Ninh Kiều, Cần Thơ", IssueDate = new DateTime(2023, 4, 25), IssuePlace = "Cục CSQLHC về TTXH", Nationality = "Việt Nam" }
        };
    }

    public List<ShareRequest> GetAll() => _requests.OrderByDescending(x => x.CreatedAt).ToList();

    public ShareRequest? GetById(int id) => _requests.FirstOrDefault(x => x.Id == id);

    public ShareRequest? GetByTransactionId(string transactionId) =>
        _requests.FirstOrDefault(x => x.TransactionId.Equals(transactionId, StringComparison.OrdinalIgnoreCase));

    public CitizenData? GetCitizenDataByTransactionId(string transactionId)
    {
        var request = GetByTransactionId(transactionId);
        if (request == null) return null;
        return _citizens.FirstOrDefault(x => x.CitizenId == request.CitizenId);
    }

    public bool Approve(int id)
    {
        var request = GetById(id);
        if (request == null) return false;
        request.Status = "Approved";
        request.UpdatedAt = DateTime.Now;
        Console.WriteLine($"Mock callback Authorizegateway: transactionId={request.TransactionId}, status=Approved");
        return true;
    }

    public bool Reject(int id)
    {
        var request = GetById(id);
        if (request == null) return false;
        request.Status = "Rejected";
        request.UpdatedAt = DateTime.Now;
        Console.WriteLine($"Mock callback Authorizegateway: transactionId={request.TransactionId}, status=Rejected");
        return true;
    }
}
