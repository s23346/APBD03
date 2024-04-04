using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp
{
    class UserServiceTests
    {
        static void AddUser_WithValidData_CallsUserDataAccessAddUser()
        {
            bool addUserCalled = false;

            Action<User> fakeUserDataAccess = (user) =>
            {
                addUserCalled = true;
            };

            var userService = new UserService();

            userService.AddUser("a", "b", "ab@gmail.com", new DateTime(2001, 1, 1), 1);

            Console.WriteLine(addUserCalled ? "Test passed" : "Test failed");
        }
    }

}