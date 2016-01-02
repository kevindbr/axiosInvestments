using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxiosInvestments.Model.PropertyModel;

namespace AxiosInvestments.ViewModel
{
    public class SingleOwnerTab
    {
        private IList<InvestmentProperty> newProperties;

        public SingleOwnerTab(IList<InvestmentProperty> newProperties, MainWindow mainWindow)
        {
            // TODO: Complete member initialization
            this.newProperties = newProperties;
        }

        public void Update()
        {

        }
    }
}
