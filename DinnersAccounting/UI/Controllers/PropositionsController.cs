using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.Dinners.Domain.Concrete;
using DA.Dinners.Model;
using UI.Models;
using DA.Dinners.Domain.Abstract;

namespace UI.Controllers
{
    public class PropositionsController : Controller
    {
        //
        // GET: /Proposition/

        //private readonly IPropositionRepository propositionRepository;
        private readonly IContinuousPropositionRepository continuousPropositionRepository;


        public PropositionsController(IPropositionRepository propositionRepository, IContinuousPropositionRepository continuousPropositionRepository)
        {
            //this.propositionRepository = propositionRepository;
            this.continuousPropositionRepository = continuousPropositionRepository;
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }
    }
}
