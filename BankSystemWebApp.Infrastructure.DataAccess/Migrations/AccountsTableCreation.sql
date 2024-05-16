create table Accounts (
    bankId int not null references Banks (Id),
    Id serial primary key,
    Name text not null,
    Surname text not null,
    Age date,
    Email text not null,
    Address text not null
);

create table AccountPassport (
    accountId int not null references Accounts (Id),
    Passport int
);

create table AccountBalance (
    accountId int not null references Accounts (Id),
    Balance numeric
);

create table AccountPin (
    accountId int not null references Accounts (Id),
    PIN bigint
);

create table AccountOperationHistory (
    accountId int not null references Accounts (Id),
    operationType text not null
);

create table AccountNotifications (
    accountId int not null references Accounts(id),
    notifications text not null
);