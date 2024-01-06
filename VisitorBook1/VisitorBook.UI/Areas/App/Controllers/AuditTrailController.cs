using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Dtos.AuditTrailDtos;
using VisitorBook.Core.Entities;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class AuditTrailController : Controller
    {
        private readonly IRepository<AuditTrail> _auditTrailRepository;
        private readonly IService<AuditTrail> _auditTrailService;

        public AuditTrailController(IRepository<AuditTrail> auditTrailRepository, IService<AuditTrail> auditTrailService)
        {
            _auditTrailRepository = auditTrailRepository;
            _auditTrailService = auditTrailService;
        }

        [Authorize(Permissions.AuditTrailManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            var auditTrails = _auditTrailRepository.GetAll();

            return View(auditTrails);
        }

        [Authorize(Permissions.AuditTrailManagement.View)]
        [HttpGet]
        public async Task<IActionResult> Trail(int id)
        {
            var auditTrail = await _auditTrailService.GetAsync<AuditTrailResponseDto>(x => x.Id == id);

            return View(auditTrail);
        }
    }
}
