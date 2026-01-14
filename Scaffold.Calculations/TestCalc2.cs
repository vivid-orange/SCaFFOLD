// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Geometry;
using Scaffold.Core.Geometry.Abstract;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using SkiaSharp;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations
{
    public class TestCalc2 : ICalculation, IInteractiveGeometry
    {

        public new string TypeName { get; } = "Test calc";
        public string InstanceName { get; set; } = "This is my test calc";


        [CalcValueType(CalcValueType.Input, "I", "Multiplier")]
        public double Multiplier { get; set; }


        [CalcValueType(CalcValueType.Input, "M", "Moment")]
        public Torque Moment { get; set; } = new Torque(20, TorqueUnit.KilonewtonMeter);


        [CalcValueType(CalcValueType.Input, "L_1", "Length 1")]
        public Length Length { get; set; } = new Length(5, LengthUnit.Millimeter);


        [CalcValueType(CalcValueType.Input, "C_x", "Centre X")]
        public Length Offset1 { get; set; } = new Length(5, LengthUnit.Millimeter);


        [CalcValueType(CalcValueType.Input, "C_y", "Centre Y")]
        public Length Offset2 { get; set; } = new Length(5, LengthUnit.Millimeter);


        [CalcValueType(CalcValueType.Input, "L_2", "Length 2")]
        public Length Length2 { get; set; } = new Length(5, LengthUnit.Millimeter);


        [CalcValueType(CalcValueType.Output, "M_o", "Moment out")]
        public Torque MomentOut { get; private set; } = new Torque(0, TorqueUnit.KilonewtonMeter);


        [CalcValueType(CalcValueType.Output, "F_req", "Force required")]
        public Force ForceRequired{ get; private set; } = new Force(0, ForceUnit.Kilonewton);

        public CalcStatus Status => CalcStatus.None;

        List<IInteractiveGeometryItem> geometry = new List<IInteractiveGeometryItem>();
        public List<IInteractiveGeometryItem> InteractiveGeometryItems => geometry;

        List<GeometryBase> _geometryBases = new List<GeometryBase>();
        public List<GeometryBase> Geometry => _geometryBases;

        public TestCalc2()
        {
            var xg = new InteractiveGeometryQuantityOnXY(
                xGetter: () => this.Offset1.Value,
                xSetter: (newValue) => { this.Offset1 = Length.From(newValue, Length.Unit); },
                yGetter: () => this.Offset2.Value,
                ySetter: (newValue) => { this.Offset2 = Length.From(newValue, Length.Unit); },
                false,
                false
                );
            geometry.Add(xg);

            var xg2 = new InteractiveGeometryQuantityOnXY(
                xGetter: () => 0,
                xSetter: null,
                yGetter: () => this.Length2.Value,
                ySetter: (newValue) => { this.Length2 = Length.From(newValue, Length2.Unit); },
                true,
                true,
                xOffset: () => this.Offset1.Value,
                yOffset: () => this.Offset2.Value
                );
            geometry.Add(xg2);

            var xg3 = new InteractiveGeometryQuantityOnXY(
                xGetter: () => this.Length.Value,
                xSetter: (newValue) => { this.Length = Length.From(newValue, Length.Unit); },
                yGetter: () => 0,
                ySetter: null,
                true,
                true,
                xOffset: () => this.Offset1.Value,
                yOffset: () => this.Offset2.Value
                );
            geometry.Add(xg3);

        }



        public void Calculate()
        {
            MomentOut = Moment * Multiplier;

            ForceRequired = (Moment / Length).ToUnit(ForceUnit.Kilonewton);

            var lines = new List<Line>();
            var topLeft = (Offset1.Value - Length.Value / 2, Offset2.Value + Length2.Value/2 );
            var topRight = ( Offset1.Value + Length.Value / 2, Offset2.Value + Length2.Value/2 );
            var bottomRight = (Offset1.Value + Length.Value / 2, Offset2.Value - Length2.Value / 2 );
            var bottomLeft = (Offset1.Value - Length.Value / 2, Offset2.Value - Length2.Value / 2 );
            lines.AddRange(CreateContinuousPath(new List<(double x, double y)> { topLeft, topRight, bottomRight, bottomLeft, topLeft }));

            _geometryBases.Clear();
            _geometryBases.AddRange(lines);

        }
        public List<IOutputItem> GetFormulae()
        {
            var returnList = new List<IOutputItem>();

            var outputs = new OutputItem("reffy", "This one goes first", "Done", new TextItem("We can explain a bit about the formula here. There is no longer a separate 'Narrative' property."));
            outputs.Expressions.Add(new LatexItem(@"M = \frac{wl^2} {8}"));
            outputs.Expressions.Add(new TextItem("and then a bit more text whcih can now be in-line", true));
            outputs.Expressions.Add(new TextItem("and then an image"));
            //outputs.Expressions.Add(new ImageOutputItem(new ImageFromSkBitmap(Utilities.CreateMultiCircleImage(Coordinates.Value, SKColors.Orange)), true));
            outputs.Expressions.Add(new TextItem("and then another formula", true));
            outputs.Expressions.Add(new LatexItem(@"E = mc^2"));
            outputs.Expressions.Add(new TextItem("all of which can be set to in-line or new line"));

            returnList.Add(outputs);

            return returnList;
        }

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
