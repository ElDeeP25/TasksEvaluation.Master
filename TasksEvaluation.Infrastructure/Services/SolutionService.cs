using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.IRepositories;
using TasksEvaluation.Core.Mapper;

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

        private List<string> _allowedFileExtensions = new() { ".pdf", ".docx" };
        private int _maxAllowedSizeFile = 5242880;

        public SolutionService(
            IBaseMapper<Solution, SolutionDTO> solutionDTOMapper,
            IBaseMapper<SolutionDTO, Solution> solutionMapper,
            IBaseRepository<Solution> solutionRepository,
          IWebHostEnvironment webHostEnvironment ,
          IBaseMapper<UploadSolutionDTO, SolutionDTO> UploadsolutionDTOMapper,
          IBaseMapper<SolutionDTO, UploadSolutionDTO> UploadsolutionMapper,
          IUnitOfWork unitOfWork

          )
        {
            _solutionDTOMapper = solutionDTOMapper;
            _solutionMapper = solutionMapper;
            _solutionRepository = solutionRepository;
            _webHostEnvironment = webHostEnvironment;
            _UploadsolutionDTOMapper= UploadsolutionDTOMapper;
            _UploadsolutionMapper = UploadsolutionMapper;
            _unitOfWork=unitOfWork;
        }

        public async Task<SolutionDTO> Create(SolutionDTO model)
        {
            var entity = _solutionMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            return _solutionDTOMapper.MapModel(await _solutionRepository.Create(entity));
        }


        public async Task<SolutionDTO> GetSolution(int id) => _solutionDTOMapper.MapModel(await _solutionRepository.GetById(id));
        public async Task<SolutionStudentDTO> GetSolutionWithStudent(int id) {

           var sol= await _solutionRepository.Find(s => s.Id == id, include: source => source.Include(s => s.Student).Include(s => s.Assignment));
            var  solution = new SolutionStudentDTO
            {
                Id = sol.Id,
                SolutionFile = sol.SolutionFile,
                Notes = sol.Notes,
                StudentId = sol.StudentId,
                AssignmentId = sol.AssignmentId,
                GradeId = sol.GradeId,
                StudentName = sol.Student?.FullName, // Mapping Student's name
                AssignmentTitle = sol.Assignment?.Title , // Mapping Assignment's title


            };
            return solution;
        } 
        public async Task<IEnumerable<SolutionDTO>> GetSolutions() => _solutionDTOMapper.MapList(await _solutionRepository.GetAll());
        public async Task<IEnumerable<SolutionStudentDTO>> GetStudenSolutions()
        {
            var solutions = await _solutionRepository.FindAll(s => s.Id > 0, include: source => source.Include(s => s.Student).Include(s => s.Assignment));
            return solutions.Select(solution => new SolutionStudentDTO
            {
                Id = solution.Id,
                SolutionFile = solution.SolutionFile,
                Notes = solution.Notes,
                StudentId = solution.StudentId,
                AssignmentId = solution.AssignmentId,
                GradeId = solution.GradeId,
                StudentName = solution.Student?.FullName, // Mapping Student's name
                AssignmentTitle = solution.Assignment?.Title , // Mapping Assignment's title

            }).ToList();
        }
        public async Task Update(SolutionDTO model)
        {
            // Retrieve the existing entity from the repository
            var existingEntity = await _solutionRepository.GetById(model.Id);

            if (existingEntity == null)
            {
                throw new InvalidOperationException("Solution not found.");
            }

            // Manually update the properties of the existing entity
            existingEntity.Notes = model.Notes;
            existingEntity.GradeId = model.GradeId;

            // Perform other property updates as needed

            // Update the entity in the repository
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
            existingData.UpdateDate = DateTime.Now;

            if (model.SolutionFile != null)
            {
                var extension = Path.GetExtension(model.SolutionFile.FileName);

                if (!_allowedFileExtensions.Contains(extension))
                {
                    return new SolutionDTO { Notes = "Only .pdf, .docx files are allowed!" };
                }

                if (model.SolutionFile.Length > _maxAllowedSizeFile)
                {
                    return new SolutionDTO { Notes = "File cannot be more than 5 MB!" };
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/pdfs", fileName);

                using var stream = File.Create(path);
                model.SolutionFile.CopyTo(stream);

                // Delete the old file
                var oldFilePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/pdfs", existingData.SolutionFile);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                existingData.SolutionFile = fileName;
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
                return new SolutionDTO { Notes = "File cannot be more than 5 MB!" };

            var fileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/pdfs", fileName);
            using var stream = File.Create(path);
            model.SolutionFile.CopyTo(stream);
            var solution = _UploadsolutionDTOMapper.MapModel(model);
            solution.SolutionFile = fileName;
            var entity = _solutionMapper.MapModel(solution);
            await _unitOfWork.Solutions.Add(entity);
            _unitOfWork.Complete();
            return _solutionDTOMapper.MapModel(entity);

        }



        public async Task<SolutionDTO> GetSolution(int assignmentId, int studentId)
        {
            var solutions = await _solutionRepository.GetAll(); // Ensure this fetches all solutions or use a more optimized query
            var sol = solutions.FirstOrDefault(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
            return _solutionDTOMapper.MapModel(sol);
        }


    }
}
