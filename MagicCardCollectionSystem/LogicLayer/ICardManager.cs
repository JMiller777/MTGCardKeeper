using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;

namespace LogicLayer
{
    public interface ICardManager
    {
        List<Card> RetrieveCardListByActive(bool active = true);
        List<Rarity> RetrieveRarityList();
        List<CardColor> RetrieveColorList();
        List<Edition> RetrieveEditionList();
        List<DataTransferObjects.Type> RetrieveTypeList();
        List<Card> RetrieveCardList();
        CardDetail RetrieveCardDetail(Card cardItem);
        bool DeactivateCardByID(int cardID);
        bool EditCard(Card card, Card oldCard);
        bool AddCard(Card card);
    }
}