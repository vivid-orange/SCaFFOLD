using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.Geometry;
using Scaffold.Core.Geometry.Abstract;

namespace Scaffold.Calculations.Eurocode.Concrete.PunchingShear
{
    public static class GeneratePerimeter
    {
        public static PolyLine generatePerimeter(double width, double depth, double offset, string colType)
        {
            float offsetF = (float)offset;
            float x = (float)width / 2f;
            float y = (float)depth / 2f;
            var perimeter = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(0, y + offsetF), new Vector2(-x, y+offsetF)),
                new Arc(){Centre = new Vector2(-x,y), Radius=offsetF, StartAngle=Math.PI/2, EndAngle=Math.PI},
                new Line(new Vector2(-x-offsetF, y), new Vector2(-x-offsetF, -y)),
                new Arc(){Centre = new Vector2(-x,-y), Radius=offsetF, StartAngle=Math.PI, EndAngle=1.5*Math.PI},
                new Line(new Vector2(-x, - y - offsetF), new Vector2(x, - y - offsetF)),
                new Arc(){Centre = new Vector2(x,-y), Radius=offsetF, StartAngle=1.5*Math.PI, EndAngle=2*Math.PI},
                new Line(new Vector2(x + offsetF, - y), new Vector2(x +offsetF, y)),
                new Arc(){Centre = new Vector2(x,y), Radius=offsetF, StartAngle=0, EndAngle=Math.PI/2},
                new Line(new Vector2(x, y + offsetF), new Vector2(0, y + offsetF)),
            });

            List<IntersectionResult> inter2;
            List<IntersectionResult> inter1;
            PolyLine perimeter2 = perimeter;

            switch (colType)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    inter2 = perimeter.intersection(new Line(new Vector2(-x, -10000), new Vector2(-x, 0)));
                    inter1 = perimeter.intersection(new Line(new Vector2(-x, 0), new Vector2(-x, 10000)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                case "CORNER":
                    inter2 = perimeter.intersection(new Line(new Vector2(-x, -10000), new Vector2(-x, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x, y), new Vector2(10000, y)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                case "RE-ENTRANT":
                    inter2 = perimeter.intersection(new Line(new Vector2(-10000, y), new Vector2(-x, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(-x, y), new Vector2(-x, 10000)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                default:
                    break;
            }

            return perimeter2;
        }
    }
}
