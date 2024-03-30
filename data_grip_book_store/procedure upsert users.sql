CREATE OR REPLACE PROCEDURE book_schema.spUser_Upsert(
    IN InputName VARCHAR,
    IN InputPasswordHash bytea,
    IN InputPasswordSalt bytea,
    IN InputRole VARCHAR = NULL,
    IN InputEmail VARCHAR = NULL,
    IN InputUserId INTEGER = -1
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NOT EXISTS (SELECT * FROM book_schema.Users WHERE id = InputUserId) THEN

        INSERT INTO book_schema.Users (
            Name,
            Role,
            Email,
            PasswordHash,
            PasswordSalt
        ) VALUES (
            InputName,
            InputRole,
            InputEmail,
            InputPasswordHash,
            InputPasswordSalt
        );
    ELSE
        UPDATE book_schema.Users
        SET 
            Name = InputName,
            PasswordHash = InputPasswordHash,
            PasswordSalt = InputPasswordSalt
        WHERE Id = InputUserId;
    END IF;
END;
$$;
