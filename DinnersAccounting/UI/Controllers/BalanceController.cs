using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.Dinners.Domain;
using DA.Dinners.Domain.Abstract;

namespace UI.Controllers
{
    [Authorize]
    public class BalanceController : Controller
    {
        private readonly IPersonRepository personRepository;

        public BalanceController(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public ActionResult Index()
        {
            Person person = personRepository.All.SingleOrDefault( p => p.DomainName == User.Identity.Name);
            
            return View(person);
        }

        public ActionResult AdminOperations()
        {
            return View(personRepository.All);
        }
    }
}
