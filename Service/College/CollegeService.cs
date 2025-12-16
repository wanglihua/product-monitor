using Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Service.College;

[Service(ServiceType.Scoped)]
public class CollegeService(Db db)
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public CollegeEntity? GetCollegeByNameOrFormerName(string nameOrFormerName)
    {
        var college = db.College.AsNoTracking()
            .Where(e => e.Name == nameOrFormerName)
            .Take(1)
            .SingleOrDefault();

        if (college is null)
        {
            var collegeChangeLog = db.CollegeChangeLog.AsNoTracking()
                .Where(e => e.FormerName == nameOrFormerName)
                .Take(1)
                .SingleOrDefault();

            if (collegeChangeLog is not null)
            {
                long collegeId = collegeChangeLog.CollegeId;
                college = db.College.AsNoTracking()
                    .Where(e => e.Id == collegeId)
                    .Take(1)
                    .SingleOrDefault();
            }
        }

        return college;
    }
}