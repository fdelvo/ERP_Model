using System;
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
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //returns all addresses
        public async Task<IHttpActionResult> GetAddresses(int page = 0, int pageSize = 0)
        {
            if (pageSize == 0)
                pageSize = await db.Addresses.CountAsync();

            var addresses = await db.Addresses
                .OrderByDescending(o => o.AddressLastName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = addresses,
                PageAmount = (db.Addresses.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        public async Task<IHttpActionResult> GetCustomers(int page = 0, int pageSize = 0)
        {
            if (pageSize == 0)
                pageSize = await db.Customers.CountAsync();

            var customers = await db.Customers
                .OrderByDescending(o => o.CustomerLastName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = customers,
                PageAmount = (db.Customers.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        public async Task<IHttpActionResult> GetSuppliers(int page = 0, int pageSize = 0)
        {
            if (pageSize == 0)
                pageSize = await db.Suppliers.CountAsync();

            var suppliers = await db.Suppliers
                .OrderByDescending(o => o.SupplierLastName)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataVm = new PaginationViewModel
            {
                DataObject = suppliers,
                PageAmount = (db.Customers.Count() + pageSize - 1) / pageSize,
                CurrentPage = page
            };

            return Ok(dataVm);
        }

        //returns an address
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> GetAddress(Guid id)
        {
            var address = await db.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();

            return Ok(address);
        }

        public async Task<IHttpActionResult> GetCustomer(Guid id)
        {
            var customer = await db.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        public async Task<IHttpActionResult> GetSupplier(Guid id)
        {
            var supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        //updates an address
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddress(Guid id, Address address)
        {
            //check if data sent from form matches the format of the data in database
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            if (id != address.AddressGuid)
                return BadRequest();

            db.Entry(address).State = EntityState.Modified;

            //save changes to the address
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomer(Guid id, Customer customer)
        {
            //check if data sent from form matches the format of the data in database
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            if (id != customer.CustomerGuid)
                return BadRequest();

            var customerToUpdate = await db.Customers.FirstOrDefaultAsync(g => g.CustomerGuid == customer.CustomerGuid);
            customerToUpdate.CustomerCompany = customer.CustomerCompany;
            customerToUpdate.CustomerForName = customer.CustomerForName;
            customerToUpdate.CustomerLastName = customer.CustomerLastName;
            customerToUpdate.CustomerAddress =
                await db.Addresses.FirstOrDefaultAsync(g => g.AddressGuid == customer.CustomerAddress.AddressGuid);

            db.Entry(customerToUpdate).State = EntityState.Modified;

            //save changes to the address
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupplier(Guid id, Supplier supplier)
        {
            //check if data sent from form matches the format of the data in database
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            if (id != supplier.SupplierGuid)
                return BadRequest();

            var supplierToUpdate = await db.Suppliers.FirstOrDefaultAsync(g => g.SupplierGuid == supplier.SupplierGuid);
            supplierToUpdate.SupplierCompany = supplier.SupplierCompany;
            supplierToUpdate.SupplierForName = supplier.SupplierForName;
            supplierToUpdate.SupplierLastName = supplier.SupplierLastName;
            supplierToUpdate.SupplierAddress =
                await db.Addresses.FirstOrDefaultAsync(g => g.AddressGuid == supplier.SupplierAddress.AddressGuid);
            db.Entry(supplierToUpdate).State = EntityState.Modified;

            //save changes to the address
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                    return NotFound();
                throw;
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
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

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
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new {id = address.AddressGuid}, address);
        }

        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            //check if data sent from form matches the format of the data in database
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            //generate new guid for the address
            customer.CustomerGuid = Guid.NewGuid();
            customer.CustomerAddress =
                await db.Addresses.FirstOrDefaultAsync(g => g.AddressGuid == customer.CustomerAddress.AddressGuid);

            //add address to db context
            db.Customers.Add(customer);

            //save changes
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerGuid))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new {id = customer.CustomerGuid}, customer);
        }

        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> PostSupplier(Supplier supplier)
        {
            //check if data sent from form matches the format of the data in database
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            //generate new guid for the address
            supplier.SupplierGuid = Guid.NewGuid();
            supplier.SupplierAddress =
                await db.Addresses.FirstOrDefaultAsync(g => g.AddressGuid == supplier.SupplierAddress.AddressGuid);

            //add address to db context
            db.Suppliers.Add(supplier);

            //save changes
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SupplierExists(supplier.SupplierGuid))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new {id = supplier.SupplierGuid}, supplier);
        }

        //deletes an address
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> DeleteAddress(Guid id)
        {
            //get the address
            var address = await db.Addresses.FindAsync(id);

            //check if address exists
            if (address == null)
                return NotFound();

            //remove address from db context
            db.Addresses.Remove(address);

            //save changes to db
            await db.SaveChangesAsync();

            return Ok(address);
        }

        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(Guid id)
        {
            //get the address
            var customer = await db.Customers.FindAsync(id);

            //check if address exists
            if (customer == null)
                return NotFound();

            //remove address from db context
            db.Customers.Remove(customer);

            //save changes to db
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> DeleteSupplier(Guid id)
        {
            //get the address
            var supplier = await db.Suppliers.FindAsync(id);

            //check if address exists
            if (supplier == null)
                return NotFound();

            //remove address from db context
            db.Suppliers.Remove(supplier);

            //save changes to db
            await db.SaveChangesAsync();

            return Ok(supplier);
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
            var user = new ApplicationUser {UserName = model.Email, Email = model.Email, Alias = model.Alias};

            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return GetErrorResult(result);

            return Ok();
        }


        //updates an user
        public async Task<IHttpActionResult> PutUser(Guid id, ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            if (id.ToString() != user.Id)
                return BadRequest();

            db.Entry(user).State = EntityState.Modified;

            //save changes to user
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> ChangePasswordForUser(ResetPasswordBindingModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                foreach (var v in ModelState.Values)
                foreach (var e in v.Errors)
                    if (e.Exception != null)
                        return
                            BadRequest(
                                "Something went wrong. Please check your form fields for disallowed or missing values.");

                return BadRequest(ModelState);
            }

            if (!UserExists(id))
                return Conflict();

            await UserManager.RemovePasswordAsync(id.ToString());

            var result = await UserManager.AddPasswordAsync(id.ToString(), model.NewPassword);

            if (!result.Succeeded)
                return GetErrorResult(result);

            return Ok();
        }

        public async Task<IHttpActionResult> DeleteUser(Guid id)
        {
            //get the user
            var user = await db.Users.FirstOrDefaultAsync(i => i.Id == id.ToString());

            //check if user exists
            if (user == null)
                return NotFound();

            //remove user from db context
            db.Users.Remove(user);

            //save changes to db
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        //check if address exists
        private bool AddressExists(Guid id)
        {
            return db.Addresses.Count(e => e.AddressGuid == id) > 0;
        }

        private bool CustomerExists(Guid id)
        {
            return db.Customers.Count(e => e.CustomerGuid == id) > 0;
        }

        private bool SupplierExists(Guid id)
        {
            return db.Suppliers.Count(e => e.SupplierGuid == id) > 0;
        }

        //check if user exists
        private bool UserExists(Guid id)
        {
            return db.Users.Count(e => e.Id == id.ToString()) > 0;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);

                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}