use "Users";


create database "Users"
ON PRIMARY
(
	name='KP2_mdf',                                         --логическое имя
	filename = 'D:\Новая папка (2)\Kursach\КП1\KP2.mdf', --физическй файл ОС
	size = 5MB,													 --первоачальный размер
	maxsize = 10MB,												 --максимальный
	filegrowth = 1MB											 --приращение
);

CREATE TABLE [dbo].[Dialogs] (
    [Sender_ID] int  NOT NULL,
    [Receiver_ID] int  NOT NULL,
    [Receiver_Name] varchar(30)  NOT NULL,
    [Sender_Name] varchar(30)  NOT NULL
);
GO

-- Creating table 'Friends'
CREATE TABLE [dbo].[Friends] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MyID] int  NOT NULL,
    [FriendID] int  NOT NULL,
    [Name] varchar(30)  NOT NULL,
    [LName] varchar(30)  NOT NULL,
    [Photo] varbinary(max)  NULL
);
GO

-- Creating table 'Log_in'
CREATE TABLE [dbo].[Log_in] (
    [Login] varchar(20)  NOT NULL,
    [Password] binary(50)  NOT NULL,
    [ID] int  NOT NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Message1] varchar(1000)  NULL,
    [SenderID] int  NOT NULL,
    [ReceiverID] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(30)  NOT NULL,
    [LName] varchar(30)  NOT NULL,
    [Email] varchar(30)  NOT NULL,
    [DR] datetime  NOT NULL,
    [Gender] bit  NOT NULL,
    [Photo] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Sender_ID], [Receiver_ID] in table 'Dialogs'
ALTER TABLE [dbo].[Dialogs]
ADD CONSTRAINT [PK_Dialogs]
    PRIMARY KEY CLUSTERED ([Sender_ID], [Receiver_ID] ASC);
GO

-- Creating primary key on [Id] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [PK_Friends]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Login] in table 'Log_in'
ALTER TABLE [dbo].[Log_in]
ADD CONSTRAINT [PK_Log_in]
    PRIMARY KEY CLUSTERED ([Login] ASC);
GO

-- Creating primary key on [ID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SenderID], [ReceiverID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_ID_Messages]
    FOREIGN KEY ([SenderID], [ReceiverID])
    REFERENCES [dbo].[Dialogs]
        ([Sender_ID], [Receiver_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ID_Messages'
CREATE INDEX [IX_FK_ID_Messages]
ON [dbo].[Messages]
    ([SenderID], [ReceiverID]);
GO

-- Creating foreign key on [FriendID] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [FK_FriendID]
    FOREIGN KEY ([FriendID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FriendID'
CREATE INDEX [IX_FK_FriendID]
ON [dbo].[Friends]
    ([FriendID]);
GO

-- Creating foreign key on [MyID] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [FK_MyID]
    FOREIGN KEY ([MyID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MyID'
CREATE INDEX [IX_FK_MyID]
ON [dbo].[Friends]
    ([MyID]);
GO

-- Creating foreign key on [ID] in table 'Log_in'
ALTER TABLE [dbo].[Log_in]
ADD CONSTRAINT [FK_ID]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ID'
CREATE INDEX [IX_FK_ID]
ON [dbo].[Log_in]
    ([ID]);
GO
