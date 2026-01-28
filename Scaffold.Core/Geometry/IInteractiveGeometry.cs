namespace Scaffold.Geometry
{
    public interface IInteractiveGeometry
    {
        List<IInteractiveGeometryItem> InteractiveGeometryItems { get; }

        List<GeometryBase> Geometry { get; }
    }
}
