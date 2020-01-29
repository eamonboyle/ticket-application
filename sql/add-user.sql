INSERT INTO [dbo].[Company] (
	[name]
	,[address1]
	,city
	,postcode
) VALUES (
	'WatchTech'
	,'1 Lagan House'
	,'Lisburn'
	,'BT86712'
)

INSERT INTO [dbo].[Role] (title) VALUES ('Administrator')

INSERT INTO [dbo].[RoleLevel] (level) VALUES (1)

INSERT INTO [dbo].[User] (
	[first_name]
	,[last_name]
	,[email]
	,[phone_number]
	,[mobile_number]
	,[role]
	,[role_level]
	,[username]
	,[password]
	,[salt]
	,[enabled]
	,[date_created]
) VALUES (
	'Eamon'
	,'Boyle'
	,'eamon.boyle@watchtech.co.uk'
	,'07777777777'
	,'07777777777'
	,1
	,1
	,'eamon'
	,'$s2$16384$8$1$Dm1n97S+o5ilfVnXRXfb+iU/Hch1dSYvtqS/9TtebIk=$PA8egI1Owk4j1AijbR+BYYjMxJNLDz05sOOeAwl0fUQ='
	,'dP1JRfwMMivXsmtvdUuZfg=='
	,1
	,GETDATE()
)