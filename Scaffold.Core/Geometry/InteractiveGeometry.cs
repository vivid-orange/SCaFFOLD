using Scaffold.Core.Geometry.Abstract;

namespace Scaffold.Core.Geometry
{
    public class InteractiveGeometry : IInteractiveGeometry
    {
        public InteractiveGeometry(List<IInteractiveGeometryItem> interactiveItemss, List<GeometryBase> geometryItems)
        {
            InteractiveGeometryItems = interactiveItemss;
            Geometry = geometryItems;
        }

        public List<IInteractiveGeometryItem> InteractiveGeometryItems { get; }

        public List<GeometryBase> Geometry { get; }
    }
}
