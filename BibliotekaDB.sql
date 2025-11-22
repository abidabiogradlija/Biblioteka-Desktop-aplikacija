CREATE TABLE Autori (
    AuthorId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Country NVARCHAR(50),
    BirthYear INT
);
CREATE TABLE Clanovi (
    MemberId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(30),
    JoinDate DATE NOT NULL
);

CREATE TABLE Knjige (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Year INT,
    ISBN NVARCHAR(20),
    AuthorId INT NOT NULL,
    Quantity INT NOT NULL,

    FOREIGN KEY (AuthorId) REFERENCES Autori(AuthorId)
);

CREATE TABLE Posudbe (
    LoanId INT IDENTITY(1,1) PRIMARY KEY,
    BookId INT NOT NULL,
    MemberId INT NOT NULL,
    LoanDate DATE NOT NULL,
    DueDate DATE NOT NULL,
    ReturnDate DATE,

    FOREIGN KEY (BookId) REFERENCES Knjige(BookId),
    FOREIGN KEY (MemberId) REFERENCES Clanovi(MemberId)
);