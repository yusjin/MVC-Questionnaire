using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace YQ.Web.DB
{
    public class StoreContext : DbContext
    {
        //public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Frame> Frames { get; set; }
    }

    public class Cart
    {

    }

    public class Order
    {

    }

    public class Frame
    {

    }
}