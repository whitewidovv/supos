delete from categories;
insert into categories ("Id", "Name", "Icon") values ('1', 'Drinks', 'test.png');
insert into categories ("Id", "Name", "Icon") values ('2', 'Entry', 'test.png');
insert into categories ("Id", "Name", "Icon") values ('3', 'Plates', 'test.png');
insert into categories ("Id", "Name", "Icon") values ('4', 'Dessert', 'test.png');

delete from taxes;
insert into taxes ("Id", "Name", "Rate", "Overrideable") values ('1', 'Default', '0.21', '1');
insert into taxes ("Id", "Name", "Rate", "Overrideable") values ('2', 'Default Force', '0.21', '0');
insert into taxes ("Id", "Name", "Rate", "Overrideable") values ('3', 'Take Away', '0.06', '1');

delete from products;
insert into products ("Id", "CategoryId", "Name", "Icon", "DefaultTaxId", "Price") values ('1', '1', 'Beer', 'test.png', '1', '1.23');
insert into products ("Id", "CategoryId", "Name", "Icon", "DefaultTaxId", "Price") values ('2', '2', 'Schrimps', 'test.png', '1', '7.66');
insert into products ("Id", "CategoryId", "Name", "Icon", "DefaultTaxId", "Price") values ('3', '3', 'Steak', 'test.png', '1', '10.82');
insert into products ("Id", "CategoryId", "Name", "Icon", "DefaultTaxId", "Price") values ('4', '4', 'Ice Cream', 'test.png', '1', '2.5');

delete from customers;
insert into customers ("Id", "Name") values ('1', 'Table 1');
insert into customers ("Id", "Name") values ('2', 'Table 2');
insert into customers ("Id", "Name") values ('3', 'Table 3');
insert into customers ("Id", "Name") values ('4', 'Table 4');
insert into customers ("Id", "Name") values ('5', 'Table 5');
insert into customers ("Id", "Name") values ('6', 'Take Away 1');
insert into customers ("Id", "Name") values ('7', 'Take Away 2');

delete from payments;
insert into payments ("Id", "Name", "Allowed") values ('1', 'Cash', '1');
insert into payments ("Id", "Name", "Allowed") values ('2', 'Bancontact', '1');
insert into payments ("Id", "Name", "Allowed") values ('3', 'Credit Card', '0');

delete from meta;
insert into meta ("Id", "Property", "Value") values ('1', 'Version', '0.1');

delete from orders;
insert into orders("Id", "CustomerId") values('2000/01/01 00:00:00.0000000', '1');

delete from orderdetails;
