using Core;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Service;

[Service(ServiceType.Scoped)]
public class PassResCateDataInitService(Db db)
{
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
        var cateListInDb = db.PassResCate.AsTracking()
            .Where(e => cateIdList.Contains(e.Id))
            .ToList();

        foreach (var cate in cateList)
        {
            long cateId = cate.Id;
            var cateInDb = cateListInDb
                .Where(e => e.Id == cateId)
                .Take(1)
                .SingleOrDefault();

            bool shouldInsert = false;
            if (cateInDb is null)
            {
                shouldInsert = true;
                cateInDb = new PassResCateEntity {
                    Id = cate.Id
                };
            }

            if (cateInDb.Name != cate.Name)
            {
                cateInDb.Name = cate.Name;
            }

            if (shouldInsert == true)
            {
                db.PassResCate.Add(cateInDb);
            }
        }

        db.SaveChanges();
    }
}