using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AxiosInvestments.Annotations;
using AxiosInvestments.Model.PersonModel;
using AxiosInvestments.Model.PropertyModel;
using OxyPlot;
using OxyPlot.Wpf;

namespace AxiosInvestments.ViewModel
{
    public class AnalysisReportTab : INotifyPropertyChanged
    {
        private IList<InvestmentProperty> mNewProperties;
        private MainWindow mMainWindow;
        private PlotModel plotModel;

        public AnalysisReportTab(IList<InvestmentProperty> newProperties, MainWindow mainWindow)
        {
            mNewProperties = newProperties;
            mMainWindow = mainWindow;
            CreateNewPlotModel((bool)mMainWindow.ProfitChk.IsChecked, (bool)mMainWindow.NonTanChk.IsChecked, false, (bool)mMainWindow.RentAppliedChk.IsChecked, (bool)mMainWindow.AllProfitChk.IsChecked);
            CreateNewPlotModelSO((bool)mMainWindow.ProfitChk.IsChecked, (bool)mMainWindow.NonTanChk.IsChecked, false, (bool)mMainWindow.RentAppliedChk.IsChecked, (bool)mMainWindow.AllProfitChk.IsChecked);
        }

        public PlotModel PlotModel
        {
            get
            {
                return plotModel;
            }
            set
            {
                plotModel = value;
                OnPropertyChanged("PlotModel");
            }
        }

        public void CreateNewPlotModelSO(Boolean profitChk, Boolean nonTanprofitChk, Boolean equityChk, Boolean rentChk, Boolean allProfitChk)
        {
            mMainWindow.PAGraph.IsLegendVisible = true;
            mMainWindow.PAGraphLeft.Maximum = GetMaximumY();

            IList<DataPoint> oneYearProfit = new List<DataPoint>();
            IList<DataPoint> oneYearCashOut = new List<DataPoint>();
            IList<DataPoint> oneYearEquity = new List<DataPoint>();
            IList<DataPoint> allProfit = new List<DataPoint>();

            if (profitChk)
                oneYearProfit = CreateDefaultProfit(252, rentChk, true);

            //if(equityChk)
            //    oneYearCashOut = CreateDefaultCashOut(252, rentChk);

            //if (equityChk)
            //    oneYearEquity = CreateDefaultEquity(252, rentChk);

            //if (equityChk)
            //    oneYearEquity = CreateDefaultEquity(252, rentChk);

            if (allProfitChk)
                allProfit = CreateAllProfit(252, rentChk, true);

            mMainWindow.SOProfitGraph.ItemsSource = oneYearProfit;
            mMainWindow.SOProfitGraph.Title = "Profit";
            mMainWindow.SOProfitGraph.LabelFormatString = "$" + "{1}";

            mMainWindow.SOAllProfit.ItemsSource = allProfit;
            mMainWindow.SOAllProfit.Title = "All Profit";
            mMainWindow.SOAllProfit.LabelFormatString = "$" + "{1}";

            //mMainWindow.CashOutLine.ItemsSource = oneYearCashOut;
            //mMainWindow.CashOutLine.Title = "Cash Out";
            //mMainWindow.CashOutLine.LabelFormatString = "$"+"{1}";

            //mMainWindow.EquityGraph.ItemsSource = oneYearEquity;
            //mMainWindow.EquityGraph.Title = "Equity";
            //        mMainWindow.EquityGraph.LabelFormatString = "$" + "{1}";
        }

        public void CreateNewPlotModel(Boolean profitChk, Boolean nonTanprofitChk, Boolean equityChk, Boolean rentChk, Boolean allProfitChk )
        {
            mMainWindow.PAGraph.IsLegendVisible = true;
            mMainWindow.PAGraphLeft.Maximum = GetMaximumY();

            IList<DataPoint> oneYearProfit = new List<DataPoint>();
            IList<DataPoint> oneYearCashOut = new List<DataPoint>();
            IList<DataPoint> oneYearEquity = new List<DataPoint>();
            IList<DataPoint> allProfit = new List<DataPoint>();

            if(profitChk)
           oneYearProfit = CreateDefaultProfit(252, rentChk, false);

            //if(equityChk)
            //    oneYearCashOut = CreateDefaultCashOut(252, rentChk);

            //if (equityChk)
            //    oneYearEquity = CreateDefaultEquity(252, rentChk);

            //if (equityChk)
            //    oneYearEquity = CreateDefaultEquity(252, rentChk);

            if (allProfitChk)
                allProfit = CreateAllProfit(252, rentChk, false);

            mMainWindow.ProfitGraph.ItemsSource = oneYearProfit;
            mMainWindow.ProfitGraph.Title = "Profit";
            mMainWindow.ProfitGraph.LabelFormatString = "$" + "{1}";

            mMainWindow.AllProfit.ItemsSource = allProfit;
            mMainWindow.AllProfit.Title = "All Profit";
            mMainWindow.AllProfit.LabelFormatString = "$" + "{1}";

            //mMainWindow.CashOutLine.ItemsSource = oneYearCashOut;
            //mMainWindow.CashOutLine.Title = "Cash Out";
            //mMainWindow.CashOutLine.LabelFormatString = "$"+"{1}";

            //mMainWindow.EquityGraph.ItemsSource = oneYearEquity;
            //mMainWindow.EquityGraph.Title = "Equity";
            //        mMainWindow.EquityGraph.LabelFormatString = "$" + "{1}";
        }

        private Double GetMaximumY()
        {
            InvestmentProperty selectedProperty = InvestmentProperty.GetSelectedInvestment(mMainWindow, mNewProperties);
            return selectedProperty.Price/2;
        }

        private IList<DataPoint> CreateDefaultEquity(double years, bool rentChk)
        {
            IList<DataPoint> dPoints = new List<DataPoint>();

            String selectedPropertyName = mMainWindow.CurrentPropertySelected.Text.Replace("Current Property: ", "");
            String selectedOwnerName = mMainWindow.CurrentOwnerSelected.Text.Replace("Current Owner: ", "");
            InvestmentProperty selectedProperty = mNewProperties.First();
            Investor investor = selectedProperty.Investors.First();
            Double percentOwnership = investor.Percentages.First().Value;

            if (selectedPropertyName != "" && !selectedPropertyName.Contains("Current Property:"))
                selectedProperty = mNewProperties.First(i => i.Name.Equals(selectedPropertyName));

            if (selectedOwnerName != "" && !selectedOwnerName.Contains("Current Property:"))
            {
                investor = selectedProperty.Investors.First(i => i.Name.Equals(selectedOwnerName));
                percentOwnership = investor.Percentages.First(p => p.Key.Equals(selectedPropertyName)).Value;
            }

            Double rent = selectedProperty.Rent;
            Double MortgagePayment = selectedProperty.Mortgage.MonthlyPaymentsProjected.First();
            //Only Interest for fixed period plus equity

            MortgagePayment += selectedProperty.Mortgage.PMI / 12;
            MortgagePayment += selectedProperty.Mortgage.PropertyTax / 12;

            Double equity = percentOwnership * selectedProperty.Price;

            for (var i = 0; i < years; i++)
            {
           //     updateInvestment(selectedProperty);
                equity += (investor.AdditionalAmount/12) + percentOwnership;
             //   dPoints.Add(new DataPoint(i, profit));
            }

            return dPoints;
        }

        private IList<DataPoint> CreateAllProfit(double years, bool rentChk, bool isSingle)
        {
            IList<DataPoint> dPoints = new List<DataPoint>();
            String selectedPropertyName = mMainWindow.CurrentPropertySelected.Text.Replace("Current Property: ", "");
            String selectedOwnerName = mMainWindow.CurrentOwnerSelected.Text.Replace("Current Owner: ", "");

            InvestmentProperty selectedProperty = InvestmentProperty.GetSelectedInvestment(mMainWindow, mNewProperties);
            Double rent = selectedProperty.Rent;
            Double MonthlyPMI = selectedProperty.Mortgage.PMI / 12;
            Double MonthlyPropertyTax = selectedProperty.Mortgage.PropertyTax / 12;

                        Investor investorS = InvestmentProperty.GetSelectedInvestor(mMainWindow, selectedProperty);
            Double percentOwnershipS = investorS.Percentages.First().Value;

            if (selectedOwnerName != "" && !selectedPropertyName.Contains("Current Property:"))
                percentOwnershipS = investorS.Percentages.First(p => p.Key.Equals(selectedPropertyName)).Value;

            Double profit = 0;
            Double equityNow = selectedProperty.TotalCashInvested * percentOwnershipS;

            if (!isSingle)
                equityNow = selectedProperty.TotalCashInvested;
            Double allInvestorExtra = investorS.AdditionalAmount/12;

            if(!isSingle)
            allInvestorExtra = InvestmentProperty.GetInvestorExtraMonthly(selectedProperty);

            for (var i = 0; i < years; i++)
            {
                    foreach (Investor investor in selectedProperty.Investors)
                    {
                        if (isSingle && investor.Equals(investorS) || !isSingle)
                        {
                            Double percentOwnership = investor.Percentages.First().Value;

                            if (selectedOwnerName != "" && !selectedPropertyName.Contains("Current Property:"))
                                percentOwnership =
                                    investor.Percentages.First(p => p.Key.Equals(selectedPropertyName)).Value;

                            if ((equityNow/selectedProperty.Price) >= .20)
                            {
                                MonthlyPMI = 0;
                                //   CreatePmiPaidLine(pmiPaid, i);
                                //   pmiPaid = true;
                            }

                            Double remainingPrinciple = selectedProperty.Price - equityNow;
                            Double MortgagePayment = InvestmentProperty.GetMonthlyPayment(selectedProperty, i,
                                                                                          remainingPrinciple);
                            Double MortgageTotal = MortgagePayment + MonthlyPMI + MonthlyPropertyTax;

                            if (investor.Equals(selectedProperty.Investors.First()))
                                equityNow += MortgagePayment -
                                             (selectedProperty.Mortgage.InterestRate/100*remainingPrinciple/12);

                            if (MortgagePayment > 0)
                            {
                                if (!rentChk)
                                    profit += (rent - MortgageTotal)*percentOwnership;
                            }

                            else
                            {
                                //    CreateMortgagePaidLine(mortPaid, i);
                                profit += (rent - MortgageTotal)*percentOwnership;
                                //    mortPaid = true;
                            }

                            if (i%8 == 0 && investor.Equals(selectedProperty.Investors.First()))
                            {
                                dPoints.Add(new DataPoint(i, Math.Floor(profit)));
                                equityNow += allInvestorExtra*8;
                            }

                            if (rentChk && investor.Equals(selectedProperty.Investors.First()))
                                //Everyones rent is being added to house equity
                                equityNow += rent - MortgageTotal;
                        }
                    }
            }

            return dPoints;
        }

        private IList<DataPoint> CreateDefaultProfit(double years, bool rentChk, bool isSingle)
        {
            IList<DataPoint> dPoints = new List<DataPoint>();
            String selectedPropertyName = mMainWindow.CurrentPropertySelected.Text.Replace("Current Property: ", "");
            String selectedOwnerName = mMainWindow.CurrentOwnerSelected.Text.Replace("Current Owner: ", "");

            InvestmentProperty selectedProperty = InvestmentProperty.GetSelectedInvestment(mMainWindow, mNewProperties);
            Investor investor = InvestmentProperty.GetSelectedInvestor(mMainWindow, selectedProperty);
            Double percentOwnership = investor.Percentages.First().Value;


            if (selectedOwnerName != "" && !selectedPropertyName.Contains("Current Property:"))
                percentOwnership = investor.Percentages.First(p => p.Key.Equals(selectedPropertyName)).Value;

            Double rent = selectedProperty.Rent;
            Double MonthlyPMI = selectedProperty.Mortgage.PMI / 12;
            Double MonthlyPropertyTax = selectedProperty.Mortgage.PropertyTax / 12;

            Double profit = 0;
            Double equityNow = selectedProperty.TotalCashInvested * percentOwnership;

            if(!isSingle)
           equityNow = selectedProperty.TotalCashInvested;

            Double allInvestorExtra = investor.AdditionalAmount / 12;

            if (!isSingle)
                allInvestorExtra = InvestmentProperty.GetInvestorExtraMonthly(selectedProperty);

            Boolean mortPaid = false;
            Boolean pmiPaid = false;

            for (var i = 0; i < years; i++)
            {
                if ((equityNow/selectedProperty.Price) >= .20)
                {
                    MonthlyPMI = 0;
                    CreatePmiPaidLine(pmiPaid, i);
                    pmiPaid = true;
                }

                Double remainingPrinciple = selectedProperty.Price - equityNow;
                Double MortgagePayment = InvestmentProperty.GetMonthlyPayment(selectedProperty, i, remainingPrinciple);
                Double MortgageTotal = MortgagePayment + MonthlyPMI + MonthlyPropertyTax;
                equityNow += MortgagePayment - (selectedProperty.Mortgage.InterestRate / 100 * remainingPrinciple / 12);

                if (MortgagePayment > 0)
                {
                    if (!rentChk)
                        profit += (rent - MortgageTotal)*percentOwnership;
                }

                else
                {
                    CreateMortgagePaidLine(mortPaid, i);
                    profit += (rent - MortgageTotal)*percentOwnership;
                    mortPaid = true;
                }

                if (i%8 == 0)
                {
                    dPoints.Add(new DataPoint(i, Math.Floor(profit)));
                    equityNow += allInvestorExtra * 8;
                }

                if (rentChk)//Everyone rent is being added to house equity
                    equityNow += rent - MortgageTotal;
            }

            return dPoints;
        }

        private void CreatePmiPaidLine(bool pmiPaid, int i)
        {
            if (pmiPaid == false)
            {
                IList<DataPoint> dataPoints = new List<DataPoint>();
                mMainWindow.PmiPaidGraph.ItemsSource = dataPoints;
                dataPoints.Add(new DataPoint(i, 0));
                dataPoints.Add(new DataPoint(i, GetMaximumY()));
                mMainWindow.PmiPaidGraph.Title = "PMI Paid";
            }
        }

        private void CreateMortgagePaidLine(bool mortPaid, int i)
        {
            if (mortPaid == false)
            {
                IList<DataPoint> dataPoints = new List<DataPoint>();
                dataPoints.Add(new DataPoint(i, 0));
                dataPoints.Add(new DataPoint(i, GetMaximumY()));
                mMainWindow.MortgagePaidGraph.ItemsSource = dataPoints;
                mMainWindow.MortgagePaidGraph.Title = "Mortgage Paid";
                mMainWindow.MortgagePaidGraph.LabelFormatString = "$" + "{0}" +" Months" ;
                mMainWindow.MortgagePaidGraph.LabelMargin = 50;
            }
        }

        private IList<DataPoint> CreateDefaultCashOut(double years, bool rentChk)
        {
            IList<DataPoint> dPoints = new List<DataPoint>();
            String selectedPropertyName = mMainWindow.CurrentPropertySelected.Text.Replace("Current Property: ", "");
            String selectedOwnerName = mMainWindow.CurrentOwnerSelected.Text.Replace("Current Owner: ", "");

            InvestmentProperty selectedProperty = InvestmentProperty.GetSelectedInvestment(mMainWindow, mNewProperties);
            Investor investor = InvestmentProperty.GetSelectedInvestor(mMainWindow, selectedProperty);
            Double percentOwnership = investor.Percentages.First().Value;

            if (selectedOwnerName != "" && !selectedPropertyName.Contains("Current Property:"))
                percentOwnership = investor.Percentages.First(p => p.Key.Equals(selectedPropertyName)).Value;

            Double rent = selectedProperty.Rent;

            //Only Interest for fixed period plus equity
            Double MonthlyPMI = selectedProperty.Mortgage.PMI / 12;
            Double MonthlyPropertyTax = selectedProperty.Mortgage.PropertyTax / 12;

            Double profit = 0;
            Double equityNow = selectedProperty.TotalCashInvested;
            Double allInvestorExtra =InvestmentProperty.GetInvestorExtraMonthly(selectedProperty);

            for (var i = 0; i < years; i++)
            {
                if ((equityNow/selectedProperty.Price) >= .20)
                    MonthlyPMI = 0;
                
                Double remainingPrinciple = selectedProperty.Price - equityNow - allInvestorExtra;
                Double MortgagePayment = InvestmentProperty.GetMonthlyPayment(selectedProperty, i, remainingPrinciple);
                Double MortgageTotal = MortgagePayment + MonthlyPMI + MonthlyPropertyTax;
                equityNow += MortgagePayment - (selectedProperty.Mortgage.InterestRate/100 * remainingPrinciple / 12);
                Double investorCashOutEquity = InvestmentProperty.GetInvestorCashOutEquity(selectedProperty, investor, i);

                if (MortgageTotal >= 0)
                    profit += (rent - MortgageTotal) * percentOwnership;

                else
                    profit += rent * percentOwnership;

                Double cashOut = Math.Floor((profit + investorCashOutEquity));
                
                if(i % 12 == 0)
                dPoints.Add(new DataPoint(i, cashOut));
            }

            return dPoints;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateGraph()
        {
            CreateNewPlotModel((bool)mMainWindow.ProfitChk.IsChecked, (bool)mMainWindow.NonTanChk.IsChecked, false, (bool)mMainWindow.RentAppliedChk.IsChecked, (bool)mMainWindow.AllProfitChk.IsChecked);
        }

        public void UpdateInvestmentAmount()
        {
            String selectedPropertyName = mMainWindow.CurrentPropertySelected.Text.Replace("Current Property: ", "");
            String selectedOwnerName = mMainWindow.CurrentOwnerSelected.Text.Replace("Current Owner: ", "");

            InvestmentProperty selectedProperty = InvestmentProperty.GetSelectedInvestment(mMainWindow, mNewProperties);
            Investor investor = InvestmentProperty.GetSelectedInvestor(mMainWindow, selectedProperty);
            Double percentOwnership = investor.Percentages.First().Value;


            if (selectedOwnerName != "" && !selectedPropertyName.Contains("Current Property:"))
                percentOwnership = investor.Percentages.First(p => p.Key.Equals(selectedPropertyName)).Value;

            Double orginalAmountInvested = selectedProperty.TotalCashInvested*percentOwnership;
            mMainWindow.OrginalInvestment.Text = "Orginal Amount Investmented: $" + orginalAmountInvested;


            Double rent = selectedProperty.Rent;
            Double MonthlyPMI = selectedProperty.Mortgage.PMI/12;
            Double MonthlyPropertyTax = selectedProperty.Mortgage.PropertyTax/12;

           
            Double equityNow = selectedProperty.TotalCashInvested;

            Double remainingPrinciple = selectedProperty.Price - equityNow;
            Double MortgagePayment = InvestmentProperty.GetMonthlyPayment(selectedProperty, 0, remainingPrinciple);
            Double MortgageTotal = MortgagePayment + MonthlyPMI + MonthlyPropertyTax;


            mMainWindow.CurrentMortgage.Text = "Current Mortgage: $" + MortgageTotal;
            mMainWindow.CurrentRent.Text = "Rent: $" + selectedProperty.Rent;
            mMainWindow.PurchasePrice.Text = "Purchase Price: $" + selectedProperty.Price;

        }
    }
}
