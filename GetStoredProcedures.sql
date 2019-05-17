/* CS02PExam_GetOrderPriority */
CREATE OR ALTER PROCEDURE CS02PExam_GetOrderPriority
AS
BEGIN
	SELECT [priority]
	FROM CS02PExam_Order
	ORDER BY [priority], id ASC
END
GO