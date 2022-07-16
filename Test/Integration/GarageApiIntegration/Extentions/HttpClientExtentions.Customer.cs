using System.Net.Http.Json;
using GarageAPI.Controllers.Schemas;
using DeepEqual.Syntax;
using DTO = GarageDataBase.DTO;
namespace GarageApiIntegration.Extentions;

public static partial class HttpClientExtentions
{
    public static async Task<DTO.Customer> GetOrCreateCustomer(this HttpClient client, GetOrSetCustomerRequest request, string expectedStatus)
    {
        var result = await client.PostAsJsonAsync("/api/customers", request);
        result.EnsureSuccessStatusCode();
        var customer = await result.Content.ReadFromJsonAsync<DTO.Customer>();

        Assert.NotNull(customer);
        customer.WithDeepEqual(request)
            .IgnoreSourceProperty(s => s.Id)
            .IgnoreSourceProperty(s => s.Status)
            .IgnoreDestinationProperty(d => d.CustomerStateId)
            .Assert();
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

