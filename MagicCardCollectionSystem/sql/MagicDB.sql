/* Check if database already exists and delete it if it does exist*/
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'MagicDB') 
BEGIN
DROP DATABASE MagicDB
print '' print '*** dropping database MagicDB'
END
GO

print '' print '*** creating database MagicDB'
GO
CREATE DATABASE MagicDB
GO

print '' print '*** using database MagicDB'
GO
USE [MagicDB]
GO

print '' print '*** Creating Collectors Table'
GO
/* ***** Object:  Table [dbo].[Collector]     ***** */
CREATE TABLE [dbo].[Collector](
	[CollectorID] 	[nvarchar](20) 			NOT NULL,
	[FirstName]		[nvarchar](100)			NOT NULL,
	[LastName]		[nvarchar](100)			NOT NULL,
	[PhoneNumber]	[nvarchar](15)			NOT NULL,
	[Email]			[nvarchar](100)			NOT NULL,
	[PasswordHash]	[nvarchar](100)			NOT NULL DEFAULT '9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e',
	[Active]		[bit]					NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CollectorID] PRIMARY KEY([CollectorID] ASC),
	CONSTRAINT [ak_Email] UNIQUE ([Email] ASC)
)
GO

print '' print '*** Creating Index for Collector.Email'
GO
CREATE NONCLUSTERED INDEX [ix_Collector_Email] ON [dbo].[Collector]([Email]);
GO

print '' print '*** Inserting User Sample Data'
GO
INSERT INTO [dbo].[Collector]
		([CollectorID], [FirstName], [LastName], [PhoneNumber], [Email])
	VALUES
		('Collector001','Jen', 'Royer', '3195556666', 'jen@magic.com'),
		('Collector002','John', 'Miller', '3195557777', 'john@magic.com'),
		('Collector003','Gandalf', 'Grey', '3195558888', 'gandalf@magic.com'),
		('Collector004','Goblin', 'Squee', '3195559999', 'squee@magic.com')
GO

print '' print '*** Creating Role Table'
GO
CREATE TABLE [dbo].[Role](
	[RoleID]				[nvarchar](50)			NOT NULL,
	[RoleDescription]		[nvarchar](1000)		NOT NULL,
	CONSTRAINT [pk_RoleID]	PRIMARY KEY ([RoleID] ASC)
)
GO

print '' print '*** Inserting Sample Role Records'
INSERT INTO [dbo].[Role]
		([RoleID], [RoleDescription])
	VALUES
		('Viewer','Can view card collection'),
		('Collector','Can add to collection'),
		('Maintenance','Can modify records'),
		('Admin','Admin')
GO

print '' print '*** Creating CollectorRole Table'
GO
CREATE TABLE [dbo].[CollectorRole](
	[CollectorID]			[nvarchar](20)			NOT NULL,
	[RoleID]				[nvarchar](50)			NOT NULL,
	[Active]				[bit]					NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CollectorIDRoleID] 	PRIMARY KEY([CollectorID] ASC, [RoleID] ASC)
)
GO

print '' print '*** Inserting Sample CollectorRole Records'
INSERT INTO [dbo].[CollectorRole]
		([CollectorID], [RoleID])
	VALUES
		('Collector001', 'Admin'),
		('Collector002', 'Collector'),
		('Collector003', 'Maintenance'),
		('Collector003', 'Viewer'),
		('Collector004', 'Admin')
GO

/* *** Foreign Key Constraints *** */

print '' print '*** Creating UserRole UserID foreign key'
GO
ALTER TABLE [dbo].[CollectorRole]  WITH NOCHECK 
	ADD CONSTRAINT [FK_CollectorID] FOREIGN KEY([CollectorID])
	REFERENCES [dbo].[Collector] ([CollectorID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating UserRole RoleID foreign key'
GO
ALTER TABLE [dbo].[CollectorRole]  WITH NOCHECK 
	ADD CONSTRAINT [FK_RoleID] FOREIGN KEY([RoleID])
	REFERENCES [dbo].[Role] ([RoleID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating Card Table'
GO
/* ***** Object:  Table [dbo].[Card]     ***** */
CREATE TABLE [dbo].[Card](
	[CardID] 				[int] IDENTITY (100000,1) 	NOT NULL,
	[Name]					[nvarchar](150)				NOT NULL,
	[ColorID]				[nvarchar](100)				NOT	NULL,
	[TypeID]				[nvarchar](100)				NOT NULL,
	[EditionID]				[nvarchar](100)				NOT NULL,
	[RarityID]				[nvarchar](100)				NOT NULL,
	[IsFoil]				[bit]						NOT NULL DEFAULT 0,
	[Active]				[bit]						NOT NULL DEFAULT 1,
	[CardText]				[nvarchar](1000)			NOT NULL,
	[ImgFileNameID]			[nvarchar](250)				NULL
	
	CONSTRAINT [pk_CardID] PRIMARY KEY([CardID] ASC)
)
GO

print '' print '*** Inserting Card Sample Data'
INSERT INTO [dbo].[Card]
	([Name], [ColorID], [TypeID], [EditionID], [RarityID], [CardText])
		VALUES
		('Akron Legionaire',	'White',	'Summon',		'Fifth Edition',	'Rare'
			,"Except for Legionnaires and artifact creatures, creatures you control cannot attack."),
		
		('Avizoa',				'Blue',		'Creature',		'Weatherlight',		'Rare'
			,"Skip your next untap phase: Avizoa gets +2/+2 until end of turn. Use this ability only once each turn."),
		
		('Bind',				'Green',	'Instant',		'Invasion',			'Rare'
			,"Counter target activated ability. (Mana abilities can't be countered.) Draw a card."),
			
		('Cave-In',				'Red',		'Sorcery',		'Mercadian Masques','Rare'
			,"You may remove a red card in your hand from the game instead of paying Cave-In's mana cost. Cave-In deals 2 damage to each creature and each player."),
			
		('Copper-Leaf Angel',	'Artifact',	'Creature',		'Prophecy',			'Rare'
			,"Tap, Sacrifice X lands: Put X +1/+1 counters on Copper-Leaf Angel."),
			
		('Lashknife Barrier', 	'White',	'Enchantment',	'Planeshift', 		'Uncommon'
			,"When Lashknife Barrier comes into play, draw a card. If a source would deal damage to a creature you control, it deals that much damage minus 1 to that creature instead."),
			
		('Terrain Generator', 	'Colorless','Land', 		'Nemesis', 			'Uncommon'
			,"Tap: Add one colorless mana to your mana pool. Two colorless mana and Tap: Put a basic land card from your hand into play tapped."),

		('Peat Bog', 			'Colorless','Land',			'Mercadian Masques', 'Common'
			,"Peat Bog comes into play tapped with two depletion counters on it. Tap, Remove a depletion counter from Peat Bog: Add two black mana to your mana pool. If there are no depletion counters on Peat Bog, sacrafice it.")
GO

print '' print '*** Creating Color Table'
/* ***** Object:  Table [dbo].[Color]     ***** */
GO
CREATE TABLE [dbo].[Color](
	[ColorID]			[nvarchar](100)					NOT NULL,
	[Description]		[nvarchar](1000)				NULL
	
	CONSTRAINT [pk_ColorID] PRIMARY KEY([ColorID] ASC)
)
GO

print '' print '*** Inserting Color Records'
INSERT INTO [dbo].[Color]
	([ColorID], [Description])
		VALUES
			('Black', 'Power, self-interest, death, sacrafice, uninhibited'),
			('Blue', 'Knowledge, deceit, cautius, deliberate, perfecting'),
			('Green', 'Nature, wildlife, connected, spiritual, tradition'),
			('Red', 'Freedom, emotion, active, impulsive, destructive'),
			('White', 'Peace, law, structured, selflessness, equality'),
			('Artifact', 'An artifact.'),
			('Colorless', 'Mana without the constraints (or benefits) of color.')
			
GO
	

print '' print '*** Creating Type Table'
/* ***** Object:  Table [dbo].[Type]     ***** */
GO
CREATE TABLE [dbo].[Type](
	[TypeID]			[nvarchar](100)			NOT NULL,
	[Active]			[bit]					NOT NULL DEFAULT 1

	CONSTRAINT [pk_TypeID] PRIMARY KEY([TypeID] ASC)
)
GO

print '' print '*** Inserting Type Records'
INSERT INTO [dbo].[Type]
		([TypeID])
	VALUES
		('Artifact'),
		('Conspiracy'),
		('Enchantment'),
		('Instant'),
		('Creature'),
		('Interrupt'),
		('Land'),
		('Mana Source'),
		('Non-Standard'),
		('Phenomenon'),
		('Planeswalker'),
		('Scheme'),
		('Sorcery'),
		('Summon'),
		('Tribal'),
		('Vanguard')
GO



print '' print '*** Creating Edition Table'
/* ***** Object:  Table [dbo].[Edition]     ***** */
GO
CREATE TABLE [dbo].[Edition](
	[EditionID]			[nvarchar](100)			NOT NULL
	
	CONSTRAINT [pk_EditionID] PRIMARY KEY([EditionID] ASC)
)
GO

print '' print '*** Inserting Edition Records'
INSERT INTO [dbo].[Edition]
		([EditionID])
	VALUES
		('Alliances'),
		('Alpha'),
		('Antiquities'),
		('Apocalypse'),
		('Arabian Nights'),
		('Beta'),
		('Classic'),
		('Exodus'),
		('Fallen Empires'),
		('Fifth Edition'),
		('Fourth Edition'),
		('HomeLands'),
		('Ice Age'),
		('Invasion'),
		('Judgment'),
		('Legends'),
		('Legions'),
		('Mercadian Masques'),
		('Mirage'),
		('Nemesis'),
		('Odyssey'),
		('Onslaught'),
		('PlaneShift'),
		('Prophecy'),
		('Revised'),
		('Scourge'),
		('Seventh Edition'),
		('Stronghold'),
		('Tempest'),
		('The Dark'),
		('Torment'),
		('Unlimited'),
		("Urza's Destiny"),
		("Urza's Legacy"),
		("Urza's Saga"),
		('Visions'),
		('Weatherlight')
GO



print '' print '*** Creating Rarity Table'
/* ***** Object:  Table [dbo].[Rarity]     ***** */
GO
CREATE TABLE [dbo].[Rarity](
	[RarityID]				[nvarchar](100)				NOT NULL
	
	CONSTRAINT [pk_RarityID] PRIMARY KEY([RarityID] ASC)
)
GO

print '' print '*** Inserting Rarity Records'
INSERT INTO [dbo].[Rarity]
		([RarityID])
	VALUES
		('Common'),
		('Uncommon'),
		('Rare'),
		('Mythic Rare'),
		('Masterpiece'),
		('Timeshifted')
GO

print '' print '*** Creating ImgFileName Table'
/* ***** Object:  Table [dbo].[ImgFileName]     ***** */
GO
CREATE TABLE [dbo].[ImgFileName](
	[ImgFileNameID]		[nvarchar](250)		NOT NULL,
	[CardID]			int					NOT NULL
	CONSTRAINT [pk_ImgFileNameID] PRIMARY KEY([ImgFileNameID] ASC)
)
GO


print '' print '*** Inserting ImgFileName Sample Data'
INSERT INTO [dbo].[ImgFileName]
		([ImgFileNameID], [CardID])
	VALUES
	('\cardData\Akron_Legionnaire.png', 100000),
	('\cardData\Avizoa.jpg', 100001),
	('\cardData\Bind.jpg', 100002),
	('\cardData\Cave_In.jpg', 100003),
	('\cardData\Copper-Leaf_Angel.jpg', 100004),
	('\cardData\Lashknife_Barrier.png', 100005),
	('\cardData\Terrain_Generator.png', 100006),
	('\cardData\Peat_Bog.png', 100007)
GO

		
/* *** Foreign Key Constraints *** */
print '' print '*** Creating Color foreign key'
GO
ALTER TABLE [dbo].[Card] WITH NOCHECK
	ADD CONSTRAINT [FK_ColorID] FOREIGN KEY([ColorID])
	REFERENCES [dbo].[Color] ([ColorID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating ImgFileName foreign key'
GO
ALTER TABLE [dbo].[Card] WITH NOCHECK
	ADD CONSTRAINT [FK_ImgFileNameID] FOREIGN KEY([ImgFileNameID])
	REFERENCES [dbo].[ImgFileName] ([ImgFileNameID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating Type foreign key'
GO
ALTER TABLE [dbo].[Card] WITH NOCHECK
	ADD CONSTRAINT [FK_TypeID] FOREIGN KEY([TypeID])
	REFERENCES [dbo].[Type] ([TypeID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating Edition foreign key'
GO
ALTER TABLE [dbo].[Card] WITH NOCHECK
	ADD CONSTRAINT [FK_EditionID] FOREIGN KEY([EditionID])
	REFERENCES [dbo].[Edition] ([EditionID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating Rarity foreign key'
GO
ALTER TABLE [dbo].[Card] WITH NOCHECK
	ADD CONSTRAINT [FK_RarityID] FOREIGN KEY([RarityID])
	REFERENCES [dbo].[Rarity] ([RarityID])
	ON UPDATE CASCADE
GO

/* *** Stored Procedures for Cards *** */

print '' print '*** Creating sp_select_card_list'
GO
CREATE PROCEDURE sp_select_card_list
AS
	BEGIN
		SELECT	[Card].[CardID], [Name], [ColorID], [TypeID], [EditionID], [RarityID], [IsFoil], [Active], [CardText], [ImgFileName].[ImgFileNameID]
		FROM	[Card], [ImgFileName]
		WHERE 	[Card].[CardID] = [ImgFileName].[CardID]
	END
GO

print '' print '*** Creating sp_select_card_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_card_by_active]
	(
	@Active 	[bit]
	)
AS
	BEGIN
		SELECT 		[CardID], [Name], [ColorID], [TypeID], 
					[EditionID], [RarityID],[IsFoil], [Active], [CardText]
		FROM 		[Card]
		WHERE 		[Active] = @Active
		ORDER BY 	[Name]
	END
GO

print '' print '*** Creating sp_select_rarity_by_id'
GO
CREATE PROCEDURE sp_select_rarity_by_id
	(
	@RarityID	[nvarchar](100)
	)
AS	
	BEGIN
		SELECT		[RarityID]
		FROM		[Rarity]
		WHERE		[RarityID] = @RarityID
	END
GO

print '' print '*** Creating sp_select_rarity_list'
GO
CREATE PROCEDURE sp_select_rarity_list
AS
	BEGIN
		SELECT		[RarityID]
		FROM		[Rarity]
	END
GO

print '' print '*** Creating sp_select_color_list'
GO
CREATE PROCEDURE sp_select_color_list
AS
	BEGIN
		SELECT		[ColorID]
		FROM		[Color]
	END
GO

print '' print '*** Creating sp_select_edition_list'
GO
CREATE PROCEDURE sp_select_edition_list
AS
	BEGIN
		SELECT		[EditionID]
		FROM		[Edition]
	END
GO

print '' print '*** Creating sp_select_type_list'
GO
CREATE PROCEDURE sp_select_type_list
AS
	BEGIN
		SELECT		[TypeID]
		FROM		[Type]
	END
GO

print '' print '*** Creating sp_select_imgfilenameid_by_cardid'
GO
CREATE PROCEDURE sp_select_imgfilenameid_by_cardid
	(
	@CardID		int
	)
AS	
	BEGIN
		SELECT		[ImgFileNameID]
		FROM		[ImgFileName]
		WHERE		[CardID] = @CardID
	END
GO

print '' print '*** Creating sp_update_card_details'
GO
CREATE PROCEDURE [dbo].[sp_update_card_details]
	(
	@CardID				[int],
	@NewName			[nvarchar](150),
	@NewColorID			[nvarchar](100),
	@NewTypeID 			[nvarchar](100),
	@NewEditionID 		[nvarchar](100),
	@NewRarityID 		[nvarchar](100),
	@NewCardText 		[nvarchar](1000),
	@NewImgFileNameID 	[nvarchar](250),
	@NewActive			[bit],
	@NewIsFoil			[bit],
	
	@OldName			[nvarchar](150),
	@OldColorID			[nvarchar](100),
	@OldTypeID 			[nvarchar](100),
	@OldEditionID 		[nvarchar](100),
	@OldRarityID 		[nvarchar](100),
	@OldCardText 		[nvarchar](1000),
	@OldImgFileNameID 	[nvarchar](250),
	@OldActive			[bit],
	@OldIsFoil			[bit]
	)
AS
	BEGIN
		UPDATE [dbo].[Card]
			SET	[Name] = @NewName
			, [CardText] = @NewCardText
			, [ColorID] = @NewColorID	
			, [TypeID] = @NewTypeID
			, [EditionID] = @NewEditionID
			, [RarityID] = @NewRarityID
			, [IsFoil] = @NewIsFoil
			, [Active] = @NewActive
			WHERE 	[CardID] = @CardID
			AND [Name] = @OldName
			AND [CardText] = @OldCardText
			AND	[ColorID] = @OldColorID
			AND	[TypeID] = @OldTypeID
			AND [EditionID] = @OldEditionID
			AND [RarityID] = @OldRarityID
			AND [IsFoil] = @OldIsFoil
			And [Active] = @OldActive
		UPDATE [dbo].[ImgFileName]
			SET [ImgFileNameID] = @NewImgFileNameID
			Where [CardID] = @CardID
			AND [ImgFileNameID] = @OldImgFileNameID
			
		RETURN @@ROWCOUNT
	END
GO	

print '' print '*** Creating sp_add_card'
GO
CREATE PROCEDURE [dbo].[sp_add_card]
	(
	@Name			[nvarchar](150),
	@ColorID		[nvarchar](100),
	@TypeID 		[nvarchar](100),
	@EditionID 		[nvarchar](100),
	@RarityID 		[nvarchar](100),
	@CardText 		[nvarchar](1000),
	@ImgFileNameID 	[nvarchar](250),
	@Active			[bit],
	@IsFoil			[bit]
	)
AS
	BEGIN
		INSERT INTO [dbo].[Card]
			([Name], [ColorID], [TypeID], [EditionID], [RarityID], [CardText], [Active], [IsFoil])
		VALUES
			(@Name, @ColorID, @TypeID, @EditionID, @RarityID, @CardText, @Active, @IsFoil)
		SELECT SCOPE_IDENTITY() AS SCOPE_IDENTITY
		
		INSERT INTO [dbo].[ImgFileName]
			([CardID], [ImgFileNameID])
		VALUES
			(SCOPE_IDENTITY(), @ImgFileNameID)
	END
GO	

/* *** Stored Procedures for collectors (Users) *** */

print '' print '*** Creating sp_update_collector_email'
GO
CREATE PROCEDURE [dbo].[sp_update_collector_email]
	(
	@CollectorID	[nvarchar](20),
	@Email			[nvarchar](100)
	)
AS
	BEGIN
		UPDATE [dbo].[Collector]
			SET		[Email] = @Email
			WHERE 	[CollectorID] = @CollectorID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
	(
	@Email			[nvarchar](100),
	@PasswordHash	[nvarchar](100)
	)
AS
	BEGIN
		SELECT COUNT([CollectorID])
		FROM 	[Collector]
		WHERE 	[Email] = @Email
		AND 	[PasswordHash] = @PasswordHash
		AND		[Active] = 1
	END
GO

print '' print '*** Creating sp_retrieve_collector_roles'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_collector_roles]
	(
	@CollectorID		[nvarchar](20)
	)
AS
	BEGIN
		SELECT 	[RoleID]
		FROM 	[CollectorRole]
		WHERE 	[CollectorRole].[CollectorID] = @CollectorID
		AND		[Active] = 1
	END
GO

print '' print '*** Creating sp_update_passwordHash'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordHash]
	(
	@CollectorID		nvarchar(20),
	@OldPasswordHash 	nvarchar(100),
	@NewPasswordHash	nvarchar(100)
	)
AS
	BEGIN	
		UPDATE [Collector]
			SET [PasswordHash] = @NewPasswordHash
			WHERE [CollectorID] = @CollectorID
			AND [PasswordHash] = @OldPasswordHash
		Return @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_retrieve_collector_by_email'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_collector_by_email]
	(
	@Email		[nvarchar](100)
	)
AS
	BEGIN
		SELECT 	[CollectorID], [FirstName], [LastName], [PhoneNumber], [Email], [Active]
		FROM 	[Collector]
		WHERE 	[Email] = @Email
	END
GO	

print '' print '*** Creating sp_update_collector_profile'
GO
CREATE PROCEDURE [dbo].[sp_update_collector_profile]
	(
	@NewFirstName	[nvarchar](100),
	@NewLastName	[nvarchar](100),
	@NewPhoneNumber	[nvarchar](15),
	@OldFirstName	[nvarchar](100),
	@OldLastName	[nvarchar](100),
	@OldPhoneNumber	[nvarchar](15),
	@CollectorID	[nvarchar](20)
	)
AS
	BEGIN
		UPDATE 		[Collector]
			SET 	[FirstName] = @NewFirstName,
					[LastName] = @NewLastName,
					[PhoneNumber] = @NewPhoneNumber
			WHERE 	[CollectorID] = @CollectorID
			  AND	[FirstName] = @OldFirstName
			  AND	[LastName] = @OldLastName
			  AND	[PhoneNumber] = @OldPhoneNumber
		RETURN @@ROWCOUNT		
	END
GO

print '' print '*** Creating sp_deactivate_card_by_id'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_card_by_id]
	(
	@CardID		[int]
	)
AS
	BEGIN
		UPDATE [Card]
			SET [Active] = 0
			WHERE [CardID] = @CardID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_deactivate_collector'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_collector]
	(
	@CollectorID		[nvarchar](20)
	)
AS
	BEGIN
		UPDATE [Collector]
			SET [Active] = 0
			WHERE [CollectorID] = @CollectorID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_add_collector'
GO
CREATE PROCEDURE [dbo].[sp_add_collector]
	(
	@CollectorID		[nvarchar](20),
	@FirstName			[nvarchar](100),
	@LastName			[nvarchar](100),
	@PhoneNumber		[nvarchar](15),
	@Email				[nvarchar](100)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Collector]
			([CollectorID], [FirstName], [LastName], [PhoneNumber], [Email])
		VALUES
			(@CollectorID, @FirstName, @LastName, @PhoneNumber, @Email)
	END
GO

print '' print '*** Creating sp_retrieve_collector_by_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_collector_by_id]
	(
	@CollectorID		[nvarchar](100)
	)
AS
	BEGIN
		SELECT 	[CollectorID], [FirstName], [LastName], [PhoneNumber], [Email], [Active]
		FROM 	[Collector]
		WHERE 	[collectorID] = @CollectorID
	END
GO