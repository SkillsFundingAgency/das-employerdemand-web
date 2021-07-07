using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Web.AppStart;
using SFA.DAS.EmployerDemand.Web.Infrastructure;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.AppStart
{
    public class WhenAddingServicesToTheContainer
    {
        [TestCase(typeof(IApiClient))]
        [TestCase(typeof(IDemandService))]
        [TestCase(typeof(ILocationService))]
        [TestCase(typeof(ICacheStorageService))]
        [TestCase(typeof(IFatUrlBuilder))]
        [TestCase(typeof(IDataProtectorService))]
        public void Then_The_Dependencies_Are_Correctly_Resolved(Type toResolve)
        {
            var hostEnvironment = new Mock<IWebHostEnvironment>();
            var serviceCollection = new ServiceCollection();
            
            var configuration = GenerateConfiguration();
            serviceCollection.AddSingleton(hostEnvironment.Object);
            serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
            serviceCollection.AddConfigurationOptions(configuration);
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddDataProtection();
            serviceCollection.AddServiceRegistration(false);
            
            var provider = serviceCollection.BuildServiceProvider();

            var type = provider.GetService(toResolve);
            Assert.IsNotNull(type);
        }

        [Test]
        public void Then_If_DevDecrypt_Then_Resolves_Dependency()
        {
            var hostEnvironment = new Mock<IWebHostEnvironment>();
            var serviceCollection = new ServiceCollection();
            
            var configuration = GenerateConfiguration();
            serviceCollection.AddSingleton(hostEnvironment.Object);
            serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
            serviceCollection.AddConfigurationOptions(configuration);
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddServiceRegistration(true);
            
            var provider = serviceCollection.BuildServiceProvider();

            var type = provider.GetService(typeof(IDataProtectorService));
            type.GetType().Should().Be(typeof(DevDataProtectorService));
        }
        
        private static IConfigurationRoot GenerateConfiguration()
        {
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("EmployerDemandApi:BaseUrl", "http://localhost:1"),
                    new KeyValuePair<string, string>("EmployerDemandApi:Key", "test")
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}