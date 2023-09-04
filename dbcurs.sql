

--Создание базы данных--
create database "HiTech";

--Создание ролей--
create role role_root;
create role role_user;
create role role_manager;
create role role_guest;
--Создание пользователей--
create user root_user1 password 'root';
grant role_root to root_user1;

create user user1 password '12345';
grant role_user to user1;

create user manager_user1 password '12345';
grant role_manager to manager_user1;

create user guest_user1 password '12345';
grant role_guest to guest_user1;
--Выдача привелегий role_guest--
GRANT EXECUTE ON FUNCTION  search_product_by_name TO role_guest;
GRANT EXECUTE ON FUNCTION  search_product_by_category TO role_guest;
GRANT EXECUTE ON FUNCTION REGISTRATION TO role_guest;
GRANT insert,select ON TABLE Users to role_guest;
GRANT insert,select ON TABLE Basket to role_guest;
select * from Users;
grant role_guest to guest_user1;
GRANT USAGE, SELECT ON SEQUENCE users_user_id_seq TO role_guest;
GRANT USAGE, SELECT ON SEQUENCE users_basket_id_seq TO role_guest;

GRANT EXECUTE ON FUNCTION FUNC_FOR_TR_DATE TO role_guest;

--Выдача привелегий role_root--
grant connect on database "HiTech" to role_root;
GRANT ALL ON SCHEMA public TO role_root;
grant all privileges on database "HiTech" to role_root;
grant all privileges on tablespace TS_USER to role_root;
grant all privileges on tablespace TS_PRODUCT to role_root;
select * from users;
--Выдача привелегий role_user--
GRANT EXECUTE ON FUNCTION REGISTRATION TO role_user;
GRANT EXECUTE ON FUNCTION signin TO role_user;
GRANT EXECUTE ON PROCEDURE add_rating TO role_user;
GRANT EXECUTE ON PROCEDURE ADD_PRODUCT_TO_BASKET TO role_user;
GRANT EXECUTE ON PROCEDURE UPDATE_USER TO role_user;
GRANT EXECUTE ON PROCEDURE update_password TO role_user;
GRANT EXECUTE ON PROCEDURE delete_from_basket TO role_user;
GRANT EXECUTE ON PROCEDURE ADD_COMMENT TO role_user;

GRANT EXECUTE ON FUNCTION  search_product_by_name TO role_user;
GRANT EXECUTE ON FUNCTION  search_product_by_category TO role_user;
GRANT EXECUTE ON FUNCTION  search_user_by_name TO role_user;
GRANT EXECUTE ON FUNCTION  CHECK_BASKET_FUNC TO role_user;
grant select on  PRODUCT_INFO TO role_user;
grant select on  Users TO role_user;

GRANT SELECT, INSERT, UPDATE ON TABLE Users TO role_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Rating TO role_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Rating TO role_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Basket TO role_user;

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Commentaries TO role_user;
GRANT SELECT ON TABLE PRODUCT TO role_user;
GRANT USAGE, SELECT ON SEQUENCE basket_basket_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE basket_product_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE rating_rating_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE rating_user_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE rating_product_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE rating_product_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE users_user_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE users_user_role_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE users_basket_id_seq TO role_user;
GRANT USAGE, SELECT ON SEQUENCE commentaries_comment_id_seq TO role_user;

-----role_manager-----
GRANT EXECUTE ON FUNCTION add_product TO role_manager;
GRANT EXECUTE ON PROCEDURE update_product TO role_manager;
GRANT EXECUTE ON PROCEDURE delete_product TO role_manager;
grant select on  PRODUCT_INFO TO role_manager;

GRANT EXECUTE ON FUNCTION REGISTRATION TO role_manager;
GRANT EXECUTE ON PROCEDURE  delete_product TO role_manager;	
GRANT EXECUTE ON FUNCTION  add_product TO role_manager;	

GRANT EXECUTE ON PROCEDURE update_user TO role_manager;
GRANT EXECUTE ON PROCEDURE update_password TO role_manager;
GRANT EXECUTE ON PROCEDURE delete_user TO role_manager;
GRANT SELECT, INSERT, UPDATE ON TABLE Users TO role_manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Product TO role_manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Basket TO role_manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Category TO role_manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Commentaries TO role_manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE Rating TO role_manager;

GRANT USAGE, SELECT ON SEQUENCE users_user_id_seq TO role_manager;
GRANT USAGE, SELECT ON SEQUENCE product_product_id_seq TO role_manager;


--Создание табличных пространств--
create tablespace TS_USER 
location 'D:\MYDOCUMENTS\BGTU\DB_KURS\HiTech\Tablespaces\TS_USER';
create tablespace TS_PRODUCT 
location 'D:\MYDOCUMENTS\BGTU\DB_KURS\HiTech\Tablespaces\TS_PRODUCT';

--Создание таблиц--
alter table Users alter column USER_DATE type date
using to_date(user_date,'YYYY-MM-DD');
create table Users(
user_id serial primary key,
	user_role serial,
	user_name varchar(30) not null,
	basket_id serial,
	user_password varchar(100) not null,
	user_mail varchar(100) unique not null,
	user_date varchar(100) not null,
	constraint FK_USERS_BASKET foreign key (basket_id) references Basket(basket_id),
	constraint FK_USERS_ROLES foreign key (user_role) references Roles(role_id)
) tablespace TS_USER;
drop table Users cascade;
drop table Basket cascade;



create table Basket(
	basket_id serial ,
	product_id serial  ,
	counts integer,
		constraint FK_BASKET_PRODUCT foreign key (product_id) references Product(product_id) on delete set null,
	CONSTRAINT FK_BASKET_USER FOREIGN KEY (basket_id) REFERENCES Users (user_id) on delete  set null
) tablespace TS_USER;

drop table Basket;
create table Roles(
	role_id serial primary key,
	role_name varchar(100) 
) tablespace TS_USER;
insert into Roles values(1,'root_role');
insert into Roles values(2,'mamager_role');
insert into Roles values(3,'user_role');
ALTER TABLE Basket ADD CONSTRAINT FK_BASKET_USER FOREIGN KEY (basket_id) REFERENCES Users (user_id) on delete  set null;

create table Commentaries(comment_id serial primary key,
						 user_id integer,
						  product_id integer ,
						  comment_text varchar(300),
                          constraint FK_COMM_PRODUCT foreign key (product_id) references Product(product_id) on delete set null,
						  constraint FK_COMM_USER foreign key (user_id) references Users(user_id) 
						 ) tablespace TS_PRODUCT;
 
drop table commentaries cascade
create table Product(
product_id serial primary key,
	category_id serial,
	product_name varchar(100) unique not null,
	product_price serial not null,
	product_img varchar(100),
	product_color varchar(100) not null,
	product_date date not null,
	product_country varchar(100) not null,
	product_description varchar(200) not null,
	product_comments varchar(100)
	constraint FK_PRODUCT_CATEGORY foreign key (category_id) references Category(category_id)
) tablespace TS_PRODUCT;



create index INDEX_PRODUCT_NAME_LOWER on Product(product_name);
drop index INDEX_TSV_APHORISM;
create index if not exists INDEX_TSV_APHORISM ON Product USING gin(text_tsv);
 drop function SEARCH_PRODUCT_BY_NAME;
  explain analyze select SEARCH_PRODUCT_BY_NAME('am');
  EXPLAIN ANALYZE SELECT Category_id FROM category WHERE Category_id = 5;
  create index catname on category(category_id);
EXPLAIN ANALYZE SELECT product_name FROM Product WHERE lower(product_name) ILIKE '%Samsung%';



create table Category(
category_id serial primary key,
	category_name varchar(50) not null
) tablespace TS_PRODUCT;
insert into  Category values(1,'Смартфон');
insert into  Category values(2,'Часы');
insert into  Category values(3,'Планшет');
insert into  Category values(4,'Браслет');
ALTER TABLE Product ADD COLUMN text_tsv tsvector;
UPDATE Product SET text_tsv = to_tsvector('russian', product_name) 
|| to_tsvector('russian', product_description);
select text_tsv from Product;
select * from Category;


create table Rating(
rating_id serial primary key,
	user_id serial not_null,
	product_id serial not null,
	rate integer not null,
	constraint FK_RATING_USER foreign key (user_id) references Users(user_id),
	constraint FK_RATING_PRODUCT foreign key (product_id) references Product(product_id)
) tablespace TS_PRODUCT;

--Создание функции регистрации--
CREATE OR REPLACE FUNCTION REGISTRATION(
    user_name_arg varchar(30),
    user_mail_arg varchar(100),
    user_password_arg varchar(100),
    user_date_arg date
     
)
RETURNS integer

AS $$
DECLARE
    USER_ID INTEGER;
BEGIN
    INSERT INTO Users(user_name , user_mail , user_password, user_date , user_role)
    VALUES (user_name_arg, user_mail_arg, user_password_arg, user_date_arg, 3)
    RETURNING Users.user_id,Users.basket_id  INTO USER_ID ;
		INSERT INTO Basket(basket_id, product_id) values(USER_ID, 5);
    RETURN USER_ID ;	
END;
$$ LANGUAGE PLPGSQL;
drop function REGISTRATION;
SELECT REGISTRATION('administrator', 'administrator@gmail.com', 'administrator2005', '2005-03-14'); --триггеры пароля и даты
delete from Basket;
select * from Commentaries ;





--Создание функции входа--
CREATE OR REPLACE FUNCTION SIGNIN(user_mail_arg VARCHAR(100), user_password_arg VARCHAR(100))
RETURNS integer
AS $$
DECLARE
role_id varchar(100) = (	select user_role from Users where user_mail = user_mail_arg);

passw varchar(100);
BEGIN
    SELECT user_password  INTO passw FROM Users WHERE user_mail   = user_mail_arg ;
    
    IF passw IS NULL THEN
        raise exception 'Почта не верна';
		elseif passw = user_password_arg then 
		return role_id;
		else raise exception ' Пароль не верен';
    END IF;
	
    RETURN role_id;
END;
$$ LANGUAGE PLPGSQL;
drop FUNCTION SIGNIN;
SELECT SIGNIN('manager@gmail.com', 'manager2005');
delete from Users ;
delete from Basket;
select * from users;



--Создание функции добавления товара--
CREATE OR REPLACE FUNCTION ADD_PRODUCT(category_id_arg INTEGER,product_name_arg VARCHAR(100),product_price_arg INTEGER,
product_img_arg VARCHAR(100), product_color_arg VARCHAR(100),product_country_arg VARCHAR(100),product_description_arg varchar(200))
RETURNS INTEGER
AS $$
DECLARE
product_id_add INTEGER;
product_date_arg date;
BEGIN
product_date_arg = current_date;
INSERT INTO Product(category_id,product_name,product_price,product_img ,product_color, product_date, product_country,product_description)
VALUES(category_id_arg,product_name_arg,product_price_arg,product_img_arg,product_color_arg,product_date_arg,product_country_arg,product_description_arg)
RETURNING Product.product_id into product_id_add;
RETURN product_id_add;
UPDATE Product SET text_tsv = to_tsvector('russian', product_name) 
|| to_tsvector('russian', product_description);
END;
$$ LANGUAGE PLPGSQL;
select ADD_PRODUCT(2,'Samsung Galaxy A20',2000,'A20.img','Черный','Южная Корея','Это смартфон');
drop function ADD_PRODUCT;
select * from Product;
alter table Product add constraint



--Создание функции оценивания товара--
CREATE OR REPLACE PROCEDURE ADD_RATING(
    user_id_arg INTEGER,
    product_id_arg INTEGER,
    rate_arg INTEGER
)
LANGUAGE PLPGSQL
AS $$
BEGIN
  
    IF NOT EXISTS (SELECT 1 FROM Users WHERE user_id = user_id_arg) THEN
        RAISE EXCEPTION 'Пользователь с ID % не найден.', user_id_arg;
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM Product WHERE product_id = product_id_arg) THEN
        RAISE EXCEPTION 'Товар с ID % не найден.', product_id_arg;
    END IF;
    

    INSERT INTO Rating(user_id, product_id, rate)
    VALUES (user_id_arg, product_id_arg, rate_arg);
END;
$$;
call ADD_RATING(36,5,15); --сработает триггер

select * from Product where product_name= '1'

--Создание процедуры оставления отзыва товару--
create or replace procedure ADD_COMMENT(user_id_arg integer, product_id_arg integer, comment_text_arg varchar(300))
LANGUAGE PLPGSQL AS $$
begin 
insert into Commentaries(user_id,product_id, comment_text) values
(user_id_arg,product_id_arg,comment_text_arg);
end;
$$;


--Создание функции просмотра отзывов товару--
create or replace function SELECT_COMMENTS( product_id_arg integer) 
returns table(user_name varchar(100),comments_text  varchar(100))
LANGUAGE PLPGSQL AS $$
BEGIN 
RETURN QUERY
SELECT u.user_name, c.comment_text
FROM Users u 
join Commentaries c on u.user_id = c.user_id where c.product_id =product_id_arg  ;
END;
$$;
select * from SELECT_COMMENTS(18);
select * from Commentaries;
insert into Roles values(4,'guest_role');









CREATE OR REPLACE PROCEDURE ADD_MOBILE_TO_BASKET(
mobile_id_arg INTEGER,
client_id_arg int
)
LANGUAGE PLPGSQL
AS $$
declare hh int;
BEGIN
hh = (select count(id_mobile) from Basket where id_mobile = mobile_id_arg);
if hh = 0 then
INSERT INTO Basket
VALUES (mobile_id_arg,client_id_arg,1);
elseif hh > 0 then
update Basket set counter = counter + 1 where id_mobile = mobile_id_arg;
END if;
end;
$$;

select * from basket
--Создание функции добавления товара в корзину--
CREATE OR REPLACE PROCEDURE ADD_PRODUCT_TO_BASKET(
    product_id_arg INTEGER,
    basket_id_arg INTEGER
)
LANGUAGE PLPGSQL
AS $$
declare cnt integer;
BEGIN
cnt = (select count(product_id) from Basket where product_id = product_id_arg);
if cnt =0 then 
INSERT INTO Basket (product_id,basket_id,counts)
    VALUES (product_id_arg,basket_id_arg,1);
	elseif cnt>0 then
	update Basket set counts = counts+1 where product_id = product_id_arg;
END if;
end;
$$;

select * from product
drop PROCEDURE ADD_PRODUCT_TO_BASKET;
call ADD_PRODUCT_TO_BASKET(39,48)
select * from Basket;
delete from Basket;
delete from Users
select * from Users;
delete from Rating;

--Создание функции комментирования товара--
create or replace procedure COMMENT_PRODUCT(product_name_arg varchar(100), product_comments_arg varchar(100)) 
language PLPGSQL as $$
begin 
update Product set product_comments =product_comments_arg where product_name =product_name_arg   ; end;
$$;



--Создание функции просмотра товаров в корзине--
CREATE OR REPLACE FUNCTION CHECK_BASKET_FUNC(basket_id_arg integer) returns table
(product_id integer,product_name varchar(100),product_price integer, product_img varchar(100))
LANGUAGE PLPGSQL AS $$
BEGIN 
IF basket_id_arg IS NOT NULL THEN 
RETURN QUERY
SELECT b.product_id, p.product_name, p.product_price, p.product_img 
FROM Basket b join Product p on b.product_id = p.product_id  where basket_id = basket_id_arg;
END IF;
END;
$$;
select * from CHECK_BASKET_FUNC(48) order by product_name offset 0 rows fetch next 8 rows only;

drop FUNCTION CHECK_BASKET_FUNC;


--Создание процедуры обновления пользователя--
CREATE OR REPLACE PROCEDURE UPDATE_USER(user_id_arg INTEGER,
										user_name_arg VARCHAR(255), 
										user_date_arg DATE,
										user_mail_arg varchar(100)
										
										) LANGUAGE PLPGSQL AS $$
BEGIN
    IF user_id_arg IS NOT null THEN
        UPDATE Users
        SET user_name = user_name_arg,
            user_date = user_date_arg,
			user_mail = user_mail_arg
        WHERE user_id = user_id_arg;
    END IF;
	IF NOT EXISTS  (SELECT 1 FROM Users WHERE Users.user_id = user_id_arg) THEN
	 RAISE EXCEPTION 'Пользователя с ID % не существует',user_id_arg; END IF;
END;
$$;

--Создание функции обновления пароля--
CREATE OR REPLACE PROCEDURE UPDATE_PASSWORD(user_id_arg INTEGER, user_password_arg TEXT) LANGUAGE PLPGSQL AS $$
BEGIN
    UPDATE Users
    SET user_password = user_password_arg
    WHERE USER_ID = IN_USER_ID;

END;
$$;
select * from users;


--Создание функции обновления товара--
CREATE OR REPLACE PROCEDURE UPDATE_PRODUCT(
  product_id_arg INTEGER,
  category_id_arg integer,
  product_name_arg varchar(100),
  product_price_arg INTEGER,
	product_img_arg varchar(100),
	product_color_arg varchar(100),
	product_country_arg varchar(100),
	product_description_arg varchar(200)
) LANGUAGE PLPGSQL AS $$
BEGIN
  IF product_id_arg IS not null THEN
    UPDATE Product
    SET
      category_id = category_id_arg,
	  product_name=product_name_arg,
	  product_price=product_price_arg,
	  product_img=product_img_arg,
	  product_color=product_color_arg,
	  product_date=current_date,
	  product_country=product_country_arg,
	  product_description=product_description_arg
    WHERE Product.product_id = product_id_arg;
	UPDATE Product SET text_tsv = to_tsvector('russian', product_name) 
|| to_tsvector('russian', product_description);
  END IF;
END;
$$;
drop procedure UPDATE_PRODUCT;
select * from users

--Создание функции удаления пользователя--
CREATE OR REPLACE PROCEDURE DELETE_USER(user_id_arg INTEGER) LANGUAGE PLPGSQL AS $$
BEGIN
    DELETE FROM Rating WHERE USER_ID = user_id_arg;
    DELETE FROM Users WHERE user_id = user_id_arg;
    DELETE FROM Basket WHERE basket_id = user_id_arg;

END;
$$;
call DELETE_USER(41)


--Создание функции удаления товара из корзины--
CREATE OR REPLACE PROCEDURE DELETE_FROM_BASKET(product_id_arg integer, basket_id_arg integer) LANGUAGE PLPGSQL AS $$
declare counter integer := (select count(product_id) from Basket where Basket.basket_id = basket_id_arg);
BEGIN
IF counter <2 then
RAISE EXCEPTION 'Минимальное количество товаров в корзине 1';
else DELETE FROM Basket WHERE Basket.product_id=product_id_arg;

END IF; END;
$$;
select * from Product;
select product_id from Product where product_name ='sdfsdf';
call DELETE_FROM_BASKET(18,48);
drop procedure DELETE_FROM_BASKET;
select MAX(product_id) from Product


--Создание функции удаления товара из магазина--
CREATE OR REPLACE PROCEDURE DELETE_PRODUCT( product_id_arg INTEGER) LANGUAGE PLPGSQL AS $$
BEGIN
    DELETE FROM Product WHERE Product.product_id = product_id_arg;
    DELETE FROM Basket WHERE Basket.product_id = product_id_arg;
	    DELETE FROM Commentaries WHERE Basket.product_id = product_id_arg;

END;
$$;
call DELETE_PRODUCT(2);
select * from Product;



--Создание функции вывода рейтинга товара--
CREATE OR REPLACE FUNCTION AVG_RATE(product_id INTEGER) RETURNS NUMERIC AS $$
DECLARE
  rating INTEGER;
  count_ratings INTEGER;
BEGIN
  SELECT SUM(rate), COUNT(*) INTO rating, count_ratings
  FROM Rating WHERE Rating.product_id = $1;
  IF count_ratings = 0 THEN RETURN 0;
  END IF;
  RETURN rating :: NUMERIC / count_ratings; 
  END;$$ 
  language PLPGSQL;
  
  
  
  --Создание функции поиска пользователя по имени--
  CREATE or replace FUNCTION SEARCH_USER_BY_NAME(user_name_arg varchar(100))
  RETURNS TABLE(
	  user_name varchar(100),
	  user_mail varchar(100),
	  user_date date	  
  ) as $$ 
  declare 
  begin 
  RETURN QUERY
  SELECT USER_INFO.user_name,USER_INFO.user_mail,
  USER_INFO.user_date FROM USER_INFO  
  where USER_INFO.user_name  ilike '%'|| user_name_arg || '%';
  end;
  $$ language PLPGSQL;
  drop function SEARCH_USER_BY_NAME;
  select * from SEARCH_USER_BY_NAME('user1');
  select * from users;
SELECT to_tsvector('russian', product_name) from Product;
  
  --Создание функции поиска товара по названию--
  CREATE or replace FUNCTION SEARCH_PRODUCT_BY_NAME(product_name_arg varchar(100))
  RETURNS TABLE(
	  product_name varchar(100),
	  product_price integer,
	  product_color varchar(100),
	  product_country varchar(100),
	  product_date date,
	  product_img varchar(100)
  ) as $$ 
  begin 
  RETURN QUERY
  SELECT Product.product_name,Product.product_price,
  Product.product_color, Product.product_country,
  Product.product_date,Product.product_img FROM Product  
  where
  Product.product_name  ilike '%'|| product_name_arg || '%' or 
  Product.text_tsv @@ plainto_tsquery('russian', product_name_arg);
  end;
  $$ language PLPGSQL;
  drop function SEARCH_PRODUCT_BY_NAME;
   select * from SEARCH_PRODUCT_BY_NAME('Am');
  select * from Product;
  
  
  
   --Создание функции поиска товара по категории--
  CREATE or replace FUNCTION SEARCH_PRODUCT_BY_CATEGORY(category_name_arg varchar(100))
  RETURNS TABLE(
	  product_name varchar(100),
	  product_price integer,
	  product_color varchar(100),
	  product_country varchar(100),
	  product_date date,
	  category_name varchar(100),
	  product_img varchar(100)
  ) as $$ 
  begin 
  RETURN QUERY
  SELECT PRODUCT_INFO.product_name,PRODUCT_INFO.product_price,
  PRODUCT_INFO.product_color, PRODUCT_INFO.product_country,
  PRODUCT_INFO.product_date, PRODUCT_INFO.category_name, PRODUCT_INFO.product_img FROM PRODUCT_INFO
  where PRODUCT_INFO.category_name  ilike '%'|| category_name_arg || '%';
  end;
  $$ language PLPGSQL;
  drop function SEARCH_PRODUCT_BY_CATEGORY;
  select * from SEARCH_PRODUCT_BY_CATEGORY('Планшеты') offset 3 rows fetch next 8 rows only;
  select * from Product;
  select * from Product_info
  select * from category
  
  --ЗАПОЛНЕНИЕ 100000 СТРОК---- 
  CREATE OR REPLACE FUNCTION INSERT_CATEGORY()
RETURNS VOID AS $$
DECLARE
    I INTEGER := 5;
BEGIN
    WHILE I <= 100000 LOOP
        INSERT INTO Category (category_name,category_id) VALUES ('CATEGORY ' || I,I);
        I := I + 1;
    END LOOP;
END;
$$ LANGUAGE PLPGSQL;

  select INSERT_CATEGORY();
 explain select category_name from Category ;
 create index INDEX_CATEGORY_NAME on Category(category_name) ;
 drop index INDEX_CATEGORY_NAME;
 
-------------ПРЕДСТАВЛЕНИЯ-------------
 -----------------------------------
 -------------------------------------
 
 
 --Представление с информацией о товаре--
 CREATE VIEW PRODUCT_INFO AS
  SELECT p.product_name,
    p.product_price,
    c.category_name,
    p.product_color,
    p.product_date,
    p.product_country,
	p.product_img,
    p.product_description,
    avg_rate(p.product_id) AS avg_rating
   FROM product p
     JOIN category c ON p.category_id = c.category_id;
	 drop view PRODUCT_INFO;
	 select * from PRODUCT_INFO;
	 select * from Users;
	 
	 
   --Представление с информацией о пользователе--

CREATE VIEW USER_INFO AS
SELECT u.user_id,
u.user_name,
    u.user_mail,
    u.user_date,
    u.user_password,
    r.role_name
   FROM users u
     JOIN roles r ON u.user_role = r.role_id;
	 select 
	 --Представление с информацией лучших продуктов--
	 CREATE VIEW BEST_PRODUCTS
 AS
 SELECT p.product_name,
    p.product_price,
    c.category_name,
    p.product_color,
    p.product_date,
    p.product_country,
    p.product_description,
    avg_rate(p.product_id) AS avg_rating
   FROM product p
     JOIN category c ON p.category_id = c.category_id
  ORDER BY avg_rating DESC LIMIT 10;
  select * from BEST_PRODUCTS;
  
  
------------- Триггеры -----------------
----------------------------------------
----------------------------------------
--
--Создание триггера, реагирующего на дату рождения позже настоящей--
CREATE or replace FUNCTION FUNC_FOR_TR_DATE() RETURNS TRIGGER AS $$
BEGIN
    IF NEW.user_date >= CURRENT_DATE THEN
        RAISE EXCEPTION 'Такой даты не существует';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE PLPGSQL;
CREATE or replace TRIGGER TR_DATE
BEFORE INSERT OR UPDATE
ON Users
FOR EACH ROW EXECUTE FUNCTION FUNC_FOR_TR_DATE();



drop FUNCTION FUNC_FOR_TR_DATE() cascade;

drop FUNCTION FUNC_FOR_TR_DATE() cascade;


drop trigger TR_DATE cascade;

select * from Users;
--Создание триггера, реагирующий на валидацию пароля--
CREATE or replace FUNCTION FUNC_FOR_TR_PASSWORD() RETURNS TRIGGER AS $$
BEGIN
  IF char_LENGTH(NEW.user_password) < 8 THEN
    RAISE EXCEPTION 'Пароль должен содержать не менее 8 символов';
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE PLPGSQL;

CREATE TRIGGER TR_PASSWORD
  BEFORE INSERT OR UPDATE ON Users
  FOR EACH ROW
  EXECUTE FUNCTION FUNC_FOR_TR_PASSWORD();


--Создание триггера, реагирующего на оценивание товара--
CREATE FUNCTION FUNC_FOR_TR_RATE() RETURNS TRIGGER AS $$
BEGIN
    IF NEW.rate > 10 or NEW.rate < 0  THEN
        RAISE EXCEPTION 'Оцените в пределах от 0 до 10';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE PLPGSQL;

CREATE or replace TRIGGER TR_RATE
BEFORE INSERT OR UPDATE
ON Rating
FOR EACH ROW EXECUTE FUNCTION FUNC_FOR_TR_RATE();









select * from users










--------------Экспорт XML----------------
------------------------------------------------
CREATE EXTENSION adminpack;
CREATE EXTENSION IF NOT EXISTS file_fdw;

CREATE OR REPLACE FUNCTION export_users(
file_path text)
RETURNS void
LANGUAGE 'plpgsql'
COST 100
VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE
xml_data xml;
xml_doc text;
BEGIN
SELECT table_to_xml('Users', true, false, '') INTO xml_data;

xml_doc := format('<?xml version="1.0" encoding="UTF-8"?>%s', xml_data::text);

PERFORM pg_file_write(file_path, xml_doc, true);
END;
$BODY$;



SELECT export_users('D:\MYDOCUMENTS\BGTU\DB_KURS\HiTech\XML\USERS.XML');







--------------ИМПОРТ-----------------------

CREATE OR REPLACE FUNCTION import_data_from_xml(filename TEXT) RETURNS VOID AS $$
DECLARE
  xml_data TEXT;
BEGIN
  -- Читаем XML данные из файла
  xml_data := pg_read_file(filename, 0, 1000);

  -- Парсим XML данные и выполняем импорт
  WITH xml_rows AS (
    SELECT unnest(xpath('/root/row', xmlparse(content xml_data))) AS xml_row
  )
  INSERT INTO Users (user_id, user_name,user_password,user_date,basket_id,user_role,user_mail)
  SELECT
    (xpath('/row/user_id/text()', xml_row))[1]::text::integer AS user_id,
 (xpath('/row/user_name/text()', xml_row))[1]::TEXT AS user_name,
 (xpath('/row/user_password/text()', xml_row))[1]::TEXT AS user_password,
 (xpath('/row/user_date/text()', xml_row))[1]::text::date AS user_date,
  (xpath('/row/basket_id/text()', xml_row))[1]::text::int AS basket_id,
  (xpath('/row/user_role/text()', xml_row))[1]::text::int AS user_role,

 (xpath('/row/user_mail/text()', xml_row))[1]::TEXT AS user_mail

 FROM
    xml_rows;

  -- Выводим сообщение об успешном импорте
  RAISE NOTICE 'Данные импортированы из файла %', filename;
END;
$$ LANGUAGE plpgsql;
------------------------------------------------
SELECT import_data_from_xml('D:\MYDOCUMENTS\BGTU\DB_KURS\HiTech\XML\USERS.XML');
select * from users



GRANT EXECUTE ON FUNCTION pg_write_server_files TO root_user1;
grant pg_write_server_files to root_user1;
SELECT current_user;

SELECT * FROM pg_extension WHERE extname = 'xml2';

CREATE EXTENSION IF NOT EXISTS xml2;




------------------------------------------------




select * from product order by product_name offset 8 rows fetch next 8 rows only;
select * from users;

select * from Product where product_name= '1'