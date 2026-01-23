namespace Scaffold.Core.Geometry
{
    public interface IInteractiveGeometry
    {
        List<IInteractiveGeometryItem> InteractiveGeometryItems { get; }

        List<Abstract.GeometryBase> Geometry { get; }

    }
}
