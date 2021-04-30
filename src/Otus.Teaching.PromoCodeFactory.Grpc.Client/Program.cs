using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Otus.Teaching.PromoCodeFactory.Grpc.Server;

namespace Otus.Teaching.PromoCodeFactory.Grpc.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Customer.CustomerClient(channel);
            await GetAndPrintCustomers(client);

            var customerId = client.AddCustomer(new AddCustomerRequest
            {
                FirstName = "Тест",
                LastName = "Тестов",
                Email = "test@test.tt"
            }).Id;
            Console.WriteLine($"id добавленного пользователя: {customerId}");
            await GetAndPrintCustomers(client);

            var editResponse = await client.EditCustomerAsync(new EditCustomerRequest
            {
                Id = customerId,
                Email = "ivan@ivan.ai",
                FirstName = "Иван",
                LastName = "Иванов",
                PreferenceIds = {new List<string> {"ef7f299f-92d7-459f-896e-078ed53ef99c"}}
            });
            Console.WriteLine(editResponse.Success
                ? "Данные успешно обновлены."
                : $"При обновлении данных возникла ошибка: {editResponse.Message}");
            await GetAndPrintCustomers(client);

            var deleteResponse = await client.DeleteCustomerAsync(new DeleteCustomerRequest {Id = customerId});
            Console.WriteLine(deleteResponse.Success
                ? "Пользователь успешно удален."
                : $"При удалении пользователя возникла ошибка: {deleteResponse.Message}");
            await GetAndPrintCustomers(client);
        }

        private static async Task GetAndPrintCustomers(Customer.CustomerClient client)
        {
            var response = await client.GetCustomersAsync(new GetCustomersRequest());
            Console.WriteLine("\nПользователи: ");
            foreach (var customer in response.Customers)
                Console.WriteLine($"'{customer.Id}' - {customer.LastName} {customer.FirstName}, {customer.Email}");
        }
    }
}