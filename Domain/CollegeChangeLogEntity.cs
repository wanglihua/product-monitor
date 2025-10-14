using Core;

namespace Domain;

public class CollegeChangeLogEntity : EntityNoUpdate
{
    public long CollegeId { get; set; }

    public string ChangeDesc { get; set; } = "";
}