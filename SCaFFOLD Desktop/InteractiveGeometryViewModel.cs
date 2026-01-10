using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Windows.Input;
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
        public ObservableCollection<GeometryLineViewModel> Lines { get; } = new ObservableCollection<GeometryLineViewModel>();

        // Scaling / Viewport Logic
        private double _scaleX = 1;
        private double _scaleY = 1;
        private double _offsetX = 0;
        private double _offsetY = 0;
        private const double CanvasSize = 1000;

        private void InitializeGeometry()
        {
            Points.Clear();
            Lines.Clear();

            if (_model == null) return;

            // 1. Load Interactive Points
            if (_model.InteractiveGeometryItems != null)
            {
                foreach (var item in _model.InteractiveGeometryItems)
                {
                    Points.Add(new GeometryPointViewModel(item, OnPointMoved));
                }
            }

            // 2. Load Static Geometry
            LoadLines();

            CalculateExtents();
        }

        private void LoadLines()
        {
            Lines.Clear();
            if (_model.Geometry != null)
            {
                foreach (var geo in _model.Geometry)
                {
                    if (geo != null)
                    {
                        var lineVm = new GeometryLineViewModel(geo.Start, geo.End);
                        // Apply the CURRENT transform immediately
                        lineVm.SetTransform(_scaleX, _scaleY, _offsetX, _offsetY);
                        Lines.Add(lineVm);
                    }
                }
            }
        }

        // Method to refresh view when model changes (e.g. during drag)
        public void Refresh()
        {
            // 1. Refresh Lines (Outputs)
            LoadLines();

            // 2. Refresh Points (Inputs/Constraints)
            // Just in case the calculation snapped/moved a point, force UI to re-read X/Y
            foreach (var point in Points)
            {
                point.RefreshPosition();
            }
        }

        private void CalculateExtents()
        {
            if (Points.Count == 0 && Lines.Count == 0) return;

            var pointXs = Points.Select(p => p.RawX);
            var pointYs = Points.Select(p => p.RawY);

            var lineXs = Lines.SelectMany(l => new[] { (double)l.RawStart.X, (double)l.RawEnd.X });
            var lineYs = Lines.SelectMany(l => new[] { (double)l.RawStart.Y, (double)l.RawEnd.Y });

            var allX = pointXs.Concat(lineXs).ToList();
            var allY = pointYs.Concat(lineYs).ToList();

            if (!allX.Any() || !allY.Any()) return;

            double minX = allX.Min();
            double maxX = allX.Max();
            double minY = allY.Min();
            double maxY = allY.Max();

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
            foreach (var l in Lines) l.SetTransform(_scaleX, _scaleY, _offsetX, _offsetY);
        }

        private void OnPointMoved()
        {
            _onGeometryChanged?.Invoke();
        }
    }

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

        // NEW: Expose Symbol and Summary for ToolTips
        public string Symbol => _model.Symbol;
        public string Summary => _model.Summary;

        // Use PositionX / PositionY properties
        public double RawX => _model.PositionX;
        public double RawY => _model.PositionY;

        public double X
        {
            get => RawX * _scaleX + _offsetX;
            set
            {
                double newRawX = (value - _offsetX) / _scaleX;
                // Check and set PositionX
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
                // Check and set PositionY
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
}
