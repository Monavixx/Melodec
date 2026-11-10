namespace Melodec.Core.Model;

public abstract record Entry
{
    public abstract string Key { get; }
    public abstract string Display { get; }
}
