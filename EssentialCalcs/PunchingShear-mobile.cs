using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using CalcCore;
using MWGeometry;
//using netDxf.Entities;
//using netDxf;
using netDxf.Tables;
using StructuralDrawing2D;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using Point = TriangleNet.Geometry.Point;

namespace EssentialCalcs
{
    [CalcName("Punching Shear to EC2")]
    [CalcAlternativeName("TestCalcs.PunchingShear")]
    [CalcAlternativeName("EssentialCalcs.PunchingShear")]
    public class PunchingShear : CalcCore.CalcBase
    {
        CalcSelectionList _colType;
        CalcSelectionList _linkArrangement;
        CalcDouble _fck;
        CalcSelectionList _loadCombType;
        CalcDouble _concPartFactor;
        CalcDouble _columnAdim;
        CalcDouble _columnBdim;
        CalcCore.CalcDouble _punchingLoad;
        CalcDouble _mz;
        CalcDouble _my;
        double udl = 0;
        CalcDouble _h;
        CalcDouble _offsety;
        CalcDouble _offsetz;
        double dy;
        double dz;
        double d_average;
        double ui;
        CalcCore.CalcDouble _beta;
        CalcDouble _stressvEdi;
        double stressvEd1;
        double reinforcementRatioy;
        double reinforcementRatioz;
        double vRdc;
        double vRdMax;
        double u1;
        CalcDouble _Asw;
        CalcDouble _rebarY;
        CalcDouble _rebarZ;
        CalcDouble _rho;
        CalcSelectionList _srMode;
        CalcDouble _srTarget;
        CalcSelectionList _stMode;
        CalcDouble _stTarget;
        double sr;
        double st;
        double fywd;
        double fywdef;
        CalcDouble _uoutef;
        CalcDouble _distToFirstLinkPerim;
        CalcDouble _distToOuterPerim;
        CalcDouble _perimSpacing;
        CalcDouble _linksInFirstPerim;
        CalcDouble _numberOfPerimeters;
        CalcDouble _legsTotal;
        CalcDouble _legDia;
        CalcDouble _holePosX;
        CalcDouble _holePosY;
        CalcDouble _holeSizeX;
        CalcDouble _holeSizeY;
        CalcDouble _hole2PosX;
        CalcDouble _hole2PosY;
        CalcDouble _hole2SizeX;
        CalcDouble _hole2SizeY;
        CalcDouble _hole3PosX;
        CalcDouble _hole3PosY;
        CalcDouble _hole3SizeX;
        CalcDouble _hole3SizeY;
        CalcSelectionList _includeLinksBeyondHole;
        CalcListOfDoubleArrays _studPositions;
        CalcSelectionList _maxBarSize;
        CalcSelectionList _minBarSize;
        CalcSelectionList _faceCheck;
        CalcSelectionList _betaCheck;
        CalcDouble _betaProposed;
        SkiaSharp.SKBitmap _fig6_13;
        SkiaSharp.SKBitmap _fig6_14;
        SkiaSharp.SKBitmap _fig6_20;
        SkiaSharp.SKBitmap _fig6_22;
        SkiaSharp.SKBitmap _figConcCentre;
        SkiaSharp.SKBitmap _figRadialLayout;

        List<Formula> expressions = new List<Formula>();
        List<Tuple<Line, Line>> _holeEdges;
        List<PolyLine> controlPerimeters;
        PolyLine controlPerimeterNoHoles;
        List<PolyLine> outerPerimeters;
        PolyLine columnOutline;
        PolyLine fullColumnOutline;
        List<PolyLine> columnOutlines;
        List<PolyLine> holeOutlines;
        List<List<Vector2>> allHoleCorners;
        PolyLine slabEdge;
        List<PolyLine> shearLinkStartPerimeters;
        List<PolyLine> shearLinkEndPerimeters;
        List<Line> shearSpurs;
        List<List<Vector2>> shearLinks;
        List<List<PolyLine>> allPerimeters;
        List<List<PolyLine>> perimetersToReinforce;
        List<PolyLine> u1reducedNoHoles;
        List<PolyLine> u1reduced;


        public PunchingShear()
        {
            _typeName = "Punching Shear to EC2";
            InstanceName = "My column";
            _drawings = new List<netDxf.DxfDocument>();

            _colType = inputValues.CreateCalcSelectionList("Column condition", "INTERNAL", new List<string> { "INTERNAL", "EDGE", "CORNER", "RE-ENTRANT" });
            _linkArrangement = inputValues.CreateCalcSelectionList("Shear link arrangement", "GRID", new List<string> { "SPURS_AUTO", "GRID", "CRUCIFORM" });
            _srMode = inputValues.CreateCalcSelectionList("Radial spacing", "AUTO", new List<string> { "AUTO", "TARGET" });
            _srTarget = inputValues.CreateDoubleCalcValue("Target radial spacing", "s_r", "mm", 0);
            _stMode = inputValues.CreateCalcSelectionList("Tangential spacing", "AUTO", new List<string> { "AUTO", "TARGET" });
            _stTarget = inputValues.CreateDoubleCalcValue("Target tangential spacing", "s_t", "mm", 0);
            _beta = outputValues.CreateDoubleCalcValue("Beta value", @"\beta", "", 2);
            _faceCheck = inputValues.CreateCalcSelectionList("Face shear", "CHECK", new List<string> { "CHECK", "IGNORE" });
            _columnAdim = inputValues.CreateDoubleCalcValue("Column A dimension", "A", "mm", 350);
            _columnBdim = inputValues.CreateDoubleCalcValue("Column B dimension", "B", "mm", 350);
            _h = inputValues.CreateDoubleCalcValue("Slab depth", "h", "mm", 225);
            _offsety = inputValues.CreateDoubleCalcValue("Offset to effective depth y dir", "d_{y,offset}", "mm", 45);
            _offsetz = inputValues.CreateDoubleCalcValue("Offset to effective depth z dir", "d_{z,offset}", "mm", 65);
            _rebarY = inputValues.CreateDoubleCalcValue("Tension reinforcement y dir", "A_{s,y}", @"mm^2/m", 1695);
            _rebarZ = inputValues.CreateDoubleCalcValue("Tension reinforcement z dir", "A_{s,z}", @"mm^2/m", 1695);
            _my = inputValues.CreateDoubleCalcValue("Moment about y axis", "M_{Ed,y}", "kNm", 33);
            _mz = inputValues.CreateDoubleCalcValue("Moment about z axis", "M_{Ed, z}", "kNm", 90);
            _fck = inputValues.CreateDoubleCalcValue("Concrete strength", @"f_{ck}", @"N/{mm^2}", 40);
            _loadCombType = inputValues.CreateCalcSelectionList("Design situation", "PERMANENT", new List<string> { "PERMANENT", "ACCIDENTAL" });
            _concPartFactor = outputValues.CreateDoubleCalcValue("Partial factor for concrete", @"\gamma_c", "", 1.5);
            _rho = outputValues.CreateDoubleCalcValue("Reinforcement ratio", @"\rho_l", "", 0);
            _punchingLoad = inputValues.CreateDoubleCalcValue("Punching shear load", "V_{Ed}", "kN", 457);
            _betaCheck = inputValues.CreateCalcSelectionList("Beta calculation", "AUTO", new List<string> { "AUTO", "CODE_DEFAULT", "MANUAL" });
            _betaProposed = inputValues.CreateDoubleCalcValue("Proposed beta factor", @"\beta_{prop}", "", 1.5);
            _stressvEdi = outputValues.CreateDoubleCalcValue("Shear stress at column face", @"v_{Ed,0}", @"N/{mm^2} ", 0);
            _Asw = outputValues.CreateDoubleCalcValue("Punching shear leg area required per perimeter", "A_{sw}", "mm^2", 0);
            _uoutef = outputValues.CreateDoubleCalcValue("Outer perimeter required", "u_{out,ef,req}", "mm", 0);
            _linksInFirstPerim = outputValues.CreateDoubleCalcValue("Number of spurs", "", "No.", 0);
            _numberOfPerimeters = outputValues.CreateDoubleCalcValue("Legs per spur", "", "No.", 0);
            _perimSpacing = outputValues.CreateDoubleCalcValue("Spacing of link perimeters", "", "mm", 0);
            _distToFirstLinkPerim = outputValues.CreateDoubleCalcValue("Distance to first link perimeter", "", "mm", 0);
            _distToOuterPerim = outputValues.CreateDoubleCalcValue("Distance to outer perimeter", "", "mm", 0);
            _legsTotal = outputValues.CreateDoubleCalcValue("Total legs", "", "No.", 0);
            _legDia = outputValues.CreateDoubleCalcValue("Leg diameter", "", "mm", 0);
            _holePosX = inputValues.CreateDoubleCalcValue("Hole 1 X position", "", "mm", 0);
            _holePosY = inputValues.CreateDoubleCalcValue("Hole 1 Y position", "", "mm", 220);
            _holeSizeX = inputValues.CreateDoubleCalcValue("Hole 1 X size", "", "mm", 0);
            _holeSizeY = inputValues.CreateDoubleCalcValue("Hole 1 Y size", "", "mm", 200);
            _hole2PosX = inputValues.CreateDoubleCalcValue("Hole 2 X position", "", "mm", 500);
            _hole2PosY = inputValues.CreateDoubleCalcValue("Hole 2 Y position", "", "mm", -300);
            _hole2SizeX = inputValues.CreateDoubleCalcValue("Hole 2 X size", "", "mm", 00);
            _hole2SizeY = inputValues.CreateDoubleCalcValue("Hole 2 Y size", "", "mm", 150);
            _hole3PosX = inputValues.CreateDoubleCalcValue("Hole 3 X position", "", "mm", -600);
            _hole3PosY = inputValues.CreateDoubleCalcValue("Hole 3 Y position", "", "mm", 300);
            _hole3SizeX = inputValues.CreateDoubleCalcValue("Hole 3 X size", "", "mm", 00);
            _hole3SizeY = inputValues.CreateDoubleCalcValue("Hole 3 Y size", "", "mm", 150);
            _includeLinksBeyondHole = inputValues.CreateCalcSelectionList("Include links beyond holes", "YES", new List<string> { "YES", "NO" });
            _studPositions = outputValues.CreateCalcListOfDoubleArrays("Stud positions", new List<double[]>());
            _maxBarSize = inputValues.CreateCalcSelectionList("Maximum link diameter", "16", new List<string> { "8", "10", "12", "16", "20", "25", "32", "40" });
            _minBarSize = inputValues.CreateCalcSelectionList("Minimum link diameter", "8", new List<string> { "8", "10", "12", "16", "20", "25", "32" });

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            var temp = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream("EssentialCalcs.resources.ControlPerimeters_Fig_6_13.png"))
            {
                _fig6_13 = SkiaSharp.SKBitmap.Decode(stream);
            }
            assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("EssentialCalcs.resources.PunchingShear_Fig_6_14.png"))
            {
                _fig6_14 = SkiaSharp.SKBitmap.Decode(stream);
            }
            assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("EssentialCalcs.resources.PunchingShear_Fig_6_20.png"))
            {
                _fig6_20 = SkiaSharp.SKBitmap.Decode(stream);
            }
            assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("EssentialCalcs.resources.PunchingShear_Fig_6_22.png"))
            {
                _fig6_22 = SkiaSharp.SKBitmap.Decode(stream);
            }
            assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("EssentialCalcs.resources.PunchingShear_ConcreteCentreLayout.png"))
            {
                _figConcCentre = SkiaSharp.SKBitmap.Decode(stream);
            }
            assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("EssentialCalcs.resources.PunchingShear_EC1992_RadialLayout.png"))
            {
                _figRadialLayout = SkiaSharp.SKBitmap.Decode(stream);
            }

            UpdateCalc();
        }

        public override List<Formula> GenerateFormulae()
        {
            var image = generateImage();
            expressions.Insert(0, new Formula
            {
                Narrative = "Diagram:",
                Image = image[0],
            });
            expressions.Insert(1, new Formula
            {
                Narrative = "Key:",
                Image = image[1]
            });



            return expressions;
        }

        private void resetFields()
        {
            controlPerimeters = new List<PolyLine>();
            controlPerimeterNoHoles = null;
            outerPerimeters = new List<PolyLine>();
            columnOutline = null;
            fullColumnOutline = null;
            columnOutlines = new List<PolyLine>();
            holeOutlines = null;
            allHoleCorners = null;
            slabEdge = null;
            shearLinkStartPerimeters = new List<PolyLine>();
            shearLinkEndPerimeters = new List<PolyLine>();
            shearSpurs = new List<Line>();
            shearLinks = new List<List<Vector2>>();
            allPerimeters = new List<List<PolyLine>>();
            perimetersToReinforce = new List<List<PolyLine>>();
            _holeEdges = null;
            u1reducedNoHoles = null;
            u1reduced = null;
            _studPositions.Value = new List<double[]>();
            _status = CalcStatus.NONE;
        }

        public override void UpdateCalc()
        {
            formulae = null;
            resetFields();
            expressions = new List<Formula>();

            // calculate effective depths in each direction
            dy = _h.Value - _offsety.Value;
            dz = _h.Value - _offsetz.Value;
            d_average = (dy + dz) / 2;
            if (_loadCombType.ValueAsString == "PERMANENT") _concPartFactor.Value = 1.5;
            else _concPartFactor.Value = 1.2;
            expressions.Add(new Formula
            {
                Ref = "cl.2.4.2.4(1)",
                Narrative = "Partial factor for concrete",
                Expression = new List<string>
                {
                    _concPartFactor.Symbol + @"=" + _concPartFactor.Value
                }
            });

            expressions.Add(new Formula()
            {
                Narrative = "Calculate effective depths",
                Expression = new List<string>
                {
                    string.Format(@"{0} = {1} - {2} = {3}{4}", "d_y", _h.Symbol, _offsety.Symbol, dy, "mm"),
                    string.Format(@"{0} = {1} - {2} = {3}{4}", "d_z", _h.Symbol, _offsetz.Symbol, dz, "mm"),
                    string.Format(@"{0} = \frac{{{1} + {2}}}{{2}} = {3}{4}", @"d_{eff}", "d_y", "d_z", d_average, "mm")
                },
                Ref = "cl. 6.4.2(1)"
            });

            if (_stMode.ValueAsString == "AUTO")
            {
                st = 1.5 * d_average;
            }
            else
            {
                st = Math.Min(_stTarget.Value, 1.5 * d_average);
            }

            // check aspect ratio of column
            double aspectRatio = Math.Max(_columnAdim.Value, _columnBdim.Value) / Math.Min(_columnAdim.Value, _columnBdim.Value);
            if (aspectRatio > 4 && aspectRatio < 6)
            {
                expressions.Add(new Formula
                {
                    Narrative = "Check aspect ratio of column",
                    Expression = new List<string> { Math.Round(aspectRatio, 2) + ">" + 4 },
                    Conclusion = "OK"
                });
            }
            else if (aspectRatio > 6)
            {
                expressions.Add(new Formula
                {
                    Narrative = "Check aspect ratio of column",
                    Expression = new List<string> { Math.Round(aspectRatio, 2) + ">" + 4 },
                    Conclusion = "Treat column as wall"
                });
            }


            generateHoles();
            generateColumnOutlines();
            generateSlabEdge();
            controlPerimeterNoHoles = generatePerimeter(_columnAdim.Value, _columnBdim.Value, 2 * d_average);
            controlPerimeters = generatePerimeterWithHoles(2 * d_average);
            u1reducedNoHoles = new List<PolyLine> { generateReducedControlPerimeter(_columnAdim.Value, _columnBdim.Value) };
            u1reduced = generateReducedControlPerimeterWithHoles();
            ui = columnOutlines.Sum(a => a.Length);
            u1 = controlPerimeters.Sum(a => a.Length);
            double u1LimitedPerimeterTR64 = 0;
            var controlPerimLimitedTR64 = generateLimitedControlPerimeter(_columnAdim.Value, _columnBdim.Value);
            var controlPerimLimitedTR64WithHoles = generateLimitedControlPerimeterWithHoles();
            u1LimitedPerimeterTR64 = controlPerimLimitedTR64.Length;
            double u1LimitedPerimeterTR64WithHoles = controlPerimLimitedTR64WithHoles.Sum(a => a.Length);

            Formula controlPerimsFormula = new Formula
            {
                Narrative = "Determine the effective column face and first control perimeters taking into account the effect of holes. " +
                "These lengths are determined directly from the geometry - refer to diagram.",
                Expression = new List<string>
                {
                    string.Format("u_{{0,no holes}} = {0}mm", Math.Round(columnOutline.Length, 2)),
                    string.Format(@"u_0 = {0} mm", Math.Round(ui, 2)),
                    Environment.NewLine,
                    @"u_1 \text{ is } 2d \text{ from column face}",
                },
            };

            controlPerimsFormula.Expression.AddRange
                (
                    new List<string>
                    {
                        "u_{1,no holes} = " + Math.Round(controlPerimeterNoHoles.Length,2) + "mm",
                        "u_1 = " + Math.Round(u1,2) + "mm",
                    }

                );


            expressions.Add(controlPerimsFormula);

            // calc Beta
            Formula betaFormula = new Formula() { Narrative = "Calculate Beta factor." + Environment.NewLine, Ref = "cl 6.4.3" };
            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    double ey = _my.Value * 1E6 / (_punchingLoad.Value * 1E3);
                    double ez = _mz.Value * 1E6 / (_punchingLoad.Value * 1E3);
                    double by = (controlPerimeterNoHoles.Segments.Max(a => a.Start.X) - controlPerimeterNoHoles.Segments.Min(a => a.Start.X));
                    double bz = (controlPerimeterNoHoles.Segments.Max(a => a.Start.Y) - controlPerimeterNoHoles.Segments.Min(a => a.Start.Y));
                    double term1 = Math.Pow(ey / bz, 2);
                    double term2 = Math.Pow(ez / by, 2);
                    _beta.Value = 1 + 1.8 * Math.Sqrt(term1 + term2);
                    betaFormula.Narrative += "Calculated based on a rectangular internal column with loading eccentric to both axes. Control perimeter dimensions as Fig 6.13.";
                    betaFormula.Expression.Add(_beta.Symbol + @"=1 + 1.8\sqrt{\left(\frac{e_y}{b_z}\right)^2+\left(\frac{e_z}{b_y}\right)^2} =" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"e_y =\frac{" + _my.Symbol + @"}{" + _punchingLoad.Symbol + "}=" + Math.Round(ey, 1) + "mm");
                    betaFormula.Expression.Add(@"e_z =\frac{" + _mz.Symbol + @"}{" + _punchingLoad.Symbol + "}=" + Math.Round(ez, 1) + "mm");
                    betaFormula.Expression.Add(@"b_y =" + Math.Round(by, 1) + "mm");
                    betaFormula.Expression.Add(@"b_z =" + Math.Round(bz, 1) + "mm");
                    betaFormula.Image = _fig6_13;
                    break;
                case "EDGE":
                    double epar = Math.Abs(_my.Value * 1E6 / (_punchingLoad.Value * 1E3));
                    double c1 = _columnAdim.Value;
                    double c2 = _columnBdim.Value;
                    double k = calck(c1 / c2);
                    double w1 = Math.Pow(c2, 2) / 4 + c1 * c2 + 4 * c1 * d_average + 8 * Math.Pow(d_average, 2) + Math.PI * d_average * c2;
                    var u1noHoles = controlPerimeterNoHoles.Length;
                    if (u1noHoles > u1LimitedPerimeterTR64 && (_columnAdim.Value > 3 * d_average || _columnBdim.Value > 3* d_average))
                    {
                        betaFormula.Narrative += " Perimeter u1 for beta calc limited in accordance with TR64 clause 4.3.2.";
                        u1noHoles = u1LimitedPerimeterTR64;
                    }
                    var u1red = u1reducedNoHoles.Sum(a => a.Length);
                    _beta.Value = (u1noHoles / u1red) + k * (u1noHoles / w1) * epar;
                    betaFormula.Narrative += "Calculated on the basis of eccentricities about both axes, but moment about the axis parallel to slab edge is towards the interior of the slab.";
                    betaFormula.Expression.Add(_beta.Symbol + @"=\frac{u_1}{u_{1^*}}+k\frac{u_1}{W_1}e_{par}=" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"u_{1,no holes limited}=" + Math.Round(u1noHoles, 2) + "mm");
                    betaFormula.Expression.Add(@"u_{1^*,no holes}=" + Math.Round(u1red, 2) + "mm");
                    betaFormula.Expression.Add(@"k=" + Math.Round(k, 2));
                    betaFormula.Expression.Add(@"e_{par} =\left|\frac{" + _my.Symbol + @"}{" + _punchingLoad.Symbol + @"}\right|=" + Math.Round(epar, 1) + "mm");
                    betaFormula.Expression.Add(@"W_1=\frac{c_2^2}{4}+c_1c_2+4c_1d+8d^2+\pi dc_2=" + Math.Round(w1, 2));
                    betaFormula.Image = _fig6_20;
                    break;
                case "CORNER":
                    u1noHoles = controlPerimeterNoHoles.Length;
                    if (u1noHoles > u1LimitedPerimeterTR64 && (_columnAdim.Value > 3 * d_average || _columnBdim.Value > 3 * d_average))
                    {
                        betaFormula.Narrative += " Perimeter u1 for beta calc limited in accordance with TR64 clause 4.3.2.";
                        u1noHoles = u1LimitedPerimeterTR64;
                    }
                    u1red = u1reducedNoHoles.Sum(a => a.Length);
                    _beta.Value = u1noHoles / u1red;
                    betaFormula.Expression.Add(_beta.Symbol + @"=\frac{u_{1,no holes}}{u_{1^*,no holes}}=" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"u_{1 no holes limited}=" + Math.Round(u1noHoles, 2) + "mm");
                    betaFormula.Expression.Add(@"u_{1^*,no holes}=" + Math.Round(u1red, 2) + "mm");
                    betaFormula.Image = _fig6_20;
                    break;
                case "RE-ENTRANT":
                    _beta.Value = 1.275;
                    betaFormula.Narrative += "Default value for beta - use with care.";
                    betaFormula.Expression.Add(_beta.Symbol + @"=" + Math.Round(_beta.Value, 3));
                    break;
                default:
                    break;
            }

            if (_betaCheck.ValueAsString == "MANUAL")
            {
                _beta.Value = _betaProposed.Value;
                betaFormula = new Formula()
                {
                    Narrative = "Manally entered beta factor",
                    Expression = new List<string>
                    {
                        _beta.Symbol + "=" + _betaProposed.Value
                    }
                };
            }
            else if (_betaCheck.ValueAsString == "CODE_DEFAULT")
            {
                if (_colType.ValueAsString == "INTERNAL")
                    _beta.Value = 1.15;
                else if (_colType.ValueAsString == "CORNER")
                    _beta.Value = 1.5;
                else if (_colType.ValueAsString == "EDGE")
                    _beta.Value = 1.4;
                else
                    _beta.Value = 1.275;
                betaFormula = new Formula()
                {
                    Narrative = "Use code defaults for beta factor. May be used where stability does not rely on" +
                    "frame action between column and slab and adjacent spans differ by no more than 25%.",
                    Ref = "cl 6.4.3(6)",
                    Expression = new List<string>
                    {
                        _beta.Symbol + "=" + _beta.ValueAsString
                    }
                };
                if (_colType.ValueAsString == "RE-ENTRANT")
                {
                    betaFormula.Narrative = "Value for re-entrant column is interpolated from" +
                        " internal and edge column. Use with care.";
                }
            }

            expressions.Add(betaFormula);

            // calculate shear stress at face
            _stressvEdi.Value = _beta.Value * _punchingLoad.Value * 1000 / (ui * d_average);
            if (_faceCheck.ValueAsString == "CHECK")
            {
                var colFaceStressFormula = new Formula()
                {
                    Narrative = "Check shear stress at column face.",
                    Expression = new List<string>
                {
                    string.Format(@"u_0 = {0} mm", Math.Round(ui, 2)),
                    _beta.Symbol + " = " + Math.Round(_beta.Value,3) + _beta.Unit,
                    String.Format(@"{0} = {1}\frac{{{2}}}{{u_0 d_{{eff}}}} = {3}{4}",
                        _stressvEdi.Symbol,
                        _beta.Symbol,
                        _punchingLoad.Symbol,
                        Math.Round(_stressvEdi.Value, 2),
                        @"N/{mm^2}"),
                },
                    Ref = "cl.6.4.5(3)"
                };

                vRdMax = shearStressResistance(_fck.Value);

                if (_stressvEdi.Value > vRdMax)
                {
                    _stressvEdi.Status = CalcStatus.FAIL;
                    _uoutef.Value = double.NaN;
                    _Asw.Value = double.NaN;
                    colFaceStressFormula.Status = CalcStatus.FAIL;
                    if (_colType.ValueAsString == "EDGE" || _colType.ValueAsString == "CORNER")
                    {
                        colFaceStressFormula.Narrative += Environment.NewLine
                            + "Check on face stress can fail for corner or edge columns when dimension is greater than 1.5d as this is limiting value for effective face perimeter. For columns with greater dimensions, the beta factor increases but effective face perimeter does not. Alternative design strategies can be adopted to mitigate this.";

                    }
                    colFaceStressFormula.Conclusion = "Too high";
                    expressions.Add(colFaceStressFormula);
                    expressions.Insert(0, new Formula
                    {
                        Conclusion = "Face stress too high",
                        Status = CalcStatus.FAIL
                    });
                    return;
                }
                else
                {
                    colFaceStressFormula.Status = CalcStatus.PASS;
                    colFaceStressFormula.Conclusion = "OK";
                    _stressvEdi.Status = CalcStatus.PASS;
                    colFaceStressFormula.Expression.Add(@"v_{Rd,max}=" + Math.Round(vRdMax, 2) + @"N/mm^2");
                    colFaceStressFormula.Expression.Add(string.Format(@"{0} \leq {1}", _stressvEdi.Symbol, @"v_{Rd,max}"));
                    expressions.Add(colFaceStressFormula);
                }
            }
            else
            {
                expressions.Add(new Formula()
                {
                    Narrative = "Face shear check ignored."
                });
            }

            reinforcementRatioy = _rebarY.Value / (1000 * dy);
            reinforcementRatioz = _rebarZ.Value / (1000 * dz);

            _rho.Value = Math.Min(Math.Pow(reinforcementRatioy * reinforcementRatioz, 0.5), 0.02);

            vRdc = shearResistanceNoRein(_rho.Value, d_average, _fck.Value, _concPartFactor.Value); expressions.Add(new Formula
            {
                Expression = new List<string>
                    {
                    @"v_{Rd,c}=\max(C_{Rd,c}k(100\rho_lf_{ck})^{1/3}+k_1\sigma_{cp},v_{min}+k_1\sigma_{cp})",
                    @"v_{Rd,c}="+Math.Round(vRdc,2)+@"N/mm^2",
                    @"\text{where}",
                    @"C_{Rd,c}=\frac{0.18}{" + _concPartFactor.Symbol + @"}=" + Math.Round(0.18/_concPartFactor.Value,2),
                    @"k=1+\sqrt{\frac{200}{d}}\leq2",
                    _rho.Symbol + @"=\min(\sqrt{\rho_{ly} \rho_{lz}},0.02)",
                    _rho.Symbol + @"=\min(\sqrt{"+Math.Round(reinforcementRatioy,3)+@"\times"+Math.Round(reinforcementRatioz,3)+"},0.02)",
                    _rho.Symbol + "=" + Math.Round(_rho.Value,5) + _rho.Unit
                    },
                Narrative = "Calculate punching shear resistance",
                Ref = "cl. 6.4.4(1)"
            });

            stressvEd1 = _beta.Value * _punchingLoad.Value * 1000 / (u1 * d_average);
            expressions.Add(new Formula
            {
                Narrative = "Calculate shear stress at first control perimeter",
                Expression = new List<string>
                {
                    "u_1 = " + Math.Round(u1,2) + "mm",
                    @"v_{Ed1} = \frac{" + _beta.Symbol + " " + _punchingLoad.Symbol + @"}{u_1 d_{average}} = " + Math.Round(stressvEd1,2) + @"N/{mm^2}"
                },
            });

            if (stressvEd1 <= vRdc)
            {
                _uoutef.Value = 0;
                _Asw.Value = 0;
                _numberOfPerimeters.Value = 0;
                _linksInFirstPerim.Value = 0;
                _legsTotal.Value = 0;
                expressions.Add(new Formula
                {
                    Expression = new List<string>
                    {
                        string.Format(@"{0} \leq {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                    },
                    Conclusion = "No shear links required",
                    Status = CalcStatus.PASS,
                    Narrative = "Check if shear links are required"
                });
                expressions.Insert(0, new Formula
                {
                    Conclusion = "No links required",
                    Status = CalcStatus.PASS
                });

                return;
            }
            else if (stressvEd1 > 2 * vRdc)
            {
                _uoutef.Value = double.NaN;
                _Asw.Value = double.NaN;
                _numberOfPerimeters.Value = double.NaN;
                _linksInFirstPerim.Value = double.NaN;
                _legsTotal.Value = double.NaN;

                expressions.Add(new Formula
                {
                    Narrative = "Check if shear stress at control perimter is less than limiting value",
                    Expression = new List<string>
                    {
                        string.Format(@"{0} > 2 \times {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                    },
                    Conclusion = "Redesign slab",
                    Status = CalcStatus.FAIL,
                    Ref = "cl. 6.4.5(1)"
                });
                expressions.Insert(0, new Formula
                {

                    Conclusion = "Redesign slab",
                    Status = CalcStatus.FAIL
                });
                _status = CalcStatus.FAIL;

                return;
            }
            else
            {
                expressions.Add(new Formula
                {
                    Expression = new List<string>
                    {
                        string.Format(@"{0} > {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                        string.Format(@"{0} \leq 2 \times {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                    },
                    Conclusion = "Links required",
                    Narrative = "Check if shear links are required and stress is less than limit",
                    Ref = "cl. 6.4.5(1)"
                });
            }

            switch (_srMode.ValueAsString)
            {
                case "AUTO":
                    sr = 0.75 * d_average;
                    break;
                case "TARGET":
                    sr = _srTarget.Value;
                    break;
                default:
                    break;
            }
            if (sr <= 0.75 * d_average)
            {
                expressions.Add(new Formula
                {
                    Narrative = "Maximum radial spacing",
                    Expression = new List<string>
                            {
                                @"s_r = " + Math.Round(sr,0) + "mm",
                                @"s_r \leq 0.75 \times d",
                                @"s_r \leq 0.75 \times " + Math.Round(d_average,0) + "mm"
                            },
                    Status = CalcStatus.PASS
                });
            }
            else
            {
                expressions.Add(new Formula
                {
                    Narrative = "Maximum radial spacing",
                    Expression = new List<string>
                            {
                                @"s_r = " + Math.Round(sr,0) + "mm",
                                @"s_r > 0.75 \times d",
                                @"s_r > 0.75 \times " + Math.Round(d_average,0) + "mm"
                            },
                    Status = CalcStatus.FAIL
                });
                expressions.Insert(0, new Formula
                {
                    Conclusion = "Radial spacing too high",
                    Status = CalcStatus.FAIL
                });

                return;
            }

            fywd = 500 / 1.15;
            fywdef = Math.Min(250 + 0.25 * d_average, fywd);
            _Asw.Value = (stressvEd1 - 0.75 * vRdc) * sr * u1 / (1.5 * fywdef);
            expressions.Add(new Formula
            {
                Narrative = "Area of shear reinforcement required, assuming shear links are perpendicular to slab",
                Expression = new List<string>
                {
                    @"f_{ywdef} = \min{(250 + 0.25 \times d_{average},f_{ywd})} = \min{(" + (250 + 0.25 * d_average) + @"," + fywd+@")} = " + fywdef + @"N/mm^2",
                    _Asw.Symbol + @" = \frac{(v_{Ed1} - 0.75v_{Rdc})s_ru_1}{1.5 \times f_{ywdef}} =" + Math.Round(_Asw.Value,0) + _Asw.Unit,
                    @"\frac{" + _Asw.Symbol + @"}{s_r}=" + Math.Round(_Asw.Value / sr,3)
                },
                Ref = "cl.6.4.5(1)"
            });

            _uoutef.Value = _beta.Value * 1000 * _punchingLoad.Value / (vRdc * d_average);

            _distToFirstLinkPerim.Value = 0.5 * d_average;
            _perimSpacing.Value = sr;
            allPerimeters = new List<List<PolyLine>>();
            allPerimeters.Add(generatePerimeterWithHoles(_distToFirstLinkPerim.Value));
            shearLinkStartPerimeters = allPerimeters[0];
            for (int i = 1; i < 25; i++) // arbitrary max number of perimeters to check to. Change to while loop.
            {
                var perim = generatePerimeterWithHoles(_distToFirstLinkPerim.Value + i * _perimSpacing.Value);
                allPerimeters.Add(perim);
                if (perim.Sum(a => a.Length) > _uoutef.Value) break;
            }

            outerPerimeters = allPerimeters.Last();
            int numberofperimeters = Math.Max(allPerimeters.Count - 2, 1);
            perimetersToReinforce = allPerimeters.GetRange(0, numberofperimeters);
            shearLinkEndPerimeters = perimetersToReinforce.Last();

            double perimeterAtLastLinks = shearLinkEndPerimeters.Sum(a => a.Length);

            //spurs
            shearSpurs = new List<Line>();
            shearLinks = new List<List<Vector2>>();
            var shearLinkStartSpurs = new List<PolyLine>();
            for (int i = 0; i < perimetersToReinforce.Count; i++)
            {
                shearLinkStartSpurs.Add(generatePerimeter(_columnAdim.Value, _columnBdim.Value, 0.5 * d_average + i * (sr)));
            }

            if (_linkArrangement.ValueAsString == "SPURS_AUTO")
            {
                perimetersToReinforce = new List<List<PolyLine>> { perimetersToReinforce[0] };
                var spurPoints = new List<List<Tuple<Vector2, Vector2>>> { spurStartPoints() };
                double distFromCol = 0.5 * d_average;
                if (_colType.ValueAsString == "INTERNAL") spurPoints = new List<List<Tuple<Vector2, Vector2>>> { spurStartPoints() };

                for (int i = 1; i < 25; i++)
                {
                    distFromCol += sr;
                    int spursOnPrevPerim = spurPoints[i - 1].Count;
                    List<Tuple<Vector2, Vector2>> newPerim = new List<Tuple<Vector2, Vector2>>();
                    for (int k = 0; k < spursOnPrevPerim; k++)
                    {
                        var vector = spurPoints[i - 1][k].Item2;
                        newPerim.Add(new Tuple<Vector2, Vector2>(spurPoints[i - 1][k].Item1 + vector * (float)sr, vector));
                    }
                    var segs = new List<GeometryBase>();
                    var testOuter = new List<GeometryBase>();
                    var prevPoint = newPerim.Last();
                    int start = 0;
                    if (_colType.ValueAsString != "INTERNAL")
                    {
                        prevPoint = newPerim.First();
                        start = 1;
                    }

                    for (int j = start; j < newPerim.Count; j++)
                    {
                        var item = newPerim[j];
                        if (
                           ((distFromCol < 2 * d_average) && ((item.Item1 - prevPoint.Item1).Length() > 1.5 * d_average))
                           ||
                           ((distFromCol >= 2 * d_average) && ((item.Item1 - prevPoint.Item1).Length() > 2 * d_average))
                            )
                        {
                            var interPoint = (prevPoint.Item1 + item.Item1) / 2;
                            var interVec = Vector2.Normalize((prevPoint.Item2 + item.Item2));
                            newPerim.Insert(j, new Tuple<Vector2, Vector2>(interPoint, interVec));
                            segs.Add(new Line(prevPoint.Item1, interPoint));
                            segs.Add(new Line(interPoint, item.Item1));
                        }
                        else
                        {
                            segs.Add(new Line(prevPoint.Item1, item.Item1));
                        }
                        prevPoint = item;
                    }
                    var newPerimLine = new PolyLine(segs);
                    // extend to slab edge to max 0.5 * st
                    if (_colType.ValueAsString == "EDGE" || _colType.ValueAsString == "CORNER" || _colType.ValueAsString == "RE-ENTRANT")
                    {
                        var v1 = Vector2.Normalize(segs.First().Start - segs.First().End);
                        var ray1 = new Line(segs.First().Start, segs.First().Start + v1 * 10000);
                        var v2 = Vector2.Normalize(segs.Last().End - segs.Last().Start);
                        var ray2 = new Line(segs.Last().End, segs.Last().End + v2 * 10000);

                        var inter1 = slabEdge.intersection(ray1);
                        var inter2 = slabEdge.intersection(ray2);
                        if (inter1[0].TypeOfIntersection == IntersectionType.WITHIN)
                        {
                            var pt1 = inter1[0].Point;
                            if ((pt1 - segs.First().Start).Length() < 0.5f * (float)st) segs.Insert(0, new Line(pt1, segs.First().Start));
                            else segs.Insert(0, new Line(segs.First().Start + v1 * 0.5f * (float)st, segs.First().Start));
                        }
                        if (inter2[0].TypeOfIntersection == IntersectionType.WITHIN)
                        {
                            var pt2 = inter2[0].Point;
                            if ((pt2 - segs.Last().End).Length() < 0.5f * (float)st) segs.Add(new Line(segs.Last().End, pt2));
                            else segs.Add(new Line(segs.Last().End, segs.Last().End + v2 * 0.5f * (float)st));
                        }
                    }
                    var newPerimWithHoles = generatePerimeterWithHoles(newPerimLine);
                    perimetersToReinforce.Add(newPerimWithHoles);
                    spurPoints.Add(newPerim);
                    if (newPerimWithHoles.Sum(a => a.Length) > _uoutef.Value)
                    {
                        break;
                    }
                }
                outerPerimeters = perimetersToReinforce.Last();
                int numberOfPerimsToRemove = (int)Math.Ceiling(1.5 * d_average / sr);
                numberOfPerimsToRemove = Math.Min(numberOfPerimsToRemove, spurPoints.Count - 2);
                if (numberOfPerimsToRemove >= 2)
                {
                    spurPoints.RemoveRange(spurPoints.Count - numberOfPerimsToRemove, numberOfPerimsToRemove);
                    perimetersToReinforce.RemoveRange(perimetersToReinforce.Count - numberOfPerimsToRemove, numberOfPerimsToRemove);
                }

                shearLinkEndPerimeters = perimetersToReinforce.Last();
                // remove links in holes
                shearLinks = new List<List<Vector2>>();
                foreach (var item in spurPoints)
                {
                    List<Vector2> filteredList = new List<Vector2>();
                    foreach (var link in item)
                    {
                        bool include = true;
                        bool anyHoleObscuresLink = false;
                        foreach (var hole in allHoleCorners)
                        {
                            float minx = hole.Min(a => a.X);
                            float miny = hole.Min(a => a.Y);
                            float maxx = hole.Max(a => a.X);
                            float maxy = hole.Max(a => a.Y);
                            if (link.Item1.X > minx && link.Item1.X < maxx && link.Item1.Y > miny && link.Item1.Y < maxy)
                            {
                                include = false;
                            }
                            if (_includeLinksBeyondHole.ValueAsString == "NO")
                            {
                                bool holeObscuresLink = true;
                                List<Vector2> pointsToCheck = new List<Vector2> {
                                    new Vector2(0f,0f),
                                    new Vector2((float)_columnAdim.Value/2, (float)_columnBdim.Value/2),
                                    new Vector2(-(float)_columnAdim.Value/2, (float)_columnBdim.Value/2),
                                    new Vector2(-(float)_columnAdim.Value/2, -(float)_columnBdim.Value/2),
                                    new Vector2((float)_columnAdim.Value/2, -(float)_columnBdim.Value/2)
                                };

                                foreach (var corner in pointsToCheck)
                                {
                                    var lineToColumn = new Line(corner, link.Item1);
                                    Vector2 prevPoint2 = hole.Last();
                                    bool holeObscuresPoint = false;
                                    foreach (var edgeEndPoint in hole)
                                    {
                                        var edgeLine = new Line(prevPoint2, edgeEndPoint);
                                        var intersectionEvent = edgeLine.intersection(lineToColumn);
                                        if (intersectionEvent.Any(a => a.TypeOfIntersection == IntersectionType.WITHIN))
                                        {
                                            holeObscuresPoint = true;
                                        }
                                        prevPoint2 = edgeEndPoint;
                                    }
                                    if (!holeObscuresPoint)
                                    {
                                        holeObscuresLink = false;
                                    }
                                }
                                if (holeObscuresLink)
                                {
                                    anyHoleObscuresLink = true;
                                }
                            }
                        }
                        if (anyHoleObscuresLink)
                            include = false;
                        //foreach (var hole in _holeEdges)
                        //{
                        //    var angle1 = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                        //    if (angle1 < 0) angle1 += Math.PI * 2;
                        //    var angle2 = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                        //    if (angle2 < 0) angle2 += Math.PI * 2;
                        //    var angle3 = Math.Atan2(link.Item1.Y, link.Item1.X);
                        //    if (angle3 < 0) angle3 += Math.PI * 2;
                        //    if (Math.Abs(angle1 - angle2) < Math.PI)
                        //    {
                        //        if (angle3 > Math.Min(angle1, angle2) && angle3 < Math.Max(angle1, angle2))
                        //        {
                        //            include = false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (angle3 < Math.Min(angle1, angle2) || angle3 > Math.Max(angle1, angle2))
                        //        {
                        //            include = false;
                        //        }
                        //    }
                        //}

                        if (include) filteredList.Add(link.Item1);
                    }
                    shearLinks.Add(filteredList);
                }
                _distToOuterPerim.Value = distFromCol;
            }

            if (_linkArrangement.ValueAsString == "GRID")
            {
                perimetersToReinforce = new List<List<PolyLine>>();
                var initialLines = generateColumnFaces();
                var links = new List<List<Tuple<Vector2, Vector2>>>();
                var perimeterLines = new List<List<Line>>();
                double offsetFromColumn = 0;
                //generateFirstLinkPerimeter
                var newLines = new List<Line>();
                var newLinks = new List<Tuple<Vector2, Vector2>>();
                foreach (var line in initialLines.Segments)
                {
                    Vector2 dir = line.End - line.Start;
                    Vector2 perp = Vector2.Normalize(new Vector2(dir.Y, -dir.X));

                    int numberOfSpursOnEdge = Math.Max(2, (int)Math.Ceiling(line.Length / (st)) + 1);
                    double armOffset = 50;
                    if (line.Length < 200) armOffset = 25;
                    var stepVec = Vector2.Normalize(dir) * (float)((line.Length - 2 * armOffset) / (double)(numberOfSpursOnEdge - 1));
                    var startPoint = line.PointAtParameter(armOffset / line.Length);
                    for (int i = 0; i < numberOfSpursOnEdge; i++)
                    {
                        newLinks.Add(new Tuple<Vector2, Vector2>(startPoint + stepVec * i + 0.5f * (float)d_average * perp, perp));
                    }
                    newLines.Add(new Line(line.Start + 0.5f * (float)d_average * perp, line.End + 0.5f * (float)d_average * perp));
                }
                offsetFromColumn += 0.5 * d_average;
                perimetersToReinforce.Add(generatePerimeterWithHoles(offsetFromColumn));
                perimeterLines.Add(newLines);
                links.Add(newLinks);
                var prevLinks = newLinks;

                //generateMorePerimeters
                newLines = new List<Line>();
                for (int i = 1; i < 25; i++)//arbitrary max number of perim
                {
                    newLinks = new List<Tuple<Vector2, Vector2>>();
                    newLines = new List<Line>();
                    offsetFromColumn += sr;
                    foreach (var link in prevLinks)
                    {
                        newLinks.Add(new Tuple<Vector2, Vector2>(link.Item1 + link.Item2 * (float)sr, link.Item2));
                    }
                    //generate perimeterlines
                    foreach (var line in perimeterLines[i - 1])
                    {
                        Vector2 dir = line.End - line.Start;
                        Vector2 perp = Vector2.Normalize(new Vector2(dir.Y, -dir.X));
                        newLines.Add(new Line(line.Start + (float)sr * perp, line.End + (float)sr * perp));
                    }

                    // generate perimeter
                    var perim = generatePerimeterWithHoles(offsetFromColumn);
                    perimetersToReinforce.Add(perim);
                    double outerPerimOffset = offsetFromColumn + 1.5 * d_average;
                    _distToOuterPerim.Value = outerPerimOffset;
                    outerPerimeters = generatePerimeterWithHoles(outerPerimOffset);
                    if (outerPerimeters.Sum(a => a.Length) > _uoutef.Value)
                    {
                        links.Add(newLinks);
                        break;
                    }
                    var newCornerLinks = new List<Tuple<Vector2, Vector2>>();
                    //generate new links from previous corners
                    int startLine = 1; int prevLine = 0;
                    if (initialLines.IsClosed == true)
                    {
                        startLine = 0;
                        prevLine = initialLines.Segments.Count - 1;
                    }
                    for (int j = startLine; j < perimeterLines[i - 1].Count; j++)
                    {
                        var line1 = perimeterLines[i - 1][prevLine];
                        var line2 = perimeterLines[i - 1][j];
                        Vector2 dir1 = line1.End - line1.Start;
                        Vector2 perp1 = Vector2.Normalize(new Vector2(dir1.Y, -dir1.X));
                        Vector2 dir2 = line2.End - line2.Start;
                        Vector2 perp2 = Vector2.Normalize(new Vector2(dir2.Y, -dir2.X));
                        var cornerPoint = line1.intersection(line2);
                        newLinks.Add(new Tuple<Vector2, Vector2>(cornerPoint[0].Point + perp1 * (float)sr, perp1));
                        newLinks.Add(new Tuple<Vector2, Vector2>(cornerPoint[0].Point + perp2 * (float)sr, perp2));
                        newCornerLinks.Add(new Tuple<Vector2, Vector2>(cornerPoint[0].Point + perp1 * (float)sr + perp2 * (float)sr, new Vector2(0f, 0f)));
                        prevLine = j;
                    }
                    perimeterLines.Add(newLines);
                    prevLinks = newLinks;
                    var linksToAdd = newLinks.Select(a => a).ToList();
                    linksToAdd.AddRange(newCornerLinks.Select(a => a).ToList());
                    links.Add(linksToAdd);
                }

                shearLinkEndPerimeters = perimetersToReinforce.Last();
                // remove links in holes
                shearLinks = new List<List<Vector2>>();
                foreach (var item in links)
                {
                    List<Vector2> filteredList = new List<Vector2>();
                    foreach (var link in item)
                    {
                        bool include = true;
                        bool anyHoleObscuresLink = false;
                        foreach (var hole in allHoleCorners)
                        {
                            float minx = hole.Min(a => a.X);
                            float miny = hole.Min(a => a.Y);
                            float maxx = hole.Max(a => a.X);
                            float maxy = hole.Max(a => a.Y);
                            if (link.Item1.X > minx && link.Item1.X < maxx && link.Item1.Y > miny && link.Item1.Y < maxy)
                            {
                                include = false;
                            }
                            if (_includeLinksBeyondHole.ValueAsString == "NO")
                            {
                                bool holeObscuresLink = true;
                                List<Vector2> pointsToCheck = new List<Vector2> {
                                    new Vector2(0f,0f),
                                    new Vector2((float)_columnAdim.Value/2, (float)_columnBdim.Value/2),
                                    new Vector2(-(float)_columnAdim.Value/2, (float)_columnBdim.Value/2),
                                    new Vector2(-(float)_columnAdim.Value/2, -(float)_columnBdim.Value/2),
                                    new Vector2((float)_columnAdim.Value/2, -(float)_columnBdim.Value/2)
                                };

                                foreach (var corner in pointsToCheck)
                                {
                                    var lineToColumn = new Line(corner, link.Item1);
                                    Vector2 prevPoint2 = hole.Last();
                                    bool holeObscuresPoint = false;
                                    foreach (var edgeEndPoint in hole)
                                    {
                                        var edgeLine = new Line(prevPoint2, edgeEndPoint);
                                        var intersectionEvent = edgeLine.intersection(lineToColumn);
                                        if (intersectionEvent.Any(a => a.TypeOfIntersection == IntersectionType.WITHIN))
                                        {
                                            holeObscuresPoint = true;
                                        }
                                        prevPoint2 = edgeEndPoint;
                                    }
                                    if (!holeObscuresPoint)
                                    {
                                        holeObscuresLink = false;
                                    }
                                }
                                if (holeObscuresLink)
                                {
                                    anyHoleObscuresLink = true;
                                }
                            }
                        }
                        if (anyHoleObscuresLink)
                            include = false;
                        //foreach (var hole in _holeEdges)
                        //{
                        //    var angle1 = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                        //    if (angle1 < 0) angle1 += Math.PI * 2;
                        //    var angle2 = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                        //    if (angle2 < 0) angle2 += Math.PI * 2;
                        //    var angle3 = Math.Atan2(link.Item1.Y, link.Item1.X);
                        //    if (angle3 < 0) angle3 += Math.PI * 2;
                        //    if (Math.Abs(angle1 - angle2) < Math.PI)
                        //    {
                        //        if (angle3 > Math.Min(angle1, angle2) && angle3 < Math.Max(angle1, angle2))
                        //        {
                        //            include = false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (angle3 < Math.Min(angle1, angle2) || (angle3 > Math.Max(angle1, angle2)))
                        //        {
                        //            include = false;
                        //        }
                        //    }
                        //}
                        List<double> lengthsFromCol = new List<double>();
                        var x = _columnAdim.Value / 2; var y = _columnBdim.Value;
                        foreach (var edge in fullColumnOutline.Segments)
                        {
                            lengthsFromCol.Add((link.Item1 - edge.Start).Length());
                        }
                        if (
                            lengthsFromCol.Min() > (offsetFromColumn + 0.5 * sr)
                            &&
                            (link.Item1.X < -x || link.Item1.X > x)
                            &&
                            (link.Item1.Y > y || link.Item1.Y < -y)
                            )
                        {
                            include = false;
                        }

                        if (include) filteredList.Add(link.Item1);
                    }
                    shearLinks.Add(filteredList);
                }
            }

            if (_linkArrangement.ValueAsString == "CRUCIFORM")
            {
                var cruciformGroups = new List<List<Tuple<Vector2, Vector2>>>();
                perimetersToReinforce = new List<List<PolyLine>>();
                var initialLines = generateColumnFaces();
                var links = new List<List<Tuple<Vector2, Vector2>>>();
                var perimeterLines = new List<List<Line>>();
                double offsetFromColumn = 0;
                //generateFirstLinkPerimeter
                var newLines = new List<Line>();
                foreach (var line in initialLines.Segments)
                {
                    var newLinks = new List<Tuple<Vector2, Vector2>>();
                    Vector2 dir = line.End - line.Start;
                    Vector2 perp = Vector2.Normalize(new Vector2(dir.Y, -dir.X));
                    int numberOfSpursOnEdge = Math.Max(2, (int)Math.Ceiling(line.Length / (st)) + 1);
                    double armOffset = 50;
                    if (line.Length < 200) armOffset = 25;
                    var stepVec = Vector2.Normalize(dir) * (float)((line.Length - 2 * armOffset) / (double)(numberOfSpursOnEdge - 1));
                    var startPoint = line.PointAtParameter(armOffset / line.Length);
                    //var stepVec = Vector2.Normalize(dir) * (float)(line.Length / (double)(numberOfSpursOnEdge - 1));
                    for (int i = 0; i < numberOfSpursOnEdge; i++)
                    {
                        newLinks.Add(new Tuple<Vector2, Vector2>(startPoint + stepVec * i + 0.5f * (float)d_average * perp, perp));
                    }
                    cruciformGroups.Add(newLinks);
                    newLines.Add(new Line(line.Start + 0.5f * (float)d_average * perp, line.End + 0.5f * (float)d_average * perp));
                }
                offsetFromColumn += 0.5 * d_average;
                perimetersToReinforce.Add(generatePerimeterWithHoles(offsetFromColumn));
                links.Add(cruciformGroups.SelectMany(i => i).ToList());

                for (int i = 0; i < 25; i++)
                {
                    var prevCruciformGroups = cruciformGroups;
                    cruciformGroups = new List<List<Tuple<Vector2, Vector2>>>();
                    foreach (var edgeLinks in prevCruciformGroups)
                    {
                        var newLinks = new List<Tuple<Vector2, Vector2>>();
                        foreach (var link in edgeLinks)
                        {
                            newLinks.Add(new Tuple<Vector2, Vector2>(link.Item1 + (float)sr * link.Item2, link.Item2));
                        }
                        cruciformGroups.Add(newLinks);
                    }

                    links.Add(cruciformGroups.SelectMany(a => a).ToList());
                    perimetersToReinforce.Add(generatePerimeterWithHoles(offsetFromColumn));
                    if (Math.Sqrt(2 * Math.Pow(offsetFromColumn, 2)) > 2 * d_average) break;
                    offsetFromColumn += sr;
                }

                bool closed = false;
                if (_colType.ValueAsString == "INTERNAL") closed = true;

                var outerPerim = new List<PolyLine>();
                for (int j = 0; j < cruciformGroups.Count; j++)
                {
                    var outPerimSegment = new List<GeometryBase>();
                    var edgeLinks = cruciformGroups[j];
                    var pt1 = edgeLinks.First().Item1 + edgeLinks.First().Item2 * 1.5f * (float)d_average;
                    var pt2 = edgeLinks.Last().Item1 + edgeLinks.Last().Item2 * 1.5f * (float)d_average;
                    var angle = Math.Atan2(edgeLinks.Last().Item2.Y, edgeLinks.Last().Item2.X);
                    if (angle < 0) angle += 2d * Math.PI;
                    if (j != 0 || closed)
                    {
                        var angle2 = angle;
                        if (angle - Math.PI * 0.75 < 0)
                        {
                            angle2 += 2 * Math.PI;
                        }
                        var arc = new Arc { Centre = edgeLinks.First().Item1, Radius = 1.5 * d_average, StartAngle = angle2 - Math.PI / 4, EndAngle = angle2 };
                        var startVec = Vector2.Normalize(new Vector2((float)Math.Cos(angle2 - Math.PI * 0.75), (float)Math.Sin(angle2 - Math.PI * 0.75)));
                        var pt3 = arc.Start + startVec * (float)d_average;
                        outPerimSegment.Add(new Line(pt3, arc.Start));
                        outPerimSegment.Add(arc);
                    }
                    outPerimSegment.Add(new Line(pt1, pt2));
                    if (j < cruciformGroups.Count - 1 || closed)
                    {
                        var arc = new Arc { Centre = edgeLinks.Last().Item1, Radius = 1.5 * d_average, StartAngle = angle, EndAngle = angle + Math.PI / 4 };
                        var endVec = Vector2.Normalize(new Vector2((float)Math.Cos(angle + Math.PI * 0.75), (float)Math.Sin(angle + Math.PI * 0.75)));
                        var pt4 = arc.End + endVec * (float)d_average;
                        outPerimSegment.Add(arc);
                        outPerimSegment.Add(new Line(arc.End, pt4));
                    }
                    outerPerim.Add(new PolyLine(outPerimSegment));
                }

                outerPerimeters = new List<PolyLine>();
                foreach (var perim in outerPerim)
                {
                    outerPerimeters.AddRange(generatePerimeterWithHoles(perim));
                }

                // remove links in holes
                shearLinks = new List<List<Vector2>>();
                foreach (var item in links)
                {
                    List<Vector2> filteredList = new List<Vector2>();
                    foreach (var link in item)
                    {
                        bool include = true;
                        bool anyHoleObscuresLink = false;
                        foreach (var hole in allHoleCorners)
                        {
                            float minx = hole.Min(a => a.X);
                            float miny = hole.Min(a => a.Y);
                            float maxx = hole.Max(a => a.X);
                            float maxy = hole.Max(a => a.Y);
                            if (link.Item1.X > minx && link.Item1.X < maxx && link.Item1.Y > miny && link.Item1.Y < maxy)
                            {
                                include = false;
                            }
                            if (_includeLinksBeyondHole.ValueAsString == "NO")
                            {
                                bool holeObscuresLink = true;
                                List<Vector2> pointsToCheck = new List<Vector2> {
                                    new Vector2(0f,0f),
                                    new Vector2((float)_columnAdim.Value/2, (float)_columnBdim.Value/2),
                                    new Vector2(-(float)_columnAdim.Value/2, (float)_columnBdim.Value/2),
                                    new Vector2(-(float)_columnAdim.Value/2, -(float)_columnBdim.Value/2),
                                    new Vector2((float)_columnAdim.Value/2, -(float)_columnBdim.Value/2)
                                };

                                foreach (var corner in pointsToCheck)
                                {
                                    var lineToColumn = new Line(corner, link.Item1);
                                    Vector2 prevPoint2 = hole.Last();
                                    bool holeObscuresPoint = false;
                                    foreach (var edgeEndPoint in hole)
                                    {
                                        var edgeLine = new Line(prevPoint2, edgeEndPoint);
                                        var intersectionEvent = edgeLine.intersection(lineToColumn);
                                        if (intersectionEvent.Any(a => a.TypeOfIntersection == IntersectionType.WITHIN))
                                        {
                                            holeObscuresPoint = true;
                                        }
                                        prevPoint2 = edgeEndPoint;
                                    }
                                    if (!holeObscuresPoint)
                                    {
                                        holeObscuresLink = false;
                                    }
                                }
                                if (holeObscuresLink)
                                {
                                    anyHoleObscuresLink = true;
                                }
                            }
                        }
                        if (anyHoleObscuresLink)
                            include = false;

                        //foreach (var hole in _holeEdges)
                        //{
                        //    var angle1 = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                        //    if (angle1 < 0) angle1 += Math.PI * 2;
                        //    var angle2 = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                        //    if (angle2 < 0) angle2 += Math.PI * 2;
                        //    var angle3 = Math.Atan2(link.Item1.Y, link.Item1.X);
                        //    if (angle3 < 0) angle3 += Math.PI * 2;
                        //    if (Math.Abs(angle1 - angle2) < Math.PI)
                        //    {
                        //        if (angle3 > Math.Min(angle1, angle2) && angle3 < Math.Max(angle1, angle2))
                        //        {
                        //            include = false;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (angle3 < Math.Min(angle1, angle2) || (angle3 > Math.Max(angle1, angle2)))
                        //        {
                        //            include = false;
                        //        }
                        //    }
                        //}
                        if (include) filteredList.Add(link.Item1);
                    }
                    shearLinks.Add(filteredList);
                }
                _distToOuterPerim.Value = offsetFromColumn + 1.5 * d_average;
            }

            List<List<Vector2>> effectiveShearLinks = new List<List<Vector2>>();
            foreach (var perim in shearLinks)
            {
                List<Vector2> perimLinks = new List<Vector2>();
                foreach (var link in perim)
                {
                    bool include = true;
                    foreach (var hole in _holeEdges)
                    {
                        var angle1 = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                        if (angle1 < 0) angle1 += Math.PI * 2;
                        var angle2 = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                        if (angle2 < 0) angle2 += Math.PI * 2;
                        var angle3 = Math.Atan2(link.Y, link.X);
                        if (angle3 < 0) angle3 += Math.PI * 2;
                        if (Math.Abs(angle1 - angle2) < Math.PI)
                        {
                            if (angle3 > Math.Min(angle1, angle2) && angle3 < Math.Max(angle1, angle2))
                            {
                                include = false;
                            }
                        }
                        else
                        {
                            if (angle3 < Math.Min(angle1, angle2) || (angle3 > Math.Max(angle1, angle2)))
                            {
                                include = false;
                            }
                        }
                    }
                    if (include)
                    {
                        perimLinks.Add(link);
                    }
                }
                effectiveShearLinks.Add(perimLinks);
            }

            _linksInFirstPerim.Value = effectiveShearLinks[0].Count;
            _numberOfPerimeters.Value = perimetersToReinforce.Count;
            _legsTotal.Value = shearLinks.Sum(a => a.Count);

            List<int> allBarSizes = new List<int> { 8, 10, 12, 16, 20, 25, 32, 40 };
            int _maxBarDia = int.Parse(_maxBarSize.ValueAsString);
            int _minBarDia = int.Parse(_minBarSize.ValueAsString);
            var barSizes = allBarSizes.Where(a => a <= _maxBarDia && a >= _minBarDia).ToList();

            _legDia.Value = calcBarSizeAndDia(_Asw.Value / _linksInFirstPerim.Value, barSizes);
            if (double.IsNaN(_legDia.Value))
            {
                expressions.Add(
                    Formula.FormulaWithNarrative("User defined maximum bar size of " + _maxBarDia + "mm is too small for the specified spacings. Either specify a larger allowable bar size, or decrease radial or tangential spacing.")
                    .AddConclusion("No valid bar dia")
                    .AddStatus(CalcStatus.FAIL));
            }
            else
            {
                expressions.Add(
                    Formula.FormulaWithNarrative("Calculate required leg area and diameter required")
                    .AddFirstExpression(@"A_s=\frac{" + _Asw.Symbol + @"}{"+_linksInFirstPerim.Value+"}="+ Math.Round(_Asw.Value/_linksInFirstPerim.Value,2))
                    .AddExpression(@"\text{Smallest diameter that meets leg area required: }" + _legDia.Value + _legDia.Unit)
                    );
            }

            var shearLinkPositions = new List<double[]>();
            foreach (var links in shearLinks)
            {
                foreach (var item in links)
                {
                    shearLinkPositions.Add(new double[3] { item.X, item.Y, 0 });
                }
            }
            _studPositions.Value = shearLinkPositions;

            var outerPerimExp = new Formula
            {
                Narrative = "Outer perimeter. The required perimeter is calculated from clause 6.4.5(4), " +
                "the provided perimeter is the first perimeter of shear links that exceeds the required " +
                "value and determined by evaluating successive perimeters until the condition is met.",
                Expression = new List<string>
                {
                    _uoutef.Symbol + @"=\frac{" + _beta.Symbol + " " + _punchingLoad.Symbol + @"}{(v_{Rd,c}d)}",
                    _uoutef.Symbol + "=" + Math.Round(_uoutef.Value,0) + _uoutef.Unit,
                    @"u_{out,ef,prov} = " + Math.Round(outerPerimeters.Sum(a => a.Length),0) + @"mm"
                },
                Ref = "cl.6.4.5(4)"
            };

            if (_uoutef.Value > outerPerimeters.Sum(a => a.Length))
            {
                outerPerimExp.Conclusion = "Outer perimeter too short";
                outerPerimExp.Status = CalcStatus.FAIL;
                expressions.Add(outerPerimExp);

                expressions.Insert(0, new Formula
                {
                    Conclusion = "Outer perimeter too short",
                    Status = CalcStatus.FAIL
                });
                return;
            }
            else
            {
                expressions.Add(outerPerimExp);
            }


            var detailingFormula = new Formula
            {
                Narrative = "Detailing dimensions:",
                Expression = new List<string>
                {
                    @"\text{Distance to first perimeter} =" + Math.Round(_distToFirstLinkPerim.Value,0) + _distToFirstLinkPerim.Unit,
                    @"\text{Perimeter spacing} =" + Math.Round(_perimSpacing.Value,0) + _perimSpacing.Unit
                }
            };
            if (_linkArrangement.ValueAsString == "GRID")
            {
                //Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_ConcreteCentreLayout.png");
                //detailingFormula.Image = new BitmapImage(uri);
                detailingFormula.Image = _figConcCentre;
                detailingFormula.Ref = "Concrete Centre Figure 9";
            }
            else if (_linkArrangement.ValueAsString == "SPURS_AUTO")
            {
                //Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_22.png");
                //detailingFormula.Image = new BitmapImage(uri);
                detailingFormula.Image = _fig6_22;
                detailingFormula.Ref = "Figure 6.22, diagram A";
            }
            else if (_linkArrangement.ValueAsString == "CRUCIFORM")
            {
                //Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_22.png");
                //detailingFormula.Image = new BitmapImage(uri);
                detailingFormula.Image = _fig6_22;
                detailingFormula.Ref = "Figure 6.22, diagram B";
            }

            expressions.Add(detailingFormula);

            generateDXFoutput();
        }

        public override List<netDxf.DxfDocument> GetDrawings()
        {
            generateDXFoutput();
            return _drawings;
        }

        private void generateDXFoutput()
        {
            var dxf = new netDxf.DxfDocument();
            var ColumnLayer = new netDxf.Tables.Layer("Column")
            {
                Color = netDxf.AciColor.Green,
                Lineweight = netDxf.Lineweight.W50
            };
            var slabEdgeLayer = new netDxf.Tables.Layer("SlabEdge")
            {
                Color = netDxf.AciColor.Yellow,
                Lineweight = netDxf.Lineweight.W35
            };
            var linksLayer = new netDxf.Tables.Layer("Links")
            {
                Color = netDxf.AciColor.Green,
                Lineweight = netDxf.Lineweight.W35
            };
            var controlPerimeterLayer = new netDxf.Tables.Layer("ControlPerimeter")
            {
                Color = netDxf.AciColor.Red,
                Lineweight = netDxf.Lineweight.W18
            };
            var outerPerimeterLayer = new netDxf.Tables.Layer("OuterPerimeter")
            {
                Color = netDxf.AciColor.Red,
                Lineweight = netDxf.Lineweight.W18
            };

            dxf.Layers.Add(ColumnLayer);
            dxf.Layers.Add(slabEdgeLayer);
            dxf.Layers.Add(linksLayer);
            dxf.Layers.Add(controlPerimeterLayer);
            dxf.Layers.Add(outerPerimeterLayer);

            foreach (var item in fullColumnOutline.Segments)
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, ColumnLayer));
            }
            foreach (var item in slabEdge.Segments)
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, slabEdgeLayer));
            }
            foreach (var item in shearLinks.SelectMany(a => a).ToList())
            {
                dxf.AddEntity(new netDxf.Entities.Circle(new netDxf.Vector2(item.X, item.Y), 10) { Layer = linksLayer });
            }
            foreach (var item in controlPerimeters.SelectMany(a => a.Segments))
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, controlPerimeterLayer));
            }
            foreach (var item in outerPerimeters.SelectMany(a => a.Segments))
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, outerPerimeterLayer));
            }

            //var filePath = _dxfFolder.ValueAsString + @"\" + this.InstanceName + @".dxf";
            //dxf.Save(filePath);
            _drawings.Clear();
            _drawings.Add(dxf);
        }

        private netDxf.Entities.EntityObject getDXFEntityFromGeometry(GeometryBase item, Layer layer)
        {
            if (item.GetType() == typeof(Line))
            {
                var dxfLine = new netDxf.Entities.Line(
                    new netDxf.Vector2(item.Start.X, item.Start.Y),
                    new netDxf.Vector2(item.End.X, item.End.Y))
                { Layer = layer };
                return dxfLine;
            }
            else if (item.GetType() == typeof(Arc))
            {
                var arc = item as Arc;
                var dxfLine = new netDxf.Entities.Arc(
                    new netDxf.Vector2(arc.Centre.X, arc.Centre.Y),
                    arc.Radius,
                    arc.StartAngle * (180 / Math.PI),
                    arc.EndAngle * (180 / Math.PI))
                { Layer = layer };
                return dxfLine;
            }
            return null;
        }

        private List<Tuple<Vector2, Vector2>> spurStartPoints2()
        {
            var returnList = new List<Tuple<Vector2, Vector2>>();

            var firstPerim = generatePerimeter(_columnAdim.Value, _columnBdim.Value, 0.5 * d_average);
            var controlPerim = generatePerimeter(_columnAdim.Value, _columnBdim.Value, 2 * d_average);
            int spurSegments = (int)Math.Ceiling(controlPerim.Length / (st));
            if (firstPerim.IsClosed)
            {
                spurSegments++;
            }
            for (int i = 0; i < spurSegments; i++)
            {
                double frac = (1d / (double)spurSegments) * (i);
                var pt1 = firstPerim.PointAtParameter((frac));
                var pt2 = controlPerim.PointAtParameter((frac));
                var vec = Vector2.Normalize(pt2 - pt1);
                returnList.Add(new Tuple<Vector2, Vector2>(pt1, vec));
            }
            return returnList;
        }

        private double calck(double c1overc2)
        {
            if (c1overc2 <= 0.5) return 0.45;
            else if (c1overc2 < 1) return (c1overc2 - 0.5) * (0.15 / 0.5) + 0.45;
            else if (c1overc2 < 3) return (c1overc2 - 1) * (0.2 / 2) + 0.6;
            else return 0.8;
        }

        private void generateHoles()
        {
            // SET UP GEOMETRY
            // holes
            var holeExpression = new Formula()
            {
                Narrative = "Effective width of opening with respect to centre of loading",
                Ref = "Figure 6.14",
            };
            allHoleCorners = new List<List<Vector2>>();
            if (_holeSizeX.Value != 0 && _holeSizeY.Value != 0)
            {
                allHoleCorners.Add(new List<Vector2> {
            new Vector2((float)_holePosX.Value, (float)(_holePosY.Value + _holeSizeY.Value)),
            new Vector2((float)(_holePosX.Value + _holeSizeX.Value), (float)(_holePosY.Value + _holeSizeY.Value)),
            new Vector2((float)(_holePosX.Value + _holeSizeX.Value), (float)(_holePosY.Value)),
            new Vector2((float)_holePosX.Value, (float)(_holePosY.Value))
            });
            }
            if (_hole2SizeX.Value != 0 && _hole2SizeY.Value != 0)
            {
                allHoleCorners.Add(new List<Vector2> {
            new Vector2((float)_hole2PosX.Value, (float)(_hole2PosY.Value + _hole2SizeY.Value)),
            new Vector2((float)(_hole2PosX.Value + _hole2SizeX.Value), (float)(_hole2PosY.Value + _hole2SizeY.Value)),
            new Vector2((float)(_hole2PosX.Value + _hole2SizeX.Value), (float)(_hole2PosY.Value)),
            new Vector2((float)_hole2PosX.Value, (float)(_hole2PosY.Value)),
            });
            }
            if (_hole3SizeX.Value != 0 && _hole3SizeY.Value != 0)
                allHoleCorners.Add(new List<Vector2>
            {
            new Vector2((float)_hole3PosX.Value, (float)(_hole3PosY.Value + _hole3SizeY.Value)),
            new Vector2((float)(_hole3PosX.Value + _hole3SizeX.Value), (float)(_hole3PosY.Value + _hole3SizeY.Value)),
            new Vector2((float)(_hole3PosX.Value + _hole3SizeX.Value), (float)(_hole3PosY.Value)),
            new Vector2((float)_hole3PosX.Value, (float)(_hole3PosY.Value)),
            });

            _holeEdges = new List<Tuple<Line, Line>>();
            var angles = new List<double>();
            for (int i = 0; i < allHoleCorners.Count; i++)
            {
                angles = new List<double>();
                var holeCorners = allHoleCorners[i];
                foreach (var corner in holeCorners)
                {
                    var ang = Math.Atan2(corner.Y, corner.X);
                    if (ang < 0) ang += 2 * Math.PI;
                    angles.Add(ang);
                }
                double minAngle = angles.Min();
                double maxAngle = angles.Max();
                if (maxAngle - minAngle > Math.PI)
                {
                    angles = new List<double>();
                    foreach (var corner in holeCorners)
                    {
                        var ang = Math.Atan2(corner.Y, corner.X);
                        angles.Add(ang);
                    }
                }
                minAngle = angles.Min();
                maxAngle = angles.Max();

                // translate coordinate system so that width and lengh of hole can be compared using radius as x-axis
                //figure 6.14
                var minx = 0d; var maxx = 0d; var miny = 0d; var maxy = 0d;
                var midAngle = (maxAngle + minAngle) / 2;
                var s = Math.Sin(-midAngle);
                var c = Math.Cos(-midAngle);
                var corn = holeCorners[0];
                var newx = corn.X * c - corn.Y * s;
                var newy = corn.X * s - corn.Y * c;
                minx = newx;
                maxx = newx;
                miny = newy;
                maxy = newy;
                foreach (var corner in holeCorners)
                {
                    s = Math.Sin(-midAngle);
                    c = Math.Cos(-midAngle);
                    newx = corner.X * c - corner.Y * s;
                    newy = corner.X * s - corner.Y * c;
                    minx = Math.Min(minx, newx);
                    maxx = Math.Max(maxx, newx);
                    miny = Math.Min(miny, newy);
                    maxy = Math.Max(maxy, newy);
                }
                maxy = minx * Math.Tan((maxAngle - minAngle) / 2);
                miny = -minx * Math.Tan((maxAngle - minAngle) / 2);
                var rangex = maxx - minx;
                var rangey = maxy - miny;
                if (rangex > rangey)
                {
                    var newrangey = Math.Sqrt(rangey * rangex);
                    holeExpression.Expression.Add(@"width_" + i + "=" + Math.Round(newrangey, 0) + "mm");
                    minAngle = midAngle - Math.Atan2(newrangey / 2, minx);
                    maxAngle = midAngle + Math.Atan2(newrangey / 2, minx);
                    if ((minAngle * maxAngle < 0) && (maxAngle - minAngle > Math.PI))
                    {
                        var newMax = minAngle + 2 * Math.PI;
                        minAngle = maxAngle;
                        maxAngle = newMax;
                    }
                }
                else
                {
                    holeExpression.Expression.Add(@"width_" + i + "=" + Math.Round(rangey, 0) + "mm");

                }
                var holeEdge1 = new Line(new Vector2(0, 0), new Vector2((float)(10000 * Math.Cos(minAngle)), (float)(10000 * Math.Sin(minAngle))));
                var holeEdge2 = new Line(new Vector2(0, 0), new Vector2((float)(10000 * Math.Cos(maxAngle)), (float)(10000 * Math.Sin(maxAngle))));
                _holeEdges.Add(new Tuple<Line, Line>(holeEdge1, holeEdge2));
            }
            if (allHoleCorners.Count > 0)
            {
                //Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_14.png");
                //holeExpression.Image = new BitmapImage(uri);
                holeExpression.Image = _fig6_14;
                expressions.Add(holeExpression);
            }

            holeOutlines = new List<PolyLine>();
            foreach (var holeCorners in allHoleCorners)
            {
                var holeSegments = new List<GeometryBase>();

                for (int i = 1; i < holeCorners.Count; i++)
                {
                    holeSegments.Add(new Line(holeCorners[i - 1], holeCorners[i]));
                }
                holeOutlines.Add(new PolyLine(holeSegments) { IsClosed = true });
            }
        }

        //private List<int> getSpursList()
        //{
        //    List<int> basicNumbers = new List<int> { 8, 12, 16, 24};
        //    switch (_linkArrangement.ValueAsString)
        //    {
        //        case "SPURS_AUTO":
        //            basicNumbers = new List<int> { 8, 12, 16, 24 };
        //            break;
        //        case "SPURS_45_DEGREES":
        //            basicNumbers = new List<int> { 8 };
        //            break;
        //        case "SPURS_30_DEGREES":
        //            basicNumbers = new List<int> { 12 };
        //            break;
        //        default:
        //            break;
        //    }
        //    switch (_colType.ValueAsString)
        //    {
        //        case "INTERNAL":
        //            return basicNumbers;
        //        case "EDGE":
        //            return basicNumbers.Select(a => (a / 2) + 1).ToList();
        //        case "CORNER":
        //            return basicNumbers.Select(a => (a / 4) + 1).ToList();
        //        default:
        //            break;
        //    }
        //    return basicNumbers;
        //}

        private PolyLine generateColumnFaces()
        {
            var x = (float)_columnAdim.Value / 2f; var y = (float)_columnBdim.Value / 2f;
            var returnList = new PolyLine(new List<GeometryBase>()
            {
                new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                new Line(new Vector2(x,-y), new Vector2(x,y)),
                new Line(new Vector2(x,y), new Vector2(-x,y))
            })
            { IsClosed = true };

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    returnList = new PolyLine(new List<GeometryBase>()
                    {
                        new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                        new Line(new Vector2(x,-y), new Vector2(x,y)),
                        new Line(new Vector2(x,y), new Vector2(-x,y)),
                    });
                    break;
                case "CORNER":

                    returnList = new PolyLine(new List<GeometryBase>()
                    {
                        new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                        new Line(new Vector2(x,-y), new Vector2(x,y)),
                    });
                    break;
                case "RE-ENTRANT":
                    returnList = new PolyLine(new List<GeometryBase>()
                    {
                        new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                        new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                        new Line(new Vector2(x,-y), new Vector2(x,y)),
                        new Line(new Vector2(x,y), new Vector2(-x,y))
                    })
                    { IsClosed = false };
                    break;
                default:
                    break;
            }
            return returnList;

        }

        private void generateColumnOutlines()
        {
            var x = (float)_columnAdim.Value / 2f; var y = (float)_columnBdim.Value / 2f;
            fullColumnOutline = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                new Line(new Vector2(x,-y), new Vector2(x,y)),
            })
            { IsClosed = true };
            columnOutline = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                new Line(new Vector2(x,-y), new Vector2(x,y)),
            })
            { IsClosed = true };

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    if (1.5 * d_average > _columnAdim.Value)
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,y)),
                            new Line(new Vector2(x,y), new Vector2(-x,y)),
                        });
                    }
                    else
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(x - 1.5f*(float)d_average,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y),new Vector2(x,y)),
                            new Line(new Vector2(x,y),new Vector2(x - 1.5f*(float)d_average,y)),
                        });
                    }
                    break;
                case "CORNER":
                    if (3 * d_average > _columnAdim.Value + _columnBdim.Value)
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,y)),
                        });
                    }
                    else
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(x-(float)Math.Min(1.5*d_average, _columnAdim.Value),-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,(float)Math.Min(1.5*d_average, _columnBdim.Value) -y)),
                        });
                    }
                    break;
                case "RE-ENTRANT":
                    columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                            new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,y)),
                            new Line(new Vector2(x,y), new Vector2(-x,y)),
                        })
                    { IsClosed = false };
                    break;
                default:
                    break;
            }
            columnOutlines = generatePerimeterWithHoles(columnOutline);
        }

        private void generateSlabEdge()
        {
            var x = (float)_columnAdim.Value / 2f;
            var y = (float)_columnBdim.Value / 2f;
            slabEdge = new PolyLine(new List<GeometryBase> { new Line(new Vector2(-x, y), new Vector2(x, y)) });
            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    slabEdge = new PolyLine(new List<GeometryBase>
                    {
                        new Line(new Vector2(-x, -10000), new Vector2(-x, 10000))
                    });
                    break;
                case "CORNER":
                    slabEdge = new PolyLine(new List<GeometryBase>
                    {
                        new Line(new Vector2(-x, -10000), new Vector2(-x, y)),
                        new Line(new Vector2(-x, y), new Vector2(10000, y))
                    });
                    break;
                case "RE-ENTRANT":
                    slabEdge = new PolyLine(new List<GeometryBase>
                    {
                        new Line(new Vector2(-10000,y), new Vector2(-x, y)),
                        new Line(new Vector2(-x, y), new Vector2(-x, 10000))
                    });
                    break;
                default:
                    break;
            }
        }

        static double calcBarSizeAndDia(double area, List<int> barSizes)
        {
            foreach (var item in barSizes)
            {
                if (Math.PI * Math.Pow(item / 2, 2) > area)
                {
                    return item;
                }
            }
            return double.NaN;
        }

        public static double shearResistanceNoRein(double rhoL, double d_eff, double charStrength, double concPartFact)
        {
            double k = Math.Min(1 + Math.Sqrt(200 / d_eff), 2);

            double cRdc = 0.18 / concPartFact;

            double resistance1 = cRdc * k * Math.Pow((100d * rhoL * charStrength), 1f / 3f);

            double resistance = Math.Max(resistance1, 0.035 * Math.Pow(k, 1.5) * Math.Pow(charStrength, 0.5));
            Console.WriteLine("rho" + rhoL + ";str" + charStrength + ";k" + k + ";vmin" + resistance + ";vRdc" + resistance1);
            return resistance;
        }

        public static double shearStressResistance(double charStrength)
        {
            // 0.5 v fcd where v = shear reduction factor for cracks
            // v = 0.6 ( 1 - fck / 250)
            // fcd = 1.0 * fck / 1.5
            // for C20 => 0.5 x 0.552 * (20/1.5) = 3.68
            List<Tuple<double, double>> charStrengthshearResistance = new List<Tuple<double, double>>
            {
                new Tuple<double, double>(12,2.2848),
                new Tuple<double, double>(16,2.9952),
                new Tuple<double, double>(20,3.68),
                new Tuple<double, double>(25,4.5),
                new Tuple<double, double>(28,4.98),
                new Tuple<double, double>(30,5.28),
                new Tuple<double, double>(32,5.58),
                new Tuple<double, double>(35,6.02),
                new Tuple<double, double>(40,6.72),
                new Tuple<double, double>(45,7.38),
                new Tuple<double, double>(50,8)
            };

            double returnDouble = 0;

            foreach (var item in charStrengthshearResistance)
            {
                if (charStrength >= item.Item1)
                {
                    returnDouble = item.Item2;
                }
            }
            return returnDouble;
        }

        private PolyLine generatePerimeter(double width, double depth, double offset)
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

            switch (_colType.ValueAsString)
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

        private PolyLine generateReducedControlPerimeter(double width, double depth)
        {
            float offsetF = 2f * (float)d_average;
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

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    float offx = (float)Math.Min(1.5 * d_average, 0.5 * _columnAdim.Value);
                    inter2 = perimeter.intersection(new Line(new Vector2(x - offx, -10000), new Vector2(x - offx, 0)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x - offx, 0), new Vector2(x - offx, 10000)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                case "CORNER":
                    float offx2 = (float)Math.Min(1.5 * d_average, 0.5 * _columnAdim.Value);
                    float offy2 = (float)Math.Min(1.5 * d_average, 0.5 * _columnBdim.Value);
                    inter2 = perimeter.intersection(new Line(new Vector2(x - offx2, -10000), new Vector2(x - offx2, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x, -y + offy2), new Vector2(10000, -y + offy2)));
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

        /// <summary>
        /// Calculates control perimeter limited in length according to TR64 clause 4.8.2
        /// </summary>
        /// <param name="width"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private PolyLine generateLimitedControlPerimeter(double width, double depth)
        {
            float offsetF = 2f * (float)d_average;
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

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    float offx = (float)Math.Min(3 * d_average, 1 * _columnAdim.Value);
                    inter2 = perimeter.intersection(new Line(new Vector2(x - offx, -10000), new Vector2(x - offx, 0)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x - offx, 0), new Vector2(x - offx, 10000)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                case "CORNER":
                    float offx2 = (float)Math.Min(3 * d_average, 1 * _columnAdim.Value);
                    float offy2 = (float)Math.Min(3 * d_average, 1 * _columnBdim.Value);
                    inter2 = perimeter.intersection(new Line(new Vector2(x - offx2, -10000), new Vector2(x - offx2, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x, -y + offy2), new Vector2(10000, -y + offy2)));
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

        private List<Tuple<Vector2, Vector2>> spurStartPoints()
        {
            var returnList = new List<Tuple<Vector2, Vector2>>();
            var innerPoints = new List<Vector2>();
            var x = (float)_columnAdim.Value / 2;
            var y = (float)_columnBdim.Value / 2;
            double startAngle = 0;
            int angleDivisions = 1;
            bool includeLastCorner = false;
            double quarterLength = 0.5 * Math.PI * 2.25 * d_average;
            angleDivisions = (int)Math.Ceiling(quarterLength / st);

            //switch (_linkArrangement.ValueAsString)
            //{
            //    case "SPURS_AUTO":
            //        angleDivisions = 3;
            //        break;
            //    case "GRID":
            //        angleDivisions = 3;
            //        break;
            //    default:
            //        break;
            //}



            double newx = 0; double newy = 0;
            if (_columnAdim.Value > 1.5 * d_average)
            {
                newx = x - Math.Min(0.5 * d_average, y);
            }
            else
            {
                newx = 0;
            }
            if (_columnBdim.Value > 1.5 * d_average)
            {
                newy = y - Math.Min(0.5 * d_average, x);
            }
            else
            {
                newy = 0;
            }

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    startAngle = 1 * Math.PI;
                    includeLastCorner = true;
                    break;
                case "EDGE":
                    innerPoints.Add(new Vector2(-(float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    startAngle = 1.5 * Math.PI;
                    break;
                case "CORNER":
                    innerPoints.Add(new Vector2(-(float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, (float)newy));
                    startAngle = 1.5 * Math.PI;
                    break;
                case "RE-ENTRANT":
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    startAngle = 1 * Math.PI;
                    includeLastCorner = false;
                    break;
                default:
                    break;
            }

            var angle = startAngle;
            for (int j = 1; j < innerPoints.Count; j++)
            {
                var point = innerPoints[j];

                Line line = new Line(innerPoints[j - 1], point);
                var dist = line.Length;
                if (dist > 0.5 * d_average) // really a check that dist>0
                {
                    int divs = (int)Math.Ceiling(dist / (st));
                    double paramFraction = 1d / divs;
                    for (int k = 0; k < divs; k++)
                    {
                        var point2 = line.PointAtParameter(k * paramFraction);
                        var ray = new Line(point2, point2 + new Vector2(10000f * (float)Math.Cos(angle), 10000f * (float)Math.Sin(angle)));
                        var vec = Vector2.Normalize(ray.End - ray.Start);
                        var inter = fullColumnOutline.intersection(ray);
                        var pt = inter[0].Point + vec * 0.5f * (float)d_average;
                        returnList.Add(new Tuple<Vector2, Vector2>(pt, Vector2.Normalize(vec)));
                    }
                }

                int cornerDivs = angleDivisions;
                if (j >= innerPoints.Count - 1 && !includeLastCorner)
                {
                    cornerDivs = 1;
                }
                for (int i = 0; i < cornerDivs; i++)
                {
                    var ray = new Line(point, point + new Vector2(10000f * (float)Math.Cos(angle), 10000f * (float)Math.Sin(angle)));
                    var vec = Vector2.Normalize(ray.End - ray.Start);
                    var inter = fullColumnOutline.intersection(ray);
                    var pt = inter[0].Point + vec * 0.5f * (float)d_average;
                    returnList.Add(new Tuple<Vector2, Vector2>(pt, Vector2.Normalize(vec)));
                    angle += Math.PI / (2d * angleDivisions);
                }

            }
            return returnList;
        }

        private List<PolyLine> generatePerimeterWithHoles(PolyLine perimeter)
        {
            var perimeters = new List<PolyLine>();
            var perimeter2 = perimeter;
            // check if hole within hole influence lines
            bool perimeterFullyInside = true;
            if (_holeEdges.Count == 0)
            {
                perimeterFullyInside = false;
            }
            foreach (var hole in _holeEdges)
            {
                var startAngle = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                var endAngle = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                if (endAngle < startAngle)
                {
                    endAngle += 2 * Math.PI;
                }
                foreach (var segment in perimeter.Segments)
                {
                    var angle1 = Math.Atan2(segment.Start.Y, segment.Start.X);
                    var angle2 = Math.Atan2(segment.End.Y, segment.End.X);
                    if (!((angle1 < endAngle && angle1 > startAngle) &&
                        (angle2 < endAngle && angle2 > startAngle) ||
                        (angle1 + Math.PI * 2 < endAngle && angle1 + Math.PI * 2 > startAngle) &&
                        (angle2 + Math.PI * 2 < endAngle && angle2 + Math.PI * 2 > startAngle)))
                    {
                        perimeterFullyInside = false;
                    }
                }
                if (perimeterFullyInside)
                {
                    return new List<PolyLine>();
                }
            }




            var listOfHoleIntersections = new List<Tuple<IntersectionResult, IntersectionResult>>();
            foreach (var hole in _holeEdges)
            {
                var inter1 = perimeter2.intersection(hole.Item1);
                var inter2 = perimeter2.intersection(hole.Item2);
                listOfHoleIntersections.Add(new Tuple<IntersectionResult, IntersectionResult>(inter1[0], inter2[0]));
            }
            int holeCounter = 0;
            var startStops = new List<Tuple<double, bool>>();

            foreach (var item in listOfHoleIntersections)
            {
                if (item.Item1.TypeOfIntersection == IntersectionType.WITHIN && item.Item2.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    if (item.Item1.Parameter < item.Item2.Parameter)
                    {
                        startStops.Add(new Tuple<double, bool>(item.Item1.Parameter, true));
                        startStops.Add(new Tuple<double, bool>(item.Item2.Parameter, false));
                    }
                    else
                    {
                        startStops.Add(new Tuple<double, bool>(item.Item1.Parameter, true));
                        startStops.Add(new Tuple<double, bool>(item.Item2.Parameter, false));
                        holeCounter++;
                    }

                }
                else if (item.Item1.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    startStops.Add(new Tuple<double, bool>(item.Item1.Parameter, true));
                }
                else if (item.Item2.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    startStops.Add(new Tuple<double, bool>(item.Item2.Parameter, false));
                    holeCounter++;
                }
            }

            var orderedStartStops = startStops.OrderBy(a => a.Item1).ToList();
            var listOfCuts = new List<double>();
            double prevParam = 0;
            foreach (var item in orderedStartStops)
            {
                if (holeCounter < 1 && item.Item2 == true)
                {
                    listOfCuts.Add(prevParam);
                    listOfCuts.Add(item.Item1);
                    prevParam = item.Item1;
                    holeCounter++;
                }
                else if (item.Item2 == true)
                {
                    holeCounter++;
                }
                else if (item.Item2 == false)
                {
                    holeCounter--;
                    prevParam = item.Item1;
                }
            }
            if (holeCounter < 1)
            {
                listOfCuts.Add(prevParam);
                listOfCuts.Add(1);
            }

            for (int i = 0; i < listOfCuts.Count / 2; i++)
            {
                perimeters.Add(perimeter2.Cut(listOfCuts[i * 2], listOfCuts[1 + i * 2]));
            }
            return perimeters;
        }


        private List<PolyLine> generatePerimeterWithHoles(double offset)
        {
            var perimeter2 = generatePerimeter(_columnAdim.Value, _columnBdim.Value, offset);
            return generatePerimeterWithHoles(perimeter2);
        }

        private List<PolyLine> generateReducedControlPerimeterWithHoles()
        {
            var perimeter = generateReducedControlPerimeter(_columnAdim.Value, _columnBdim.Value);
            return generatePerimeterWithHoles(perimeter);
        }

        private List<PolyLine> generateLimitedControlPerimeterWithHoles()
        {
            var perimeter = generateLimitedControlPerimeter(_columnAdim.Value, _columnBdim.Value);
            return generatePerimeterWithHoles(perimeter);
        }

        private SkiaSharp.SKPath generateGeometry(PolyLine polyline)
        {
            SkiaSharp.SKPath path = new SkiaSharp.SKPath();
            path.MoveTo(new SkiaSharp.SKPoint(polyline.Start.X, polyline.Start.Y));
            foreach (var item in polyline.Segments)
            {
                if (item.GetType() == typeof(Arc))
                {
                    var arc = item as Arc;
                    float startDegrees = (float)(arc.StartAngle * 180 / Math.PI);
                    float endDegrees = (float)((arc.EndAngle - arc.StartAngle) * 180 / Math.PI);
                    path.AddArc(new SkiaSharp.SKRect((float)(arc.Centre.X - arc.Radius),
                        (float)(arc.Centre.Y - arc.Radius),
                        (float)(arc.Centre.X + arc.Radius),
                        (float)(arc.Centre.Y + arc.Radius)),
                        startDegrees,
                        endDegrees);
                }
                if (item.GetType() == typeof(Line))
                {
                    var line = item as Line;
                    path.LineTo(line.End.X, line.End.Y);
                }
            }
            return path;
        }

        private List<SkiaSharp.SKPath> generateVoidCross(List<PolyLine> polylines)
        {
            var returnGeometry = new List<SkiaSharp.SKPath>();
            foreach (var polyline in polylines)
            {
                var seg = polyline.Segments;
                SkiaSharp.SKPath path = new SkiaSharp.SKPath();
                path.MoveTo(seg[0].Start.X, seg[0].Start.Y);
                path.LineTo(seg[2].Start.X, seg[2].Start.Y);
                returnGeometry.Add(path);
                path = new SkiaSharp.SKPath();
                path.MoveTo(seg[1].Start.X, seg[1].Start.Y);
                path.LineTo(seg[3].Start.X, seg[3].Start.Y);
                returnGeometry.Add(path);
            }
            return returnGeometry;
        }

        private List<SkiaSharp.SKPath> generateGeometry(List<PolyLine> polylines)
        {
            var figPaths = new List<SkiaSharp.SKPath>();
            foreach (var polyline in polylines)
            {
                figPaths.Add(generateGeometry(polyline));

            }
            return figPaths;
        }

        private List<SkiaSharp.SKBitmap> generateImage()
        {
            var voidGeometry = generateVoidCross(holeOutlines);
            //var spursGeometry = new List<SkiaSharp.SKPath>();
            var linksGeometry = new List<SkiaSharp.SKPath>();
            var holeEdgeLinesGeometry = new List<SkiaSharp.SKPath>();
            var axes = new List<SkiaSharp.SKPath>();
            //    var axesLabels = new GeometryGroup();
            var allPerimetersFlattened = new List<PolyLine>();
            foreach (var item in perimetersToReinforce)
            {
                foreach (var section in item)
                {
                    allPerimetersFlattened.Add(section);
                }
            }
            var innerLinksPerimeterGeometry = generateGeometry(allPerimetersFlattened);
            var outerLinksPerimeterGeometry = generateGeometry(shearLinkEndPerimeters);

            foreach (var hole in _holeEdges)
            {
                var line = hole.Item1;
                var path1 = new SkiaSharp.SKPath();
                path1.MoveTo(line.Start.X, line.Start.Y);
                path1.LineTo(line.End.X, line.End.Y);
                holeEdgeLinesGeometry.Add(path1);
                line = hole.Item2;
                var path2 = new SkiaSharp.SKPath();
                path2.MoveTo(line.Start.X, line.Start.Y);
                path2.LineTo(line.End.X, line.End.Y);
                holeEdgeLinesGeometry.Add(path2);
            }
            {
                var path1 = new SkiaSharp.SKPath();
                path1.MoveTo(-10000, 0);
                path1.LineTo(10000, 0);
                var path2 = new SkiaSharp.SKPath();
                path2.MoveTo(0, -10000);
                path2.LineTo(0, 10000);
                axes.Add(path1);
                axes.Add(path2);
            }

            //    foreach (var spur in shearSpurs)
            //    {
            //        spursGeometry.Children.Add(new LineGeometry(new Point(spur.Start.X, spur.Start.Y), new Point(spur.End.X, spur.End.Y)));
            //    }
            foreach (var spur in shearLinks)
            {
                foreach (var link in spur)
                {
                    SkiaSharp.SKPath path = new SkiaSharp.SKPath();
                    path.AddCircle(link.X, link.Y, 15);
                    linksGeometry.Add(path);
                }
            }

            DisplayDataSet dataset = DisplayDataSet.GetDisplayDataSet();
            dataset.AddFormattingInstruction("ControlPerimeter", new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Transparent }, new SkiaSharp.SKPaint { StrokeWidth = 6, Color = SkiaSharp.SKColors.Red });
            dataset.AddFormattingInstruction("EffectiveControlPerimeter", new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Transparent }, new SkiaSharp.SKPaint { StrokeWidth = 15, Color = SkiaSharp.SKColors.Red });
            dataset.AddFormattingInstruction("LoadedFace", new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Transparent }, new SkiaSharp.SKPaint { StrokeWidth = 9, Color = SkiaSharp.SKColors.Red });
            dataset.AddFormattingInstruction("OuterPerimeter", new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Transparent }, new SkiaSharp.SKPaint { StrokeWidth = 9, Color = SkiaSharp.SKColors.Green });

            StructuralDrawing2D.StructuralDrawing2D drawing = new StructuralDrawing2D.StructuralDrawing2D(dataset);
            drawing.AddElement(DisplayFormatPreset.ConcreteSlabPlan, generateGeometry(slabEdge), false);
            foreach (var item in generateGeometry(holeOutlines))
            {
                drawing.AddElement(DisplayFormatPreset.ConcreteSlabPlan, item, true);
            }
            drawing.AddElement(DisplayFormatPreset.ConcreteColumnSection, generateGeometry(fullColumnOutline), true);
            foreach (var item in generateGeometry(columnOutlines))
            {
                drawing.AddElement("LoadedFace", item, true);
            }
            foreach (var item in generateVoidCross(holeOutlines))
            {
                drawing.AddElement(DisplayFormatPreset.Hatching, item, true);
            }
            foreach (var item in innerLinksPerimeterGeometry)
            {
                drawing.AddElement(DisplayFormatPreset.Centreline, item, true);
            }
            foreach (var item in outerLinksPerimeterGeometry)
            {
                drawing.AddElement(DisplayFormatPreset.Centreline, item, true);
            }
            foreach (var item in generateGeometry(controlPerimeters))
            {
                drawing.AddElement("ControlPerimeter", item, true);
            }
            foreach (var item in generateGeometry(u1reduced))
            {
                drawing.AddElement("EffectiveControlPerimeter", item, true);
            }
            foreach (var item in generateGeometry(outerPerimeters))
            {
                drawing.AddElement("OuterPerimeter", item, true);
            }

            //drawing.AddElement(DisplayFormatPreset.Centreline, spursGeometry, true);
            foreach (var item in linksGeometry)
            {
                drawing.AddElement(DisplayFormatPreset.RebarSection, item, true);
            }

            foreach (var item in holeEdgeLinesGeometry)
            {
                drawing.AddElement(DisplayFormatPreset.Centreline, item, false);

            }
            foreach (var item in axes)
            {
                drawing.AddElement(DisplayFormatPreset.Gridline, item, false);
            }
            drawing.AddText(new DrawingText("Y", drawing.GetBounds().Right, -25, 50));
            drawing.AddText(new DrawingText("Y", drawing.GetBounds().Left, -25, 50));
            drawing.AddText(new DrawingText("Z", 0, drawing.GetBounds().Top , 50));
            drawing.AddText(new DrawingText("Z",0, drawing.GetBounds().Bottom , 50));


            var keyImage = new SkiaSharp.SKBitmap(1000,200);
            using (SkiaSharp.SKCanvas canvas = new SkiaSharp.SKCanvas(keyImage))
            {
                var paintText = new SkiaSharp.SKPaint { TextSize = 25, TextAlign = SkiaSharp.SKTextAlign.Left };
                var paint = dataset.DrawingFormatting[DisplayFormatPreset.ConcreteColumnSection.ToString()].Outline;
                canvas.DrawLine(0, 25, 200, 25, paint);
                canvas.DrawText("Column face", 250, 35, paintText);
                paint = dataset.DrawingFormatting[DisplayFormatPreset.ConcreteSlabPlan.ToString()].Outline;
                canvas.DrawLine(0, 75, 200, 75, paint);
                canvas.DrawText("Slab edge", 250, 85, paintText);
                paint = dataset.DrawingFormatting[DisplayFormatPreset.Centreline.ToString()].Outline;
                canvas.DrawLine(0, 125, 200, 125, paint);
                canvas.DrawText("Hole effect", 250, 135, paintText);
                paint = dataset.DrawingFormatting["EffectiveControlPerimeter"].Outline;
                canvas.DrawLine(0, 175, 200, 175, paint);
                canvas.DrawText("Reduced control u1*", 250, 185, paintText);
                paint = dataset.DrawingFormatting["ControlPerimeter"].Outline;
                canvas.DrawLine(500, 25, 700, 25, paint);
                canvas.DrawText("Control perimeter u1", 750, 35, paintText);
                paint = dataset.DrawingFormatting["OuterPerimeter"].Outline;
                canvas.DrawLine(500, 75, 700, 75, paint);
                canvas.DrawText("Outer perimeter", 750, 85, paintText);
                paint = dataset.DrawingFormatting["LoadedFace"].Outline;
                canvas.DrawLine(500, 125, 700, 125, paint);
                canvas.DrawText("Loaded column face", 750, 135, paintText);
                paint = dataset.DrawingFormatting[DisplayFormatPreset.RebarSection.ToString()].Infill;
                canvas.DrawCircle(550, 175, 15, paint);
                canvas.DrawCircle(600, 175, 15, paint);
                canvas.DrawCircle(650, 175, 15, paint);
                paint = dataset.DrawingFormatting[DisplayFormatPreset.RebarSection.ToString()].Outline;
                paint.IsStroke = true; paint.StrokeWidth = 5;
                canvas.DrawCircle(550, 175, 15, paint);
                canvas.DrawCircle(600, 175, 15, paint);
                canvas.DrawCircle(650, 175, 15, paint);
                canvas.DrawText("Shear links", 750, 185, paintText);

            }


            //    var keyImage = new RenderTargetBitmap(1000, 200, 96, 96, PixelFormats.Pbgra32);
            //    var keyVisual = new DrawingVisual();
            //    using (var r = keyVisual.RenderOpen())
            //    {
            //        GeometryCollection textKey = new GeometryCollection();

            //        var columnLine = new LineGeometry(new Point(0, 25), new Point(200, 25));
            //        textKey.Add(gettext("Column face", new Point(250, 25)));
            //        var slabLine = new LineGeometry(new Point(0, 75), new Point(200, 75));
            //        textKey.Add(gettext("Slab edge", new Point(250, 75)));
            //        var holeEdgeLine = new LineGeometry(new Point(0, 125), new Point(200, 125));
            //        textKey.Add(gettext("Hole effect", new Point(250, 125)));
            //        var redControlPerimLine = new LineGeometry(new Point(0, 175), new Point(200, 175));
            //        textKey.Add(gettext("Effective control" + Environment.NewLine + "perimeter u1*", new Point(250, 175)));
            //        var conPerimLine = new LineGeometry(new Point(500, 25), new Point(700, 25));
            //        textKey.Add(gettext("Control perimeter u1", new Point(750, 25)));
            //        var OutPerimLine = new LineGeometry(new Point(500, 75), new Point(700, 75));
            //        textKey.Add(gettext("Outer perimeter", new Point(750, 75)));
            //        var colLoadedFaceLine = new LineGeometry(new Point(500, 125), new Point(700, 125));
            //        textKey.Add(gettext("Loaded column face", new Point(750, 125)));
            //        var linksCircle = new GeometryGroup()
            //        {
            //            Children = new GeometryCollection(new List<Geometry> {
            //                new EllipseGeometry(new Point(550, 175), 8, 8),
            //                new EllipseGeometry(new Point(600, 175), 8, 8),
            //                new EllipseGeometry(new Point(650, 175), 8, 8)
            //            })
            //        };
            //        textKey.Add(gettext("Links", new Point(750, 175)));

            //        var textKeyGroup = new GeometryGroup() { Children = textKey };

            //        var dashedLine = new Pen(Brushes.Gray, 2) { DashStyle = new DashStyle(new List<double> { 20, 5, 5, 5 }, 0) };
            //        var dashedLine2 = new Pen(Brushes.Gray, 2) { DashStyle = new DashStyle(new List<double> { 10, 5 }, 0) };

            //        r.DrawGeometry(Brushes.LightGreen, new Pen(Brushes.Green, 5), columnLine);
            //        r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Yellow, 3), slabLine);
            //        r.DrawGeometry(Brushes.Transparent, dashedLine2, holeEdgeLine);
            //        r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 8), redControlPerimLine);
            //        r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 4), conPerimLine);
            //        r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Green, 5), OutPerimLine);
            //        r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 5), colLoadedFaceLine);
            //        r.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Black, 1), linksCircle);
            //        r.DrawGeometry(Brushes.Black, new Pen(Brushes.Black, 1), textKeyGroup);
            //    }

            //    keyImage.Render(keyVisual);


            //    //TEST
            var bitmap = drawing.GenerateBitmapImage(1000, 1000, 50);
            return new List<SkiaSharp.SKBitmap> { bitmap, keyImage};
        }

        //private Geometry gettext(string text, Point pos)
        //{
        //    FormattedText returnText = new FormattedText(text,
        //        CultureInfo.CurrentCulture,
        //        FlowDirection.LeftToRight,
        //        new Typeface(new FontFamily("Franklin Gothic Book"), FontStyles.Normal, FontWeights.Light, FontStretches.Normal),
        //        24,
        //        Brushes.Black);
        //    var pos1 = new Point(pos.X, pos.Y - returnText.Height / 2);
        //    Geometry geometry = returnText.BuildGeometry(pos1);
        //    return geometry;
        //}

        //private Geometry gettext(string text, Point pos, double scale)
        //{
        //    FormattedText returnText = new FormattedText(text,
        //        CultureInfo.CurrentCulture,
        //        FlowDirection.LeftToRight,
        //        new Typeface(new FontFamily("Franklin Gothic Book"), FontStyles.Normal, FontWeights.Light, FontStretches.Normal),
        //        48,
        //        Brushes.Black);
        //    var pos1 = new Point(pos.X - returnText.Width / 2, pos.Y - returnText.Height / 2);
        //    Geometry geometry = returnText.BuildGeometry(pos1);
        //    geometry.Transform = new ScaleTransform(scale, -scale, pos.X, pos.Y);
        //    return geometry;
        //}
        //}

        public override List<MW3DModel> Get3DModels()
        {
            MWMesh mesh = new MWMesh();
            double minx = -_columnAdim.Value / 2;
            double maxx = -minx;
            double miny = -_columnBdim.Value / 2;
            double maxy = -miny;

            mesh.addNode(minx, miny, -1000, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, miny, -1000, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, maxy, -1000, new MWPoint2D(0.5, 0.5));
            mesh.addNode(minx, maxy, -1000, new MWPoint2D(0.5, 0.5));
            mesh.addNode(minx, miny, 1000, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, miny, 1000, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, maxy, 1000, new MWPoint2D(0.5, 0.5));
            mesh.addNode(minx, maxy, 1000, new MWPoint2D(0.5, 0.5));

            mesh.setIndices(new List<int[]>
            {   new int[] { 0, 4, 7 },
                new int[] { 7, 3, 0 },
                new int[] { 1, 5, 4 },
                new int[] { 4, 0, 1 },
                new int[] { 2, 6, 5 },
                new int[] { 5, 1, 2 },
                new int[] { 2, 3, 6 },
                new int[] { 6, 3, 7 },
                new int[] { 4, 5, 6 },
                new int[] { 6, 7, 4 }
            });
            mesh.Opacity = 1;
            mesh.Brush = new MWBrush(0, 255, 128);

            var returnModel = new MW3DModel(mesh);

            foreach (var item in shearLinks)
            {
                foreach (var item2 in item)
                {
                    returnModel.Meshes.Add(meshLink(12, item2));
                }
            }

            foreach (var item in meshSlab())
            {
                returnModel.Meshes.Add(item);
            }

            return new List<MW3DModel> { returnModel };
        }

        private List<MWMesh> meshSlab()
        {
            double minx = -2500;
            double maxx = 2500;
            double miny = -2500;
            double maxy = 2500;
            if (_colType.ValueAsString=="EDGE")
            {
                minx = -_columnAdim.Value / 2;
            }
            else if (_colType.ValueAsString=="CORNER")
            {
                minx = -_columnAdim.Value / 2;
                maxy = _columnBdim.Value / 2;
            }
            else if (_colType.ValueAsString=="RE-ENTRANT")
            {
                maxx = -_columnAdim.Value / 2;
                miny = _columnBdim.Value / 2;
            };

            // generate mesh for slab with holes with Triangle
            var slabOutline = new TriangleNet.Geometry.Polygon();
            slabOutline.AddContour(new List<Vertex> { new Vertex(minx, miny), new Vertex(maxx, miny), new Vertex(maxx, maxy), new Vertex(minx, maxy) }, 1);
            if (_holeSizeX.Value > 0 && _holeSizeY.Value > 0)
            {
                slabOutline.AddContour(new List<Vertex> {
                new Vertex(_holePosX.Value, _holePosY.Value),
                new Vertex(_holePosX.Value + _holeSizeX.Value, _holePosY.Value),
                new Vertex(_holePosX.Value + _holeSizeX.Value, _holePosY.Value + _holeSizeY.Value),
                new Vertex(_holePosX.Value, _holePosY.Value + _holeSizeY.Value)}, 2, new Point(_holePosX.Value + _holeSizeX.Value / 2, _holePosY.Value + _holeSizeY.Value / 2));
            }
            if (_hole2SizeX.Value > 0 && _hole2SizeY.Value > 0)
            {
                slabOutline.AddContour(new List<Vertex> {
                new Vertex(_hole2PosX.Value, _hole2PosY.Value),
                new Vertex(_hole2PosX.Value + _hole2SizeX.Value, _hole2PosY.Value),
                new Vertex(_hole2PosX.Value + _hole2SizeX.Value, _hole2PosY.Value + _hole2SizeY.Value),
                new Vertex(_hole2PosX.Value, _hole2PosY.Value + _hole2SizeY.Value)}, 3, new Point(_hole2PosX.Value + _hole2SizeX.Value / 2, _hole2PosY.Value + _hole2SizeY.Value / 2));
            }
            var meshedSlab = slabOutline.Triangulate();
            var meshTriangles = meshedSlab.Triangles.ToList();
            MWMesh slabMesh2 = new MWMesh();
            foreach (var vertex in meshedSlab.Vertices)
            {
                slabMesh2.addNode(vertex.X, vertex.Y, _h.Value / 2, new MWPoint2D(0.5, 0.5));
            }
            foreach (var triangle in meshTriangles)
            {
                slabMesh2.MeshIndices.Add(new int[] { triangle.GetVertexID(0), triangle.GetVertexID(1), triangle.GetVertexID(2) });
            }

            var mesh = slabMesh2;

            mesh.Opacity = 0.5;
            mesh.Brush = new MWBrush(255, 255, 0);

            var outline = mesh.GetMeshOutlines();
            var sidemeshes = new List<MWMesh>();
            foreach (var individualOutline in outline)
            {
                var outlinePoints = new List<MWPoint3D>();
                foreach (var item in individualOutline)
                {
                    outlinePoints.Add(mesh.Nodes[item].Point);
                }
                var sideMesh = MWMesh.makeExtrudedPolygon(new MWPoint3D(0, 0, 0), new MWPoint3D(0, 0, -_h.Value), outlinePoints);
                sideMesh.Opacity = 0.5;
                sideMesh.Brush = new MWBrush(255, 255, 0);
                sidemeshes.Add(sideMesh);
            }
            

            var bottomMesh = new MWMesh();
            foreach (var item in mesh.Nodes)
            {
                bottomMesh.addNode(item.Point.X, item.Point.Y, item.Point.Z - _h.Value, new MWPoint2D(0.5, 0.5));
            }
            var newIndices = new List<int[]>();
            foreach (var item in mesh.MeshIndices)
            {
                newIndices.Add(new int[] { item[0], item[1], item[2] });
            }
            bottomMesh.setIndices(newIndices);
            bottomMesh.Opacity = 0.5;
            bottomMesh.Brush = new MWBrush(255, 255, 0);
            bottomMesh.reverseFaceDirections();

            var returnList = new List<MWMesh>();
            returnList.Add(mesh);
            foreach (var item in sidemeshes)
            {
                returnList.Add(item);
            }
            returnList.Add(bottomMesh);

            return returnList;

        }

        MWMesh meshLink(double dia, Vector2 centre)
        {
            MWMesh mesh = new MWMesh();
            double minx = centre.X - dia / 2;
            double maxx = centre.X + dia / 2;
            double miny = centre.Y -dia / 2;
            double maxy = centre.Y + dia / 2;
            double d = d_average;

            mesh.addNode(minx, miny, -d/2, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, miny, -d/2, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, maxy, -d/2, new MWPoint2D(0.5, 0.5));
            mesh.addNode(minx, maxy, -d/2, new MWPoint2D(0.5, 0.5));
            mesh.addNode(minx, miny, d/2, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, miny, d/2, new MWPoint2D(0.5, 0.5));
            mesh.addNode(maxx, maxy, d/2, new MWPoint2D(0.5, 0.5));
            mesh.addNode(minx, maxy, d/2, new MWPoint2D(0.5, 0.5));

            mesh.setIndices(new List<int[]>
            {   new int[] { 0, 4, 7 },
                new int[] { 7, 3, 0 },
                new int[] { 1, 5, 4 },
                new int[] { 4, 0, 1 },
                new int[] { 2, 6, 5 },
                new int[] { 5, 1, 2 },
                new int[] { 2, 3, 6 },
                new int[] { 6, 3, 7 },
                new int[] { 4, 5, 6 },
                new int[] { 6, 7, 4 }
            });
            mesh.Opacity = 1;
            mesh.Brush = new MWBrush(128, 128, 128);
            return mesh;
        }
    }
}
