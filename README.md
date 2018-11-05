Simple asp.net core 2.1 application demonstrates issue with NSwag UI while running behind nginx reverse-proxy.
Running locally with docker-compose, the follows happens:

When running local (dotnet run), or as single docker container:
http://localhost:5000/swagger/v1.0.json - works as expected, renders swagger JSON
http://localhost:5000/swagger - works as expected, renders swagger UI
http://localhost:5000/swagger/index.html - works as expected, renders swagger UI

When running behind nginx (sudo docker-compose up):
http://localhost:8080/simpleapp/swagger/v1.0.json - works as expected, renders swagger JSON
http://localhost:8080/simpleapp/swagger - 404 not found
http://localhost:8080/simpleapp/swagger/index.html - display UI with error: "Not Found /swagger/v1.0.json"

In general, both simpleapp service and nginx are configures to run on the same docker-compose default network, so nginx will be able to access simpleapp service by service name (simpleapp).

Nginx is configured to forward calls on localhost:8080/simpleapp to the service.
Under this configuration, running: localhost:8080/simpleapp/api/values, works as expected.

For more information about the setup, see:
nginx.conf
Dockerfile
docker-compose.yml

