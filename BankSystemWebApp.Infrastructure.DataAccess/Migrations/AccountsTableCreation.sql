create type AccountType as enum (
    'Classic',
    'Credit',
    'Deposit'
);

create table Accounts (
    bankId int not null references Banks (id),
    id serial primary key,
    name text not null,
    surname text not null,
    age date,
    email text not null,
    address text not null,
    type AccountType
);

create table AccountPassport (
    accountId int not null references Accounts (id),
    Passport int
);

create table AccountBalance (
    accountId int not null references Accounts (id),
    Balance numeric
);

create table AccountPin (
    accountId int not null references Accounts (id),
    PIN bigint
);

create table AccountCreditPercentage (
    accountId int not null references Accounts (id),
    percent float
);

create table AccountOperationHistory (
    accountId int not null references Accounts (id),
    operationType text not null
);

create table AccountNotifications (
    accountId int not null references Accounts(id),
    notifications text not null
);