using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Otus.Teaching.PromoCodeFactory.Integration;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.IntegrationTests.WebHost.Controllers
{
    [Collection(EfDatabaseCollection.DbCollection)]
    public class PartnersControllerTests : IClassFixture<EfDatabaseFixture>
    {
        private readonly IRepository<Partner> _partnersRepository;
        private readonly PartnersController _partnersController;

        public PartnersControllerTests(EfDatabaseFixture efDatabaseFixture)
        {
            _partnersRepository = new EfRepository<Partner>(efDatabaseFixture.DbContext);
            _partnersController = new PartnersController(_partnersRepository, new NotificationGateway());
        }
        
        
        [Fact]
        public async Task CancelPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.Parse("def47943-7aaf-44a1-ae21-05aa4948b165");

            // Act
            var result = await _partnersController.CancelPartnerPromoCodeLimitAsync(partnerId);
 
            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }
        
        [Fact]
        public async Task CancelPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.Parse("0da65561-cf56-4942-bff2-22f50cf70d43");

            // Act
            var result = await _partnersController.CancelPartnerPromoCodeLimitAsync(partnerId);
 
            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }
    }
}