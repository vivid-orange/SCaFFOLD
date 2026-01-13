using System.Numerics;

namespace Scaffold.Core.CalcQuantities;

public static class CalcQuantityExtensions
{
    public static T Sum<T>(this IEnumerable<T> source)
    where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        return source.Aggregate(T.AdditiveIdentity, (acc, item) => item + acc);
    }

    public static T Average<T>(this IEnumerable<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, double, T>
    {
        (T value, int count) result = source.Aggregate(
            (value: T.AdditiveIdentity, count: 0),
            (acc, item) => (value: item + acc.value, count: acc.count + 1));

        return result.value / result.count;
    }
}
