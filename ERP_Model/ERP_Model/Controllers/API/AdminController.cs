using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ERP_Model.Models;
using ERP_Model.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

/*
api controller to handle administration (addresses, users, customers, suppliers)
*/

namespace ERP_Model.Controllers.API
{
    public class AdminController : ApiController
    {
        //get database context
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //returns all addresses
        public async Task<IHttpActionResult> GetAddresses(int page = 0, int pageSize = 0)
        {
            if(pageSize == 0)
            {
                pageSize = await db.Addresses.CountAsync();
            }

            var addresses = await db.Addresses
                .OrderByDescending(o => o.AddressLastName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = addresses,
                PageAmount = (db.Addresses.Count() + pageSize - 1)/pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        //returns an address
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> GetAddress(Guid id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        //updates an address
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddress(Guid id, Address address)
        {
            //check if data sent from form matches the format of the data in database
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != address.AddressGuid)
            {
                return BadRequest();
            }

            db.Entry(address).State = EntityState.Modified;

            //save changes to the address
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //creates a new address
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> PostAddress(Address address)
        {
            //check if data sent from form matches the format of the data in database
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //generate new guid for the address
            address.AddressGuid = Guid.NewGuid();

            //add address to db context
            db.Addresses.Add(address);

            //save changes
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AddressExists(address.AddressGuid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = address.AddressGuid }, address);
        }

        //deletes an address
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> DeleteAddress(Guid id)
        {
            //get the address
            Address address = await db.Addresses.FindAsync(id);

            //check if address exists
            if (address == null)
            {
                return NotFound();
            }

            //remove address from db context
            db.Addresses.Remove(address);

            //save changes to db
            await db.SaveChangesAsync();

            return Ok(address);
        }

        //returns all users
        public async Task<IHttpActionResult> GetUsers(int page, int pageSize)
        {
            var users = await db.Users
                .OrderByDescending(o => o.Alias)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = users,
                PageAmount = (db.Users.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };
            return Ok(dataVm);
        }

        //returns a user
        public async Task<IHttpActionResult> GetUser(Guid id)
        {
            var user = await db.Users.FirstOrDefaultAsync(i => i.Id == id.ToString());

            return Ok(user);
        }

        //creates an user
        public async Task<IHttpActionResult> PostUser(RegisterBindingModel model)
        {
            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, Alias = model.Alias };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        //updates an user
        public async Task<IHttpActionResult> PutUser(Guid id, ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id.ToString() != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            //save changes to user
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> ChangePasswordForUser(ResetPasswordBindingModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!UserExists(id))
            {
                return Conflict();
            }

            await UserManager.RemovePasswordAsync(id.ToString());

            IdentityResult result = await UserManager.AddPasswordAsync(id.ToString(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        public async Task<IHttpActionResult> DeleteUser(Guid id)
        {
            //get the user
            ApplicationUser user = await db.Users.FirstOrDefaultAsync(i => i.Id == id.ToString());

            //check if user exists
            if (user == null)
            {
                return NotFound();
            }

            //remove user from db context
            db.Users.Remove(user);

            //save changes to db
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //check if address exists
        private bool AddressExists(Guid id)
        {
            return db.Addresses.Count(e => e.AddressGuid == id) > 0;
        }

        //check if user exists
        private bool UserExists(Guid id)
        {
            return db.Users.Count(e => e.Id == id.ToString()) > 0;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
