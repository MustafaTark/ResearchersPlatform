using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResearchersPlatform_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchersPlatform_DAL.Configurations
{
    public class ResearcherEntityTypeConfigurations : IEntityTypeConfiguration<Researcher> 
    {
        public void Configure(EntityTypeBuilder<Researcher> builder)
        {


            //Many_To_One
            builder.HasMany(r => r.IdeasLeader)
                .WithOne(i => i.ResearcherCreator)
                .HasForeignKey(j => j.CreatorId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(r => r.Requests).WithOne(r => r.ResearcherObject)
                .HasForeignKey(j => j.ResearcherId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(r => r.Papers).WithOne(p => p.ResearcherObject)
                .HasForeignKey(j => j.ResearcherId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(r => r.Invitations).WithOne(p => p.ResearcherObj)
                .HasForeignKey(j => j.ResearcherId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(r => r.ExpertRequests).WithOne(p => p.ResearcherObject)
                .HasForeignKey(j => j.ParticipantId).OnDelete(DeleteBehavior.NoAction);

            //Many_To_Many
            builder.HasMany(r => r.Tasks).WithMany(t => t.Participants);
            builder.HasMany(r => r.Notifications).WithMany(n => n.Researchers);
            builder.HasMany(r => r.Ideas).WithMany(i => i.Participants);

        }
    }
}
