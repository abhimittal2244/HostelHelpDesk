using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Services;
using HostelHelpDesk.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HostelHelpDesk.Persistence.Data
{
    public static class DbSeeder
    {
        

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HostelComplaintsDB>();

            // Ensure DB exists and is up-to-date
            context.Database.Migrate();

            // Seed Hostels if none exist
            if (!context.Hostels.Any())
            {
                context.Hostels.AddRange(
                    new Hostel { Id=1, HostelName = "A" },
                    new Hostel { Id=2, HostelName = "B" }
                );
            }

            if (!context.Rooms.Any())
            {
                context.Rooms.AddRange(
                    new Room { Id=1, RoomNo="A101", HostelId=1}
                );
            }

            if (!context.Admins.Any())
            {
                CreatePasswordHash("admin", out byte[] hash, out byte[] salt);
                context.Admins.AddRange(
                    new Admin { Id=1, FirstName = "admin", LastName = "admin", Email = "admin", PhoneNumber = "1234567890", Role = Shared.Enums.Role.Admin, PasswordHash = hash, PasswordSalt = salt }
                );
            }

            if (!context.Students.Any())
            {
                CreatePasswordHash("12345", out byte[] hash, out byte[] salt);
                context.Students.AddRange(
                    new Student { Id=2, FirstName="Sameer", LastName="Batra", Email="sameer@gmail.com", PasswordHash = hash, PasswordSalt = salt, PhoneNumber="9867985645", HostelId=1, Role=Shared.Enums.Role.Student, RollNo=1243, RoomId=1 }
                );
            }

            // Seed Caretakers if none exist
            if (!context.Caretakers.Any())
            {

                CreatePasswordHash("12345", out byte[] hash, out byte[] salt);
                var hostel1 = await context.Hostels.FindAsync(1);
                
                context.Caretakers.AddRange(
                    new Caretaker {Id=3, FirstName = "Ravi", LastName = "Kumar", HostelId = 1, PhoneNumber = "12567890", Email = "ravi@gmail.com", Role = Shared.Enums.Role.Caretaker, PasswordHash = hash, PasswordSalt = salt},
                    new Caretaker {Id=4, FirstName = "Priya", LastName = "Rajdev", HostelId = 2, PhoneNumber = "1234567", Email = "priya@gmail.com", Role = Shared.Enums.Role.Caretaker, PasswordHash = hash, PasswordSalt = salt }

                );
            }

            if (!context.ComplaintTypes.Any())
            {
                context.ComplaintTypes.AddRange(
                    new ComplaintType { Id = 1, Name = "Plumbing" },
                    new ComplaintType { Id = 2, Name = "Furnituring" },
                    new ComplaintType { Id = 3, Name = "Electrical" }
                );
            }

            if (!context.WorkerTypes.Any())
            {
                //var data = new ComplaintTypeWorkerTypeDto
                //{
                //    ComplaintTypeName = "Plumbing",
                //    WorkerTypeName = "Plumber"
                //};
                context.WorkerTypes.AddRange(
                    new WorkerType { Id = 1, Name = "Plumber", ComplaintTypeId = 1 },
                    new WorkerType { Id = 2, Name = "Carpenter", ComplaintTypeId = 2 },
                    new WorkerType { Id = 3, Name = "Electrician", ComplaintTypeId = 3 }
                );
            }
            await context.SaveChangesAsync();
            if (!context.Workers.Any())
            {
                CreatePasswordHash("12345", out byte[] hash, out byte[] salt);
                var plumber = context.WorkerTypes.First(wt => wt.Id == 1);
                var carpenter = context.WorkerTypes.First(wt => wt.Id == 2);
                var electrician = context.WorkerTypes.First(wt => wt.Id == 3);
                context.Workers.AddRange(
                    new Worker { Id=5, FirstName="Rampal", LastName="Kumar", Email="rampal@gmail.com", Role=Shared.Enums.Role.Worker, PasswordHash=hash, PasswordSalt=salt, PhoneNumber="6780831234", WorkerSpecialization = new List<WorkerType> { plumber, electrician } },
                    new Worker
                    {
                        Id = 6,
                        FirstName = "Suresh",
                        LastName = "Yadav",
                        Email = "suresh@gmail.com",
                        Role = Shared.Enums.Role.Worker,
                        PasswordHash = hash,
                        PasswordSalt = salt,
                        PhoneNumber = "9876543210",
                        WorkerSpecialization = new List<WorkerType> { carpenter, electrician }
                    }
                );
            }

            if (!context.Timeslots.Any())
            {
                context.Timeslots.AddRange(
                    new Timeslot { Id = 1, StartTime = TimeOnly.Parse("10:00"), EndTime = TimeOnly.Parse("12:00") },
                    new Timeslot { Id = 2, StartTime = TimeOnly.Parse("02:00"), EndTime = TimeOnly.Parse("04:00") },
                    new Timeslot { Id = 3, StartTime = TimeOnly.Parse("06:00"), EndTime = TimeOnly.Parse("08:00") }
                );
            }


            await context.SaveChangesAsync();
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
