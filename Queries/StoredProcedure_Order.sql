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

CREATE OR ALTER PROCEDURE CS02PExam_GetAllOrdersByWorkteam
	@workteam			INT
AS
BEGIN
	SELECT id, orderNumber, [address], remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork
	FROM CS02PExam_Order
	WHERE workteam = @workteam
	ORDER BY [priority], id ASC
END
GO

/*CREATE OR ALTER PROCEDURE CS02PExam_SwapOrderPriorities
	@firstOrder		INT				= NULL,
	@secondOrder	INT				= NULL
AS
BEGIN
	
END
GO*/

CREATE OR ALTER PROCEDURE CS02PExam_GetOrderPriority
	@id		INT				= NULL
AS
BEGIN
	SELECT [priority]
	FROM CS02PExam_Order
	WHERE id = @id
	ORDER BY [priority], id ASC
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_SetOrderPriority
	@id				INT				= NULL,
	@priority		INT				= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET [priority] = @priority
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderOrderNumber
	@id				INT				= NULL,
	@orderNumber	INT				= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET orderNumber = @orderNumber
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderAddress
	@id				INT				= NULL,
	@address		NVARCHAR(128)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET [address] = @address
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderRemark
	@id				INT				= NULL,
	@remark			NVARCHAR(128)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET remark = @remark
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderArea
	@id				INT				= NULL,
	@area			INT				= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET area = @area
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderAmount
	@id				INT				= NULL,
	@amount			INT				= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET amount = @amount
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderPrescription
	@id				INT				= NULL,
	@prescription	NVARCHAR(128)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET prescription = @prescription
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderDeadline
	@id				INT				= NULL,
	@deadline		DATETIME2		= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET deadline = @deadline
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderStartDate
	@id				INT				= NULL,
	@startDate		DATETIME2		= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET startDate = @startDate
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderCustomer
	@id				INT				= NULL,
	@customer		NVARCHAR(64)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET customer = @customer
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderMachine
	@id				INT				= NULL,
	@machine		NVARCHAR(64)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET machine = @machine
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_UpdateOrderAsphaltWork
	@id				INT				= NULL,
	@asphaltWork	NVARCHAR(64)	= NULL
AS
BEGIN
	UPDATE CS02PExam_Order
	SET asphaltWork = @asphaltWork
	WHERE id = @id
END
GO

CREATE OR ALTER PROCEDURE CS02PExam_DeleteOrder
	@id	INT
AS
BEGIN
	DELETE FROM CS02PExam_Order
	WHERE id = @id
END
GO