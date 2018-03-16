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
    public class UserController : ApiController
    {
        private UserContext db = new UserContext();

        // GET api/user
        public IQueryable<User> GetUsers()
        {
            if (db.Users.Include(u => u.Department).LongCount() != 0)
                return db.Users.Include(u => u.Department);
            else
                return null;
        }

        // GET api/user/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            if (db.Users.Include(u => u.Department).Where(u => u.Id == id).LongCount() != 0)
            {
                return Ok(db.Users.Include(u => u.Department).Single(u => u.Id == id));
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/user
        [HttpPost]
        public IHttpActionResult CreateUser([FromBody]User user)
        {

            if (db.Departments.Find(user.DepartmentId) == null)
            {
                ModelState.AddModelError("DepartmentId", "Такого Department не существует.");
            }

            String userName = user.UserName;
            if (userName.Length == 0)
            {
                ModelState.AddModelError("UserName", "UserName не может быть пустым.");
            }
            else if (db.Users.Where(u => (u.UserName == userName)).LongCount() != 0)
            {
                ModelState.AddModelError("UserName", "Такой user уже существует, введите другое имя в поле UserName.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                db.Users.Add(user);
                db.SaveChanges();
                return Ok(user);
            }
        }

        [HttpPut]
        // PUT api/user/5
        public IHttpActionResult EditUser(int id, [FromBody]User user)
        {
            if (db.Departments.Find(user.DepartmentId) == null)
            {
                ModelState.AddModelError("DepartmentId", "Такого Department не существует.");
            }

            string userName = user.UserName;
            if (userName.Length == 0)
            {
                ModelState.AddModelError("UserName", "UserName не может быть пустым.");
            }
            else if (db.Users.Where(u => (u.UserName == userName && u.Id != id)).LongCount() != 0)
            {
                ModelState.AddModelError("UserName", "Такой user уже существует, введите другое имя в поле UserName.");
            }

            if (db.Users.Find(id) == null)
            {
                ModelState.AddModelError("UserId", "Такой user не существует.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                }
                catch
                {
                    User user2 = db.Users.Find(id);
                    user2.UserName = userName;
                    user2.DepartmentId = user.DepartmentId;
                }
                finally
                {
                    db.SaveChanges();
                }
                return Ok(user);
            }
        }

        // DELETE api/user/5
        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return Ok(user);
            }
            else
            {
                return NotFound();
            }

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