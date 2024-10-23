CREATE DATABASE Pindufast;

use master;

use Pindufast;

create table Carro(
	id int not null IDENTITY primary key,
	Placa varchar(7) not null,
	Nome varchar(50) not null,
	Portas int not null,
	Preco decimal not null,
	Imagem Image,
	DataPublicacao DateTime not null,
	Ativo bit not null
);

