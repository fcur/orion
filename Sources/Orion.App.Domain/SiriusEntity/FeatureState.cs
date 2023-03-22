namespace Orion.App.Domain.SiriusEntity;

public sealed record FeatureState(
    ContentId ContentId,
    ContentTitle ContentTitle,
    DateTimeOffset StartAt,
    DateTimeOffset? EndAt,
    FeatureContent[] Contents)
{
    public Dictionary<string, object> GetAvailableContent()
    {
        var data = Contents.Select(v => new AvailableContent(v.Type, v.Locale)).ToArray();
        return new Dictionary<string, object>() { { "Contents", data } };
    }

    //public bool Equals(FeatureState? other)
    //{
    //    if (!ReferenceEquals((object)this, other))
    //    {
    //        if (EqualityContract == other?.EqualityContract)
    //        {
    //            var contentIdEq = EqualityComparer<ContentId>.Default.Equals(ContentId, other.ContentId);
    //            var contentTitleEq = EqualityComparer<ContentTitle>.Default.Equals(ContentTitle, other.ContentTitle);
    //            var startAtEq = EqualityComparer<DateTimeOffset>.Default.Equals(StartAt, other.StartAt);
    //            var endAtEq = EqualityComparer<DateTimeOffset?>.Default.Equals(EndAt, other.EndAt);
    //            var contentsEq = EqualityComparer<FeatureContent[]>.Default.Equals(Contents, other.Contents)
    //                || (Contents.Length == other.Contents.Length && !Contents.Except(other.Contents).Any());
    //            return contentIdEq && contentTitleEq && startAtEq && endAtEq && contentsEq;
    //        }
    //        return false;
    //    }

    //    return true;
    //}

    //public override int GetHashCode()
    //{
    //    // return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) 
    //    //          * -1521134295 + EqualityComparer<ContentId>.Default.GetHashCode(ContentId))
    //    //     * -1521134295 + EqualityComparer<ContentTitle>.Default.GetHashCode(ContentTitle)) 
    //    //     * -1521134295 + EqualityComparer<IReadOnlyCollection<FeatureContent>>.Default.GetHashCode(Contents);
    //    return HashCode.Combine(ContentId, ContentTitle, StartAt, EndAt, Contents);
    //}
}