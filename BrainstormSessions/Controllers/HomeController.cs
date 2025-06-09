using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using BrainstormSessions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using log4net;

namespace BrainstormSessions.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;
        private readonly Serilog.ILogger _logger;

        public HomeController(IBrainstormSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
            _logger = new LoggerConfiguration()
                .WriteTo.Log4Net() // Redirect logs to Log4Net
                .CreateLogger();
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

            return RedirectToAction(actionName: nameof(Index));
        }
    }
}
