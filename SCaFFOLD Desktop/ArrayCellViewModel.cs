namespace Scaffold.Desktop;

public class ArrayCellViewModel : ViewModelBase
{
    private readonly double[] _sourceArray;
    private readonly int _index;
    private readonly Action _onValueChanged;

    public ArrayCellViewModel(double[] sourceArray, int index, Action onValueChanged)
    {
        _sourceArray = sourceArray;
        _index = index;
        _onValueChanged = onValueChanged;
    }

    public double Value
    {
        get => _sourceArray[_index];
        set
        {
            if (Math.Abs(_sourceArray[_index] - value) > 0.0001)
            {
                _sourceArray[_index] = value;
                OnPropertyChanged();
                _onValueChanged?.Invoke();
            }
        }
    }
}
