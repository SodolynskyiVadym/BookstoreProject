CREATE OR REPLACE PROCEDURE book_schema.spOrder_Upsert(
    IN InputBooksId INTEGER ARRAY,
    IN InputQuantities INTEGER ARRAY,
    IN InputDestination VARCHAR,
    IN InputUserId INTEGER = NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    i INT;
    OutputOrderId INT;
BEGIN
    INSERT INTO book_schema.ordergenerallyinfo (userId, destination, createdAt, isDelevered)
    VALUES (InputUserId, InputDestination, NOW(), FALSE)
    RETURNING LASTVAL() INTO OutputOrderId; 


    FOR i IN 1..array_length(InputBooksId, 1) LOOP
        INSERT INTO book_schema.OrderDetailInfo (orderId, bookid, quantity)
        VALUES (OutputOrderId, InputBooksId[i], InputQuantities[i]);
    END LOOP;
END;
$$;

DROP PROCEDURE book_schema.spOrder_Upsert
