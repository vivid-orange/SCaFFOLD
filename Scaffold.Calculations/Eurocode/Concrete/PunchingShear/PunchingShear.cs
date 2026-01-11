// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Geometry;
using Scaffold.Core.Geometry.Abstract;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace Scaffold.Calculations.Eurocode.Concrete.PunchingShear
{
    public class PunchingShear : CalculationBase, ICalcValue, IInteractiveGeometry
    {
        public override string InstanceName { get; set; }

        public string Symbol => "";

        [InputCalcValue]
        public CalcSelectionList ColumnCondition { get; } = new CalcSelectionList("Column condition", 0, new List<string> { "INTERNAL", "EDGE", "CORNER", "RE-ENTRANT" });

        [InputCalcValue]
        public EffectiveDepths EffectiveDepthCalc { get; } = new EffectiveDepths();

        [InputCalcValue]
        public CalcSIQuantity<Length> ColumnADimension { get; } = new ("Column A dimension", "A", new Length(400, UnitsNet.Units.LengthUnit.Millimeter));

        [InputCalcValue]
        public CalcSIQuantity<Length> ColumnBDimension { get; } = new("Column B dimension", "B", new Length(400, UnitsNet.Units.LengthUnit.Millimeter));

        [InputCalcValue]
        public CalcListOfDoubleArrays Holes { get; } = new CalcListOfDoubleArrays("Hole positions", "", [[200,200], [300, 300]]);

        List<IInteractiveGeometryItem> _interactiveGeometryItems = new List<IInteractiveGeometryItem>();
        public List<IInteractiveGeometryItem> InteractiveGeometryItems => _interactiveGeometryItems;

        List<GeometryBase> _geometryItems = new List<GeometryBase>();
        public List<GeometryBase> Geometry => _geometryItems;

        public PunchingShear()
        {
            _interactiveGeometryItems.Add(new InteractiveGeometryQuantityOnX(ColumnADimension, 0, true));
            _interactiveGeometryItems.Add(new InteractiveGeometryQuantityOnY(0, ColumnBDimension, true));
            _interactiveGeometryItems.Add(new InteractiveGeometryDoubleArrays("", Holes.Value[0]));
        }

        public override void Calculate()
        {
            _geometryItems.Clear();
            double cx = ColumnADimension.Value / 2;
            double cy = ColumnBDimension.Value / 2;
            _geometryItems.AddRange(CreateContinuousPath(new List<(double x, double y)> { (-cx, -cy), (-cx, cy), (cx, cy), (cx, -cy), (-cx, -cy) }));

            PolyLine controlPerimeter = GeneratePerimeter.generatePerimeter(ColumnADimension.Value, ColumnBDimension.Value, 200, ColumnCondition.Value);

            _geometryItems.Add(controlPerimeter);

        }
       
        public override List<IOutputItem> GetFormulae()
        { return new List<IOutputItem>(); }

        public bool TryParse(string strValue) => throw new NotImplementedException();
        public string GetValueAsString() => throw new NotImplementedException();

        /// <summary>
        /// Converts a list of coordinates into a continuous chain of Line objects.
        /// </summary>
        private static List<Line> CreateContinuousPath(List<(double x, double y)> points)
        {
            var lines = new List<Line>();

            // We need at least 2 points to make a line
            if (points == null || points.Count < 2)
                return lines;

            // Iterate up to the second-to-last point
            for (int i = 0; i < points.Count - 1; i++)
            {
                var current = points[i];
                var next = points[i + 1];

                // Convert doubles to Vector2 (which usually takes floats)
                Vector2 start = new Vector2((float)current.x, (float)current.y);
                Vector2 end = new Vector2((float)next.x, (float)next.y);

                lines.Add(new Line(start, end));
            }

            return lines;
        }
    }


}
