using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Entities;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class ContactMessageController : Controller
    {
        private readonly IRepository<ContactMessage> _contactMessageRepository;

        public ContactMessageController(IRepository<ContactMessage> contactMessageRepository)
        {
            _contactMessageRepository = contactMessageRepository;
        }

        [Authorize(Permissions.ContactMessageManagement.View)]
        public IActionResult Index()
        {
            var contactMessages = _contactMessageRepository.GetAll();

            return View(contactMessages);
        }

        [Authorize(Permissions.ContactMessageManagement.View)]
        public async Task<IActionResult> Message(int id)
        {
            var contactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);

            return View(contactMessage);
        }
    }
}
