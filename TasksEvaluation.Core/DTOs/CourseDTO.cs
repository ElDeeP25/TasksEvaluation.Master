using System;

namespace TasksEvaluation.Core.DTOs
{
    public class CourseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public string ImageUrl { get; set; }
        public CourseType Type { get; set; }
    }

    public enum CourseType
    {
        Frontend,
        Backend,
        Design,
        Other
    }
}
