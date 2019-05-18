CREATE OR ALTER PROCEDURE CS02PExam_CreateOffday
	@workteam		INT				= NULL,
	@reason			INT				= NULL,
	@startDate		DATETIME2		= NULL,
	@duration		INT				= NULL
AS
BEGIN
	INSERT INTO CS02PExam_Offday (workteam, reason, startDate, duration)
	OUTPUT Inserted.id
	VALUES
	(@workteam, @reason, @startDate, @duration)
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_GetAllOffdaysByWorkteam
	@workteam	INT
AS
BEGIN
	SELECT id, workteam, reason, startDate, duration
	FROM CS02PExam_Offday
	WHERE workteam = @workteam
	ORDER BY id ASC
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_DeleteOffday
	@id	INT
AS
BEGIN
	DELETE FROM CS02PExam_Offday
	WHERE id = @id
END
GO