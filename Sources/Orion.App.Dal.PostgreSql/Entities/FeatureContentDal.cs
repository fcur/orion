using Microsoft.EntityFrameworkCore;

namespace Orion.App.Dal.PostgreSql.Entities;

[Owned]
internal sealed class FeatureContentDal
{
    public string Id { get; set; } = null!;
    public  string Locale { get; set; } = null!;
    public  string Type { get; set; } = null!;
}