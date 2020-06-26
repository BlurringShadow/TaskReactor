using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Data.Database.Migration
{
    [DbContext(typeof(TaskReactorDbContext))]
    class TaskReactorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder) => TaskReactorDbContext.BuildEntity(modelBuilder);
    }
}