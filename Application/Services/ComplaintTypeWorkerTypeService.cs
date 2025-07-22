using Microsoft.EntityFrameworkCore;
using System;
using HostelHelpDesk.Application.DTO;
using HostelHelpDesk.Domain.Models;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Persistence.Repository;

namespace HostelHelpDesk.Application.Services
{
    public class ComplaintTypeWorkerTypeService
    {
        private readonly HostelComplaintsDB _context;
        private readonly ComplaintTypeWorkerTypeRepository _repository;

        public ComplaintTypeWorkerTypeService(HostelComplaintsDB context, ComplaintTypeWorkerTypeRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        /*
        public async Task CreateComplaintTypeWorkerTypeAsync(ComplaintTypeWorkerTypeDto dto)
        {
            var complaintType = await _context.ComplaintTypes
        .FirstOrDefaultAsync(ct => ct.Name == dto.ComplaintTypeName);

            if (complaintType == null)
            {
                complaintType = new ComplaintType { Name = dto.ComplaintTypeName };
                _context.ComplaintTypes.Add(complaintType);
                await _context.SaveChangesAsync();
            }

            var workerType = new WorkerType
            {
                Name = dto.WorkerTypeName,
                ComplaintTypeId = complaintType.Id
            };

            _context.WorkerTypes.Add(workerType);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WorkerTypeResponseDto>> GetAllWorkerTypesAsync()
        {
            var workerTypes = await _context.WorkerTypes
                //.Include(wt => wt.ComplaintTypes)
                .ToListAsync();

            return workerTypes.Select(wt => new WorkerTypeResponseDto
            {
                Id = wt.Id,
                Name = wt.Name
            }).ToList();
        }

        public async Task<List<ComplaintTypeResponseDto>> GetAllComplaintTypesAsync()
        {
            var types = await _context.ComplaintTypes
                //.Include(ct => ct.EligibleWorkerTypes)
                .ToListAsync();

            var response = types.Select(ct => new ComplaintTypeResponseDto
            {
                Id = ct.Id,
                Name = ct.Name,
                //EligibleWorkerTypes = ct.EligibleWorkerTypes.Select(wt => new WorkerTypeDto
                //{
                //    Id = wt.Id,
                //    Name = wt.Name
                //}).ToList()
            }).ToList();

            return response;
        }

        public async Task<List<ComplaintTypeWorkerTypeDto>> GetAllComplaintTypeWorkerTypeAsync()
        {
            var mappings = await _context.WorkerTypes
                .Include(wt => wt.ComplaintType)
                .ToListAsync();

            return mappings
                .OrderBy(wt => wt.ComplaintType.Name)
                .Select(wt => new ComplaintTypeWorkerTypeDto
                {
                    ComplaintTypeName = wt.ComplaintType.Name,
                    WorkerTypeName = wt.Name
                }).ToList();
        }
        */

        public async Task CreateMappingAsync(ComplaintTypeWorkerTypeDto dto)
        {
            bool exists = await _repository.MappingExistsAsync(dto.ComplaintTypeName, dto.WorkerTypeName);
            if (exists)
                throw new InvalidOperationException("Mapping already exists.");

            await _repository.CreateAsync(dto);
        }

        public async Task<List<WorkerTypeResponseDto>> GetAllWorkerTypesAsync()
        {
            var workerTypes = await _repository.GetAllWorkerTypesAsync();
            return workerTypes.Select(wt => new WorkerTypeResponseDto
            {
                Id = wt.Id,
                Name = wt.Name
            }).ToList();
        }

        public async Task<List<ComplaintTypeResponseDto>> GetAllComplaintTypesAsync()
        {
            var complaintTypes = await _repository.GetAllComplaintTypesAsync();
            return complaintTypes.Select(ct => new ComplaintTypeResponseDto
            {
                Id = ct.Id,
                Name = ct.Name,
            }).ToList();
        }

        public async Task<List<ComplaintTypeWorkerTypeDto>> GetAllMappingsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> MappingExistsAsync(string complaintTypeName, string workerTypeName)
        {
            return await _repository.MappingExistsAsync(complaintTypeName, workerTypeName);
        }

        public async Task<bool> DeleteMappingAsync(int complaintTypeId, int workerTypeId)
        {
            return await _repository.DeleteAsync(complaintTypeId, workerTypeId);
        }
    }
}
