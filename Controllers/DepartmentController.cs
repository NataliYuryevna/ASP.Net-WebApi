using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BusinessAutomation.Models;

namespace BusinessAutomation.Controllers
{
    public class DepartmentController : ApiController
    {
        private UserContext db = new UserContext();

        // GET api/Department
        public IQueryable<Department> GetDepartments()
        {
            return db.Departments;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}