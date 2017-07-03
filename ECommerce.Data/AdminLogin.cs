using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class AdminLogin
    {
        private string _connection { get; set; }
        public AdminLogin(string connection)
        {
            _connection = connection;
        }

        public void AddAdmin(AdminUser admin)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                context.AdminUsers.InsertOnSubmit(admin);
                context.SubmitChanges();
            }
        }
    }
}
