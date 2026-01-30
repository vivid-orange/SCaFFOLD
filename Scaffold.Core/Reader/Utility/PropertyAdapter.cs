using System.Linq.Expressions;
using System.Reflection;

namespace Scaffold.Reader.Utility;

internal class PropertyAdapter<TModel, TProp> : IPropertyAdapter
{
    private readonly Func<TModel, TProp> _getter;
    private readonly Action<TModel, TProp> _setter;
    private readonly string _symbol;
    private readonly string _displayName;
    private readonly string[] _headings;

    public PropertyAdapter(PropertyInfo prop, CalcParameterAttribute attr)
    {
        _symbol = attr.Symbol;
        _displayName = attr.EntityLabel ?? prop.Name;
        _headings = attr.Headings;

        // Compile Getter
        ParameterExpression param = Expression.Parameter(typeof(TModel), "m");
        MemberExpression access = Expression.Property(param, prop);
        _getter = Expression.Lambda<Func<TModel, TProp>>(access, param).Compile();

        // Compile Setter
        if (prop.CanWrite && prop.GetSetMethod() != null)
        {
            ParameterExpression valueParam = Expression.Parameter(typeof(TProp), "v");
            MethodCallExpression assign = Expression.Call(param, prop.GetSetMethod(), valueParam);
            _setter = Expression.Lambda<Action<TModel, TProp>>(assign, param, valueParam).Compile();
        }
    }

    public ICalcValue Create(object instance)
    {
        TModel model = (TModel)instance;

        // Create closures around the specific instance
        Func<TProp> boundGetter = () => _getter(model);
        Action<TProp> boundSetter = _setter == null ? null : (v) => _setter(model, v);

        return new DelegateCalcValue<TProp>(
            boundGetter,
            boundSetter,
            _symbol,
            _displayName,
            _headings);
    }
}
