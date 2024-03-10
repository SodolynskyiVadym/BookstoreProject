CREATE TABLE book_schema.Authors (
    Id SERIAL PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Biography TEXT,
    BirthYear DATE,
    DeathYear DATE,
    PhotoUrl VARCHAR(255)
);


CREATE TABLE book_schema.BookDetailInfo (
    Id SERIAL PRIMARY KEY,
    BookId INTEGER NOT NULL,
    NumberPages INTEGER NOT NULL,
    BookLanguage VARCHAR(255) NOT NULL,
    YearPublication DATE NOT NULL,
    Description TEXT NOT NULL,
    PublisherId INTEGER NOT NULL
);


CREATE TABLE book_schema.BookGenerallyInfo (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    AuthorId INTEGER,
    AvailableQuantity INTEGER NOT NULL,
    Price INTEGER NOT NULL,
    Discount INTEGER NOT NULL,
    Likes INTEGER NOT NULL,
    PhotoUrl VARCHAR(255)
);


CREATE TABLE book_schema.Publishers (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    PhotoUrl VARCHAR(255)
);


CREATE TABLE book_schema.Reviews (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL,
    Description TEXT,
    Mark INTEGER NOT NULL
);


CREATE TABLE book_schema.Users (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Role VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash BYTEA NOT NULL,
    PasswordSalt BYTEA NOT NULL
);


CREATE TABLE book_schema.OrderDetailInfo (
    Id SERIAL PRIMARY KEY,
    OrderId INTEGER NOT NULL,
    BookId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL
);


CREATE TABLE book_schema.OrderGenerallyInfo (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER,
    Destination VARCHAR NOT NULL,
    CreatedAt DATE NOT NULL,
    IsDelevered BOOLEAN
);

ALTER TABLE book_schema.bookgenerallyinfo ADD CONSTRAINT book_gen_idauthor FOREIGN KEY (authorId) REFERENCES book_schema.authors (id);

ALTER TABLE book_schema.bookdetailinfo ADD CONSTRAINT bookdet_bookgen FOREIGN KEY (bookid) REFERENCES book_schema.bookgenerallyinfo (id);

ALTER TABLE book_schema.bookdetailinfo ADD CONSTRAINT book_det_ipublisher FOREIGN KEY (publisherid) REFERENCES book_schema.publishers (id);

ALTER TABLE book_schema.reviews ADD CONSTRAINT review_userid FOREIGN KEY (userid) REFERENCES book_schema.users (id);

ALTER TABLE book_schema.OrderDetailInfo ADD CONSTRAINT orderdet_ordergen FOREIGN KEY (OrderId) REFERENCES book_schema.OrderGenerallyInfo (id);

ALTER TABLE book_schema.OrderDetailInfo ADD CONSTRAINT orderdet_bookid FOREIGN KEY (BookId) REFERENCES book_schema.BookGenerallyInfo (id);


DROP TABLE book_schema.Authors;
DROP TABLE book_schema.Users;
DROP TABLE book_schema.Reviews;
DROP TABLE book_schema.Publishers;
DROP TABLE book_schema.BookGenerallyInfo;
DROP TABLE book_schema.BookDetailInfo;
DROP TABLE book_schema.OrderDetailInfo;
DROP TABLE book_schema.OrderGenerallyInfo;
