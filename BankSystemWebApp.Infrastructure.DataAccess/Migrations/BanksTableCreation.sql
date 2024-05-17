create table Banks (
    Id serial primary key,
    Name text not null
);

create table BanksEntryKeys (
    bankId int not null references Banks (Id),
    adminEntryKey int
);

create table BanksBalance (
    bankId int not null references Banks (Id),
    Balance numeric
);

create table BankOperationHistory (
    bankId int not null references Banks (Id),
    operationType text not null
);