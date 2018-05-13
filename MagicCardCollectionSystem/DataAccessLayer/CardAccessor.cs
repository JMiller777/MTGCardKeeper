using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CardAccessor : ICardAccessor
    {
        public List<Card> RetrieveCardByActive(bool active = true)
        {
            var cardList = new List<Card>();

            // connection 
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_card_by_active";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type 
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Active", active);

            // try-catch 
            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // has some rows? 
                if (reader.HasRows)
                {
                    // process the reader 
                    while (reader.Read())
                    {
                        /*
                        [CardID] 				[int] IDENTITY (100000,1) 	NOT NULL,
                        [Name]					[nvarchar](150)				NOT NULL,
                        [Color]					[nvarchar](100)				NOT	NULL,
                        [Type]					[nvarchar](100)				NOT NULL,
                        [Edition]				[nvarchar](100)				NOT NULL,
                        [Rarity]				[nvarchar](100)				NOT NULL,
                        [IsFoil]				[bit]						NOT NULL DEFAULT 0,
                        [Active]				[bit]						NOT NULL DEFAULT 1,
                        [CardText]				[nvarchar](1000)			NOT NULL,
                        [ImgFileName]			[nvarchar](50)				NULL,
                        [Comments]				[nvarchar](1000)			NULL   
                        */
                        var cd = new Card()
                        {
                            CardID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ColorID = reader.GetString(2),
                            TypeID = reader.GetString(3),
                            EditionID = reader.GetString(4),
                            RarityID = reader.GetString(5),
                            IsFoil = reader.GetBoolean(6),
                            Active = reader.GetBoolean(7),
                            CardText = reader.GetString(8)
                        };
                        // don't forget to add the card to the list...
                        cardList.Add(cd);
                    }
                }
                // if there aren't any rows to read through
                else
                {
                    throw new ApplicationException("Data not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database access error.", ex);
            }
            finally
            {
                // close the connection
                conn.Close();
            }

            return cardList;
        }
        public List<Card> RetrieveCardList()
        {
            var cardList = new List<Card>();

            // connection 
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_card_list";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type 
            cmd.CommandType = CommandType.StoredProcedure;

            // try-catch 
            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // has some rows? 
                if (reader.HasRows)
                {
                    // process the reader 
                    while (reader.Read())
                    {
                        var cd = new Card()
                        {
                            CardID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ColorID = reader.GetString(2),
                            TypeID = reader.GetString(3),
                            EditionID = reader.GetString(4),
                            RarityID = reader.GetString(5),
                            IsFoil = reader.GetBoolean(6),
                            Active = reader.GetBoolean(7),
                            CardText = reader.GetString(8),
                            ImgFileName = reader.GetString(9)                           
                        };
                        cardList.Add(cd);
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database access error.", ex);
            }
            finally
            {
                conn.Close();
            }
            return cardList;
        }
        public Rarity RetrieveRarityByID(string rarityID)
        {
            Rarity rarity = null;

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_rarity_by_id";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters and values
            cmd.Parameters.AddWithValue("@RarityID", rarityID);

            // try-catch
            try
            {
                // open the conneciton 
                conn.Open();

                // execute command
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    rarity = new Rarity()
                    {
                        RarityID = reader.GetString(0),
                        Description = reader.IsDBNull(1) ? null : reader.GetString(1)
                    };
                }
                else
                {
                    throw new ApplicationException("Record not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data access error.", ex);
            }
            finally
            {
                conn.Close();
            }

            return rarity;
        }
        public List<Rarity> RetrieveRarityList()
        {
            var rarityList = new List<Rarity>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_rarity_list";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type 
            cmd.CommandType = CommandType.StoredProcedure;

            // no parameters here

            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // rows?
                if (reader.HasRows)
                {
                    // process the reader
                    while (reader.Read())
                    {
                        var rar = new Rarity()
                        {
                            RarityID = reader.GetString(0)
                        };
                        rarityList.Add(rar);
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database access error.", ex);
            }
            finally
            {
                conn.Close();
            }

            return rarityList;
        }
        public List<CardColor> RetrieveColorList()
        {
            var colorList = new List<CardColor>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_color_list";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type 
            cmd.CommandType = CommandType.StoredProcedure;

            // no parameters here

            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // rows?
                if (reader.HasRows)
                {
                    // process the reader
                    while (reader.Read())
                    {
                        var color = new CardColor()
                        {
                            ColorID= reader.GetString(0)
                        };
                        colorList.Add(color);
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database access error.", ex);
            }
            finally
            {
                conn.Close();
            }

            return colorList;
        }
        public List<Edition> RetrieveEditionList()
        {
            var editionList = new List<Edition>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_edition_list";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type 
            cmd.CommandType = CommandType.StoredProcedure;

            // no parameters here

            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // rows?
                if (reader.HasRows)
                {
                    // process the reader
                    while (reader.Read())
                    {
                        var edition = new Edition()
                        {
                            EditionID = reader.GetString(0)
                        };
                        editionList.Add(edition);
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database access error.", ex);
            }
            finally
            {
                conn.Close();
            }

            return editionList;
        }
        public List<DataTransferObjects.Type> RetrieveTypeList()
        {
            var typeList = new List<DataTransferObjects.Type>();

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_type_list";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type 
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // rows?
                if (reader.HasRows)
                {
                    // process the reader
                    while (reader.Read())
                    {
                        var type = new DataTransferObjects.Type()
                        {
                            TypeID = reader.GetString(0)
                        };
                        typeList.Add(type);
                    }
                }
                else
                {
                    throw new ApplicationException("Data not found.");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database access error.", ex);
            }
            finally
            {
                conn.Close();
            }

            return typeList;
        }
        public ImgFileName RetrieveImgFileNameByCardID(int cardID)
        {
            ImgFileName imgFileName = null;

            // connection
            var conn = DBConnection.GetDBConnection();

            // command text
            var cmdText = @"sp_select_imgfilenameid_by_cardid";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters and values
            cmd.Parameters.AddWithValue("@CardID", cardID);

            // try-catch to make connection
            try
            {
                // open the connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    imgFileName = new ImgFileName()
                    {
                        ImgFileNameID = reader.GetString(0)
                        // CardID = reader.GetInt32(1)
                    };
                }
                else
                {
                    throw new ApplicationException("Record not found.");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data access error.", ex);
            }
            finally
            {
                conn.Close();
            }

            return imgFileName;
        }
        public bool DeactivateCardByID(int cardID)
        {
            int result = 0;
            var cmdText = @"sp_deactivate_card_by_id";
            var conn = DBConnection.GetDBConnection();

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CardID", cardID);

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int EditCard(Card card, Card oldCard)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_update_card_details";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CardID", card.CardID);
            cmd.Parameters.AddWithValue("@NewName", card.Name);
            cmd.Parameters.AddWithValue("@NewColorID", card.ColorID);
            cmd.Parameters.AddWithValue("@NewTypeID", card.TypeID);
            cmd.Parameters.AddWithValue("@NewEditionID", card.EditionID);
            cmd.Parameters.AddWithValue("@NewRarityID", card.RarityID);
            cmd.Parameters.AddWithValue("@NewCardText", card.CardText);
            cmd.Parameters.AddWithValue("@NewImgFileNameID", card.ImgFileName);
            cmd.Parameters.AddWithValue("@NewActive", card.Active);
            cmd.Parameters.AddWithValue("@NewIsFoil", card.IsFoil);

            cmd.Parameters.AddWithValue("@OldName", oldCard.Name);
            cmd.Parameters.AddWithValue("@OldColorID", oldCard.ColorID);
            cmd.Parameters.AddWithValue("@OldTypeID", oldCard.TypeID);
            cmd.Parameters.AddWithValue("@OldEditionID", oldCard.EditionID);
            cmd.Parameters.AddWithValue("@OldRarityID", oldCard.RarityID);
            cmd.Parameters.AddWithValue("@OldCardText", oldCard.CardText);
            cmd.Parameters.AddWithValue("@OldImgFileNameID", oldCard.ImgFileName);
            cmd.Parameters.AddWithValue("@OldActive", oldCard.Active);
            cmd.Parameters.AddWithValue("@OldIsFoil", oldCard.IsFoil);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            return rows;
        }
        public int InsertCard(Card card)
        {
            int newId = 0;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_add_card";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@Name", card.Name);
            cmd.Parameters.AddWithValue("@ColorID", card.ColorID);
            cmd.Parameters.AddWithValue("@TypeID", card.TypeID);
            cmd.Parameters.AddWithValue("@EditionID", card.EditionID);
            cmd.Parameters.AddWithValue("@RarityID", card.RarityID);
            cmd.Parameters.AddWithValue("@CardText", card.CardText);
            cmd.Parameters.AddWithValue("@ImgFileNameID", card.ImgFileName);
            cmd.Parameters.AddWithValue("@Active", card.Active);
            cmd.Parameters.AddWithValue("@IsFoil", card.IsFoil);

            try
            {
                conn.Open();
                decimal id = (decimal)cmd.ExecuteScalar();
                newId = (int)id;
            }
            catch (Exception)
            {
                throw;
            }
            return newId;
        }

    }
}