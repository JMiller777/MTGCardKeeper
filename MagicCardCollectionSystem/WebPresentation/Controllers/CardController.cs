using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTransferObjects;
using LogicLayer;

namespace WebPresentation.Controllers
{
    public class CardController : Controller
    {
        private ICardManager _cdMgr = new CardManager();

        // GET: Default
        public ActionResult Index()
        {
            return View(_cdMgr.RetrieveCardList());
        }

        // GET: Card/Details/5
        public ActionResult Details(int id)
        {
            Card cd = (_cdMgr.RetrieveCardList().Find(c => c.CardID == id));

            char[] seps = {'\\'};
            string[] iNameParts = cd.ImgFileName.Split(seps);
            string fileName = iNameParts[iNameParts.Length - 1];
            cd.ImgFileName = fileName;
            return View(cd);
        }

        // GET: Card/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Card/Create
        [HttpPost]
        public ActionResult Create(Card card)
        {
            try
            {
                _cdMgr.AddCard(card);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Card/Edit/5
        public ActionResult Edit(int id)
        {
            var cdList = _cdMgr.RetrieveCardList();
            Card card = cdList.Find(cd => cd.CardID == id);

            return View(card);
        }

        // POST: Card/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Card card)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cdList = _cdMgr.RetrieveCardList();
                    var oldCard = cdList.Find(cd => cd.CardID == id);

                    _cdMgr.EditCard(card, oldCard);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        // GET: Card/Delete/5
        public ActionResult Delete(int id)
        {
            _cdMgr.DeactivateCardByID(id);
            return RedirectToAction("Index");
        }

        //// POST: Card/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        _cdMgr.DeactivateCardByID(id);

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
