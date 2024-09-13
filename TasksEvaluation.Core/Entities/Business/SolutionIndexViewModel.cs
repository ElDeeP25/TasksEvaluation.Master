using TasksEvaluation.Core.DTOs;

public class SolutionIndexViewModel
{
    public IEnumerable<SolutionDTO> Solutions { get; set; }
    public IEnumerable<EvaluationGradeDTO> Grades { get; set; }
}
