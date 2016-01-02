using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AxiosInvestments.Model.PropertyModel;

namespace AxiosInvestments.ViewModel
{
    public class InvestmentPropertyTab
    {
        private IList<InvestmentProperty> mNewProperties;
        private MainWindow mMainWindow;
        private MainViewModel mMainViewModel;



        public InvestmentPropertyTab(IList<InvestmentProperty> newProperties, MainWindow mainWindow, MainViewModel mainViewModel)
        {
            mNewProperties = newProperties;
           mMainWindow = mainWindow;
            mMainViewModel = mainViewModel;

            CreateTabButtonGroupProperty();
            CreateTabButtonGroupOwner();
            
        }

        /// <summary>
        /// At this point we know that this will be our first radio button group.
        /// </summary>
        /// <param name="newProperties"></param>
        private void CreateTabButtonGroupProperty()
        {
            foreach (var investmentProperty in mNewProperties)
            {               
                RadioButton newRadio = new RadioButton();
                newRadio.Name = investmentProperty.Name.Replace(" ","_") + "Radio";
                newRadio.GroupName = "MainGroupProperty";
                newRadio.HorizontalAlignment = HorizontalAlignment.Left;
                newRadio.VerticalAlignment = VerticalAlignment.Top;
                newRadio.Margin = new Thickness(20,20,20,20);
                newRadio.Content = investmentProperty.Name;
                newRadio.Checked += UpdateCurrentProperty;

                mMainWindow.InvestmentRadioButtonGroup.Children.Add(newRadio);
            }
        }

        /// <summary>
        /// At this point we know that this will be our first radio button group.
        /// </summary>
        /// <param name="newProperties"></param>
        private void CreateTabButtonGroupOwner()
        {
            foreach (var investmentProperty in mNewProperties)
            {
                foreach (var investor in investmentProperty.Investors)
                {
                    RadioButton newRadio = new RadioButton();
                    newRadio.Name = investor.Name.Replace(" ", "_") + "Radio";
                    newRadio.GroupName = "MainGroupOwner";
                    newRadio.HorizontalAlignment = HorizontalAlignment.Left;
                    newRadio.VerticalAlignment = VerticalAlignment.Top;
                    newRadio.Margin = new Thickness(20, 20, 20, 20);
                    newRadio.Checked += UpdateCurrentInvestor;
                    newRadio.Content = investor.Name;
                    newRadio.HorizontalAlignment = HorizontalAlignment.Left;
                    
                    mMainWindow.OwnerRadioButtonGroup.Children.Add(newRadio);
                }
            }
        }

        private void UpdateCurrentProperty(object sender, EventArgs e)
        {
            RadioButton buttonClicked = sender as RadioButton;
            mMainWindow.CurrentPropertySelected.Text = "Current Property: " + buttonClicked.Name.Replace("_", " ").Replace("Radio","");
            mMainViewModel.AnalysisReportTab.UpdateGraph();
            mMainViewModel.AnalysisReportTab.UpdateInvestmentAmount();
        }

        private void UpdateCurrentInvestor(object sender, EventArgs e)
        {
            RadioButton buttonClicked = sender as RadioButton;
            mMainWindow.CurrentOwnerSelected.Text = "Current Owner: " + buttonClicked.Name.Replace("_", " ").Replace("Radio", "");
            mMainViewModel.AnalysisReportTab.UpdateGraph();
            mMainViewModel.AnalysisReportTab.UpdateInvestmentAmount();
        }

        public void Update()
        {

        }
    }
}
