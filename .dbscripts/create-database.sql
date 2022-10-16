CREATE TABLE payroll_loans (
    Id UUID NOT NULL PRIMARY KEY, 
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
    EntityId UUID,
    CreatedAt DATE NOT NULL,
    Type Varchar(350) NOT NULL,
    Version INT NOT NULL,
    SerializedEvent json not null,
  	 CONSTRAINT fk_payroll_loans_events
      FOREIGN KEY(EntityId) 
	  REFERENCES payroll_loans(id)
);