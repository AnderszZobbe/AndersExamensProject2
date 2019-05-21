CREATE OR ALTER PROCEDURE CS02PExam_CreateWorkteam
	@foreman		NVARCHAR(64)	= NULL
AS
BEGIN
	INSERT INTO CS02PExam_Workteam (foreman)
	OUTPUT Inserted.id
	VALUES
	(@foreman)
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_GetAllWorkteams
AS
BEGIN
	SELECT id, foreman
	FROM CS02PExam_Workteam
	ORDER BY id ASC
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateWorkteamForeman
	@id			INT				= NULL,
	@foreman	NVARCHAR(64)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Workteam
	SET foreman = @foreman
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE DeleteWorkteam
	@id	INT
AS
BEGIN
	DELETE FROM CS02PExam_Workteam
	WHERE id = @id
END
GO