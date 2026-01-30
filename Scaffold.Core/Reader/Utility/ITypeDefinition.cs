namespace Scaffold.Reader.Utility;

internal interface ITypeDefinition
{
    List<ICalcValue> CreateInputs(object instance);
    List<ICalcValue> CreateOutputs(object instance);
}
