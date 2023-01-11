using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orion.App.Dal.PostgreSql.Entities;

[Table("Feeds")]
internal sealed class FeedDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    
    public string FeedId { get; set; } = null!;
    
    public  string FeedName { get; set; }= null!;
    
    public  DateTimeOffset StartAt { get; set; }
    
    [Column(TypeName = "jsonb")]
    public FeedDataDal[] Data { get; set; } = null!;
    
    public long Version { get; set; }
    
    [Timestamp]
    public uint ConcurrencyToken { get; set; }
    
    internal static void ConfigureModel(EntityTypeBuilder<FeedDal> entityTypeBuilder)
    {
        entityTypeBuilder.HasIndex(v => v.FeedId);
    }
}