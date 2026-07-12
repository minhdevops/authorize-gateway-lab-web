using AuthorizeGatewayLab.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizeGatewayLab.Web.Controllers;

public class ShareRequestsController : Controller
{
    private readonly IShareRequestService _shareRequestService;

    public ShareRequestsController(IShareRequestService shareRequestService)
    {
        _shareRequestService = shareRequestService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var requests = _shareRequestService.GetAll();
        return View(requests);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var request = _shareRequestService.GetById(id);
        if (request == null) return NotFound();
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Approve(int id)
    {
        if (_shareRequestService.Approve(id))
            TempData["Success"] = "Đã chia sẻ dữ liệu thành công.";
        else
            TempData["Error"] = "Không tìm thấy yêu cầu chia sẻ.";

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Reject(int id)
    {
        if (_shareRequestService.Reject(id))
            TempData["Success"] = "Đã từ chối yêu cầu chia sẻ.";
        else
            TempData["Error"] = "Không tìm thấy yêu cầu chia sẻ.";

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public IActionResult SharedInfo(string transactionId)
    {
        var citizenData = _shareRequestService.GetCitizenDataByTransactionId(transactionId);
        if (citizenData == null) return NotFound();

        ViewBag.TransactionId = transactionId;
        return View(citizenData);
    }
}
