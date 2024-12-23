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
using Fitness.ViewModels;
using Fitness.Models;


namespace Fitness.Views
{
    public partial class NutritionistNotificationsUC : UserControl
    {
        User _user;
        
        public NutritionistNotificationsUC(User user)
        {
            InitializeComponent();
            _user = user;
            DataContext = new NutritionistNotificationsVM();
        }
    }
}
