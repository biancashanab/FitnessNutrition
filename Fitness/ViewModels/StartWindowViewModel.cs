using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fitness.Models;

namespace Fitness.ViewModels
{
    public class StartWindowViewModel : BaseViewModel
    {
        public StartWindowViewModel() { }

        public ICommand start { get;  }
    }

}
