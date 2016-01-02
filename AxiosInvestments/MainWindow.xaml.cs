using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AxiosInvestments.Model;
using AxiosInvestments.Model.PersonModel;
using AxiosInvestments.Model.PropertyModel;
using AxiosInvestments.ViewModel;
using OxyPlot;

namespace AxiosInvestments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IList<InvestmentProperty> mInvestmentProperties;
        private IList<Investor> mNewInvestors;
        private MainViewModel mMainViewModel;


        public MainWindow()
        {
            mInvestmentProperties = new List<InvestmentProperty>();
            mNewInvestors = new List<Investor>();

            Double firstHalf = 0.002917 * Math.Pow((1 + 0.002917), 360f);
            Double secondHalf = ((Math.Pow((1 + 0.002917), (360)))) - 1;
            Double priciple = 400000*firstHalf/secondHalf;

            InitializeComponent();
        }

        private void AddPropertyClicked(object sender, RoutedEventArgs e)
        {
            AddPropertyPopup.IsOpen = true;
            mNewInvestors = new List<Investor>();

        }

        private void SavePropertyClicked(object sender, RoutedEventArgs e)
        {
           InvestmentProperty newProperty = new InvestmentProperty();
            newProperty.Name = InvestmentPropertyNamePopup.Text;
            newProperty.Price = Convert.ToDouble(InvestmentPropertyPricePopup.Text);
            newProperty.Rent = Convert.ToDouble(InvestmentRentPopup.Text);
            newProperty.MaintanceCost = Convert.ToDouble(InvestmentMaintancePopup.Text);
            newProperty.Description = InvestmentDescriptionPopup.Document.ToString();
            newProperty.PayOffPeriod = Convert.ToDouble(InvestmentPayOffPopup.Text);
            newProperty.PurchaseDate = Convert.ToDateTime(InvestmentDatePopup.Text);
            newProperty.Investors = new List<Investor>();
            Double allOtherCash = mNewInvestors.Sum(i => i.TotalCashInvested);

            foreach (var newInvestor in mNewInvestors)
            {
                newInvestor.Percentages = new Dictionary<string, double>();
                newInvestor.Percentages.Add(newProperty.Name, newInvestor.TotalCashInvested / allOtherCash);
                newProperty.Investors.Add(newInvestor);
                newProperty.TotalCashInvested += newInvestor.TotalCashInvested;
            }

            InvestmentProperty.UpdateEquity(newProperty);
            Mortgage newMortgage = new Mortgage(newProperty);
            newProperty.Mortgage = newMortgage;

            InvestmentProperty.UpdatePayments(newProperty);
            mInvestmentProperties.Add(newProperty);
         
            ClearPropertyWindow();
            AddPropertyPopup.IsOpen = false;
            UpdateTabs();

        }

        private void ClearPropertyWindow()
        {
            InvestmentPropertyNamePopup.Clear();
            InvestmentPropertyPricePopup.Clear();
            InvestmentRentPopup.Clear();
            InvestmentMaintancePopup.Clear();
            InvestmentDescriptionPopup.Document = new FlowDocument();
            InvestmentPayOffPopup.Clear();
        }

        private void UpdateTabs()
        {
            if(mMainViewModel == null)
                mMainViewModel = new MainViewModel(this, mInvestmentProperties);

         //   mMainViewModel.UpdateTabs();
        }

        private void CancelPropertyClicked(object sender, RoutedEventArgs e)
        {
            AddPropertyPopup.IsOpen = false;
            ClearInvestorWindow();
        }

        private void SaveInvestorClicked(object sender, RoutedEventArgs e)
        {
            String investorName = InvestorNamePopup.Text;
            Double cashDown = Convert.ToDouble(CashDownPopup.Text);
           
            InvestorsListPopup.Text = InvestorsListPopup.Text + "  " + investorName + "(" +cashDown+ ")";

            Investor newInvestor = new Investor();
            newInvestor.Name = investorName;
            newInvestor.TotalCashInvested = cashDown;
            newInvestor.AdditionalAmount = Convert.ToDouble(AdditionalDownPopup.Text);
            mNewInvestors.Add(newInvestor);

            AddInvestorPopup.IsOpen = false;
            ClearInvestorWindow();
        }

        private void ClearInvestorWindow()
        {
            InvestorNamePopup.Clear();
           CashDownPopup.Clear();
            AdditionalDownPopup.Clear();
        }

        private void CancelInvestorClicked(object sender, RoutedEventArgs e)
        {
            AddInvestorPopup.IsOpen = false;
        }

        private void AddInvestorClicked(object sender, RoutedEventArgs e)
        {
            AddInvestorPopup.IsOpen = true;
        }

        private void CreateDefaultClicked(object sender, RoutedEventArgs e)
        {
            CreateDefaultInvestors();
            CreateDefaultInvestment();


        }

        private void CreateDefaultInvestment()
        {
            InvestmentProperty newProperty = new InvestmentProperty();
            newProperty.Name = "Glen Haven House";
            newProperty.Price = 280000;
            newProperty.Rent = 1800;
            newProperty.MaintanceCost = 60;
            newProperty.Description = "New house off Horsetooth rd and Timberline. http://www.zillow.com/homedetails/3719-Kentford-Rd-Fort-Collins-CO-80525/13871691_zpid/";
            newProperty.PayOffPeriod = 30;
            newProperty.PurchaseDate = Convert.ToDateTime("04/23/2016 14:50:50.42");
            newProperty.Investors = new List<Investor>();
            Double allOtherCash = mNewInvestors.Sum(i => i.TotalCashInvested);

            foreach (var newInvestor in mNewInvestors)
            {
                newInvestor.Percentages = new Dictionary<string, double>();
                newInvestor.Percentages.Add(newProperty.Name, newInvestor.TotalCashInvested / allOtherCash);
                newProperty.Investors.Add(newInvestor);
                newProperty.TotalCashInvested += newInvestor.TotalCashInvested;
            }

            InvestmentProperty.UpdateEquity(newProperty);
            Mortgage newMortgage = new Mortgage(newProperty);
            newProperty.Mortgage = newMortgage;
            mInvestmentProperties.Add(newProperty);
            UpdateTabs();
        }

        private void CreateDefaultInvestors()
        {
            Investor newInvestor = new Investor();
            newInvestor.Name = "Kevin Brown";
            newInvestor.TotalCashInvested = 10000;
            newInvestor.AdditionalAmount = 10000;

            Investor newInvestor2 = new Investor();
            newInvestor2.Name = "Patrick Schmidt";
            newInvestor2.TotalCashInvested = 15000;
            newInvestor2.AdditionalAmount = 10000;

            Investor newInvestor3 = new Investor();
            newInvestor3.Name = "Ben and Bri Sutton";
            newInvestor3.TotalCashInvested = 30000;
            newInvestor3.AdditionalAmount = 10000;

            Investor newInvestor4 = new Investor();
            newInvestor4.Name = "Jordan Flatt";
            newInvestor4.TotalCashInvested = 10000;
            newInvestor4.AdditionalAmount = 10000;

            mNewInvestors.Add(newInvestor);
            mNewInvestors.Add(newInvestor2);
            mNewInvestors.Add(newInvestor3);
            mNewInvestors.Add(newInvestor4);
        }

        private void RentAppliedChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (AllProfitChk != null)
                mMainViewModel.AnalysisReportTab.CreateNewPlotModel((bool)ProfitChk.IsChecked, (bool)NonTanChk.IsChecked, false, (bool)RentAppliedChk.IsChecked, (bool)AllProfitChk.IsChecked);
            
        }

        private void SORentAppliedChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (SOAllProfitChk != null)
                mMainViewModel.AnalysisReportTab.CreateNewPlotModel((bool)SOProfitChk.IsChecked, (bool)SONonTanChk.IsChecked, false, (bool)SORentAppliedChk.IsChecked, (bool)SOAllProfitChk.IsChecked);

        }
    }
}
