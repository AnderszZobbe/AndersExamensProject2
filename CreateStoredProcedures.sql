/* CS02PExam_CreateWorkteam */
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

/* CS02PExam_CreateOffday */
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

/* CS02PExam_CreateOrder */
CREATE OR ALTER PROCEDURE CS02PExam_CreateOrder
	@workteam		INT				= NULL,
	@orderNumber	INT				= NULL,
	@address		NVARCHAR(128)	= NULL,
	@remark			NVARCHAR(128)	= NULL,
	@area			INT				= NULL,
	@amount			INT				= NULL,
	@prescription	NVARCHAR(128)	= NULL,
	@deadline		DATETIME2		= NULL,
	@startDate		DATETIME2		= NULL,
	@customer		NVARCHAR(64)	= NULL,
	@machine		NVARCHAR(64)	= NULL,
	@asphaltWork	NVARCHAR(64)	= NULL
AS
BEGIN
	INSERT INTO CS02PExam_Order (workteam, orderNumber, [address], remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork)
	OUTPUT Inserted.id
	VALUES
	(@workteam, @orderNumber, @address, @remark, @area, @amount, @prescription, @deadline, @startDate, @customer, @machine, @asphaltWork)
	
	UPDATE CS02PExam_Order
	SET [priority] = SCOPE_IDENTITY()
	WHERE id = SCOPE_IDENTITY()
END
GO

/* CS02PExam_CreateAssignment */
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