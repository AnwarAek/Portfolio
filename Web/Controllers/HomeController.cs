using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork<Owner> _owner;
        private readonly IUnitOfWork<PortfolioItem> _portfolioItem;
        private readonly IUnitOfWork<About> _about;
        private readonly IUnitOfWork<Adresse> _adresse;

        public HomeController(
            IUnitOfWork<Owner> owner,
            IUnitOfWork<PortfolioItem> PortfolioItem,
            IUnitOfWork<About> about,
            IUnitOfWork<Adresse> adresse)
        {
            _owner = owner;
            _portfolioItem = PortfolioItem;
            _about = about;
            _adresse = adresse;
        }
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Owner = _owner.Entity.GetAll().First(),
                PortfolioItems = _portfolioItem.Entity.GetAll().ToList(),
                About = _about.Entity.GetAll().First(),
                Adresse = _adresse.Entity.GetAll().First()
            };

            return View(model);
        }
    } 
}
