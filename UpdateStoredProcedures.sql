/* CS02PExam_UpdateWorkteam */
CREATE OR ALTER PROCEDURE CS02PExam_UpdateWorkteam
	@id			INT				= NULL,
	@foreman	NVARCHAR(64)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Workteam
	SET foreman = @foreman
	WHERE id = @id
END
GO

/* CS02PExam_UpdateOrderStartDate */
CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderStartDate
	@id			INT				= NULL,
	@startDate	DATETIME2		= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET startDate = @startDate
	WHERE id = @id
END
GO

/* CS02PExam_UpdateOrderPriority */
CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderPriority
	@id			INT				= NULL,
	@priority	INT				= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET [priority] = @priority
	WHERE id = @id
END
GO

/* CS02PExam_UpdateOrder */
CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrder
	@id				INT				= NULL,
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
	UPDATE CS02PExam_Order
	SET orderNumber = @orderNumber,
	[address] = @address,
	remark = @remark,
	area = @area,
	amount = @amount,
	prescription = @prescription,
	deadline = @deadline,
	startDate = @startDate,
	customer = @customer,
	machine = @machine,
	asphaltWork = @asphaltWork
	WHERE id = @id
END
GO