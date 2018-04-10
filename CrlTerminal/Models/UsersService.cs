using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            return UsersList.First(el => el.Phone == phone);
        }

        public bool AnyUser(string phone)
        {
            return UsersList.Any(el => el.Phone == phone);
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
