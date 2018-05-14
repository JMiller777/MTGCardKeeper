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
            var cdList = _cdMgr.RetrieveCardList();

            var colorList = _cdMgr.RetrieveColorList().ToList();
            List<string> allColors = new List<string>();

            var editionList = _cdMgr.RetrieveEditionList().ToList();
            List<string> allEditions = new List<string>();

            var rarityList = _cdMgr.RetrieveRarityList().ToList();
            List<string> allRarities = new List<string>();

            var typeList = _cdMgr.RetrieveTypeList().ToList();
            List<string> allTypes = new List<string>();

            foreach (var color in colorList)
            {
                    allColors.Add(color.ColorID.ToString());
             
            }
            ViewBag.Colors = allColors;

            foreach (var edition in editionList)
            {
             
                    allEditions.Add(edition.EditionID.ToString());
             
            }
            ViewBag.Editions = allEditions;

            foreach (var type in typeList)
            {
             
                    allTypes.Add(type.TypeID.ToString());
             
            }
            ViewBag.Types = allTypes;

            foreach (var rarity in rarityList)
            {
             
                    allRarities.Add(rarity.RarityID.ToString());
             
            }
            ViewBag.Rarities = allRarities;

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

            var colorList = _cdMgr.RetrieveColorList().ToList();
            List<string> allColors = new List<string>();

            var editionList = _cdMgr.RetrieveEditionList().ToList();
            List<string> allEditions = new List<string>();

            var rarityList = _cdMgr.RetrieveRarityList().ToList();
            List<string> allRarities = new List<string>();

            var typeList = _cdMgr.RetrieveTypeList().ToList();
            List<string> allTypes = new List<string>();

            Card card = cdList.Find(cd => cd.CardID == id);

            foreach (var color in colorList)
            {
                if (color.ToString() != card.ColorID)
                {
                    allColors.Add(color.ColorID.ToString());
                }
            }
            ViewBag.Colors = allColors;

            foreach (var edition in editionList)
            {
                if (edition.ToString() != card.EditionID)
                {
                    allEditions.Add(edition.EditionID.ToString());
                }
            }
            ViewBag.Editions = allEditions;

            foreach (var type in typeList)
            {
                if (type.ToString() != card.TypeID)
                {
                    allTypes.Add(type.TypeID.ToString());
                }
            }
            ViewBag.Types = allTypes;

            foreach (var rarity in rarityList)
            {
                if (rarity.ToString() != card.RarityID)
                {
                    allRarities.Add(rarity.RarityID.ToString());
                }
            }
            ViewBag.Rarities = allRarities;

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
