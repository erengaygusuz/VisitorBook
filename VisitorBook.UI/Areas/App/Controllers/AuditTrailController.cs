using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Dtos.AuditTrailDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class AuditTrailController : Controller
    {
        private readonly IRepository<AuditTrail> _auditTrailRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;
        private readonly IStringLocalizer<Language> _localization;

        public AuditTrailController(IRepository<AuditTrail> auditTrailRepository, IMapper mapper, 
            INotyfService notyfService, IStringLocalizer<Language> localization)
        {
            _auditTrailRepository = auditTrailRepository;
            _mapper = mapper;
            _notyfService = notyfService;
            _localization = localization;
        }

        [Authorize(Permissions.AuditTrailManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            var auditTrails = _auditTrailRepository.GetAll().OrderByDescending(x => x.CreatedDate);

            return View(auditTrails);
        }

        [Authorize(Permissions.AuditTrailManagement.View)]
        [HttpGet]
        public async Task<IActionResult> Trail(int id)
        {
            var auditTrail = await _auditTrailRepository.GetAsync(x => x.Id == id);

            if (auditTrail == null)
            {
                _notyfService.Error(_localization["AuditTrails.TrailNotFoundMessage.Text"].Value);

                return View();
            }

            var auditTrailResponseDto = _mapper.Map<AuditTrailResponseDto>(auditTrail);

            var auditTrailAffectedColumnResponseDtos = new List<AuditTrailAffectedColumnResponseDto>();

            if (auditTrailResponseDto != null)
            {
                if (auditTrailResponseDto.OldValues.Any() || auditTrailResponseDto.NewValues.Any() || auditTrailResponseDto.AffectedColumns.Any())
                {
                    bool IsOldValueExist = false;

                    if (auditTrailResponseDto.OldValues.Count > 0)
                    {
                        IsOldValueExist = true;
                    }

                    bool IsNewValueExist = false;

                    if (auditTrailResponseDto.NewValues.Count > 0)
                    {
                        IsNewValueExist = true;
                    }

                    if (IsOldValueExist)
                    {
                        for (int i = 0; i < auditTrailResponseDto.OldValues.Count; i++)
                        {
                            var auditTrailAffectedColumnResponseDto = new AuditTrailAffectedColumnResponseDto();

                            auditTrailAffectedColumnResponseDto.ColumnName = auditTrailResponseDto.OldValues.Keys.ElementAt(i);

                            auditTrailAffectedColumnResponseDto.OldValue = auditTrailResponseDto.OldValues.Values.ElementAt(i);

                            if (IsNewValueExist)
                            {
                                auditTrailAffectedColumnResponseDto.NewValue = auditTrailResponseDto.NewValues.Values.ElementAt(i);
                            }

                            else
                            {
                                auditTrailAffectedColumnResponseDto.NewValue = "";
                            }

                            auditTrailAffectedColumnResponseDtos.Add(auditTrailAffectedColumnResponseDto);
                        }
                    }

                    else
                    {
                        for (int i = 0; i < auditTrailResponseDto.NewValues.Count; i++)
                        {
                            var auditTrailAffectedColumnResponseDto = new AuditTrailAffectedColumnResponseDto();

                            auditTrailAffectedColumnResponseDto.ColumnName = auditTrailResponseDto.NewValues.Keys.ElementAt(i); ;

                            if (IsOldValueExist)
                            {
                                auditTrailAffectedColumnResponseDto.OldValue = auditTrailResponseDto.OldValues.Values.ElementAt(i);
                            }

                            else
                            {
                                auditTrailAffectedColumnResponseDto.OldValue = "";
                            }

                            auditTrailAffectedColumnResponseDto.NewValue = auditTrailResponseDto.NewValues.Values.ElementAt(i);

                            auditTrailAffectedColumnResponseDtos.Add(auditTrailAffectedColumnResponseDto);
                        }
                    }
                }
            }

            var auditTrailViewModel = new AuditTrailViewModel()
            {
                Username = auditTrailResponseDto.Username,
                TableName = auditTrailResponseDto.TableName,
                Type = auditTrailResponseDto.Type,
                CreatedDate = auditTrailResponseDto.CreatedDate,
                PrimaryKey = auditTrailResponseDto.PrimaryKey,
                AffectedColumns = auditTrailAffectedColumnResponseDtos
            };

            return View(auditTrailViewModel);
        }
    }
}
