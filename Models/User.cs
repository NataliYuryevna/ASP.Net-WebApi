using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessAutomation.Models
{
    public class User
    {
        public int Id {get; set;}
        public String UserName {get; set;}
        public int DepartmentId {get; set;}

        public Department Department {get; set;}
    }
}