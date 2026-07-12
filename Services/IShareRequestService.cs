using AuthorizeGatewayLab.Web.Models.Entities;

namespace AuthorizeGatewayLab.Web.Services;

public interface IShareRequestService
{
    List<ShareRequest> GetAll();
    ShareRequest? GetById(int id);
    ShareRequest? GetByTransactionId(string transactionId);
    CitizenData? GetCitizenDataByTransactionId(string transactionId);
    bool Approve(int id);
    bool Reject(int id);
}
