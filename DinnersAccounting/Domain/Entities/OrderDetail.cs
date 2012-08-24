using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DA.Dinners.Model;
using System.ComponentModel.DataAnnotations;

namespace DA.Dinners.Domain
{
    /// <summary>
    /// Describe single item of an order : product itme and quantity
    /// </summary>
    public class OrderDetail
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the quantity of product in order
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the product instance
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }

    }
}
