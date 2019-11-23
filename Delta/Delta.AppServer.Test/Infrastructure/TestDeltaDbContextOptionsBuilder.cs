using System;
using MartinCostello.Logging.XUnit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure
{
    public class TestDeltaDbContextOptionsBuilder
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _databaseName;
        private readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot;

        public TestDeltaDbContextOptionsBuilder(ITestOutputHelper testOutputHelper)
            : this(testOutputHelper, Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
        {
        }

        public TestDeltaDbContextOptionsBuilder(ITestOutputHelper testOutputHelper, string databaseName,
            InMemoryDatabaseRoot inMemoryDatabaseRoot)
        {
            _testOutputHelper = testOutputHelper;
            _databaseName = databaseName;
            _inMemoryDatabaseRoot = inMemoryDatabaseRoot;
        }

        public DbContextOptions<DeltaContext> Build()
        {
            var builder = new DbContextOptionsBuilder<DeltaContext>();
            ConfigureDbContextOptionsBuilder(builder);
            return builder.Options;
        }

        public void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder options)
        {
            var loggerOptions = new XUnitLoggerOptions
            {
                Filter = (_, __) => true,
                IncludeScopes = true
            };

            options
                .UseInMemoryDatabase(_databaseName, _inMemoryDatabaseRoot)
                .UseLoggerFactory(new LoggerFactory(new[] {new XUnitLoggerProvider(_testOutputHelper, loggerOptions)}))
                .UseLazyLoadingProxies()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(w => w.Log(InMemoryEventId.TransactionIgnoredWarning))
                .ConfigureWarnings(w => w.Log(CoreEventId.ManyServiceProvidersCreatedWarning));
        }
    }
}