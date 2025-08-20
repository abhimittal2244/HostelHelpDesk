using Microsoft.EntityFrameworkCore;
using HostelHelpDesk.Persistence.Data;

namespace HostelHelpDesk.Application.Services.Background
{
    public class ComplaintAssignmentService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ComplaintAssignmentService> _logger;

        public ComplaintAssignmentService(IServiceScopeFactory scopeFactory, ILogger<ComplaintAssignmentService> logger)
        //public ComplaintAssignmentService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Complaint assignment service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<HostelComplaintsDB>();

                try
                {
                    var unassignedComplaints = await db.Complaints
                        .Where(c => c.Worker == null)
                        .Include(c => c.ComplaintType)
                        .Include(c => c.Timeslot)
                        .Include(c => c.Hostel)
                        .ToListAsync();

                    foreach (var complaint in unassignedComplaints)
                    {
                        // Get eligible worker types for the complaint
                        var workerTypes = await db.WorkerTypes
                            .Where(wt => wt.ComplaintTypeId == complaint.ComplaintType.Id)
                            .ToListAsync();

                        var eligibleWorkerIds = await db.Workers
                            .Where(w => w.WorkerSpecialization.Any(ws => workerTypes.Contains(ws)))
                            .Select(w => w.Id)
                            .ToListAsync();

                        //var availableWorker = await db.Workers
                        //    .Include(w => w.WorkerSpecialization)
                        //    .Where(w =>
                        //        eligibleWorkerIds.Contains(w.Id) &&
                        //        !db.Complaints.Any(c =>
                        //            c.Worker.Id == w.Id &&
                        //            c.TimeslotId == complaint.TimeslotId))
                        //    .FirstOrDefaultAsync();

                        // Find worker who has < 2 complaints in the same timeslot
                        var availableWorker = await db.Workers
                            .Include(w => w.WorkerSpecialization)
                            .Where(w =>
                                eligibleWorkerIds.Contains(w.Id) &&
                                db.Complaints.Count(c => c.Worker.Id == w.Id && c.TimeslotId == complaint.TimeslotId) < 2
                            )
                            .FirstOrDefaultAsync();

                        if (availableWorker != null)
                        {
                            complaint.Worker = availableWorker;
                            complaint.Status = Shared.Enums.ComplaintStatus.ASSIGNED;
                            _logger.LogInformation($"Assigned complaint {complaint.ComplaintNo} to worker {availableWorker.Id}");
                        }
                    }

                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while assigning complaints.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Wait before next run
            }
        }
    }

}
