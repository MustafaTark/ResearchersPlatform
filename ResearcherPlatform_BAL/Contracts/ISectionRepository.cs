﻿using ResearchersPlatform_BAL.DTO;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_BAL.Contracts
{
    public interface ISectionRepository
    {
        Task<IEnumerable<SectionDto>> GetSectionsToCourse(Guid courseId);
        void CreateSectionsToCourse(Guid courseId,List<Section> sections);
        Task<SectionDto?> GetSectionByIdAsync(Guid sectionId, bool trackChanges);
        Task<Section?> GetSingleSectionByIdAsync(Guid sectionId, bool trackChanges);
        void DeleteSection(Section section);
    }
}
