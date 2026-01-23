using VividOrange.Profiles;

public interface ICalcProfile<TSelf> : ICalcProfile where TSelf : ICalcProfile<TSelf>
{
    abstract static TSelf CreateFromDescription(string description);
}

public interface ICalcProfile : IProfile { }
