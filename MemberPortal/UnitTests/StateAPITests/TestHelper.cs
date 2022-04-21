using Microsoft.EntityFrameworkCore;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace StateAPITests
{
    public class TestHelper
    {
        private readonly SeiuContext _context;
        public TestHelper()
        {
            var builder = new DbContextOptionsBuilder<SeiuContext>();
            builder.UseInMemoryDatabase(databaseName: "SEIUDbInMemory");

            var dbContextOptions = builder.Options;
            _context = new SeiuContext(dbContextOptions);
            // Delete existing db before creating a new one
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}
