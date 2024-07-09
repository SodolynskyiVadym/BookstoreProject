CREATE OR REPLACE PROCEDURE book_schema.spBook_Upsert(
    IN InputNumberPages INTEGER,
    IN InputBookLanguage VARCHAR(255),
    IN InputYearPublication DATE,
    IN InputDescription TEXT,
    IN InputPublisherId INTEGER,
    IN InputName VARCHAR(255),
    IN InputImageUrl VARCHAR(255),
    IN InputAuthorId INTEGER,
    IN InputAvailableQuantity INTEGER,
    IN InputPrice INTEGER,
    IN InputDiscount INTEGER,
    IN InputGenres VARCHAR ARRAY,
    IN InputBookId INTEGER = -1
)
LANGUAGE plpgsql
AS $$
DECLARE 
    OutputBookId INT;
BEGIN
    IF NOT EXISTS (SELECT * FROM book_schema.BookGenerallyInfo WHERE id = InputBookId) THEN

        INSERT INTO book_schema.BookGenerallyInfo (
            Name,
            AuthorId,
            AvailableQuantity,
            imageurl,
            Price,
            Discount
        ) VALUES (
            InputName,
            InputAuthorId,
            InputAvailableQuantity,
            InputImageUrl,
            InputPrice,
            InputDiscount
        )
        RETURNING LASTVAL() INTO OutputBookId;

        INSERT INTO book_schema.BookDetailInfo (
            BookId,
            NumberPages,
            BookLanguage,
            YearPublication,
            Description,
            PublisherId
        ) VALUES (
            OutputBookId,
            InputNumberPages,
            InputBookLanguage,
            InputYearPublication,
            InputDescription,
            InputPublisherId
        );
        
        FOR i IN 1..array_length(InputGenres, 1) LOOP
            INSERT INTO book_schema.Genres (
                BookId,
                Genre
            ) VALUES (
                OutputBookId,
                InputGenres[i]
            );
        END LOOP;
    ELSE
        UPDATE book_schema.BookGenerallyInfo
        SET 
            Name = InputName,
            AuthorId = InputAuthorId,
            AvailableQuantity = InputAvailableQuantity,
            Price = InputPrice,
            Discount = InputDiscount
        WHERE Id = InputBookId;

        UPDATE book_schema.BookDetailInfo
        SET 
            NumberPages = InputNumberPages,
            BookLanguage = InputBookLanguage,
            YearPublication = InputYearPublication,
            Description = InputDescription,
            PublisherId = InputPublisherId
        WHERE BookId = InputBookId;
            
            IF array_length(InputGenres, 1) > 0 THEN
            DELETE FROM book_schema.Genres WHERE BookId = InputBookId;
                
            FOR i IN 1..array_length(InputGenres, 1) LOOP
                INSERT INTO book_schema.Genres (
                    BookId,
                    Genre
                ) VALUES (
                     InputBookId,
                    InputGenres[i]
                );
            END LOOP;
            END IF;
    END IF;
END;
$$;


DROP PROCEDURE book_schema.spBook_Upsert
