using Microsoft.EntityFrameworkCore;

namespace Domain;

public partial class Db
{
    public DbSet<ActionLogEntity> ActionLog { get; set; }

    public DbSet<AdminUserEntity> AdminUser { get; set; }

    public DbSet<AuthResultEntity> AuthResult { get; set; }

    public DbSet<ClientKeyEntity> ClientKey { get; set; }

    public DbSet<CollegeChangeLogEntity> CollegeChangeLog { get; set; }

    public DbSet<CollegeEntity> College { get; set; }

    public DbSet<ExceptionMonitorEntity> ExceptionMonitor { get; set; }

    public DbSet<GroupEntity> Group { get; set; }

    public DbSet<GroupPassResEntity> GroupPassRes { get; set; }

    public DbSet<GroupUserEntity> GroupUser { get; set; }

    public DbSet<PassResCateEntity> PassResCate { get; set; }

    public DbSet<PassResEntity> PassRes { get; set; }

    public DbSet<ProductEntity> Product { get; set; }

    public DbSet<ProductEvalEntity> ProductEval { get; set; }

    public DbSet<ProductEvalEntryEntity> ProductEvalEntry { get; set; }

    public DbSet<ProductInstallEntity> ProductInstall { get; set; }
    
    public DbSet<ProductInstallAttachmentEntity> ProductInstallAttachment { get; set; }

    public DbSet<ProductInstallMonitorEntity> ProductInstallMonitor { get; set; }

    public DbSet<ProductInstallMonitorEntryEntity> ProductInstallMonitorEntry { get; set; }

    public DbSet<ProductRentEntity> ProductRent { get; set; }

    public DbSet<ProductRentEntryEntity> ProductRentEntry { get; set; }

    public DbSet<ProductSaleEntity> ProductSale { get; set; }

    public DbSet<ProductSaleEntryEntity> ProductSaleEntry { get; set; }

    public DbSet<ProductServiceEntity> ProductService { get; set; }

    public DbSet<ProductSpecialServiceEntity> ProductSpecialService { get; set; }

    public DbSet<ProductUpdateEntity> ProductUpdate { get; set; }

    public DbSet<ProductUrlEntity> ProductUrl { get; set; }

    public DbSet<ProductUrlMonitorEntity> ProductUrlMonitor { get; set; }

    public DbSet<ProductUrlMonitorEntryEntity> ProductUrlMonitorEntry { get; set; }

    public DbSet<ProductVersionEntity> ProductVersion { get; set; }

    public DbSet<TestEntity> Test { get; set; }

    public DbSet<UserEntity> User { get; set; }

    public DbSet<UserPassResEntity> UserPassRes { get; set; }

    public DbSet<UserPassResEntity> UserPassResEntity { get; set; }

    public DbSet<UserProvinceEntity> UserProvince { get; set; }

    public DbSet<WebSessionDataEntity> WebSessionData { get; set; }

    public DbSet<WebSessionEntity> WebSession { get; set; }
}