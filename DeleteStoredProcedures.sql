/* CS02PExam_CreateWorkteam */
CREATE OR ALTER PROCEDURE CS02PExam_CreateWorkteam
	@foreman		NVARCHAR(64)
AS
BEGIN
	INSERT INTO CS02PExam_Workteam (foreman)
	OUTPUT Inserted.id
	VALUES
	(@foreman)
END
GO

/* CS02PExam_CreateOffday */
CREATE OR ALTER PROCEDURE CS02PExam_CreateOffday
	@workteam		INT,
	@reason			INT,
	@startDate		DATETIME2,
	@duration		INT
AS
BEGIN
	INSERT INTO CS02PExam_Offday (workteam, reason, startDate, duration)
	OUTPUT Inserted.id
	VALUES
	(@workteam, @reason, @startDate, @duration)
END
GO

/* CS02PExam_CreateOrder */
CREATE OR ALTER PROCEDURE CS02PExam_CreateOrder
	@workteam		INT,
	@orderNumber	INT,
	@street			NVARCHAR(128),
	@remark			NVARCHAR(128),
	@area			INT,
	@amount			INT,
	@prescription	NVARCHAR(128),
	@deadline		DATETIME2,
	@startDate		DATETIME2,
	@customer		NVARCHAR(64),
	@machine		NVARCHAR(64),
	@asphaltWork	NVARCHAR(64)
AS
BEGIN
	INSERT INTO CS02PExam_Order (workteam, orderNumber, street, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork)
	OUTPUT Inserted.id
	VALUES
	(@workteam, @orderNumber, @street, @remark, @area, @amount, @prescription, @deadline, @startDate, @customer, @machine, @asphaltWork)
	
	UPDATE CS02PExam_Order
	SET [priority] = SCOPE_IDENTITY()
	WHERE id = SCOPE_IDENTITY()
END
GO

/* CS02PExam_CreateAssignment */
CREATE OR ALTER PROCEDURE CS02PExam_CreateAssignment
	@order			INT,
	@workform		INT,
	@duration		INT
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

/* CS02PExam_GetAllWorkteams */
CREATE OR ALTER PROCEDURE CS02PExam_GetAllWorkteams
AS
BEGIN
	SELECT id, foreman
	FROM CS02PExam_Workteam
END
GO