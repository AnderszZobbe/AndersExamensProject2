/* CS02PExam_UpdateWorkteam */
CREATE OR ALTER PROCEDURE CS02PExam_UpdateWorkteam
	@id			INT,
	@foreman	NVARCHAR(64)
AS
BEGIN
	UPDATE CS02PExam_Workteam
	SET foreman = @foreman
	WHERE id = @id
END
GO

/* CS02PExam_UpdateOrderStartDate */
CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderStartDate
	@id			INT,
	@startDate	DATETIME2
AS
BEGIN
	UPDATE CS02PExam_Order
	SET startDate = @startDate
	WHERE id = @id
END
GO

/* CS02PExam_UpdateOrderPriority */
CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderPriority
	@id			INT,
	@priority	INT
AS
BEGIN
	UPDATE CS02PExam_Order
	SET [priority] = @priority
	WHERE id = @id
END
GO