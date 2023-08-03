using ContactList.Common.Contracts;
using ContactList.DataContexts;
using ContactList.DataServices;
using ContactList.Models;
using ContactList.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ContactList.Controllers
{
    [Authorize]
    public class ContactsController : BaseContactListController
    {
        private readonly ContactListAppContext _context;

        public ContactsController(ContactListAppContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            try
            {
                var cs = new ContactsService(_context, GetCurrentUserId());

                return View(await cs.List());
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex) 
            {
                return Problem(ex.Message);
            }
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await GetContact(id);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactRequest req)
        {
            try
            {
                Contact contact = new Contact();
                var cs = new ContactsService(_context, GetCurrentUserId());

                if (ModelState.IsValid)
                {
                    contact = cs.Create(req);

                    return RedirectToAction(nameof(Index));
                }

                return View(contact);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await GetContact(id);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactRequest req)
        {
            try
            {
                var cs = new ContactsService(_context, GetCurrentUserId());
                var contact = await cs.Details(id);

                if (ModelState.IsValid)
                {
                    contact = await cs.Edit(id, contact, req);

                    return RedirectToAction(nameof(Index));
                }

                return View(contact);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await GetContact(id);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cs = new ContactsService(_context, GetCurrentUserId());
                await cs.Remove(id);

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private bool ContactExists(int id)
        {
          return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<IActionResult> GetContact(int? id)
        {
            try
            {
                var cs = new ContactsService(_context, GetCurrentUserId());

                return View(await cs.Details(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
