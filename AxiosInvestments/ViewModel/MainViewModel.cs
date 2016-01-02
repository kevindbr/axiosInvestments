using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using AxiosInvestments.Model.PropertyModel;
using OxyPlot;


namespace AxiosInvestments.ViewModel
{
    public class MainViewModel 
    {
        private InvestmentPropertyTab mInvestmentPropertyTab;
        private SingleOwnerTab mSingleOwnerTab;
        private MainWindow mMyWindow;

        public MainViewModel(MainWindow window, IList<InvestmentProperty> investmentProperties)
        {
            mMyWindow = window;
            mInvestmentPropertyTab = new InvestmentPropertyTab(investmentProperties, mMyWindow, this);
            PropertyDetailsTab = new PropertyDetailsTab(investmentProperties,mMyWindow);
            AnalysisReportTab = new AnalysisReportTab(investmentProperties,mMyWindow);
            mSingleOwnerTab = new SingleOwnerTab(investmentProperties,mMyWindow);
        }

        public MainViewModel()
        {
          
        }


        public InvestmentPropertyTab InvestmentPropertyTab { get; set; }
        public PropertyDetailsTab PropertyDetailsTab { get; set; }
        public AnalysisReportTab AnalysisReportTab { get; set; }
        public SingleOwnerTab SingleOwnerTab { get; set; }

        internal void UpdateTabs()
        {
            //InvestmentPropertyTab.Update();
            //AnalysisReportTab.Update();
            //PropertyDetailsTab.Update();
            //SingleOwnerTab.Update();
        }
    }
}