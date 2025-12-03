using Core;
using Domain;
using Domain.PassRes;
using Microsoft.EntityFrameworkCore;

namespace Service;

[Service(ServiceType.Scoped)]
public class PassResDataInitService(Db db)
{
    public void InitData()
    {
        var passResList = new List<PassResEntity>();
        passResList.AddRange(PassRes.DataMonitor.AllData);
        passResList.AddRange(PassRes.ProductService.AllData);
        passResList.AddRange(PassRes.ProjectManage.AllData);
        passResList.AddRange(PassRes.BasicSetting.AllData);
        passResList.AddRange(PassRes.SystemManage.AllData);

        // 检查 pass res id 唯一性
        var passResIdMap = new Dictionary<long, PassResEntity>();
        foreach (PassResEntity passRes in passResList)
        {
            if (passResIdMap.TryGetValue(passRes.Id, out PassResEntity? passResExist))
            {
                throw new Exception($"授权资源 {passRes.Name} 和 {passResExist.Name} 两者 id#{passRes.Id} 相重！");
            }

            passResIdMap[passRes.Id] = passRes;
        }

        var passResIdList = passResList.Select(e => e.Id).ToList();
        var passResListInDb = db.PassRes.AsTracking()
            .Where(e => passResIdList.Contains(e.Id))
            .ToList();

        foreach (var passRes in passResList)
        {
            long passResId = passRes.Id;
            var passResInDb = passResListInDb
                .Where(e => e.Id == passResId)
                .Take(1)
                .SingleOrDefault();

            bool shouldInsert = false;
            if (passResInDb is null)
            {
                shouldInsert = true;
                passResInDb = new PassResEntity {
                    Id = passRes.Id
                };
            }

            if (passResInDb.CateId != passRes.CateId)
            {
                passResInDb.CateId = passRes.CateId;
            }

            if (passResInDb.Name != passRes.Name)
            {
                passResInDb.Name = passRes.Name;
            }

            if (shouldInsert == true)
            {
                db.PassRes.Add(passResInDb);
            }
        }

        db.SaveChanges();
    }
}