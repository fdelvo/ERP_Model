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

namespace ERP_Model.Controllers.API
{
    public class AdminController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Address> GetAddresses()
        {
            return db.Addresses;
        }

        // GET: api/Stocks/5
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

        // PUT: api/Stocks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddress(Guid id, Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != address.AddressGuid)
            {
                return BadRequest();
            }

            db.Entry(address).State = EntityState.Modified;

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

        // POST: api/Stocks
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> PostAddress(Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            address.AddressGuid = Guid.NewGuid();

            db.Addresses.Add(address);

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

        // DELETE: api/Stocks/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> DeleteAddress(Guid id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            db.Addresses.Remove(address);
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
