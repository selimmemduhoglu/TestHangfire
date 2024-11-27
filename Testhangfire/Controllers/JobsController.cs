using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Testhangfire.Services;

namespace Testhangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        // 1. Enqueue Job: Hemen çalıştırılan bir iş
        [HttpPost("enqueue")]
        public IActionResult EnqueueJob()
        {
            string jobId = BackgroundJob.Enqueue(() => _jobService.SendEmailAsync(
                "test@example.com", "Enqueue Test", "This is an enqueue job."));

            return Ok($"Job Enqueued. Job ID: {jobId}");
        }

        // 2. Scheduled Job: Gelecekte çalıştırılan bir iş
        [HttpPost("schedule")]
        public IActionResult ScheduleJob()
        {
            var jobId = BackgroundJob.Schedule(() => _jobService.SendEmailAsync(
                "test@example.com", "Scheduled Test", "This is a scheduled job."), TimeSpan.FromMinutes(1));

            return Ok($"Job Scheduled. Job ID: {jobId}, Will run in 1 minute.");
        }

        // 3. Recurring Job: Periyodik olarak çalıştırılan bir iş
        [HttpPost("recurring")]
        public IActionResult RecurringJobExample()
        {
            RecurringJob.AddOrUpdate("recurringjob", () => _jobService.SendEmailAsync("test@example.com", "Recurring Test", "This is a recurring job."), Cron.Minutely); // Her dakika çalışır
            return Ok("Recurring job scheduled to run every minute.");
        }

        // 4. ContinueWith Job: Başka bir işten sonra çalıştırılan iş
        [HttpPost("continuewith")]
        public IActionResult ContinueWithJob()
        {
            var firstJobId = BackgroundJob.Enqueue(() => _jobService.SendEmailAsync(
                "test@example.com", "First Job", "This is the first job."));

            var secondJobId = BackgroundJob.ContinueWith(firstJobId, () => _jobService.SendEmailAsync(
                "test@example.com", "Second Job", "This is the second job."));

            return Ok($"First Job ID: {firstJobId}, Second Job ID: {secondJobId}");
        }

        // 5. Delayed Job: Gecikmeli çalıştırılan iş
        [HttpPost("delayed")]
        public IActionResult DelayedJob()
        {
            string jobId = BackgroundJob.Schedule(() => _jobService.ProcessDataAsync("Delayed Data"), TimeSpan.FromSeconds(30));

            return Ok($"Job Scheduled to run in 30 seconds. Job ID: {jobId}");
        }
    }
}