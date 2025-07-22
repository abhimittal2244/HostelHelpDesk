using ClosedXML.Excel;
using CsvHelper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Persistence.Repository;

namespace HostelHelpDesk.Application.Services
{
    public class RoomService(RoomRepository roomRepo, HostelComplaintsDB dB, HostelRepository hostelRepo)
    {
        private readonly RoomRepository _roomRepo = roomRepo;
        private readonly HostelRepository _hostelRepo = hostelRepo;
        private readonly HostelComplaintsDB _DB = dB;

        /*
        public async Task<RoomResponseDto> AddRoomAsync(RoomRequestDto dto)
        {
            var room = new Room
            {
                RoomNo = dto.RoomNo,
                HostelId = dto.HostelId
            };

            await _roomRepo.AddAsync(room);
            var hostel = await _DB.Hostels.FindAsync(room.HostelId);
            return new RoomResponseDto {
                RoomId = room.Id,
                RoomNo = room.RoomNo,
                HostelId = room.HostelId,
                HostelName = hostel.HostelName
            };
        }

        public Task DeleteRoomAsync(int hid, int roomNo)
        {
            throw new NotImplementedException();
        }

        public Task<RoomResponseDto> GetRoomByHostelAsync(int hid)
        {
            throw new NotImplementedException();
        }
        */

        public async Task<RoomResponseDto> AddRoomAsync(RoomRequestDto dto)
        {
            var hostel = await _hostelRepo.FindAsync(dto.HostelId) ?? throw new ArgumentException("Invalid Hostel ID");
            bool exists = await _roomRepo.IsRoomExistAsync(dto.RoomNo, dto.HostelId);
            if (exists)
                throw new InvalidOperationException("Room already exists in this hostel");

            var room = new Room
            {
                RoomNo = dto.RoomNo,
                HostelId = dto.HostelId
            };

            var addedRoom = await _roomRepo.AddAsync(room);

            return new RoomResponseDto
            {
                RoomId = addedRoom.Id,
                RoomNo = addedRoom.RoomNo,
                HostelId = addedRoom.HostelId,
                HostelName = hostel.HostelName
            };
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepo.GetAllAsync();
            return rooms.Select(r => new RoomResponseDto
            {
                RoomId = r.Id,
                RoomNo = r.RoomNo,
                HostelId = r.HostelId,
                HostelName = r.Hostel.HostelName
            });
        }

        public async Task<IEnumerable<RoomResponseDto>> GetRoomsByHostelIdAsync(int hostelId)
        {
            var rooms = await _roomRepo.FindByHostelIdAsync(hostelId);
            return rooms.Select(r => new RoomResponseDto
            {
                RoomId = r.Id,
                RoomNo = r.RoomNo,
                HostelId = r.HostelId,
                HostelName = r.Hostel.HostelName
            });
        }

        public async Task<RoomResponseDto> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepo.FindAsync(id) ?? throw new InvalidDataException("Invalid Room Id Entered");


            return new RoomResponseDto
            {
                RoomId = room.Id,
                RoomNo = room.RoomNo    ,
                HostelId = room.HostelId,
                HostelName = room.Hostel.HostelName
            };
        }

        public async Task<bool> IsRoomExistAsync(string roomNumber, int hostelId)
        {
            return await _roomRepo.IsRoomExistAsync(roomNumber, hostelId);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var deleted = await _roomRepo.DeleteAsync(id);
            return deleted != null;
        }

        public async Task<byte[]> UploadRoomsAsync(IFormFile file)
        {

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<RoomUploadDto>().ToList();

            var resultRecords = new List<RoomUploadResultDto>();

            foreach (var record in records)
            {
                var result = new RoomUploadResultDto
                {
                    RoomNo = record.RoomNo,
                    HostelName = record.HostelName
                };

                try
                {
                    var hostel = await _hostelRepo.FindAllByNameAsync(record.HostelName);
                    if (hostel == null || !hostel.Any())
                    {
                        result.Status = "Failed";
                        result.Message = "Hostel not found";
                        resultRecords.Add(result);
                        continue;
                    }

                    var hostelEntity = hostel.First();

                    // Check if room already exists
                    var exists = await _roomRepo.IsRoomExistAsync(record.RoomNo, hostelEntity.Id);
                    if (exists)
                    {
                        result.Status = "Failed";
                        result.Message = "Room already exists";
                        //results.Add(result);
                        //continue;
                    }
                    else
                    {
                        var room = new Room
                        {
                            RoomNo = record.RoomNo,
                            Hostel = hostelEntity
                        };

                        await _roomRepo.AddAsync(room);
                        result.Status = "Success";
                        result.Message = "Hostel added successfully";
                    }
                }
                catch (Exception ex)
                {
                    result.Status = "Failed";
                    result.Message = $"Error: {ex.Message}";
                }

                resultRecords.Add(result);
            }

            // Convert resultRecords to downloadable CSV
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(resultRecords);
            writer.Flush();
            memoryStream.Position = 0;

            /*
            // Generate Excel from results
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Room Upload Results");
            worksheet.Cell(1, 1).Value = "RoomNo";
            worksheet.Cell(1, 2).Value = "HostelName";
            worksheet.Cell(1, 3).Value = "Status";

            for (int i = 0; i < resultRecords.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = resultRecords[i].RoomNo;
                worksheet.Cell(i + 2, 2).Value = resultRecords[i].HostelName;
                worksheet.Cell(i + 2, 3).Value = resultRecords[i].Status;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            */


            //return new FileResultDto
            //{
            //    Stream = stream,
            //    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //    FileName = $"RoomUploadResult_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            //};

            return memoryStream.ToArray();
        }


    }
}
