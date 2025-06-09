using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using BrainstormSessions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Sinks.EmailPickup;

namespace BrainstormSessions.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;
        private readonly Serilog.ILogger _logger;

        public HomeController(IBrainstormSessionRepository sessionRepository, Serilog.ILogger logger)
        {
            _sessionRepository = sessionRepository;
            _logger = logger; // Use the DI-provided, global logger
        }

        public async Task<IActionResult> Index()
        {

            _logger.Information("Fetching list of brainstorming sessions.");
            var sessionList = await _sessionRepository.ListAsync();

            var model = sessionList.Select(session => new StormSessionViewModel()
            {
                Id = session.Id,
                DateCreated = session.DateCreated,
                Name = session.Name,
                IdeaCount = session.Ideas.Count
            });

            _logger.Information("Fetched {Count} brainstorming sessions successfully.", sessionList.Count());
            return View(model);
        }

        public class NewSessionModel
        {
            [Required]
            public string SessionName { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewSessionModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state received while trying to create a new session.");
                return BadRequest(ModelState);
            }
            else
            {
                _logger.Information("Adding new brainstorming session.");
                await _sessionRepository.AddAsync(new BrainstormSession()
                {
                    DateCreated = DateTimeOffset.Now,
                    Name = model.SessionName
                });
                _logger.Information("Successfully added new brainstorming session.");
            }


            // Trigger an email via Serilog (you can use Error Level if you want to guarantee email delivery)
            _logger.Error("A new session was created: {SessionName} at {Timestamp}.",
                model.SessionName,
                DateTimeOffset.Now);
            return RedirectToAction(actionName: nameof(Index));
        }
    }
}
