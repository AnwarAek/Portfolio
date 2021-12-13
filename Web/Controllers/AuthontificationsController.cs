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
    public class AuthontificationsController : Controller
    {
        private readonly IUnitOfWork<Authontification> _authentification;

        public AuthontificationsController(IUnitOfWork<Authontification> authentification)
        {
            _authentification = authentification;
        }


        // GET: Authontifications/Create
        public IActionResult Login()
        {
            return View();
        }

        // POST: Authontifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Authontification connexion)
        {
            if (ModelState.IsValid)
            {
                var authentification = _authentification.Entity.GetAll().First();
                if(connexion.UserName == authentification.UserName && connexion.Password == authentification.Password)
                {
                    return RedirectToAction("Index" , "PortfolioItems");
                }
                else
                {
                    ViewBag.Message = "Veuillez vérifier vos données";

                    return View(connexion);
                }
            }
            return View(connexion);
        }

        // GET: Authontifications/Edit/5
        public IActionResult GetPass(Guid? id)
        {
            return View();
        }

        // POST: Authontifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetPass(Guid id, Authontification connexion)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var authentification = _authentification.Entity.GetAll().First();
                    if (connexion.UserName == authentification.UserName && connexion.PassPhrase == authentification.PassPhrase)
                    {
                        return RedirectToAction(nameof(Edit));
                    }
                    else
                    {
                        ViewBag.Message = "Veuillez vérifier vos données";

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(connexion);
        }


            // GET: Authontifications/Edit/5
            public IActionResult Edit(Guid? id)
        {
            return View();
        }

        // POST: Authontifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Authontification connexion)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var authentification = _authentification.Entity.GetAll().First();
                    authentification.Password = connexion.Password;
                    _authentification.Entity.Update(authentification);
                    _authentification.Save();
                    return RedirectToAction(nameof(Login));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(connexion);
        }

        private bool AuthontificationExists(Guid id)
        {
            return _authentification.Entity.GetAll().Any(a => a.Id == id);
        }

    }
}
