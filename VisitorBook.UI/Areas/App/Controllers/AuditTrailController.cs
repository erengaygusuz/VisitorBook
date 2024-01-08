using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Dtos.AuditTrailDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.ViewModels;

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
            var auditTrailResponseDto = await _auditTrailService.GetAsync<AuditTrailResponseDto>(x => x.Id == id);

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
