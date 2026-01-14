using System;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Geometry
{
    public class InteractiveGeometryQuantityOnY : IInteractiveGeometryItem
    {
        // Instead of holding the object, we hold the accessors
        private readonly Func<IQuantity> _quantityProviderY;
        private readonly double _offsetY = 0;
        private readonly Action<double> _valueUpdater;
        private readonly Func<IQuantity> _quantityProviderX;
        private bool _isMovingX = false;

        private readonly bool _centred = false;
        private readonly double _xCoordinate = 0;
        private readonly int[] _constraints = [0, 1, 0];

        // Constructor requires delegates to bridge the "Live Link"
        public InteractiveGeometryQuantityOnY(
            Func<IQuantity> quantityProvider,
            Action<double> valueUpdater,
            double xCoordinate,
            bool centred = false,
            double offsetY = 0)
        {
            _quantityProviderY = quantityProvider;
            _valueUpdater = valueUpdater;
            _xCoordinate = xCoordinate;
            _centred = centred;
            _offsetY = offsetY;
        }

        public InteractiveGeometryQuantityOnY(
            Func<IQuantity> quantityProviderX,
    Func<IQuantity> quantityProviderY,
    Action<double> valueUpdater,
    double xCoordinate,
    bool centred = false,
    double offsetY = 0)
        {
            _quantityProviderY = quantityProviderY;
            _quantityProviderX = quantityProviderX;
            _valueUpdater = valueUpdater;
            _xCoordinate = xCoordinate;
            _centred = centred;
            _isMovingX = true;
            _offsetY = offsetY;
        }

        public double PositionX
        {
            get
            {
                // 1. Get the LATEST instance from the parent property
                if (!_isMovingX) return _xCoordinate;
                else
                {
                    var currentQuantity = _quantityProviderX();

                    if (_centred)
                    {
                        return (double)currentQuantity.Value / 2;
                    }
                    return (double)currentQuantity.Value;
                }
            }
            set
            {

            }
        }

        public double PositionY
        {
            get
            {
                // 1. Get the LATEST instance from the parent property
                var currentQuantity = _quantityProviderY();

                if (_centred)
                {
                    return _offsetY + (double)currentQuantity.Value / 2;
                }
                return _offsetY + (double)currentQuantity.Value;
            }
            set

            {
                // 2. Calculate the new raw value
                double targetValue = _centred ? (value - _offsetY) * 2 : (value - _offsetY);

                // 3. Invoke the setter delegate to update the parent property
                // This keeps the "Business Logic" of HOW to create the new object 
                // inside the parent class, not here.
                _valueUpdater(targetValue);
            }
        }

        public int[] Constraints => _constraints;
    }
}
