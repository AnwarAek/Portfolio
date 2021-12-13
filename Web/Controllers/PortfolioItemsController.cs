using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure;
using Web.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Web.Controllers
{
    public class PortfolioItemsController : Controller
    {
        private readonly IUnitOfWork<PortfolioItem> _portfolio;
        private readonly IWebHostEnvironment _host;

        public PortfolioItemsController(IUnitOfWork<PortfolioItem> portfolio,
            IWebHostEnvironment host)
        {
            _portfolio = portfolio;
            _host = host;
        }

        // GET: PortfolioItems
        public  IActionResult Index()
        {
            return View(_portfolio.Entity.GetAll());
        }

        // GET: PortfolioItems/Details/5
        public  IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Search if portfolio existe and return it
            var portfolioItem = _portfolio.Entity.GetById(id);
            if (portfolioItem == null)
            {
                return NotFound();
            }

            return View(portfolioItem);
        }

        // GET: PortfolioItems/Create
        public IActionResult Create()
        {
            //return the form normally with no code
            return View();
        }

        // POST: PortfolioItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(PortfolioViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //to uplaod file in wwwroot folder
                    UploadFile(model.File);

                    PortfolioItem portfolioItem = new PortfolioItem
                    {
                        ProgectName = model.ProgectName,
                        Description = model.Description,
                        ImageUrl = model.File.FileName
                    };
                    _portfolio.Entity.Insert(portfolioItem);
                    _portfolio.Save();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                throw;
            }
            ModelState.AddModelError("", "You have to fill All  required fields");
            return View(model);
        }

        // GET: PortfolioItems/Edit/5
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //chercher par id 
            var portfolioItem = PortfolioItemExist(id);
            PortfolioViewModel portfolioViewModel = new PortfolioViewModel
            {
                Id          = portfolioItem.Id,
                ProgectName = portfolioItem.ProgectName,
                Description = portfolioItem.Description,
                ImageUrl    = portfolioItem.ImageUrl
            };

            if (portfolioViewModel == null)
            {
                return NotFound();
            }
            return View(portfolioViewModel);
        }

        // POST: PortfolioItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, PortfolioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Update file
                    UpdateFile(model.File , model.ImageUrl);

                    //Update object
                    PortfolioItem portfolioItem = new PortfolioItem
                    {
                        Id = model.Id,
                        ProgectName = model.ProgectName,
                        Description = model.Description,
                        ImageUrl = model.File.FileName
                    };
                    _portfolio.Entity.Update(portfolioItem);
                    _portfolio.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioItemExists(model.Id))
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
            ModelState.AddModelError("", "You have to fill All  required fields");
            return View(model);
        }

        // GET: PortfolioItems/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //renvoi information about portfolioItem
            var portfolioItem = PortfolioItemExist(id);
            return View(portfolioItem);
        }

        // POST: PortfolioItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id , PortfolioItem item)
        {
            if(item.ImageUrl != null)
            {
                DeleteFile(item.ImageUrl);
            }

            _portfolio.Entity.Delete(id);
            _portfolio.Save();
            return RedirectToAction(nameof(Index));
        }

        private  PortfolioItem PortfolioItemExist(Guid id)
        {
            var portfolio = _portfolio.Entity.GetById(id);
            return portfolio;
        }

        private bool PortfolioItemExists(Guid id)
        {
            return _portfolio.Entity.GetAll().Any(p => p.Id == id);
        }

        private void UploadFile(IFormFile File)
        {
            if (File != null)
            {
                string portfolioPath = Path.Combine(_host.WebRootPath, @"img/portfolio");
                string NewFilePAth = Path.Combine(portfolioPath, File.FileName);
                File.CopyTo(new FileStream(NewFilePAth, FileMode.Create));
            }
        }
        private void UpdateFile(IFormFile File , string oldFileName)
        {
            if (File != null)
            {
                string portfolioPath = Path.Combine(_host.WebRootPath, @"img/portfolio");
                //delete the old file(image)
                string OldFilePath = Path.Combine(portfolioPath, oldFileName);
                System.IO.File.Delete(OldFilePath);
                //Save the new File(image)
                string NewFilePAth = Path.Combine(portfolioPath, File.FileName);
                File.CopyTo(new FileStream(NewFilePAth, FileMode.Create));
            }
        }
        private void DeleteFile(string oldFileName)
        {
                string portfolioPath = Path.Combine(_host.WebRootPath, @"img/portfolio");
                //delete the old file(image)
                string OldFilePath = Path.Combine(portfolioPath, oldFileName);
                System.IO.File.Delete(OldFilePath);
        }
    }
}
