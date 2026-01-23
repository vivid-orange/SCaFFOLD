[AttributeUsage(AttributeTargets.Class)]
public class CalculationMetadataAttribute : Attribute
{
    public string CalculationName { get; set; }
    public string ReferenceName { get; set; }

    /// <summary>
    /// Define type and title using property names.
    /// </summary>
    public CalculationMetadataAttribute() { }

    /// <summary>
    /// Define just the type name of this calculation, independent of the class name.
    /// e.g. Punching Shear to EC2
    /// </summary>
    public CalculationMetadataAttribute(string typeName) => CalculationName = typeName;

    /// <summary>
    /// Define just the type name of this calculation, independent of the class name.
    /// e.g. Column 3A
    /// </summary>
    public CalculationMetadataAttribute(string typeName, string title)
    {
        CalculationName = typeName;
        ReferenceName = title;
    }
}
