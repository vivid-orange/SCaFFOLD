using MagmaWorks.Taxonomy.Serialization;
using Scaffold.Core.Attributes;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.Abstract;

public abstract class CalcObjectInput<T> : ICalcObjectInput<T>, ITaxonomySerializable where T : ICalcValue
{
    public virtual string DisplayName { get; set; } = _calculationName;
    public virtual CalcStatus Status { get; set; } = CalcStatus.None;

    private static string _calculationName = typeof(T).Name.SplitPascalCaseToString();

    private T _output = default;

    [OutputCalcValue]
    public T Output
    {
        get
        {
            return _output ??= InitialiseOutput();
        }
        set { SetOutput(value); }
    }

    public static implicit operator T(CalcObjectInput<T> value) => value.Output;
    public virtual void Calculate() { }
    protected abstract T InitialiseOutput();
    public virtual IEnumerable<IOutputItem> GetFormulae() => new List<IOutputItem>();
    protected virtual void SetOutput(T value)
    {
        _output = value;
    }

    public bool TryParse(string strValue)
    {
        try
        {
            var obj = strValue.FromJson<CalcObjectInput<T>>();
            Output = obj.Output;
            if (obj.DisplayName != null)
            {
                DisplayName = obj.DisplayName;
            }

            Status = obj.Status;
            return true;
        }
        catch
        {
            return false;
        }
    }
    public string ValueAsString() => this.ToJson();
    public List<ICalcValue> GetInputs() { return null; }
    public List<ICalcValue> GetOutputs() { return null; }
}
