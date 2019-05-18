CREATE OR ALTER PROCEDURE CS02PExam_CreateAssignment
	@order			INT				= NULL,
	@workform		INT				= NULL,
	@duration		INT				= NULL
AS
BEGIN
	INSERT INTO CS02PExam_Assignment ([order], workform, duration)
	OUTPUT Inserted.id
	VALUES
	(@order, @workform, @duration)
	
	UPDATE CS02PExam_Assignment
	SET [priority] = SCOPE_IDENTITY()
	WHERE id = SCOPE_IDENTITY()
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_GetAllAssignmentsByOrder
	@order			INT
AS
BEGIN
	SELECT id, [priority], [order], workform, duration
	FROM CS02PExam_Assignment
	WHERE [order] = @order
	ORDER BY [priority], id ASC
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_DeleteAssignment
	@id				INT
AS
BEGIN
	DELETE FROM CS02PExam_Assignment
	WHERE id = @id
END
GO