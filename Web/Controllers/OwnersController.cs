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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Web.ViewModels;

namespace Web.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IUnitOfWork<Owner> _owner;
        private readonly IWebHostEnvironment _webHost;

        public OwnersController(IUnitOfWork<Owner> owner ,
            IWebHostEnvironment webHost)
        {
            _owner = owner;
            _webHost = webHost;
        }

        // GET: Owners
        public IActionResult Index()
        {
            return View(_owner.Entity.GetAll());
        }

        // GET: Owners/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = OwnerExist(id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OwnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Ajout de file image
                var fileName = UploadFile(model.File) ?? string.Empty;
                Owner owner = new Owner
                {
                    FullName = model.FullName,
                    Profile = model.Profile,
                    Avatar = model.File.FileName
                };
                _owner.Entity.Insert(owner);
                _owner.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Owners/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = OwnerExist(id);
            OwnerViewModel model = new OwnerViewModel
            {
                Id = owner.Id,
                FullName = owner.FullName,
                Profile =owner.Profile,
                Imageurl = owner.Avatar
            };

            if (owner == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, OwnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string FileName = string.Empty;
                    if(model.File != null)
                    {
                        FileName = UpdateFile(model.File, model.Imageurl);
                    }
                    Owner item = new Owner
                    {
                        Id = model.Id,
                        FullName = model.FullName,
                        Profile = model.Profile,
                        Avatar = FileName
                    };
                    _owner.Entity.Update(item);
                    _owner.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Owners/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = OwnerExist(id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var owner = OwnerExist(id);
            if(owner.Avatar != null)
            {
                DeleteFile(owner.Avatar);
            }

            _owner.Entity.Delete(id);
            _owner.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(Guid id)
        {
            return _owner.Entity.GetAll().Any(x => x.Id == id);
        }

        private Owner OwnerExist(Guid? id)
        {
            return _owner.Entity.GetById(id);
        }

        private string UploadFile(IFormFile File)
        {
            if (File != null)
            {
                string portfolioPath = Path.Combine(_webHost.WebRootPath, @"img/owner");
                string NewFilePAth = Path.Combine(portfolioPath, File.FileName);
                File.CopyTo(new FileStream(NewFilePAth, FileMode.Create));

                return File.FileName;
            }
            else
            {
                return null;
            }
        }
        private string UpdateFile(IFormFile File, string imageUrl)
        {
            if (File != null)
            {
                string portfolioPath = Path.Combine(_webHost.WebRootPath, @"img/owner");
                //delete the old file(image)
                string OldFilePath = Path.Combine(portfolioPath, imageUrl);
                System.IO.File.Delete(OldFilePath);
                //Save the new File(image)
                string NewFilePAth = Path.Combine(portfolioPath, File.FileName);
                File.CopyTo(new FileStream(NewFilePAth, FileMode.Create));

                return File.FileName;
            }
            return imageUrl;
        }
        private void DeleteFile(string oldFileName)
        {
            if(oldFileName != null)
            {
                string portfolioPath = Path.Combine(_webHost.WebRootPath, @"img/owner");
                //delete the old file(image)
                string OldFilePath = Path.Combine(portfolioPath, oldFileName);
                System.IO.File.Delete(OldFilePath);
            }
        }
    }
}
