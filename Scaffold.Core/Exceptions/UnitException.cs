namespace Scaffold.Core.Exceptions;

public class UnitsNotSameException : ScaffoldException
{
    public UnitsNotSameException(string displayName1, string displayName2, string unit1, string unit2) :
    base($"Cannot compare two CalcValues with different units ({displayName1}: {unit1} <> {displayName2}: {unit2})")
    { }

    public UnitsNotSameException(IQuantity q1, IQuantity q2) :
    base($"Units not the same. Cannot change {q1.Unit} to {q2.Unit}")
    { }
}
