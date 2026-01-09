using Scaffold.Core;
using Scaffold.Core.Interfaces;
using Scaffold.Core.CalcValues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace SCaFFOLD_Desktop
{
    public class CalculationViewModel : ViewModelBase
    {
        private ICalculation _currentCalculation;
        private readonly Stack<ICalculation> _navigationStack = new Stack<ICalculation>();

        public ObservableCollection<ICalculation> Breadcrumbs { get; } = [];
        public ObservableCollection<CalcValueViewModel> Inputs { get; } = [];
        public ObservableCollection<CalcValueViewModel> Outputs { get; } = [];
        public ObservableCollection<OutputItemViewModel> CalculationDetails { get; } = [];

        public string CurrentTitle => _currentCalculation?.DisplayName;
        public ICommand NavigateUpCommand { get; }

        public CalculationViewModel(ICalculation rootCalculation)
        {
            NavigateUpCommand = new RelayCommand(NavigateBack);
            NavigateTo(rootCalculation);
        }

        private void NavigateTo(ICalculation calculation)
        {
            if (_currentCalculation != null && _currentCalculation != calculation)
            {
                _navigationStack.Push(_currentCalculation);
            }

            _currentCalculation = calculation;
            UpdateBreadcrumbs();
            RefreshData();
        }

        private void NavigateBack(object targetCalculation)
        {
            var target = targetCalculation as ICalculation;
            if (target == null) return;
            if (_currentCalculation == target) return;

            if (_navigationStack.Contains(target))
            {
                while (_navigationStack.Count > 0 && _navigationStack.Peek() != target)
                {
                    _navigationStack.Pop();
                }
                if (_navigationStack.Count > 0)
                {
                    _currentCalculation = _navigationStack.Pop();
                }
            }
            else if (_navigationStack.Count > 0)
            {
                _currentCalculation = _navigationStack.Pop();
            }

            UpdateBreadcrumbs();
            RefreshData();
        }

        private void UpdateBreadcrumbs()
        {
            Breadcrumbs.Clear();
            foreach (var item in _navigationStack.Reverse())
            {
                Breadcrumbs.Add(item);
            }
            if (_currentCalculation != null)
            {
                Breadcrumbs.Add(_currentCalculation);
            }
            OnPropertyChanged(nameof(CurrentTitle));
        }

        private void RefreshData()
        {
            Inputs.Clear();
            Outputs.Clear();
            CalculationDetails.Clear();

            if (_currentCalculation == null) return;

            foreach (var input in _currentCalculation.GetInputs())
            {
                Type declaredType = GetDeclaredTypeForInput(_currentCalculation, input);
                Inputs.Add(new CalcValueViewModel(
                    input,
                    OnCalculationUpdate,
                    (complex) => NavigateTo(complex),
                    declaredType,
                    ReplaceInput
                ));
            }

            foreach (var output in _currentCalculation.GetOutputs())
            {
                Outputs.Add(new CalcValueViewModel(output, null, null));
            }

            RebuildCalculationDetails();
        }

        private void OnCalculationUpdate()
        {
            // 1. Run the calculation (Renamed from Update per requirements)
            _currentCalculation.Calculate();

            // 2. Refresh Inputs 
            // We assume input instances are stable (unless explicitly replaced), 
            // so we just refresh the text/units in case formatting changed.
            foreach (var input in Inputs) input.Refresh();

            // 3. Regenerate Outputs
            // Since output objects can be completely new instances, we must clear 
            // the old wrappers and fetch the fresh list from the calculation.
            Outputs.Clear();
            foreach (var output in _currentCalculation.GetOutputs())
            {
                Outputs.Add(new CalcValueViewModel(output, null, null));
            }

            // 4. Rebuild Details
            RebuildCalculationDetails();
        }

        private void RebuildCalculationDetails()
        {
            CalculationDetails.Clear();
            var newItems = _currentCalculation.GetFormulae();
            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    CalculationDetails.Add(new OutputItemViewModel(item));
                }
            }
        }

        private Type GetDeclaredTypeForInput(ICalculation calculation, ICalcValue instance)
        {
            if (calculation == null || instance == null) return null;
            var props = calculation.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.CanRead)
                {
                    try
                    {
                        var value = prop.GetValue(calculation);
                        if (ReferenceEquals(value, instance)) return prop.PropertyType;
                    }
                    catch { }
                }
            }
            return null;
        }

        private void ReplaceInput(ICalcValue oldInput, ICalcValue newInput)
        {
            if (_currentCalculation == null) return;

            bool propertyUpdated = false;
            var props = _currentCalculation.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.CanRead)
                {
                    try
                    {
                        var value = prop.GetValue(_currentCalculation);
                        if (ReferenceEquals(value, oldInput))
                        {
                            if (prop.CanWrite)
                            {
                                prop.SetValue(_currentCalculation, newInput);
                                propertyUpdated = true;
                            }
                            else if (prop.GetSetMethod(true) != null)
                            {
                                prop.SetValue(_currentCalculation, newInput);
                                propertyUpdated = true;
                            }

                            if (propertyUpdated) break;
                        }
                    }
                    catch { }
                }
            }

            if (!propertyUpdated) return;

            var existingVM = Inputs.FirstOrDefault(vm => ReferenceEquals(vm.Model, oldInput));
            if (existingVM != null)
            {
                int index = Inputs.IndexOf(existingVM);
                if (index >= 0)
                {
                    var newVM = new CalcValueViewModel(
                        newInput,
                        OnCalculationUpdate,
                        (complex) => NavigateTo(complex),
                        existingVM.DeclaredType,
                        ReplaceInput
                    );

                    Inputs[index] = newVM;
                }
            }
            else
            {
                RefreshData();
            }

            OnCalculationUpdate();
        }
    }
}
