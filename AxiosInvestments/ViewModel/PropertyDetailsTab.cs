using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AxiosInvestments.Annotations;
using AxiosInvestments.Model.PersonModel;
using AxiosInvestments.Model.PropertyModel;
using OxyPlot;

namespace AxiosInvestments.ViewModel
{
   public class PropertyDetailsTab : INotifyPropertyChanged
    {
        private IList<InvestmentProperty> mNewProperties;
        private MainWindow mMainWindow;
        private PlotModel mPlotModel;

        public PropertyDetailsTab(IList<InvestmentProperty> newProperties, MainWindow mainWindow)
        {
            mNewProperties = newProperties;
            mMainWindow = mainWindow;

            mMainWindow.CurrentPropertySelected.TextChanged += UpdatePropertyDetailsTabProperty;
            mMainWindow.CurrentOwnerSelected.TextChanged += UpdatePropertyDetailsTabOwner;
        }

        public PlotModel PropertyDetailTabPlotModel
        {
            get
            {
                return mPlotModel;
            }
            set
            {
                mPlotModel = value;
                OnPropertyChanged("PropertyDetailTabPlotModel");
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdatePropertyDetailsTabProperty(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            TextBox TextBox = sender as TextBox;
            String propertyName = TextBox.Text.Replace("Current Property: ","");

            if (!propertyName.Contains("Current Owner: "))//For some reason these events are being forwarded to the owner obj
            {
                InvestmentProperty propertyToUpdate = new InvestmentProperty();

                foreach (InvestmentProperty investmentProperty in mNewProperties)
                {
                    if (investmentProperty.Name == propertyName)
                        propertyToUpdate = investmentProperty;
                }

                StringBuilder formattedDetails = new StringBuilder();
                formattedDetails.AppendLine("Property Name: " + propertyToUpdate.Name);
                formattedDetails.AppendLine("Price: " + propertyToUpdate.Price.ToString());
                formattedDetails.AppendLine("Rent: " + propertyToUpdate.Rent.ToString());
                formattedDetails.AppendLine("Maintance Cost: " + propertyToUpdate.MaintanceCost.ToString());
                //  formattedDetails.AppendLine(propertyToUpdate.Mortgage.MonthlyPaymentsProjected.First().ToString());
                formattedDetails.AppendLine("Total Cash Invested: " + propertyToUpdate.TotalCashInvested.ToString());
                formattedDetails.AppendLine("Total Equity: " + propertyToUpdate.TotalEquity.ToString());
                formattedDetails.AppendLine("Property Description: " + propertyToUpdate.Description.ToString());
                formattedDetails.AppendLine("Years to Pay: " + propertyToUpdate.PayOffPeriod.ToString());

                foreach (Investor investor in propertyToUpdate.Investors)
                {
                    formattedDetails.AppendLine(investor.Name);
                }

                mMainWindow.SelectedPropertyTextBlock.Text = formattedDetails.ToString();
            }
        }

        private void UpdatePropertyDetailsTabOwner(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            TextBox TextBox = sender as TextBox;
            String propertyName = TextBox.Text.Replace("Current Owner: ", "");

        }

        public event PropertyChangedEventHandler PropertyChanged;


       public void Update()
       {
  
       }
    }
}
