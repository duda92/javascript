using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.Dinners.Domain.Concrete;
using DA.Dinners.Model;
using DA.Dinners.Domain.Abstract;

namespace UI.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Edits the existing product
        /// </summary>
        /// <param name="id">The id of existing product</param>
        /// <returns></returns>
        public PartialViewResult Edit(int id = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Product product = productRepository.Find(id);
                if (product == null)
                    return null;
                return PartialView(product);
            }

            return null; 
        }

        /// <summary>
        /// Edits the existing product
        /// </summary>
        /// <param name="id">The id of existing product</param>
        /// <returns></returns>
        [HttpPost]
        public string Edit(Product model)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    if(!ModelState.IsValid)
                        return "Fail";

                    productRepository.InsertOrUpdate(model);
                    productRepository.Save();
                }
                catch
                {
                    return "Fail";
                }
                return "Success";
            }
            return null;
        }

    }
}
