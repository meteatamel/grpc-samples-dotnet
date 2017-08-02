FROM gcr.io/google-appengine/aspnetcore:1.1

RUN mkdir /app
ADD . /app
WORKDIR /app

EXPOSE 50051
ENTRYPOINT ["dotnet", "GreeterServer.dll"]
