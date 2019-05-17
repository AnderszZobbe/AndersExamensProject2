/* CS02PExam_DeleteWorkteam */
CREATE OR ALTER PROCEDURE CS02PExam_DeleteWorkteam
	@id	INT
AS
BEGIN
	DELETE FROM CS02PExam_Workteam
	WHERE id = @id
END
GO

/* CS02PExam_DeleteOffday */
CREATE OR ALTER PROCEDURE CS02PExam_DeleteOffday
	@id	INT
AS
BEGIN
	DELETE FROM CS02PExam_Offday
	WHERE id = @id
END
GO

/* CS02PExam_DeleteOrder */
CREATE OR ALTER PROCEDURE CS02PExam_DeleteOrder
	@id	INT
AS
BEGIN
	DELETE FROM CS02PExam_Order
	WHERE id = @id
END
GO

/* CS02PExam_DeleteAssignment */
CREATE OR ALTER PROCEDURE CS02PExam_DeleteAssignment
	@id	INT
AS
BEGIN
	DELETE FROM CS02PExam_Assignment
	WHERE id = @id
END
GO