DROP TABLE IF EXISTS CS02PExam_Assignment;
DROP TABLE IF EXISTS CS02PExam_Order;
DROP TABLE IF EXISTS CS02PExam_Offday;
DROP TABLE IF EXISTS CS02PExam_Workteam;

CREATE TABLE CS02PExam_Workteam (
	id				INT				NOT NULL	IDENTITY	PRIMARY KEY,
	foreman			NVARCHAR(64)	NOT NULL				UNIQUE,
);

CREATE TABLE CS02PExam_Offday (
	id				INT				NOT NULL	IDENTITY	PRIMARY KEY,
	workteam		INT				NOT NULL,
	reason			INT				NOT NULL,
	startDate		DATETIME2		NOT NULL,
	duration		INT				NOT NULL,

	CONSTRAINT FK_CS02PExam_Offday_Workteam FOREIGN KEY (workteam)
		REFERENCES CS02PExam_Workteam (id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
);

CREATE TABLE CS02PExam_Order (
	id				INT				NOT NULL	IDENTITY	PRIMARY KEY,
	[priority]		INT				NULL,
	workteam		INT				NOT NULL,
	orderNumber		INT				NULL,
	[address]		NVARCHAR(128)	NULL,
	remark			NVARCHAR(128)	NULL,
	area			INT				NULL,
	amount			INT				NULL,
	prescription	NVARCHAR(128)	NULL,
	deadline		DATETIME2		NULL,
	startDate		DATETIME2		NULL,
	customer		NVARCHAR(64)	NULL,
	machine			NVARCHAR(64)	NULL,
	asphaltWork		NVARCHAR(64)	NULL,

	CONSTRAINT FK_CS02PExam_Order_Workteam FOREIGN KEY (workteam)
		REFERENCES CS02PExam_Workteam (id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
);

CREATE TABLE CS02PExam_Assignment (
	id				INT				NOT NULL	IDENTITY	PRIMARY KEY,
	[priority]		INT				NULL,
	[order]			INT				NOT NULL,
	workform		INT				NOT NULL,
	duration		INT				NOT NULL,

	CONSTRAINT FK_CS02PExam_Assignment_Order FOREIGN KEY ([order])
		REFERENCES CS02PExam_Order (id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
);