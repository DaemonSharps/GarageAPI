using System;
using System.Globalization;
using System.Net.Http.Json;
using GarageAPI.Controllers.Schemas;
using DTO = GarageDataBase.DTO;
namespace GarageApiIntegration.Extentions
{
    public static partial class HttpClientExtentions
    {
        public static async Task<DTO.Customer> GetOrCreateCustomer(this HttpClient client, GetOrSetCustomerRequest request, string expectedStatus)
        {
            var result = await client.PostAsJsonAsync("/api/customers", request);
            result.EnsureSuccessStatusCode();
            var customer = await result.Content.ReadFromJsonAsync<DTO.Customer>();

            Assert.NotNull(customer);
            Assert.Equal(request.Email, customer.Email);
            Assert.Equal(request.FirstName, customer.FirstName);
            Assert.Equal(request.LastName, customer.LastName);
            Assert.Equal(request.SecondName, customer.SecondName);
            Assert.Equal(expectedStatus, customer.Status);

            return customer;
        }

        public static async Task<List<DTO.Customer>> GetCustomersByFilter(this HttpClient client, GetCustomersByFilterRequest request)
        {
            var querry = $"/api/customers?Email={request.Email}&Page={request.Page}&PerPage={request.PerPage}";
            var customers = await client.GetFromJsonAsync<List<DTO.Customer>>(querry);
            Assert.NotEmpty(customers);
            return customers;
        }
    }
}

