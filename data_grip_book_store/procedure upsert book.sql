CREATE OR REPLACE PROCEDURE book_schema.spBook_Upsert(
    IN InputNumberPages INTEGER,
    IN InputBookLanguage VARCHAR(255),
    IN InputYearPublication DATE,
    IN InputDescription TEXT,
    IN InputPublisherId INTEGER,
    IN InputName VARCHAR(255),
    IN InputAuthorId INTEGER,
    IN InputAvailableQuantity INTEGER,
    IN InputPrice INTEGER,
    IN InputDiscount INTEGER,
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
            Price,
            Discount
        ) VALUES (
            InputName,
            InputAuthorId,
            InputAvailableQuantity,
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
    ELSE
        IF InputBookId IS NOT NULL THEN
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
        ELSE
            RAISE EXCEPTION 'Book not found';
        END IF;
    END IF;
END;
$$;


-- DROP PROCEDURE book_schema.spBook_Upsert
