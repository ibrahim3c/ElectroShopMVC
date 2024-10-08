﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyShop.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyShop.Entities.Models
{
	public  class OrderDetails
	{
        public int Id { get; set; }
		[ForeignKey(nameof(OrderHeader))]
		public int OrderHeaderId {  get; set; }
		[ValidateNever]
		public OrderHeader OrderHeader { get; set; }

		[ForeignKey(nameof(Product))]
		public int ProductId {  get; set; }
		[ValidateNever]
		public Product Product { get; set; }

		public int Quantity {  get; set; }
		public decimal Price {  get; set; }



    }
}
