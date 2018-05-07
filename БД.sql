use "Users";

create database "Users"
ON PRIMARY
(
	name='KP_mdf',                                         --логическое имя
	filename = 'D:\ДЭиВИ\Курсовой проект Назаренко В.В. 9гр\Kурсовой_Проект.mdf', --физическй файл ОС
	size = 5MB,													 --первоачальный размер
	maxsize = 10MB,												 --максимальный
	filegrowth = 1MB											 --приращение
);

create table [User]
(
	ID int identity(1,1) not null constraint PK_ID primary key,	
	Name varchar(30) not null,
	LName varchar(30) not null,
	Email varchar(30) not null,
	DR date not null,
	Gender bit not null,
	Photo varbinary(max)
);

create table Log_in
(
	[Login] varchar(20) constraint PK_Login primary key ,
	[Password] varchar(20) not null,
	ID int not null constraint FK_ID foreign key references [User](ID),
	[TCP] varchar(15) not null
) ;
drop table Messages
create table [Messages]
(
	
	ID int identity(1,1) not null constraint PK_messageID primary key,
	[Message] varchar(500),
	Receiver_ID int not null,
	Sender_ID int not null,
	DialogID int not null constraint FK_MesDiaID foreign key references Dialog(ID),
);
drop table Dialog
create table Dialog
(
	
	ID int identity(1,1) not null constraint PK_dialogID primary key,
	Receiver_ID int not null,
	Sender_ID int not null,
	Receiver_Name varchar(30) not null,
);
