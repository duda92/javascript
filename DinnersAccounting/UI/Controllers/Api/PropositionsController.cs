﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DA.Dinners.Domain.Concrete;
using DA.Dinners.Model;
using DA.Dinners.Domain.Abstract;
using UI.Models;
using DA.Dinners.Domain;
using System.Json;

namespace UI.Controllers.Api
{
    [Authorize]
    public class PropositionsController : ApiController
    {
        private IContinuousPropositionRepository continuousPropositionRepository;
        private IDayPropositionRepository dayPropositionRepository;

        public PropositionsController(IContinuousPropositionRepository continuousPropositionRepository, IDayPropositionRepository dayPropositionRepository)
        {
            this.continuousPropositionRepository = continuousPropositionRepository;
            this.dayPropositionRepository = dayPropositionRepository;
        }

        /// <summary>
        /// Gets the current ContinuousProposition by Date
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        public ContinuousProposition Current()
        {
            continuousPropositionRepository = new ContinuousPropositionRepository();
            continuousPropositionRepository.Save();
            ContinuousProposition cp = continuousPropositionRepository.
                    AllIncluding(prop => prop.Products, prop => prop.DayPropositions.Select(dp => dp.Products)).
                    SingleOrDefault(p => DateTime.Now.CompareTo(p.StartDate) > 0);
            if (cp == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return cp;
        }

        /// <summary>
        /// Gets ContinuousProposition by id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        public ContinuousProposition Get(int id)
        {
            ContinuousProposition cp = continuousPropositionRepository.
                    AllIncluding(prop => prop.Products, prop => prop.DayPropositions.Select(dp => dp.Products)).
                    SingleOrDefault(pp => pp.Id == id);
            if (cp == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return cp;
        }

        [AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Delete(int id)
        {
            var cp = continuousPropositionRepository.Find(id);
            if (cp == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            continuousPropositionRepository.Delete(id);
            continuousPropositionRepository.Save();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [AcceptVerbs("GET", "POST")]
        public Dictionary<int, string> GetPropositionsDates()
        {
            IQueryable<ContinuousProposition> continuousPropositions = continuousPropositionRepository.AllIncluding(prop => prop.Products, prop => prop.DayPropositions.Select(dp => dp.Products));
            Dictionary<int, string> ranges = new Dictionary<int, string>();
            foreach (var cp in continuousPropositions)
                ranges.Add(cp.Id, string.Format("{0} - {1}", cp.StartDate.ToShortDateString(), cp.EndDate.ToShortDateString()));
            return ranges;
        }

        [AcceptVerbs("GET", "POST")]
        public int Create(ContinuousProposition cp)
        {
            cp.Init();
            continuousPropositionRepository.InsertOrUpdate(cp);
            continuousPropositionRepository.Save();
            return cp.Id;
        }

        [AcceptVerbs("GET")]
        public string PropositionForOrder(DateTime date)
        {
            dynamic json_proposition = new JsonObject();

            ContinuousProposition cp = continuousPropositionRepository.AllIncluding(con_prop => con_prop.DayPropositions.Select(dp => dp.Products), con_prop => con_prop.Products).SingleOrDefault(prop => prop.EndDate >= date && prop.StartDate <= date);
            if (cp != null)
            {
                json_proposition.every_day_products = new JsonArray();
                foreach(var product in cp.Products)
                    json_proposition.every_day_products.Add(Product_(product));
                
                DayProposition dp = cp.DayPropositions.SingleOrDefault(p => p.Date.Date == date.Date);
                if (dp != null)
                {
                    json_proposition.day_products = new JsonArray();
                    foreach (var product in dp.Products)
                        json_proposition.day_products.Add(Product_(product));
                }
            }
            else
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            return json_proposition.ToString();
        }

        private dynamic Product_(Product product)
        {
            dynamic json_product = new JsonObject();
            json_product.Id = product.Id;
            json_product.isComplex = product.isComplex;
            json_product.Price = product.Price;
            json_product.Summary = product.Summary;
            json_product.Title = product.Title;
            return json_product;
        }


    }
}
