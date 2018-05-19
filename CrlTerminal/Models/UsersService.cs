using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrlTerminal.Models
{
    public interface IUsersService
    {
        Collection<User> GetUsersList();

        void UpdateUsersList();

        User GetUser(string phone);

        bool AnyUser(string phone);
    }

    public class UsersService : IUsersService
    {
        private static Collection<User> UsersList = new Collection<User>();
        private MySQLControll UsersControll = new MySQLControll();

        public User GetUser(string phone)
        {
            string pattern = "(.*)" + phone + "$";

            return UsersList.First(el => Regex.IsMatch(el.Phone, pattern));
        }

        public bool AnyUser(string phone)
        {
            string pattern = "(.*)" + phone + "$";

            return UsersList.Any(el => Regex.IsMatch(el.Phone, pattern));
        }

        public Collection<User> GetUsersList()
        {
            return UsersList;
        }

        public void UpdateUsersList()
        {
            UsersControll.UserListLoad(UsersList);
        }
    }
}
