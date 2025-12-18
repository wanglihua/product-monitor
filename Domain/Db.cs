using Core;
using Core.Config;
using Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Domain;

public partial class Db : DbContext
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public Db() //  : base() 这个调用是多余的
    {
    }

    // services.AddDbContext<Db>(optionsBuilder => {
    //     // 在这里配置，同时配合Db(DbContextOptions options)构造器，和DbContextOptionsBuilder.IsConfigured判断
    //     // 可以防止 Db.OnConfiguring() 中重复配置
    //     Db.Configure(optionsBuilder);
    // });
    public Db(DbContextOptions options) : base(options)
    {
    }

    public static void Configure(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrWhiteSpace(DatabaseConfig.ConnectionString) == true)
        {
            DatabaseConfig.ReadConfig(StaticConfigurationManager.Configuration);
        }

        int compatibilityLevel = SqlServerCompatibilityLevel.GetByVersion(DatabaseConfig.SqlVersion);

        optionsBuilder.UseSqlServer(DatabaseConfig.ConnectionString, options => {
            options.UseCompatibilityLevel(compatibilityLevel);

            if (compatibilityLevel == SqlServerCompatibilityLevel.SqlServer2008)
            {
                options.UseRowNumberForPaging();
            }
        });

        if (compatibilityLevel == SqlServerCompatibilityLevel.SqlServer2008)
        {
            // 好像这个注释掉也没事，还是先放着吧
            optionsBuilder.ReplaceService<IQueryTranslationPostprocessorFactory, SqlServer2008QueryTranslationPostprocessorFactory>();
        }

#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
#endif
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // base.OnConfiguring(optionsBuilder); 没必要调用，基类中啥都没做

        if (optionsBuilder.IsConfigured == false)
        {
            log.Info("Db.OnConfiguring call Configure()");

            Configure(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        log.Info("Db.OnModelCreating");

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Dummy).Assembly);
    }

    public override int SaveChanges()
    {
        var entityEntryList = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .ToList();

        foreach (var entityEntry in entityEntryList)
        {
            if (entityEntry.State == EntityState.Added)
            {
                // Entity
                if (entityEntry.Entity is Entity entity)
                {
                    entity.Id = EntityIdGenerator.NewId();

                    DateTime now = DateTime.Now;
                    entity.CreateTime = now;
                    entity.UpdateTime = now;
                }
                else if (entityEntry.Entity is EntityNoUpdate entityNoUpdate)
                {
                    entityNoUpdate.Id = EntityIdGenerator.NewId();
                    entityNoUpdate.CreateTime = DateTime.Now;
                }
                else if (entityEntry.Entity is EntityNoCreateNoUpdate entityNoCreateNoUpdate)
                {
                    entityNoCreateNoUpdate.Id = EntityIdGenerator.NewId();
                }

                // EntityAssignedId
                if (entityEntry.Entity is EntityAssignedId entityAssignedId)
                {
                    DateTime now = DateTime.Now;
                    entityAssignedId.CreateTime = now;
                    entityAssignedId.UpdateTime = now;
                }
                else if (entityEntry.Entity is EntityAssignedIdNoUpdate entityAssignedIdNoUpdate)
                {
                    entityAssignedIdNoUpdate.CreateTime = DateTime.Now;
                }
            }

            if (entityEntry.State == EntityState.Modified)
            {
                // Entity
                if (entityEntry.Entity is Entity entity)
                {
                    entity.UpdateTime = DateTime.Now;
                }

                // EntityAssignedId
                if (entityEntry.Entity is EntityAssignedId entityAssignedId)
                {
                    entityAssignedId.UpdateTime = DateTime.Now;
                }
            }
        }

        return base.SaveChanges();
    }
}