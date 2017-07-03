using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class UserManager
    {
        private string _connection { get; set; }
        public UserManager(string connection)
        {
            _connection = connection;
        }

        public void AddUser(string firstName, string lastName, string email, string password)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);

            using (var context = new ECommerceDataContext(_connection))
            {
                AdminUser user = new AdminUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PasswordHash = hash,
                    Salt = salt
                };
                context.AdminUsers.InsertOnSubmit(user);
                context.SubmitChanges();
            }
        }

        public AdminUser Login(string emailAddress, string password)
        {
            AdminUser user = GetUser(emailAddress);
            if (user == null)
            {
                return null;
            }

            bool isMatch = PasswordHelper.IsMatch(password, user.PasswordHash, user.Salt);
            if (isMatch)
            {
                return user;
            }

            return null;
        }

        public AdminUser GetUser(string emailAddress)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                return context.AdminUsers.FirstOrDefault(u => u.Email == emailAddress);
            }
        }
    }
}