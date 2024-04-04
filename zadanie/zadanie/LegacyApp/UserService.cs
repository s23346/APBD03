using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly ClientRepository clientRepository;
        private readonly UserCreditService userCreditService;

        public UserService()
        {
            this.clientRepository = new ClientRepository();
            this.userCreditService = new UserCreditService();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsValidName(firstName, lastName) || !IsValidEmail(email) || !IsOver21(dateOfBirth))
                return false;

            var client = clientRepository.GetById(clientId);

            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            UserDataAccess.AddUser(user);

            if (ShouldApplyCreditLimit(client))
                SetUserCredit(client, user);

            return true;
        }

        private bool ShouldApplyCreditLimit(Client client)
        {
            if (client.Type == "ImportantClient")
                return true;

            return false;
        }

        private bool IsValidEmail(string email)
        {
            if (email.Contains("@") && email.Contains("."))
                return true;

            return false;
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            var user = new User
            {
                LastName = lastName,
                EmailAddress = email,
                FirstName = firstName,
                Client = client,
                DateOfBirth = dateOfBirth
            };

            return user;
        }

        private void SetUserCredit(Client client, User user)
        {
            if (client.Type == "ImportantClient")
            {
                var creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit *= 2;
                user.CreditLimit = creditLimit;
            }
        }

        private bool IsOver21(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            return age >= 21;
        }

        private bool IsValidName(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                return true;

            return false;
        }
    }
}
