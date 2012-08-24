using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dispatcher;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Microsoft.Practices.Unity;
using DA.Dinners.Domain.Concrete;
using System.Web.Http.Dependencies;
using UI.Controllers.Api;

namespace UI.App_Start
{
    class ApiContainer : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            // This example does not support child scopes, so we simply return 'this'.
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(PropositionsController))
            {
                return new PropositionsController(new ContinuousPropositionRepository(), new DayPropositionRepository());
            }
            else if (serviceType == typeof(ProductsController))
            {
                return new ProductsController( new ProductRepository(), new PropositionRepository());
            }
            else if (serviceType == typeof(OrdersController))
            {
                return new OrdersController(new OrderRepository(), new PersonRepository(), new ProductRepository());
            }
            else if (serviceType == typeof(PeopleController))
            {
                return new PeopleController(new PersonRepository());
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }
    }
}