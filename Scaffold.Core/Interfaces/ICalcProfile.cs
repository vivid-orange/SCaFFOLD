using VividOrange.Taxonomy.Profiles;

namespace Scaffold.Core.Interfaces;

public interface ICalcProfile<TSelf> : ICalcProfile where TSelf : ICalcProfile<TSelf>
{
#if NET7_0_OR_GREATER
    abstract static TSelf CreateFromDescription(string description);
#endif
}

public interface ICalcProfile : IProfile { }
