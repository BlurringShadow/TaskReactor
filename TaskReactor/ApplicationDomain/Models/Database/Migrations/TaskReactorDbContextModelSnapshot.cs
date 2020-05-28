using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ApplicationDomain.Models.Database.Migrations
{
    [DbContext(typeof(TaskReactorDbContext))]
    class TaskReactorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder) => TaskReactorDbContext.BuildEntity(modelBuilder);
    }
}
