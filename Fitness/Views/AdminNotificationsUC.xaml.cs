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
using Fitness.Models;
using Fitness.ViewModels;

namespace Fitness.Views
{
    public partial class AdminNotificationsUC : UserControl
    {
        User _user;
        public AdminNotificationsUC(User user)
        {
            InitializeComponent();
            _user = user;
            DataContext = new AdminNotificationsVM(_user);   
        }
    }
}
