using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure
{
    public class TestStartup : Startup.Startup
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _databaseName;
        private readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot;

        public TestStartup(IConfiguration configuration, ITestOutputHelper testOutputHelper) : base(configuration)
        {
            _testOutputHelper = testOutputHelper;
            _databaseName = Guid.NewGuid().ToString();
            _inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
        }

        protected override void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            var builder = new TestDeltaDbContextOptionsBuilder(_testOutputHelper, _databaseName, _inMemoryDatabaseRoot);
            builder.ConfigureDbContextOptionsBuilder(options);
        }
    }
}