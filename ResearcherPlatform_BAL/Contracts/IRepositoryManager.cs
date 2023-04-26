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
        ISectionQuizRepository SectionQuiz { get; }
        ISectionRepository Section { get; }
        IFinalQuizRepository FinalQuiz { get; }
        ISpecialityRepository Speciality { get; }
        IPaperRepository Paper { get; }
        IRequestRepository Request { get; }
        IInvitationRepository Invitation { get; }
        Task SaveChangesAsync();
    }
}
