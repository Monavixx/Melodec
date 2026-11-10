namespace Melodec.Core.Model;

public sealed record TrackEntry : Entry
{
    public string Artist { get; }
    public string Title { get; }
    public override string Key => throw new NotImplementedException();

    public override string Display => throw new NotImplementedException();
}
