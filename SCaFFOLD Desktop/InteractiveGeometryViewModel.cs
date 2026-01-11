using System;
using System.Collections.Generic; // For List recursion
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Windows; // For Point, Size
using System.Windows.Input;
using System.Windows.Media; // For SweepDirection
using Scaffold.Core.Geometry;
using Scaffold.Core.Geometry.Abstract;

namespace SCaFFOLD_Desktop
{
    public class InteractiveGeometryViewModel : ViewModelBase
    {
        private readonly IInteractiveGeometry _model;
        private readonly Action _onGeometryChanged;

        public InteractiveGeometryViewModel(IInteractiveGeometry model, Action onGeometryChanged)
        {
            _model = model;
            _onGeometryChanged = onGeometryChanged;
            InitializeGeometry();
        }

        public ObservableCollection<GeometryPointViewModel> Points { get; } = new ObservableCollection<GeometryPointViewModel>();

        // Renamed to support mixed geometry types (Lines and Arcs)
        public ObservableCollection<ViewModelBase> StaticGeometry { get; } = new ObservableCollection<ViewModelBase>();

        // Scaling / Viewport Logic
        private double _scaleX = 1;
        private double _scaleY = 1;
        private double _offsetX = 0;
        private double _offsetY = 0;
        private const double CanvasSize = 1000;

        private void InitializeGeometry()
        {
            Points.Clear();
            StaticGeometry.Clear();

            if (_model == null) return;

            // 1. Load Interactive Points
            if (_model.InteractiveGeometryItems != null)
            {
                foreach (var item in _model.InteractiveGeometryItems)
                {
                    Points.Add(new GeometryPointViewModel(item, OnPointMoved));
                }
            }

            // 2. Load Static Geometry (Lines, Arcs, Polylines)
            if (_model.Geometry != null)
            {
                foreach (var geo in _model.Geometry)
                {
                    AddGeometry(geo);
                }
            }

            CalculateExtents();
        }

        // Recursive helper to handle Polylines and mixed types
        private void AddGeometry(GeometryBase geo)
        {
            if (geo == null) return;

            if (geo is Line line)
            {
                var vm = new GeometryLineViewModel(line.Start, line.End);
                vm.SetTransform(_scaleX, _scaleY, _offsetX, _offsetY);
                StaticGeometry.Add(vm);
            }
            else if (geo is Arc arc)
            {
                var vm = new GeometryArcViewModel(arc);
                vm.SetTransform(_scaleX, _scaleY, _offsetX, _offsetY);
                StaticGeometry.Add(vm);
            }
            else if (geo is PolyLine poly)
            {
                // Decompose PolyLine into segments
                if (poly.Segments != null)
                {
                    foreach (var segment in poly.Segments)
                    {
                        AddGeometry(segment);
                    }
                }
            }
        }

        public void Refresh()
        {
            // 1. Refresh Static Geometry
            StaticGeometry.Clear();
            if (_model.Geometry != null)
            {
                foreach (var geo in _model.Geometry)
                {
                    AddGeometry(geo);
                }
            }

            // 2. Refresh Points
            foreach (var point in Points)
            {
                point.RefreshPosition();
            }
        }

        private void CalculateExtents()
        {
            if (Points.Count == 0 && StaticGeometry.Count == 0) return;

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;
            bool hasData = false;

            // Points Extents
            if (Points.Count > 0)
            {
                minX = Points.Min(p => p.RawX);
                maxX = Points.Max(p => p.RawX);
                minY = Points.Min(p => p.RawY);
                maxY = Points.Max(p => p.RawY);
                hasData = true;
            }

            // Static Geometry Extents
            foreach (var item in StaticGeometry)
            {
                if (item is GeometryLineViewModel l)
                {
                    minX = Math.Min(minX, Math.Min(l.RawStart.X, l.RawEnd.X));
                    maxX = Math.Max(maxX, Math.Max(l.RawStart.X, l.RawEnd.X));
                    minY = Math.Min(minY, Math.Min(l.RawStart.Y, l.RawEnd.Y));
                    maxY = Math.Max(maxY, Math.Max(l.RawStart.Y, l.RawEnd.Y));
                    hasData = true;
                }
                else if (item is GeometryArcViewModel a)
                {
                    // Approximation using bounding box of the full circle for safety
                    // (Could be optimized to exact arc bounds if needed)
                    double r = a.RawRadius;
                    minX = Math.Min(minX, a.RawCentre.X - r);
                    maxX = Math.Max(maxX, a.RawCentre.X + r);
                    minY = Math.Min(minY, a.RawCentre.Y - r);
                    maxY = Math.Max(maxY, a.RawCentre.Y + r);
                    hasData = true;
                }
            }

            if (!hasData) return;

            double width = maxX - minX;
            double height = maxY - minY;

            if (width < 0.001) width = 10;
            if (height < 0.001) height = 10;

            minX -= width * 0.05;
            minY -= height * 0.05;
            width *= 1.1;
            height *= 1.1;

            double scale = Math.Min(CanvasSize / width, CanvasSize / height);

            _scaleX = scale;
            _scaleY = -scale;
            _offsetX = -minX * _scaleX;
            _offsetY = CanvasSize - (minY * _scaleY);

            UpdateTransforms();
        }

        private void UpdateTransforms()
        {
            foreach (var p in Points) p.SetTransform(_scaleX, _scaleY, _offsetX, _offsetY);

            foreach (var item in StaticGeometry)
            {
                if (item is GeometryLineViewModel l) l.SetTransform(_scaleX, _scaleY, _offsetX, _offsetY);
                else if (item is GeometryArcViewModel a) a.SetTransform(_scaleX, _scaleY, _offsetX, _offsetY);
            }
        }

        private void OnPointMoved()
        {
            _onGeometryChanged?.Invoke();
        }
    }

    // --- VIEW MODELS FOR GEOMETRY TYPES ---

    public class GeometryLineViewModel : ViewModelBase
    {
        public Vector2 RawStart { get; }
        public Vector2 RawEnd { get; }
        private double _scaleX, _scaleY, _offsetX, _offsetY;

        public GeometryLineViewModel(Vector2 start, Vector2 end)
        {
            RawStart = start;
            RawEnd = end;
        }

        public double X1 => RawStart.X * _scaleX + _offsetX;
        public double Y1 => RawStart.Y * _scaleY + _offsetY;
        public double X2 => RawEnd.X * _scaleX + _offsetX;
        public double Y2 => RawEnd.Y * _scaleY + _offsetY;

        public void SetTransform(double sx, double sy, double ox, double oy)
        {
            _scaleX = sx; _scaleY = sy; _offsetX = ox; _offsetY = oy;
            OnPropertyChanged(nameof(X1));
            OnPropertyChanged(nameof(Y1));
            OnPropertyChanged(nameof(X2));
            OnPropertyChanged(nameof(Y2));
        }
    }

    public class GeometryArcViewModel : ViewModelBase
    {
        private readonly Arc _model;
        private double _scaleX, _scaleY, _offsetX, _offsetY;

        public GeometryArcViewModel(Arc model)
        {
            _model = model;
        }

        public Vector2 RawCentre => _model.Centre;
        public double RawRadius => _model.Radius;

        // WPF ArcSegment Properties
        public Point StartPoint => Transform(_model.Start);
        public Point EndPoint => Transform(_model.End);

        public Size Size
        {
            get
            {
                // Uniform scaling assumption
                double r = Math.Abs(_model.Radius * _scaleX);
                return new Size(r, r);
            }
        }

        public bool IsLargeArc
        {
            get
            {
                double angleDiff = _model.EndAngle - _model.StartAngle;
                // Normalize angle
                while (angleDiff < 0) angleDiff += 2 * Math.PI;
                return angleDiff > Math.PI;
            }
        }

        // Coordinate flip (Y-up to Y-down) reverses visual direction
        public SweepDirection SweepDirection => SweepDirection.Counterclockwise;

        public double RotationAngle => 0; // Circular arc

        private Point Transform(Vector2 v)
        {
            return new Point(v.X * _scaleX + _offsetX, v.Y * _scaleY + _offsetY);
        }

        public void SetTransform(double sx, double sy, double ox, double oy)
        {
            _scaleX = sx; _scaleY = sy; _offsetX = ox; _offsetY = oy;
            OnPropertyChanged(nameof(StartPoint));
            OnPropertyChanged(nameof(EndPoint));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(IsLargeArc));
        }
    }

    // GeometryPointViewModel remains unchanged from previous steps
    public class GeometryPointViewModel : ViewModelBase
    {
        private readonly IInteractiveGeometryItem _model;
        private readonly Action _onMoved;
        private double _scaleX, _scaleY, _offsetX, _offsetY;

        public GeometryPointViewModel(IInteractiveGeometryItem model, Action onMoved)
        {
            _model = model;
            _onMoved = onMoved;
        }

        public string Symbol => _model.Symbol;
        public string Summary => _model.Summary;
        public double RawX => _model.PositionX;
        public double RawY => _model.PositionY;

        public double X
        {
            get => RawX * _scaleX + _offsetX;
            set
            {
                double newRawX = (value - _offsetX) / _scaleX;
                if (Math.Abs(_model.PositionX - newRawX) > 0.0001)
                {
                    _model.PositionX = newRawX;
                    OnPropertyChanged();
                    _onMoved?.Invoke();
                }
            }
        }

        public double Y
        {
            get => RawY * _scaleY + _offsetY;
            set
            {
                double newRawY = (value - _offsetY) / _scaleY;
                if (Math.Abs(_model.PositionY - newRawY) > 0.0001)
                {
                    _model.PositionY = newRawY;
                    OnPropertyChanged();
                    _onMoved?.Invoke();
                }
            }
        }

        public void RefreshPosition()
        {
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }

        public void SetTransform(double sx, double sy, double ox, double oy)
        {
            _scaleX = sx; _scaleY = sy; _offsetX = ox; _offsetY = oy;
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }
    }
}
