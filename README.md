# Description the photo album service
There is the external URL in the type <br />
**JsonPlaceholderAlbum** <br />
**JsonPlaceholderUser** <br /> <br />
The main URL for receiving data <br />

|URL|
| ----- |
|http://jsonplaceholder.typicode.com/albums|
|http://jsonplaceholder.typicode.com/users|

## Operation in the service
Routing for working with the table of users <br />

| URI   | Method | Description | HATEOAS |
| ----- | -----  | -----   | ----- |
| api/users | GET | Receiving the list of users | self |
|api/users?pageSize=20&page=1|GET|Receiving the filtered list of users. pageSize=20 – amount of users on the page, page – the number of current page||
|api/users/1|GET|Receiving the user with idUser=1|self|
|api/users/1/albums|GET|Receiving all albums for the user with idUser=1|self|

Routing for working with the table of albums  <br />

| URI   | Method | Description | HATEOAS |
| ----- | -----  | -----   | ----- |
| api/albums | GET | Receiving the list of users | self |
| api/albums/1 |GET|Receiving the filtered list of users. pageSize=20 – amount of users on the page, page – the number of current page|self|
| api/albums?pageSize=20&page=1 |GET|Receiving the user with idUser=1||

Available formats of data. You need to specify appropriate Accept parameter in the Header. <br />

|Accept|Format|
| ----- | -----  |
|application/json|**JSON**|
|application/xml|**XML**|

## Details of realization
Dependency injection has resolved with using **Autofac**. <br />
**AutoMapper** has used for mapping types. <br />
The sensitive field has hidden in the response with using ViewModel types. <br />
Service has applied partially hateoas. <br />
There is the pagination in requests. <br />
Applying asynchronous requests. <br />

