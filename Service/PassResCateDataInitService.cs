using Core;
using Domain;

namespace Service;

[Service(ServiceType.Scoped)]
public class PassResCateDataInitService
{
    private readonly Db db;

    public PassResCateDataInitService(Db db)
    {
        this.db = db;
    }

    public void InitData()
    {
        var cateList = PassResCate.GetLst();

        // 检查 cate id 唯一性
        var cateIdMap = new Dictionary<long, PassResCateEntity>();
        foreach (var cate in cateList)
        {
            if (cateIdMap.TryGetValue(cate.Id, out PassResCateEntity? cateExist))
            {
                throw new Exception($"授权资源类别 {cate.Name} 和 {cateExist.Name} 两者 id#{cate.Id} 相重！");
            }

            cateIdMap[cate.Id] = cate;
        }

        var cateIdList = cateList.Select(e => e.Id).ToList();
        var cateListInDb = db.Set<PassResCateEntity>()
            .Where(e => cateIdList.Contains(e.Id))
            .ToList();

        foreach (var cate in cateList)
        {
            long cateId = cate.Id;
            var cateInDb = cateListInDb
                .Where(e => e.Id == cateId)
                .Take(1)
                .SingleOrDefault();

            bool shouldSave = false;
            bool isUpdate = true;
            if (cateInDb is null)
            {
                shouldSave = true;
                isUpdate = false;
                cateInDb = new PassResCateEntity {
                    Id = cate.Id
                };
            }

            if (cateInDb.Name != cate.Name)
            {
                shouldSave = true;
                cateInDb.Name = cate.Name;
            }

            if (shouldSave == true)
            {
                if (isUpdate == true)
                {
                    db.Update<PassResCateEntity>(cateInDb);
                }
                else
                {
                    db.PassResCate.Add(cateInDb);
                }
            }
        }

        db.SaveChanges();
    }
}