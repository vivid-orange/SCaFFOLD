using System;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Geometry
{
    public class InteractiveGeometryQuantityOnXY : IInteractiveGeometryItem
    {
        // Instead of holding the object, we hold the accessors
        private readonly Func<double> _quantityProviderX;
        private readonly Func<double> _quantityProviderY;
        private readonly Func<double> _offsetX;
        private readonly Func<double> _offsetY;
        private readonly Action<double> _valueUpdaterX;
        private readonly Action<double> _valueUpdaterY;

        private readonly bool _centredX = false;
        private readonly bool _centredY = false;

        // Constructor requires delegates to bridge the "Live Link"
        public InteractiveGeometryQuantityOnXY(
            Func<double> xGetter,
            Action<double> xSetter,
            Func<double> yGetter,
            Action<double> ySetter,
            bool isCentredOnX = false,
            bool isCentredOnY = false,
            Func<double> xOffset = null,
            Func<double> yOffset = null)
        {
            _quantityProviderX = xGetter ?? (() => 0.0);
            _quantityProviderY = yGetter ?? (() => 0.0);
            _offsetX = xOffset ?? (() => 0.0);
            _offsetY = yOffset ?? (() => 0.0);
            _valueUpdaterX = xSetter;
            _valueUpdaterY = ySetter;
            _centredX = isCentredOnX;
            _centredY = isCentredOnY;
        }

        public double PositionX
        {
            get
            {
                double currentQuantity = _quantityProviderX();
                double offsetX = _offsetX();

                if (_centredX)
                {
                    return offsetX + currentQuantity / 2;
                }
                return offsetX + currentQuantity;
            }
            set
            {
                if (_valueUpdaterX == null) return;
                double offsetX = _offsetX();
                if (_centredX)
                {
                    double targetValue = (value - offsetX) * 2;
                    _valueUpdaterX(targetValue);
                }
                else
                {
                    double targetValue = (value - offsetX);
                    _valueUpdaterX(targetValue);
                }
            }
        }

        public double PositionY
        {
            get
            {
                double currentQuantity = _quantityProviderY();
                double offsetY = _offsetY();

                if (_centredY)
                {
                    return offsetY + currentQuantity / 2;
                }
                return offsetY + currentQuantity;
            }
            set
            {
                if (_valueUpdaterY == null) return;
                double offsetY = _offsetY();
                if (_centredY)
                {
                    double targetValue = (value - offsetY) * 2;
                    _valueUpdaterY(targetValue);
                }
                else
                {
                    double targetValue = (value - offsetY);
                    _valueUpdaterY(targetValue);
                }
            }
        }
    }
}
