using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxiosInvestments.Model.PropertyModel;

namespace AxiosInvestments.Model.PersonModel
{
   public class Investor
    {
       public String Name { get; set; }
       public Double TotalMonthlySalary { get; set; }
       public Double TotalCashInvested { get; set; }
       public Double AdditionalAmount { get; set; }
       public IList<InvestmentProperty> InvestmentProperties { get; set; }
       public Dictionary<String, Double> Percentages { get; set; }
       public Dictionary<String, Double> MonthlySalaries { get; set; } 
    }
}
