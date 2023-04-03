using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface IRepositoryManager
    {
        IStudentRepository Student { get; }
        ICourseRepository Course { get; }
        IIdeaRepository Idea { get; }
        ITaskRepository Task { get; }
        IResearcherRepository Researcher { get; }
        Task SaveChangesAsync();
    }
}
