using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Web.ViewModels;
using System.IO;

namespace Web.Controllers
{
    public class AboutsController : Controller
    {
        private readonly IUnitOfWork<About> _about;
        private readonly IWebHostEnvironment _webHost;

        public AboutsController(IUnitOfWork<About> about,
            IWebHostEnvironment webHost)
        {
            _about = about;
            _webHost = webHost;
        }

        // GET: Abouts
        public IActionResult Index()
        {
            return View(_about.Entity.GetAll());
        }

        // GET: Abouts/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = AboutExist(id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // GET: Abouts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Abouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(Guid id , AboutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var FileName = string.Empty;
                if(model.PdfFile != null)
                {
                    FileName = UploadFile(model.PdfFile) ?? string.Empty;
                }

                About about = new About
                {
                    FirstResume = model.FirstResume,
                    SecondResume = model.SecondResume,
                    PdfUrl = FileName
                };
                _about.Entity.Insert(about);
                _about.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Abouts/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = AboutExist(id);

            AboutViewModel model = new AboutViewModel
            {
                Id = about.Id,
                FirstResume = about.FirstResume,
                SecondResume = about.SecondResume,
                PdfUrl = about.PdfUrl
            };

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Abouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, AboutViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var FileName = string.Empty;
                   if(model.PdfFile != null)
                    {
                        FileName = UpdateFile(model.PdfFile, model.PdfUrl);
                    }

                    About about = new About
                    {
                        Id = model.Id,
                        FirstResume = model.FirstResume,
                        SecondResume= model.SecondResume,
                        PdfUrl = FileName
                    };

                    _about.Entity.Update(about);
                    _about.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(model.Id))
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

        // GET: Abouts/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = AboutExist(id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var about = AboutExist(id);
            if(about.PdfUrl != null)
            {
                DeleteFile(about.PdfUrl);
            }
            _about.Entity.Delete(id);
            _about.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutExists(Guid id)
        {
            return _about.Entity.GetAll().Any(a => a.Id == id);
        }
        private About AboutExist(Guid? id)
        {
            return _about.Entity.GetById(id);
        }
        private string UploadFile(IFormFile File)
        {
            if (File != null)
            {
                string portfolioPath = Path.Combine(_webHost.WebRootPath, @"docs");
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
                string portfolioPath = Path.Combine(_webHost.WebRootPath, @"docs");
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
            if (oldFileName != null)
            {
                string portfolioPath = Path.Combine(_webHost.WebRootPath, @"docs");
                //delete the old file(image)
                string OldFilePath = Path.Combine(portfolioPath, oldFileName);
                System.IO.File.Delete(OldFilePath);
            }
        }
    }
}
