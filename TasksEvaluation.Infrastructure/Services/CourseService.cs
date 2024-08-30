using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.Mapper;

namespace TasksEvaluation.Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IBaseMapper<Course, CourseDTO> _courseDTOMapper;
        private readonly IBaseMapper<CourseDTO, Course> _courseMapper;
        private readonly IBaseRepository<Course> _courseRepository;

        public CourseService(
            IBaseMapper<Course, CourseDTO> courseDTOMapper,
            IBaseMapper<CourseDTO, Course> courseMapper,
            IBaseRepository<Course> courseRepository)
        {
            _courseDTOMapper = courseDTOMapper;
            _courseMapper = courseMapper;
            _courseRepository = courseRepository;
        }

        public async Task<CourseDTO> Create(CourseDTO model)
        {
            var entity = _courseMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            return _courseDTOMapper.MapModel(await _courseRepository.Create(entity));
        }

        public async Task Delete(int id)
        {
            var entity = await _courseRepository.GetById(id);
            await _courseRepository.Delete(entity);
        }

        public async Task<CourseDTO> GetCourse(int id) => _courseDTOMapper.MapModel(await _courseRepository.GetById(id));

        public async Task<IEnumerable<CourseDTO>> GetCourses() => _courseDTOMapper.MapList(await _courseRepository.GetAll());

        public async Task Update(CourseDTO model)
        {
            var existingData = _courseMapper.MapModel(model);
            existingData.UpdateDate = DateTime.Now;
            await _courseRepository.Update(existingData);
        }
    }
}
