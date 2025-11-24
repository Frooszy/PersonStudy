FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev

WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .

EXPOSE 5000

ENTRYPOINT ["dotnet", "watch", "run"]
