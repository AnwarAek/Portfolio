using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure;
using Core.Interfaces;

namespace Web.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IUnitOfWork<Contact> _contact;

        public ContactsController(IUnitOfWork<Contact> contact)
        {
            _contact = contact;
        }

        // GET: Contacts
        public IActionResult Index()
        {
            return View(_contact.Entity.GetAll().ToList());
        }

        // GET: Contacts/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = SerachById(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Contact()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _contact.Entity.Insert(contact);
                _contact.Save();
                return RedirectToAction(nameof(Contact));
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = SerachById(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var contact = SerachById(id);
            _contact.Entity.Delete(id);
            _contact.Save();
            return RedirectToAction(nameof(Index));
        }

        private Contact SerachById(Guid? id)
        {
            return _contact.Entity.GetById(id);
        }
    }
}
