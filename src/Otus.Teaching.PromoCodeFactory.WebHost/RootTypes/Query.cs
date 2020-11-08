using HotChocolate.Types;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.RootTypes
{
    public class Query
    {
        //private readonly IRepository<Preference> preferencesRepository;
        private readonly IRepository<Customer> customerRepository;

        public Query(/*IRepository<Preference> preferencesRepository, */IRepository<Customer> customerRepository)
        {
            //this.preferencesRepository = preferencesRepository;
            this.customerRepository = customerRepository;
        }

        //[UseFiltering]
        //public async Task<IEnumerable<Preference>> GetPreferencesAsync()
        //{
        //    return await preferencesRepository.GetAllAsync();
        //}

        [UseFiltering]
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await customerRepository.GetAllAsync();
        }
    }
}
