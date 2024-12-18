using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using Fitness.Models;
using Fitness.ViewModels;
using LiveCharts;

namespace Fitness.Views
{
    public partial class HomeUC : UserControl
    {
        public ChartValues<double> ChartValues { get; set; }
        public List<string> Labels { get; set; }
        public HomeUC()
        {
            ChartValues = new ChartValues<double> { 3, 5, 7, 4, 6 };
            Labels = new List<string> { "Jan", "Feb", "Mar", "Apr", "May" };
            InitializeComponent();
            this.DataContext = new HomeVM();
        }
    }
}
