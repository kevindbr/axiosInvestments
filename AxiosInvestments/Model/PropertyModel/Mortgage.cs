using System;
using System.Configuration;
using System.Collections.Generic;

namespace AxiosInvestments.Model.PropertyModel
{
    public class Mortgage
    {

        //Usually 1% of loan amount if under 20%
        public Double PMI { get; set; }

        //Mill levy in Fort Collins is 7.9 http://www.larimer.org/treasurer/frequent.htm
        //To calculate property tax (mill levy/1000) * House value
        public Double PropertyTax { get; set; }

        //Changes daily, see if we can hit a service
        public Double InterestRate { get; set; }

        //Term of the loan
        public IList<Double> MonthlyEquity { get; set; }

        public Double OrginalLoanAmount { get; set; }

        public Double RemainingLoanAmount { get; set; }

        public IList<Double> MonthlyPaymentsMade { get; set; }

        public IList<Double> MonthlyPaymentsProjected { get; set; }

        public DateTime StartDate { get; set; }


        public Mortgage(InvestmentProperty investmentProperty)
        {
            OrginalLoanAmount = investmentProperty.Price - (investmentProperty.Price*investmentProperty.TotalEquity);
            RemainingLoanAmount = OrginalLoanAmount;
            MonthlyPaymentsMade = new List<double>();
            StartDate = investmentProperty.PurchaseDate;
            PMI = 0;
            PropertyTax = (7.9 / 1000) * investmentProperty.Price;
            InterestRate = Convert.ToDouble(ConfigurationManager.AppSettings["InterestRate"]);

            if (investmentProperty.TotalEquity < 20)
                PMI = OrginalLoanAmount*.01;
        }
    }
}
