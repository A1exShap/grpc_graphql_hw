using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using TechTalk.SpecFlow;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.IntegrationTests.Bdd.Steps
{
    public class CancelPartnerLimitStepDefinitions
    {
        [Binding]
        public class StepDefinitions
        {
            private readonly ScenarioContext _scenarioContext;
            private Guid _partnerId;
            private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();
            private PartnerResponse _partner;

            public StepDefinitions(ScenarioContext scenarioContext)
            {
                _scenarioContext = scenarioContext;
            }
            [Given(@"the partnerId is '(.*)'")]
            public void GivenThePartnerIdIs(string p0)
            {
                _partnerId = Guid.Parse(p0);
            }
        
            [Given(@"the partner is existed")]
            public async Task GivenThePartnerIsExisted()
            {
                var client = _factory.CreateClient();
                var partnerResult = await client.GetAsync($"/api/v1/partners/{_partnerId}");
                partnerResult.IsSuccessStatusCode.Should().BeTrue();
                
                var partner = JsonConvert.DeserializeObject<PartnerResponse>(await partnerResult.Content.ReadAsStringAsync());
                partner.Should().NotBeNull();

                _partner = partner;
            }
            
            [Given(@"the partner is active")]
            public void GivenThePartnerIsActive()
            {
                _partner.IsActive.Should().BeTrue();
            }
        
            [Given(@"the partner has active limit")]
            public void GivenThePartnerHasActiveLimit()
            {
                var hasActive = _partner.PartnerLimits.Any(x => x.CancelDate == null);
                hasActive.Should().BeTrue();
            }

            [When(@"Partner manager cancel limit")]
            public async Task WhenPartnerManagerCancelLimit()
            {
                var cancelPartnerResult = await _factory.CreateClient()
                    .PostAsync($"/api/v1/partners/{_partnerId}/canceledLimits", null);

                cancelPartnerResult.IsSuccessStatusCode.Should().BeTrue();
            }
        
            [Then(@"the limit should have CancelDate as Now")]
            public async Task ThenTheLimitShouldHaveCancelDateAsNow()
            {
                var partnerResult = await _factory.CreateClient()
                    .GetAsync($"/api/v1/partners/{_partnerId}");
                partnerResult.IsSuccessStatusCode.Should().BeTrue();
                
                var partner = JsonConvert.DeserializeObject<PartnerResponse>(await partnerResult.Content.ReadAsStringAsync());

                var now = partner.PartnerLimits.FirstOrDefault(
                    x => 
                        DateTime.ParseExact(x.CancelDate, "dd.MM.yyyy hh:mm:ss",  CultureInfo.InvariantCulture).Date 
                         == DateTime.Now.Date);

                now.Should().NotBeNull();
            }
        }
    }
}