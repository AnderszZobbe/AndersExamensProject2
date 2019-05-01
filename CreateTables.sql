CREATE TABLE CS02PExam_Workteam (
	id		int				NOT NULL	IDENTITY	primary key,
	foreman	nvarchar(64)	NOT NULL				UNIQUE
);