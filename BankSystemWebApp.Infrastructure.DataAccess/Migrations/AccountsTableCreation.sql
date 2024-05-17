create type AccountType as enum (
    'Classic',
    'Credit',
    'Deposit'
);

create table Accounts (
    bankId int not null references Banks (Id),
    id serial primary key,
    name text not null,
    surname text not null,
    age date,
    email text not null,
    address text not null,
    type AccountType
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