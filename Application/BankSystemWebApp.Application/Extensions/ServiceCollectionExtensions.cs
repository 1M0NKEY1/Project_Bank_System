using BankSystemWebApp.Application.Accounts;
using BankSystemWebApp.Application.Banks;
using BankSystemWebApp.Application.CenterBank;
using BankSystemWebApp.Application.Contracts.Services.Accounts;
using Contracts.Banks;
using Contracts.CenterBank;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystemWebApp.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IAccountService, AccountService>();
        collection.AddScoped<IBankService, BankService>();
        collection.AddScoped<ICentreBankService, CentreBankService>();



        return collection;
    }
}