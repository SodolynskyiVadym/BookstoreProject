CREATE OR REPLACE PROCEDURE book_schema.spOrder_Delete(
    IN InputOrderId INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    booksId INTEGER ARRAY;
    i INT;
BEGIN
    SELECT ARRAY(SELECT bookId FROM book_schema.orderdetailinfo WHERE orderId = InputOrderId) INTO booksId;


    FOR i IN 1..array_length(booksId, 1) LOOP
        UPDATE book_schema.bookgenerallyinfo 
        SET availableQuantity = availableQuantity + 
            (SELECT quantity FROM book_schema.orderdetailinfo 
             WHERE orderId = InputOrderId AND bookId = booksId[i])
        WHERE id=booksId[i];

        DELETE FROM book_schema.orderdetailinfo 
        WHERE orderId = InputOrderId AND bookId = booksId[i];
    END LOOP;


    DELETE FROM book_schema.ordergenerallyinfo WHERE Id = InputOrderId;
END;
$$;




-- DROP PROCEDURE book_schema.spOrder_Delete;

