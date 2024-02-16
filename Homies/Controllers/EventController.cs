using Homies.Data;
using Homies.Data.Constants;
using Homies.Data.Models;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly HomiesDbContext _data;

        public EventController(HomiesDbContext context)
            => _data = context;

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var events = await _data
                .Events
                .Select(e => new EventViewModel(e.Id, e.Name, e.Start, e.Type.Name, e.Organiser.UserName))
                .ToListAsync();

            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            var joinedEvents = await _data
                .EventsParticipants
                .AsNoTracking()
                .Where(ep => ep.HelperId == GetUser())
                .Select(ep => new EventViewModel(ep.EventId, ep.Event.Name, ep.Event.Start, ep.Event.Type.Name, ep.Event.OrganiserId))
                .ToListAsync();

            return View(joinedEvents);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new EventFormModel()
            {
                Types = await GetEventTypes()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Types = await GetEventTypes();

                return View(model);
            }

            if (!DateTime.TryParseExact(model.Start, DataConstants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var start))
            {
                ModelState.AddModelError(nameof(model.Start), string.Format(DataConstants.ErrorMessages.DateFormatError, nameof(model.Start)));

                model.Types = await GetEventTypes();

                return View(model);
            }

            if (!DateTime.TryParseExact(model.End, DataConstants.DateTimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var end))
            {
                ModelState.AddModelError(nameof(model.End), string.Format(DataConstants.ErrorMessages.DateFormatError, nameof(model.End)));

                model.Types = await GetEventTypes();

                return View(model);
            }

            if (end < start)
            {
                ModelState.AddModelError(nameof(model.End), string.Format(DataConstants.ErrorMessages.InvalidPeriod, nameof(model.Start), nameof(model.End)));
                model.Types = await GetEventTypes();

                return View(model);
            }

            var dataEvent = new Event()
            {
                Name = model.Name,
                Description = model.Description,
                Start = start,
                End = end,
                CreatedOn = DateTime.Now,
                OrganiserId = GetUser(),
                TypeId = model.TypeId
            };

            await _data.Events.AddAsync(dataEvent);
            await _data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var isEventExist = await _data
                .Events
                .AnyAsync(e => e.Id == id);

            if (!isEventExist)
            {
                return NotFound();
            }

            var isAlreadyJoined = await _data
                .EventsParticipants
                .AnyAsync(ep => ep.EventId == id && ep.HelperId == GetUser());

            if (isAlreadyJoined)
            {
                return RedirectToAction(nameof(All));
            }

            var eventParticipant = new EventParticipant()
            {
                EventId = id,
                HelperId = GetUser()
            };

            await _data.EventsParticipants.AddAsync(eventParticipant);
            await _data.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var isEventExist = await _data
                .Events
                .AnyAsync(e => e.Id == id);

            if (!isEventExist)
            {
                return NotFound();
            }

            var eventParticipant = await _data
                .EventsParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == id && ep.HelperId == GetUser());

            if (eventParticipant == null)
            {
                return BadRequest();
            }

            _data.EventsParticipants.Remove(eventParticipant);
            await _data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var model = await this._data.Events.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            if (model.OrganiserId != GetUser())
            {
                return Unauthorized();
            }

            return View(new EventFormModel()
            {
                Name = model.Name,
                Description = model.Description,
                Start = model.Start.ToString(DataConstants.DateTimeFormat),
                End = model.End.ToString(DataConstants.DateTimeFormat),
                TypeId = model.TypeId,
                Types = await GetEventTypes()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Types = await GetEventTypes();

                return View(model);
            }

            if (!DateTime.TryParseExact(model.Start, DataConstants.DateTimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var start))
            {
                ModelState.AddModelError(nameof(model.Start), string.Format(DataConstants.ErrorMessages.DateFormatError, nameof(model.Start)));

                model.Types = await GetEventTypes();

                return View(model);
            }

            if (!DateTime.TryParseExact(model.End, DataConstants.DateTimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var end))
            {
                ModelState.AddModelError(nameof(model.End), string.Format(DataConstants.ErrorMessages.DateFormatError, nameof(model.End)));

                model.Types = await GetEventTypes();

                return View(model);
            }

            if (end < start)
            {
                ModelState.AddModelError(nameof(model.End), string.Format(DataConstants.ErrorMessages.InvalidPeriod, nameof(model.Start), nameof(model.End)));
                model.Types = await GetEventTypes();

                return View(model);
            }

            var entity = await this._data.Events.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Start = start;
            entity.End = end;
            entity.TypeId = model.TypeId;

            await this._data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var model = await _data.Events.Include(e => e.Type).Include(e => e.Organiser).FirstOrDefaultAsync(e => e.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(new EventDetailsModel(model.Id, model.Name, model.Description, model.CreatedOn, model.Start, model.End, model.Type.Name, model.Organiser.UserName));
        }

        private async Task<IEnumerable<TypeViewModel>> GetEventTypes()
        {
            return await _data
                .Types
                .AsNoTracking()
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        private string GetUser() => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
