using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Mvc;

namespace HotelRoomManagementSystem.Models
{
    public class SampleDBContext : DbContext
    {
        public DbSet<HotelModelClass> hotelModelClass { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Hotel_Info> hotel_Info { get; set; }
        public DbSet<Guest_Info> guest_Info { get; set; }
        public DbSet<AuthenticateUser> authenticateUser { get; set; }
        public DbSet<AllGuestInfo> allGuestInfo { get; set; }
    }
}