using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxiosInvestments.Model;
using AxiosInvestments.Model.PersonModel;

namespace AxiosInvestments.Model.PropertyModel
{
    public class InvestmentProperty
    {
        public String Name { get; set; }
        public Double Price { get; set; }
        public Double Rent { get; set; }
        public Double TotalCashInvested { get; set; }
        public Double TotalEquity { get; set; }
        public Double MaintanceCost { get; set; }
        public String Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public IList<Investor> Investors { get; set; }
        public Mortgage Mortgage { get; set; }
        public Double PayOffPeriod { get; set; }

        public static void UpdateEquity(InvestmentProperty newProperty)
        {
            newProperty.TotalEquity = newProperty.TotalCashInvested/newProperty.Price;
        }

        /// <summary>
        /// Updates the projected payoff period from within mortgage object, based on user selected pay off years 
        /// Formula = M = P [ interest(1 + i)^n ] / [ (1 + i)^n – 1]
        /// 
        /* M = monthly mortgage payment
             P = the principal, or the initial amount you borrowed.
             interest = your monthly interest rate. Your lender likely lists interest rates as an annual figure, so you’ll need to divide by 12, for each month of the year. So, if your rate is 5%, then the monthly rate will look like this: 0.05/12 = 0.004167.
             n = the number of payments, or the payment period in months. If you take out a 30-year fixed rate mortgage, this means: n = 30 years x 12 months per year, or 360 payments.*/
        /// </summary>
        /// <param name="property"></param>
        public static void UpdatePayments(InvestmentProperty property)
        {
            property.Mortgage.MonthlyPaymentsMade = new List<double>();
            property.Mortgage.MonthlyPaymentsProjected = new List<double>();

            Double yearsToPayOff = property.PayOffPeriod;

            Double p = property.Price - property.TotalCashInvested;
            Double interest = property.Mortgage.InterestRate/100;
            Double n = yearsToPayOff*12;
            Double monthlyPayment = p* (interest * Math.Pow(1+interest,(n)) ) / (Math.Pow((1+interest), (n-1)));

            for (var i = 0; i < yearsToPayOff; i++)
                for (var m = 0; m < 12; m++)
                    property.Mortgage.MonthlyPaymentsProjected.Add(monthlyPayment);
        }

        /// <summary>
        /// Just want to calculate what this montly payment should be based on the current amount and interest rate
        /// Must apply extra amount each investor may or may not contribute
        /// Must adjust years to pay off
        /// Must take into account equity applied up until this point.  Can't save inside selected property object because of other graphs
        /// 
        /// </summary>
        /// <param name="selectedProperty"></param>
        /// <returns></returns>
        public static Double GetMonthlyPayment(InvestmentProperty selectedProperty, Int32 monthNum, Double remainingPrinciple)
        {
            Double yearsToPayOff = selectedProperty.PayOffPeriod;
            Double interest = selectedProperty.Mortgage.InterestRate / 100/12;
            Double n = (yearsToPayOff * 12) - monthNum;

            Double monthlyPayment = (remainingPrinciple * (interest * Math.Pow((1 + interest), (n)))) / (Math.Pow((1 + interest), (n )) - 1);

            return monthlyPayment;
        }

        public static Double GetInvestorExtraMonthly(InvestmentProperty selectedProperty)
        {
            return selectedProperty.Investors.Sum(investor => investor.AdditionalAmount/12);
        }

        public static double GetInvestorCashOutEquity(InvestmentProperty selectedProperty, Investor investor, Double monthNum)
        {
            Double investorEquity = investor.TotalCashInvested + (monthNum*investor.AdditionalAmount/12);

            return investorEquity;
        }

        public static InvestmentProperty GetSelectedInvestment(MainWindow mainWindow, IList<InvestmentProperty> properties)
        {
            String selectedPropertyName = mainWindow.CurrentPropertySelected.Text.Replace("Current Property: ", "");
            InvestmentProperty selectedProperty = properties.First();

            if (selectedPropertyName != "" && !selectedPropertyName.Contains("Current Property:"))
                selectedProperty = properties.First(i => i.Name.Equals(selectedPropertyName));

            return selectedProperty;
        }

        public static Investor GetSelectedInvestor(MainWindow mainWindow, InvestmentProperty property)
        {
            String selectedOwnerName = mainWindow.CurrentOwnerSelected.Text.Replace("Current Owner: ", "");
            Investor investor = property.Investors.First();

            if (selectedOwnerName != "" && !selectedOwnerName.Contains("Current Owner:"))
                investor = property.Investors.First(i => i.Name.Equals(selectedOwnerName));

            return investor;
        }
    }

}
