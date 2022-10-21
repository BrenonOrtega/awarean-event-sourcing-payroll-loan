CREATE TABLE payroll_loans (
    Id UUID NOT NULL primary key, 
    DocumentNumber varchar(15), 
    InstallmentNumber INT NOT NULL, 
    PaidInstallments INT NOT NULL, 
    Amount NUMERIC(18, 2), 
    InterestRate NUMERIC(18, 4), 
    LatestVersion INT NOT NULL, 
    CreatedAt DATE NOT NULL, 
    UpdatedAt DATE NOT NULL
);

CREATE TABLE payroll_loans_events (
    Id SERIAL PRIMARY KEY,
    EntityId UUID NOT NULL,
    CreatedAt DATE NOT NULL,
    Type Varchar(350) NOT NULL,
    Version INT NOT NULL,
    SerializedEvent json not null,
    CONSTRAINT unique_payroll_loans_events_EntityId_version
    UNIQUE(EntityId, Version)
);