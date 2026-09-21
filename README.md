# PROYECTO BACKEND EZKART
Este es un proyecto backend de un mini e-commerce con diferentes proyectos como servicios. Se pensaron en 3 servicios mas la API Gateway, cada uno con su propia
base de datos. Los servicios son:
- **Auth** : Servicio de autenticacion de usuario, entrega JWT Token y Refresh Token por medio de cookies al logear.
- **Inventario**: Servicio que maneja el inventario de la aplicacion, responsabilidad principal manejo de stock.
- **Compras**: Servicio que maneja el carrito y compras de la aplicacion.

Cada microservicio tiene su pagina swagger para probar endpoints.

## Desiciones tecnicas consideradas
- En cada servicio se creo en capas, Api, Infraestuctura, Aplicacion y Dominio. Teniendo Dominio sin dependencia de ninguna capa, quedanddo con al siguiente jerarquia:
    1. Dominio.
    2. Aplicacion.
    3. Api.
    4. Infraestuctura
    
    Esto permite mayor desacoplamiento dentro de cada microservio, delegando responsabilidades bien definidas aplicando inversion de dependencia con interfaces.

- La entrega de token es por cookie con httpOnly asi evitando el acceso del token con script ejecutado en el navegador.
- Se automatizo ejecucion de migraciones y seeds con propositos de reducir el esfuerzo para ejecutar la aplicacion.
- Se agrego validaciones de iniciacion especialmente parametros del JWT Token. Se dejo visible un development para agilizar el levant.
- Se creo un docker compose con las bases necesarias para la aplicacion. 
- Se utilizo un proyecto .NET con Yarp para crear un reverse Proxy. 
- Para el problema de concurrencia se creo la fila de version en base de datos, asi no permitiendo actualizacion de la tabla al tener version diferente. Se creo una 
unidad de trabajo para cortar la traza de transaccines con un cancellation token.
- La comunicacion entre microservicios es por Http. Sin eventos diminuyendo complejidad.
- Las consultas de datos se paginaron.
- Solo se considero autenticacion para el consumo de apis.

## Ejecucion del proyecto.
1. Levantar las bases ubicandonse la carpeta docker y ejecutar el comando:
``
docker compose -f docker-compose.db.yaml up -d
``
2. Posicionarse en el proyecto .Api de cada servicio y ejecutar 
``
    dotnet run build
``

### Considerar
Las key del Jwt en appSettings deben ser iguales en todos los proyectos.