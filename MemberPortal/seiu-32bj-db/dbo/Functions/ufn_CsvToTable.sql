CREATE FUNCTION [dbo].[ufn_CsvToTable]
(
	@StringInput VARCHAR(MAX)
)
RETURNS @ReturnTable TABLE
(
	ReturnValue varchar(200)
)
AS
BEGIN
	DECLARE @String    VARCHAR(200)

    WHILE LEN(@StringInput) > 0
    BEGIN

        SET @String      = LEFT(@StringInput, 
                                ISNULL(NULLIF(CHARINDEX(',', @StringInput) - 1, -1),
                                LEN(@StringInput)))

        SET @StringInput = SUBSTRING(@StringInput,
                                     ISNULL(NULLIF(CHARINDEX(',', @StringInput), 0),
                                     LEN(@StringInput)) + 1, LEN(@StringInput))

        INSERT INTO @ReturnTable ( ReturnValue )

        VALUES ( @String )

    END
    
    RETURN
END
