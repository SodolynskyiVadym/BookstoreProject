CREATE TABLE book_schema.Authors (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Biography TEXT,
    BirthYear DATE,
    DeathYear DATE
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
    ImageUrl VARCHAR(255) NOT NULL,
    AvailableQuantity INTEGER NOT NULL,
    Price INTEGER NOT NULL,
    Discount INTEGER NOT NULL
);


CREATE TABLE book_schema.Genres (
    Id SERIAL PRIMARY KEY,
    BookId INTEGER NOT NULL,
    Genre VARCHAR(255) NOT NULL
);


CREATE TABLE book_schema.Publishers (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);


CREATE TABLE book_schema.Reviews (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL,
    BookId INTEGER NOT NULL,
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


CREATE TABLE book_schema.OrderedBooks (
    Id SERIAL PRIMARY KEY,
    OrderId INTEGER NOT NULL,
    BookId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL
);


CREATE TABLE book_schema.Payments (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER,
    Destination VARCHAR NOT NULL,
    PhoneNumber VARCHAR NOT NULL,
    CreatedAt DATE NOT NULL
);

ALTER TABLE book_schema.bookgenerallyinfo ADD CONSTRAINT book_gen_author FOREIGN KEY (authorId) REFERENCES book_schema.authors (id);

ALTER TABLE book_schema.bookdetailinfo ADD CONSTRAINT book_det_book_gen FOREIGN KEY (bookid) REFERENCES book_schema.bookgenerallyinfo (id);

ALTER TABLE book_schema.bookdetailinfo ADD CONSTRAINT book_det_publisher FOREIGN KEY (publisherid) REFERENCES book_schema.publishers (id);

ALTER TABLE book_schema.Genres ADD CONSTRAINT genres_book_gen FOREIGN KEY (BookId) REFERENCES book_schema.BookGenerallyInfo (id);

ALTER TABLE book_schema.Reviews ADD CONSTRAINT review_user FOREIGN KEY (userid) REFERENCES book_schema.users (id);

ALTER TABLE book_schema.Payments ADD CONSTRAINT payment_user FOREIGN KEY (UserId) REFERENCES book_schema.Users (id);

ALTER TABLE book_schema.OrderedBooks ADD CONSTRAINT order_book_payment FOREIGN KEY (OrderId) REFERENCES book_schema.Payments (id);

ALTER TABLE book_schema.OrderedBooks ADD CONSTRAINT order_book_book_gen FOREIGN KEY (BookId) REFERENCES book_schema.BookGenerallyInfo (id);

ALTER TABLE book_schema.reviews ADD CONSTRAINT review_book_gen FOREIGN KEY (bookid) REFERENCES book_schema.bookgenerallyinfo (id);

ALTER TABLE book_schema.reviews ADD CONSTRAINT review_unique UNIQUE (UserId, BookId);



DROP TABLE book_schema.OrderedBooks;
DROP TABLE book_schema.Genres;
DROP TABLE book_schema.Reviews;
DROP TABLE book_schema.BookDetailInfo;
DROP TABLE book_schema.BookGenerallyInfo;
DROP TABLE book_schema.Authors;
DROP TABLE book_schema.Publishers;
DROP TABLE book_schema.Payments;
DROP TABLE book_schema.Users;




