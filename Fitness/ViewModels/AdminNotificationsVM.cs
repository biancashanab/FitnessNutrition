using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fitness.ViewModels
{
    public class AdminNotificationsVM : BaseViewModel
    {
        private ObservableCollection<User> _users;
        private User _user;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public ICommand DeleteUserCommand { get; }

        public AdminNotificationsVM(User user)
        {
            _user = user;
            Users = _user.GetAllUsers();

            DeleteUserCommand = new RelayCommand<User>(DeleteUser);
        }

        private void DeleteUser(User user)
        {
                _user.DeleteUser(user.Id);
                Users.Remove(user);
        }
    }
}
