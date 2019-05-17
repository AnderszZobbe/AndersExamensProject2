/* CS02PExam_GetAllWorkteams */
CREATE OR ALTER PROCEDURE CS02PExam_GetAllWorkteams
AS
BEGIN
	SELECT id, foreman
	FROM CS02PExam_Workteam
	ORDER BY id ASC
END
GO

/* CS02PExam_GetAllOffdays */
CREATE OR ALTER PROCEDURE CS02PExam_GetAllOffdays
AS
BEGIN
	SELECT id, workteam, reason, startDate, duration
	FROM CS02PExam_Offday
	ORDER BY id ASC
END
GO

/* CS02PExam_GetAllOrders */
CREATE OR ALTER PROCEDURE CS02PExam_GetAllOrders
AS
BEGIN
	SELECT id, [priority], workteam, orderNumber, street, remark, area, amount, prescription, deadline, startDate, customer, machine, asphaltWork
	FROM CS02PExam_Order
	ORDER BY [priority], id ASC
END
GO

/* CS02PExam_GetAllAssignments */
CREATE OR ALTER PROCEDURE CS02PExam_GetAllAssignments
AS
BEGIN
	SELECT id, [priority], [order], workform, duration
	FROM CS02PExam_Assignment
	ORDER BY [priority], id ASC
END
GO