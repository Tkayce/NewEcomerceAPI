﻿using System.ComponentModel.DataAnnotations.Schema;
using static ECommerceAPI.Model.OrderProduct;

namespace ECommerceAPI.Model
{
    public class Order
    {
      
             public int Id { get; set; }
            public int CustomerId { get; set; }
            public Customer Customer { get; set; }
            public DateTime OrderDate { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            public decimal TotalAmount { get; set; }

            public string OrderStatus { get; set; }

            public ICollection<OrderProduct> OrderProduct { get; set; }  // ✅ Corrected Name
        }

    
}
