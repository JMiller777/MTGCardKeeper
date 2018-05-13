using DataAccessLayer;
using DataTransferObjects;
using System;
using System.Collections.Generic;

namespace LogicLayer
{
    public class CardManager : ICardManager
    {
        private ICardAccessor _cardAccessor;

        public CardManager()
        {
            _cardAccessor = new CardAccessor();
        }

        public List<Card> RetrieveCardListByActive(bool active = true)
        {
            List<Card> cardList = null;

            try
            {
                cardList = _cardAccessor.RetrieveCardByActive(active);
            }
            catch (Exception)
            {
                throw;
            }
            return cardList;
        }
        public List<Rarity> RetrieveRarityList()
        {
            try
            {
                return _cardAccessor.RetrieveRarityList();
            }
            catch
            {
                throw;
            }
        }
        public List<CardColor> RetrieveColorList()
        {
            try
            {
                return _cardAccessor.RetrieveColorList();
            }
            catch
            {
                throw;
            }
        }
        public List<Edition> RetrieveEditionList()
        {
            try
            {
                return _cardAccessor.RetrieveEditionList();
            }
            catch
            {
                throw;
            }
        }
        public List<DataTransferObjects.Type> RetrieveTypeList()
        {
            try
            {
                return _cardAccessor.RetrieveTypeList();
            }
            catch
            {
                throw;
            }
        }
        public List<Card> RetrieveCardList()
        {
            try
            {
                return _cardAccessor.RetrieveCardList();
            }
            catch
            {
                throw;
            }
        }
        public CardDetail RetrieveCardDetail(Card cardItem)
        {
            CardDetail cardDetail = null;
            ImgFileName imgFileName = null;

            try
            {

                imgFileName = _cardAccessor.RetrieveImgFileNameByCardID(cardItem.CardID);

                cardDetail = new CardDetail()
                {
                    Card = cardItem,
                    ImgFileName = imgFileName
                };
            }
            catch
            {
                throw;
            }

            return cardDetail;
        }
        public bool DeactivateCardByID(int cardID)
        {
            var result = false;

            try
            {
                result = _cardAccessor.DeactivateCardByID(cardID);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public bool EditCard(Card card, Card oldCard)
        {
            var result = false;

            if (card.Name == "" || card.RarityID == "" || card.TypeID == "" || card.EditionID == "" || card.ColorID == "" || card.CardText == "" || card.ImgFileName == "" || card.Active == null || card.IsFoil == null || card.CardID == null)
            {
                throw new ApplicationException("You must fill out all the fields.");
            }
            try
            {
                result = (0 != _cardAccessor.EditCard(card, oldCard));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public bool AddCard(Card card)
        {
            var result = false;

            if (card.Name == "" || card.RarityID == "" || card.TypeID == "" || card.EditionID == "" || card.ColorID == "" || card.CardText == "" || card.ImgFileName == "" || card.Active == null || card.IsFoil == null || card.CardID == null)
            {
                throw new ApplicationException("You must complete all fields to add a new card.");
            }
            try
            {
                result = (0 != _cardAccessor.InsertCard(card));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
