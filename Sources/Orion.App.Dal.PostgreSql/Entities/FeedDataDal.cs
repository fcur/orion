using Microsoft.EntityFrameworkCore;

namespace Orion.App.Dal.PostgreSql.Entities;

[Owned]
internal sealed class FeedDataDal
{
    public  string? Ax { get; set; }
    public  string? Bx { get; set; }
    public  string? Cx { get; set; }
    public  string? Dx { get; set; }
}