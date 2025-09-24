using Core;
using Core.Config;
using Microsoft.EntityFrameworkCore;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Domain;

public class Db : DbContext
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        log.Warn("Db.OnConfiguring");

        base.OnConfiguring(optionsBuilder);

        // 数据 migration 和 单元测试时会用到这个逻辑
        if (string.IsNullOrWhiteSpace(DatabaseConfig.ConnectionString) == true)
        {
            DatabaseConfig.ReadConfig(StaticConfigurationManager.Configuration);
        }

        optionsBuilder.UseSqlServer(DatabaseConfig.ConnectionString);
        // 这里不用这个了 optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution); // 对 no tracking 的 entity Update 时 会更新所有字段

#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        log.Warn("Db.OnModelCreating");

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Dummy).Assembly);
    }

    /// <summary>
    /// 数据库主键都是long，防止用int参数Find()时有异常
    /// </summary>
    public T? Find<T>(int id) where T : class
    {
        return base.Find<T>((long)id);
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
                if (entityEntry.Entity is Entity entity)
                {
                    entity.Id = EntityIdGenerator.NewId();
                    entity.CreateTime = DateTime.Now;
                    entity.UpdateTime = DateTime.Now;
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
            }

            if (entityEntry.State == EntityState.Modified)
            {
                if (entityEntry.Entity is Entity entity)
                {
                    entity.UpdateTime = DateTime.Now;
                }
            }
        }

        return base.SaveChanges();
    }
}