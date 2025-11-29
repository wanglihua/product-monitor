using Core;
using Domain;
using Domain.PassRes;

namespace Service;

[Service(ServiceType.Scoped)]
public class PassResDataInitService
{
    private readonly Db db;

    public PassResDataInitService(Db db)
    {
        this.db = db;
    }

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
        var passResListInDb = db.Set<PassResEntity>()
            .Where(e => passResIdList.Contains(e.Id))
            .ToList();

        foreach (var passRes in passResList)
        {
            long passResId = passRes.Id;
            var passResInDb = passResListInDb
                .Where(e => e.Id == passResId)
                .Take(1)
                .SingleOrDefault();

            bool shouldSave = false;
            bool isUpdate = true;
            if (passResInDb is null)
            {
                shouldSave = true;
                isUpdate = false;
                passResInDb = new PassResEntity {
                    Id = passRes.Id
                };
            }

            if (passResInDb.CateId != passRes.CateId)
            {
                shouldSave = true;
                passResInDb.CateId = passRes.CateId;
            }

            if (passResInDb.Name != passRes.Name)
            {
                shouldSave = true;
                passResInDb.Name = passRes.Name;
            }

            if (shouldSave == true)
            {
                if (isUpdate == true)
                {
                    db.PassRes.Update(passRes);
                }
                else
                {
                    db.PassRes.Add(passResInDb);
                }
            }
        }

        db.SaveChanges();
    }
}