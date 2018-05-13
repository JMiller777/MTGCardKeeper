using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ICardAccessor
    {
        List<Card> RetrieveCardByActive(bool active = true);
        List<Card> RetrieveCardList();
        Rarity RetrieveRarityByID(string rarityID);
        List<Rarity> RetrieveRarityList();
        List<CardColor> RetrieveColorList();
        List<Edition> RetrieveEditionList();
        List<DataTransferObjects.Type> RetrieveTypeList();
        ImgFileName RetrieveImgFileNameByCardID(int cardID);
        bool DeactivateCardByID(int cardID);
        int EditCard(Card card, Card oldCard);
        int InsertCard(Card card);
    }
}