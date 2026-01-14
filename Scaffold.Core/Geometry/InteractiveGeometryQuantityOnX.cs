using System;
using Scaffold.Core.CalcValues; 

namespace Scaffold.Core.Geometry
{
    public class InteractiveGeometryQuantityOnX : IInteractiveGeometryItem
    {
        // Instead of holding the object, we hold the accessors
        private readonly Func<IQuantity> _quantityProvider;
        private readonly Action<double> _valueUpdater;

        private readonly bool _centred = false;
        private readonly double _yCoordinate = 0;
        private readonly int[] _constraints = [1, 0, 0];

        // Constructor requires delegates to bridge the "Live Link"
        public InteractiveGeometryQuantityOnX(
            Func<IQuantity> quantityProvider,
            Action<double> valueUpdater,
            double yCoordinate,
            bool centred = false)
        {
            _quantityProvider = quantityProvider;
            _valueUpdater = valueUpdater;
            _yCoordinate = yCoordinate;
            _centred = centred;
        }

        public double PositionX
        {
            get
            {
                // 1. Get the LATEST instance from the parent property
                var currentQuantity = _quantityProvider();

                if (_centred)
                {
                    return (double)currentQuantity.Value / 2;
                }
                return (double)currentQuantity.Value;
            }
            set
            {
                // 2. Calculate the new raw value
                double targetValue = _centred ? value * 2 : value;

                // 3. Invoke the setter delegate to update the parent property
                // This keeps the "Business Logic" of HOW to create the new object 
                // inside the parent class, not here.
                _valueUpdater(targetValue);
            }
        }

        public double PositionY
        {
            get => _yCoordinate;
            set { /* Implementation for Y if needed */ }
        }

        public int[] Constraints => _constraints;
    }
}
