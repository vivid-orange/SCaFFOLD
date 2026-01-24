// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Scaffold.Calculations;
using Scaffold.Core.Interfaces;

namespace SCaFFOLD_Mobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 1. Create a dummy/test calculation instance
            // Replace 'ConcreteBeamCalculation' with one of your actual calculation classes
            ICalculation testCalc = new TestCalc2();

            // 2. Navigate to the CalculationPage automatically
            // We use 'false' for animation to make it feel instant on startup
            await Navigation.PushAsync(new CalculationPage(testCalc), animated: false);
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }


    }
}
