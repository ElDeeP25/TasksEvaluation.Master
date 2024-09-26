using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.IRepositories;
using TasksEvaluation.Core.Mapper;
using TasksEvaluation.Infrastructure.Data;

namespace TasksEvaluation.Infrastructure.Services
{
    public class SolutionService : ISolutionService
    {
        private readonly IBaseMapper<Solution, SolutionDTO> _solutionDTOMapper;
        private readonly IBaseMapper<SolutionDTO, Solution> _solutionMapper;
        private readonly IBaseMapper<UploadSolutionDTO, SolutionDTO> _UploadsolutionDTOMapper;
        private readonly IBaseMapper<SolutionDTO, UploadSolutionDTO> _UploadsolutionMapper;
        private readonly IBaseRepository<Solution> _solutionRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private List<string> _allowedFileExtensions = new() { ".pdf", ".docx", ".jpg" };
        private int _maxAllowedSizeFile = 268436456; // 256MB

        public SolutionService(
            IBaseMapper<Solution, SolutionDTO> solutionDTOMapper,
            IBaseMapper<SolutionDTO, Solution> solutionMapper,
            IBaseRepository<Solution> solutionRepository,
            IWebHostEnvironment webHostEnvironment,
            IBaseMapper<UploadSolutionDTO, SolutionDTO> uploadSolutionDTOMapper,
            IBaseMapper<SolutionDTO, UploadSolutionDTO> uploadSolutionMapper,
            IUnitOfWork unitOfWork,
            ApplicationDbContext context
        )
        {
            _solutionDTOMapper = solutionDTOMapper;
            _solutionMapper = solutionMapper;
            _solutionRepository = solutionRepository;
            _webHostEnvironment = webHostEnvironment;
            _UploadsolutionDTOMapper = uploadSolutionDTOMapper;
            _UploadsolutionMapper = uploadSolutionMapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<SolutionDTO> Create(SolutionDTO model)
        {
            var entity = _solutionMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            return _solutionDTOMapper.MapModel(await _solutionRepository.Create(entity));
        }

        public async Task<SolutionDTO> GetSolution(int id) => _solutionDTOMapper.MapModel(await _solutionRepository.GetById(id));

        public async Task<SolutionStudentDTO> GetSolutionWithStudent(int id)
        {
            var sol = await _solutionRepository.Find(s => s.Id == id, include: source => source.Include(s => s.Student).Include(s => s.Assignment));
            return new SolutionStudentDTO
            {
                Id = sol.Id,
                SolutionFile = sol.SolutionFile,
                Notes = sol.Notes,
                StudentId = sol.StudentId,
                AssignmentId = sol.AssignmentId,
                GradeId = sol.GradeId,
                StudentName = sol.Student?.FullName,
                AssignmentTitle = sol.Assignment?.Title,
            };
        }

        public async Task<IEnumerable<SolutionDTO>> GetSolutions() => _solutionDTOMapper.MapList(await _solutionRepository.GetAll());

        public async Task<IEnumerable<SolutionStudentDTO>> GetStudenSolutions()
        {
            var solutions = await _solutionRepository.FindAll(s => s.Id > 0, include: source => source.Include(s => s.Student).Include(s => s.Assignment).Include(s=> s.Grade ));
            return solutions.Select(solution => new SolutionStudentDTO
            {
                Id = solution.Id,
                SolutionFile = solution.SolutionFile,
                Notes = solution.Notes,
                StudentId = solution.StudentId,
                AssignmentId = solution.AssignmentId,
                GradeId = solution.GradeId,
                StudentName = solution.Student?.FullName,
                AssignmentTitle = solution.Assignment?.Title,
                GardeName = solution.Grade.Grade
            }).ToList();
        }

        public async Task Update(SolutionDTO model)
        {
            var existingEntity = await _solutionRepository.GetById(model.Id);

            if (existingEntity == null)
            {
                throw new InvalidOperationException("Solution not found.");
            }

            existingEntity.Notes = model.Notes;
            existingEntity.GradeId = model.GradeId;

            // Debug output to check the state before updating
            Console.WriteLine($"Updating Solution: Id={existingEntity.Id}, Notes={existingEntity.Notes}, GradeId={existingEntity.GradeId}");

            await _solutionRepository.Update(existingEntity);
        }

        public async Task<SolutionDTO> Update(UploadSolutionDTO model)
        {
            var existingData = await _solutionRepository.GetById(model.Id);
            if (existingData == null)
            {
                return new SolutionDTO { Notes = "Solution not found!" };
            }

            existingData.Notes = model.Notes;
            existingData.GradeId = model.GradeId;

            if (model.SolutionFile != null)
            {
                string extension = Path.GetExtension(model.SolutionFile.FileName);
                if (!_allowedFileExtensions.Contains(extension))
                {
                    throw new Exception("Invalid file extension.");
                }

                if (model.SolutionFile.Length > _maxAllowedSizeFile)
                {
                    throw new Exception("File size exceeds the limit.");
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, model.SolutionFile.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.SolutionFile.CopyToAsync(fileStream);
                }

                existingData.SolutionFile = model.SolutionFile.FileName;
            }

            await _solutionRepository.Update(existingData);
            return _solutionDTOMapper.MapModel(existingData);
        }


        public async Task DeleteSolution(int id)
        {
            var entity = await _solutionRepository.GetById(id);
            if (entity != null)
            {
                var filePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/pdfs", entity.SolutionFile);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                await _solutionRepository.Delete(entity);
            }
        }

        public async Task<SolutionDTO> UploadSolution(UploadSolutionDTO model)
        {
            var extension = Path.GetExtension(model.SolutionFile.FileName);

            if (!_allowedFileExtensions.Contains(extension))
                return new SolutionDTO { Notes = "Only .pdf, .docx files are allowed!" };

            if (model.SolutionFile.Length > _maxAllowedSizeFile)
                return new SolutionDTO { Notes = "File cannot be more than 256 MB!" };

            var fileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/img", fileName);
            using var stream = File.Create(path);
            model.SolutionFile.CopyTo(stream);
            var solution = _UploadsolutionDTOMapper.MapModel(model);
            solution.SolutionFile = fileName;
            var entity = _solutionMapper.MapModel(solution);
            await _unitOfWork.Solutions.Add(entity);
            _unitOfWork.Complete();
            return _solutionDTOMapper.MapModel(entity);
        }

        public async Task<IEnumerable<EvaluationGrade>> GetEvaluationGrades()
        {
            return await _context.EvaluationGrades.ToListAsync();
        }

        public async Task<SolutionDTO> GetSolution(int assignmentId, int studentId)
        {
            var solutions = await _solutionRepository.GetAll();
            var sol = solutions.FirstOrDefault(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
            return _solutionDTOMapper.MapModel(sol);
        }

        public object GetSolutionById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
