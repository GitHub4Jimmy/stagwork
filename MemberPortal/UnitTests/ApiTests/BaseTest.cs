using Microsoft.EntityFrameworkCore;
using StagwellTech.SEIU.CommonCoreEntities.Data;

namespace StagwellTech.SEIU.CommonCoreEntities.Services.UnitTests
{
    public class BaseTest
    {
        protected SeiuContext Context { get; set; }

        protected void BuildContext()
        {
            var builder = new DbContextOptionsBuilder<SeiuContext>();
            builder.UseInMemoryDatabase("testDb");
            var options = builder.Options;
            Context = new SeiuContext(options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

    }
}