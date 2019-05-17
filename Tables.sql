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
	[priority]		INT										UNIQUE,
	workteam		INT				NOT NULL,
	orderNumber		INT,
	street			NVARCHAR(128),
	remark			NVARCHAR(128),
	area			INT,
	amount			INT,
	prescription	NVARCHAR(128),
	deadline		DATETIME2,
	startDate		DATETIME2,
	customer		NVARCHAR(64),
	machine			NVARCHAR(64),
	asphaltWork		NVARCHAR(64),

	CONSTRAINT FK_CS02PExam_Order_Workteam FOREIGN KEY (workteam)
		REFERENCES CS02PExam_Workteam (id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
);

CREATE TABLE CS02PExam_Assignment (
	id				INT				NOT NULL	IDENTITY	PRIMARY KEY,
	[priority]		INT										UNIQUE,
	[order]			INT				NOT NULL,
	workform		INT				NOT NULL,
	duration		INT				NOT NULL,

	CONSTRAINT FK_CS02PExam_Assignment_Order FOREIGN KEY ([order])
		REFERENCES CS02PExam_Workteam (id)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
);