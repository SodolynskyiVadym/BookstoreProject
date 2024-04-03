CREATE OR REPLACE PROCEDURE book_schema.spOrder_Upsert(
    IN InputBooksId INTEGER ARRAY,
    IN InputQuantities INTEGER ARRAY,
    IN InputDestination VARCHAR,
    IN InputPhoneNumber VARCHAR,
    IN InputUserId INTEGER = NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    i INT;
    OutputOrderId INT;
BEGIN
    INSERT INTO book_schema.ordergenerallyinfo (userId, destination, phonenumber, createdAt)
    VALUES (InputUserId, InputDestination, InputPhoneNumber, NOW())
    RETURNING LASTVAL() INTO OutputOrderId; 


    FOR i IN 1..array_length(InputBooksId, 1) LOOP
        INSERT INTO book_schema.OrderDetailInfo (orderId, bookid, quantity)
        VALUES (OutputOrderId, InputBooksId[i], InputQuantities[i]);
    END LOOP;

    FOR k IN 1..array_length(InputBooksId, 1) LOOP
        UPDATE book_schema.bookgenerallyinfo
        SET availablequantity = bookgenerallyinfo.availablequantity - InputQuantities[k]
        WHERE id = InputBooksId[k];
    END LOOP;
END;
$$;

DROP PROCEDURE book_schema.spOrder_Upsert
