using Refit;

namespace Orion.App.Integration.DataProvider.Dto;

public class GetFeedsParams
{
    [AliasAs("auth")]
    public  string Auth { get; set; }
    
    [AliasAs("userId")]
    public string UserId { get; set; }
    
    [Query]
    [AliasAs("page")]
    public  uint Page  { get; set; }
    
     [Query]
    [AliasAs("size")]
    public  uint Size  { get; set; }
    
    [Query]
    [AliasAs("timestamp")]
    public  long TimeStamp { get; set; }
    
}