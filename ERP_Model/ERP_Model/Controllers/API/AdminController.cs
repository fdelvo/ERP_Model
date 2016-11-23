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

/*
api controller to handle administration (addresses, users, customers, suppliers)
*/

namespace ERP_Model.Controllers.API
{
    public class AdminController : ApiController
    {
        //get database context
        private ApplicationDbContext db = new ApplicationDbContext();

        //returns all addresses
        public IQueryable<Address> GetAddresses()
        {
            return db.Addresses;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(Guid id)
        {
            return db.Addresses.Count(e => e.AddressGuid == id) > 0;
        }
    }
}
