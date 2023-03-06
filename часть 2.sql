create table "Clients"
(
	Id bigserial, 
	ClientName varchar(200)
);

create table  "ClientContacts" 
(
Id bigserial, -- Id контакта
ClientId bigint, -- Id клиента
ContactType varchar(255), -- тип контакта
ContactValue varchar(255) -- значение контакта
);


select c.clientname,count(cr.id) from "Clients" c
join "ClientContacts" cr on cr.ClientId = c.Id
group by c.clientname

select c.id, c.clientname from "Clients" c
join "ClientContacts" cr on cr.ClientId = c.Id
group by c.Id, c.clientname
having count(cr.id)>2