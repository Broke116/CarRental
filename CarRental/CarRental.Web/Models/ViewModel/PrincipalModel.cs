using System.Collections.Generic;
using CarRental.Data.App_Data;

namespace CarRental.Web.Models.ViewModel
{
    public class PrincipalModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string[] Roles { get; set; }
    }
}