use tempdb
go
drop database dbtools
go
create database dbtools
go
use dbtools
go
drop table comment
drop table reservationTool
drop table reservation
drop table employee
drop table location
drop table tool
go
drop function getToolsCurrentLocation
go
drop proc replaceTool
go
drop proc toolStats
go
create table employee
(
id int identity primary key nonclustered,
name varchar(50) not null,
initials varchar(5) not null,
password char(8) not null,
hiredate date not null,
is_active bit default 1 not null,
constraint pass_length_is_valid  check (datalength(password) = 8),
constraint name_has_value check (datalength(name) > 0 ),
constraint initials_has_value check (datalength(initials) > 0 )
);

go
create clustered index emp_is_active on employee(is_active)
create nonclustered index emp_password on employee(password)
go

create table tool
(
id int identity primary key nonclustered,
description varchar(50) not null,
buydate date not null,
in_use bit default 1 not null
);

go
create clustered index tool_in_use on tool(in_use)
create nonclustered index tool_buydate on tool(buydate)
go

create table comment
(
id int identity primary key nonclustered,
made_on datetime not null,
comment varchar(250) not null,
toolid int foreign key references tool(id) on delete cascade,
employeeid int foreign key references employee(id) on delete cascade
);

go
create clustered index comment_employeeid on comment(employeeid)
create nonclustered index comment_toolid on commment(toolid)
create nonclustered index comment_made_on on comment(made_on)
go

create table location
(
id int identity primary key nonclustered,
location varchar(50) not null unique,
is_active bit default 1 not null
);

go
create clustered index location_is_active on location(is_active)
go

create table reservation
(
id int identity primary key nonclustered,
startdate datetime not null,
enddate datetime not null,
employeeid int null foreign key references employee(id) on delete cascade,
locationid int null foreign key references location(id),
constraint is_date_valid check (enddate > startdate)
);

go
create clustered index reservation_enddate on reservation(enddate)
create nonclustered index reservation_startdate on reservation(startdate)
create nonclustered index reservation_location on reservation(location)
go

create table reservationTool
(
id int identity primary key nonclustered,
toolid int foreign key references tool(id) on delete cascade,
reservationid int foreign key references reservation(id) on delete cascade
)

go
create clustered index reservationTool_reservationid on reservationTool(reservationid)
create nonclustered index reservationTool_toolid on reservationTool(toolid)
go

create trigger delete_on_tool
on tool
for delete
as
delete from reservation where id = (
select reservation.id
from reservation left join reservationTool on reservation.id = reservationTool.reservationid
where toolid is null)

go

-- Krav D3
create proc replaceTool
@ToolId int
as
insert into tool values ('Replaces ID: ' + CONVERT(varchar,@ToolId), GETDATE(), 1) -- Nyt tool

-- Updater RT tabel de steder hvor startdato er i fremtiden som har det gamle ID
update reservationTool set toolid = @@IDENTITY where id in (
	select rt.id from reservation r join reservationTool rt on r.id = rt.reservationid
	where r.startdate > GETDATE() and rt.toolid = @ToolId)

go

create function getToolsCurrentLocation()
returns @result table
(
ID int,
Tool varchar(50),
Location varchar(50)
)
as
begin
declare @allToolReservations table
(
ID int,
Tool varchar(50),
Location varchar(50),
startdate datetime,
enddate datetime
)
-- Samtlige reservationer
insert into @allToolReservations 

	select t.id, t.description, l.location, r.startdate, r.enddate
	FROM tool t LEFT JOIN reservationTool rt ON t.id = rt.toolid 
	LEFT JOIN reservation r ON rt.reservationid = r.id 
	LEFT JOIN location l ON r.locationid = l.id
	where t.in_use = 1
			
insert into @result
	-- Alle tools med reservationer lige nu
	select at.ID, at.Tool, ISNULL(at.Location,'In for service')
	from @allToolReservations at
	where at.startdate <= GETDATE() AND at.enddate >= GETDATE()

insert into @result
	-- Alle tools med fremtidige/tidligere reservationer
	select at.ID, at.Tool, 'Warehouse'
	from @allToolReservations at
	where (at.startdate > GETDATE() OR at.enddate < GETDATE()) AND at.ID NOT IN(select ID from @result)
    
insert into @result
	-- Alle tools uden reservationer
	select at.ID, at.Tool, 'Warehouse'
	from @allToolReservations at
	where at.startdate is null

return
end

go

-- Krav D2
create proc toolStats
@start date
as

declare @end date 
set @end = DATEADD(month,1,@start)

print 'Tool use from: ' + convert(varchar,@start) + ' to ' + convert(varchar,@end)
print '---------------------------------------'

declare p cursor
for select id, description from tool
declare @id int, @description varchar(50)
open p
fetch p into @id,@description
while @@fetch_status != -1
begin
  
	declare @hoursUsed int = 0
  
	declare p2 cursor
	for select resStart, resEnd from 
	
	(select startdate as resStart, enddate as resEnd
	from reservation r join reservationTool rt on r.id = rt.reservationid
							join tool t on rt.toolid = t.id
							where t.id = @id and t.in_use = 1
											 and ((r.startdate > @start and r.startdate < @end)
																	or 
												 (r.startdate < @start and r.enddate > @end)
												                    or
												 (r.enddate > @start and r.enddate < @end))
											 ) as res
	
	declare @resStart datetime,@resEnd datetime
	open p2
	fetch p2 into @resStart,@resEnd
	while @@fetch_status != -1
	begin
	  
	-- En reservering dækker hele måneden
	if @resStart < @start and @resEnd > @end
		BEGIN
			set @hoursUsed = @hoursUsed + 22*9
			declare @MonthEnd datetime = DATEADD(DAY,1,@end)
			while DATEPART(DY,@MonthEnd) < DATEPART(DY,@resEnd)
				BEGIN	
					IF DATENAME(DW,@MonthEnd) <> 'SATURDAY' and DATENAME(DW,@MonthEnd) <> 'SUNDAY'
						set @hoursUsed = @hoursUsed + 9	
					set @MonthEnd = DATEADD(DAY,1,@MonthEnd)
				END
			-- Læg timer på slutdagen til
			set @hoursUsed = @hoursUsed + DATEPART(HOUR,@resEnd) - 7
		END
		
	-- En reservering starter og slutter samme dag
	if DATEDIFF(DY,@resStart,@resEnd) < 1
		set @hoursUsed = @hoursUsed + DATEDIFF(HOUR,@resStart,@resEnd)

	-- En reservering starter i forrige måned og slutter i denne
	if @resStart < @start and @resEnd <= @end
		BEGIN	
			declare @MonthStart datetime = @start
			while DATEPART(DY,@MonthStart) < DATEPART(DY,@resEnd)
				BEGIN	
					IF DATENAME(DW,@MonthStart) <> 'SATURDAY' and DATENAME(DW,@MonthStart) <> 'SUNDAY'
						set @hoursUsed = @hoursUsed + 9	
					set @MonthStart = DATEADD(DAY,1,@MonthStart)
				END
			-- Læg timer på slutdagen til
			set @hoursUsed = @hoursUsed + DATEPART(HOUR,@resEnd) - 7
		END
		
	-- En reservering starter i denne måned og slutter i samme eller næste
	if @resStart >= @start and DATEPART(DY,@resStart) <> DATEPART(DY,@resEnd)
		BEGIN	
			declare @resStartTemp datetime = DATEADD(DAY,1,@resStart)
			while DATEPART(DY,@resStartTemp) < DATEPART(DY,@resEnd)
				BEGIN	
					IF DATENAME(DW,@resStartTemp) <> 'SATURDAY' and DATENAME(DW,@resStartTemp) <> 'SUNDAY'
						set @hoursUsed = @hoursUsed + 9 
					set @resStartTemp = DATEADD(DAY,1,@resStartTemp)
				END
			-- Læg timer på start og slutdag til
			set @hoursUsed = @hoursUsed + 16 - DATEPART(HOUR,@resStart)
			set @hoursUsed = @hoursUsed + DATEPART(HOUR,@resEnd) - 7
		END
		
	fetch p2 into @resStart,@resEnd
	end
	close p2
	deallocate p2
	
	print convert(varchar,(@hoursUsed*100) /(22*9)) + '% ID: ' + convert(varchar,@id)+ ': ' + @description

fetch p into @id,@description
end
close p
deallocate p

go
-- exec toolStats '2012-05-31'
go
-- Giver af og til problemer hvis ikke denne er på
alter database dbtools
set ALLOW_snapshot_isolation on
go

-- US english format på tider
insert into employee values
('Allan West','AW','theshark','2012-01-24',1),
('Michael Keaton','MC','MK123456','2011-02-01',1),
('Kevin Conroy','KC','KC123456','2012-01-14',1),
('Christian Bale','CB','CB123456','2008-02-20',0)

insert into tool values
('Dims XT no.78','2010-05-05',1),
('Cutter CXT 227','2003-05-06',1),
('Tingest 8','2008-02-04',0),
('Drill X1','2005-05-05',1),
('Crane XPY No.5','1998-05-15',1),
('Lathe ER 1','2007-06-17',1),
('Autohammer PP2','2010-03-21',0),
('Truck T37','1995-05-05',1),
('Battery Charger X5', '2002-05-06',1)

insert into location values
('Ford Mondeo #2',1),
('XL-Byg, Randers',1),
('Erhvervsakademiet',1),
('Kurts skurvogn',0)

go