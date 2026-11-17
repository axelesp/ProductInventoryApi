# ProductInventorySolution

Aplicación APIRest realizada en .NET 8 con visual studio 2022 para productos con inventario:
1. Devuelve todos los productos: api/Product/GetAllProducts
2. Devuelve un producto por su ID: api/Product/GetProduct/{Id}
3. Agrega un producto a la BD: api/Product/CreateProduct
4. Modifica el inventario de un producto por su ID: api/Product/ModifyStock/{Id}/stock
5. Elimina un producto por su ID: api/Product/DeleteProduct/{Id}

La solución está dividida en los dominios:
1. ProductInventoryApi/ProductInventoryApi -> APIRest
2. ProductInventoryApi/ProductInventory.Business -> Servicios y lógica de negocio
3. ProductInventoryApi/ProductInventory.Entities -> Entidades e interfaces
4. ProductInventoryApi/ProductInventory.Data -> Contexto de Datos

Está configurada para desplegarse en docker. Para correrlo y depurar desde VS se deben hacer cambios en el archivo:
ProductInventoryApi/Program.cs

Se crea una base de datos SqlServer en demanda, ya sea en entorno windows o docker.

Correr la solución con Docker:
1. Al descargarla del repositorio no hay que hacer alguna configuración, en el directorio raiz cuenta con los archivos:
	a) DockerFile
	b) docker-compose.yml
2. Abrir un consola de comandos ubicándose en la carpeta raiz de la solución "ProductInventoryApi"
3. Tener acceso a internet para poder descargar los complementos que se usaran en Docker.
3. Correr el comando: docker-compose up --build -d
4. Si fue exitosase la compilación, se deberá ver:
	Image productinventoryapi-api                     Built   
 ✔ Network productinventoryapi_product_inventory_net Created 
 ✔ Volume productinventoryapi_sqlserver_data         Created 
 ✔ Container inventory_oauth2                        Started 
 ✔ Container sqlserver                               Started 
 ✔ Container productInventory_Api                    Started  
5. En el navegador ingresar la url: http://localhost:8080/swagger/index.html, deberá cargarse la interfaz de swagger para probar la API.
6. Para probar la autenticación OAuth se debe configurar la herramienta de gestión de identidades Keycloak.
7. En otra ventana del navegador ingresar: http://localhost:8081/admin/
8. Firmarse con usuario "Admin", Password: "Admin"
9. Crear Realm: En el combobox en el esquina superior izquierda que dice "Keycloak", hacer clic y seleccionar "Create Realm", en 'Realm Name' ponerle "inventory", clic en 'Create'
10. Crear Scopes: En el menú de la izquierda, seleccionar "Client Scopes" y clic en "Create client Scope", ponerle como nombre y Descripción "inventory.read" y activar "Include in token scope", Salvar.
	Crear otro Scope con el Nombre y Descripción "inventory.write",	activar "Include in token scope", Salvar.
11. Crear cliente para swagger:
	• En menú de la izquierda "Clients" -> Create client, poner Client ID: swagger-client, Name: swagger-client, clic en "Next".
	• Activar "Client authentication" y seleccionar la opción "Service accounts roles", clic en "Next".
	• En "Valid redirect URIs" poner un *, "Web origins" poner un *. Clic en "Save".
	  Nota: Al probar la aplicación, una configuración predeterminada Cross Origin de Keycloak bloquea las peticiones del puerto 8080 desde swagger, por eso se optó por poner * para pruebas.
	• En la pestaña "Client scopes", agregar los scopes creados previamente inventory.read e inventory.write, hacienco clic en el botón "Add client scope".
	• En la lista de Client scopes, entrar al scope que se creo automáticamente "swagger-client-dedicated", clic en Add mapper -> By configuration -> Audience -> poner en "Included Custom Audience:" api y guardar.
	• En la pestaña Credentials de swagger-client, copiar el Client Secret.
	Se puede probar el Realm creado ingresando a la URL "http://localhost:8081/realms/inventory/.well-known/openid-configuration" y se ve un JSON, si no se ve o envía página inválida no quedó bien configurado.
12. En la ventana que tiene la Api de swagger, hacer clic en "Authorize"
13. Ingresar en client_id "swagger-client", en Client Secret ingresar el 'Client secret' que se copio de Keycloak, 
	seleccionar ambos scopes (inventory.read e inventory.write) y pulsa Authorize.
14. Probar la API: Agregar un producto con el recurso: api/Product/CreateProduct

Se puede ver el resultado del uso de los recursos conectándose a la BD que se encuntra en un contenedor separado en Docker (usando DBeaver).
1. Conectar por HOST
2. URL -> jdbc:sqlserver://;serverName=localhost;port=1434;databaseName=ProductInventory
3. HOST: localhost
4. Puerto: 1434
5. Usuario: sa
6. Psw: experis.api26

Correr la solución con visual studio:
1. En el directorio raiz de la solución "ProductInventoryApi/", renombrar el archivo program.cs por _program_docker_cs,
2. Renombrar el archivo _program_VS_cs por program.cs
3. En el directorio "ProductInventoryApi/ProductInventoryDbContext/Repositors" renombrar el archivo ProductDapperRepository.cs por ProductDapperRepository_Docker_cs
4. Renombr el archivo ProductDapperRepository_VS_cs por ProductDapperRepository.cs 
5. Crear una carpeta "Data" en C: "C:\Data"
6. Abrir la solución "ProductInventoryApi/ProductInventorySolution.sln"
6. Compilar y Ejecutar la aplicación con IISExpress.
7. Ejecutar el recurso al comienzo del la lista en swagger: "Auth Local (Solo Dev)" - api/dev-token
	Este recurso Devuelve un token de prueba que se ingresa en "Authorize" para probar las APIs
8. Se crea una base de datos local de sql server en el directorio "C:\Data\ProductInventory.mdf"