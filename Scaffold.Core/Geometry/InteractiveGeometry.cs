namespace Scaffold.Geometry
{
    public class InteractiveGeometry : IInteractiveGeometry
    {
        public List<IInteractiveGeometryItem> InteractiveGeometryItems { get; }
        public List<GeometryBase> Geometry { get; }

        public InteractiveGeometry(List<IInteractiveGeometryItem> interactiveItemss, List<GeometryBase> geometryItems)
        {
            InteractiveGeometryItems = interactiveItemss;
            Geometry = geometryItems;
        }
    }
}
