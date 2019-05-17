/* CS02PExam_GetOrderPriority */
CREATE OR ALTER PROCEDURE CS02PExam_GetOrderPriority
	@id	INT	= NULL
AS
BEGIN
	SELECT [priority]
	FROM CS02PExam_Order
	WHERE id = @id
	ORDER BY [priority], id ASC
END
GO