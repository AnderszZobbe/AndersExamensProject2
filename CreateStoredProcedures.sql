/* CS02PExam_GetAllWorkteams */
CREATE OR ALTER PROCEDURE CS02PExam_GetAllWorkteams
AS
BEGIN
	SELECT id, foreman
	FROM CS02PExam_Workteam
END
GO

/* CS02PExam_CreateWorkteam */
CREATE OR ALTER PROCEDURE CS02PExam_CreateWorkteam
	@foreman	nvarchar(64)
AS
BEGIN
	INSERT INTO CS02PExam_Workteam (foreman)
	OUTPUT Inserted.id
	VALUES
	(@foreman)
END
GO