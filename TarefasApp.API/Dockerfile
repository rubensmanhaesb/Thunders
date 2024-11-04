# Usa a imagem base do ASP.NET Core 8.0, que contém o ambiente de execução (runtime) necessário para rodar aplicações .NET.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Define o usuário 'app' para executar os comandos a partir deste ponto, melhorando a segurança.
USER app

# Define o diretório de trabalho como /app. Todos os comandos subsequentes serão executados a partir desse diretório.
WORKDIR /app

# Expõe a porta 8080 para que a aplicação esteja acessível através dessa porta no contêiner.
EXPOSE 8080

# Usa a imagem do SDK do .NET 8.0, que contém todas as ferramentas necessárias para compilar e publicar o código.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Cria uma variável de ambiente chamada BUILD_CONFIGURATION que pode ser passada na linha de comando, com o valor padrão Debug.
ARG BUILD_CONFIGURATION=Debug

# Define o diretório de trabalho para o processo de build como /src.
WORKDIR /src

# Copia os arquivos de projeto (.csproj) da API e de cada camada da solução para o diretório de trabalho no contêiner.
COPY ["TarefasApp.API/TarefasApp.API.csproj", "TarefasApp.API/"]
COPY ["TarefasApp.Application/TarefasApp.Application.csproj", "TarefasApp.Application/"]
COPY ["TarefasApp.Domain/TarefasApp.Domain.csproj", "TarefasApp.Domain/"]
COPY ["TarefasApp.Infra.Storage/TarefasApp.Infra.Storage.csproj", "TarefasApp.Infra.Data.MongoDB/"]
COPY ["TarefasApp.Infra.Data/TarefasApp.Infra.Data.csproj", "TarefasApp.Infra.Data.SqlServer/"]
COPY ["TarefasApp.Infra.Messages/TarefasApp.Infra.Messages.csproj", "TarefasApp.Infra.Message/"]

# Restaura as dependências do projeto principal (TarefasApp.API), baixando pacotes NuGet necessários.
RUN dotnet restore "./TarefasApp.API/TarefasApp.API.csproj"

# Copia todos os arquivos da solução para o contêiner.
COPY . .

# Define o diretório de trabalho como o diretório da API principal.
WORKDIR "/src/TarefasApp.API"

# Compila o projeto no modo definido por BUILD_CONFIGURATION (padrão é Debug) e gera os arquivos binários no diretório /app/build.
RUN dotnet build "./TarefasApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# A partir da imagem 'build' anterior, publica a aplicação, gerando uma versão pronta para produção.
FROM build AS publish

# Usa a mesma variável de configuração de build (Debug ou outra fornecida pelo usuário).
ARG BUILD_CONFIGURATION=Debug

# Publica o projeto, gerando os arquivos de saída no diretório /app/publish e desabilitando o uso do AppHost (otimização para contêineres).
RUN dotnet publish "./TarefasApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Usa a imagem base para rodar a aplicação (base ASP.NET runtime), criando o estágio final do contêiner.
FROM base AS final

# Define o diretório de trabalho final como /app.
WORKDIR /app

# Copia os arquivos publicados da fase anterior para o diretório de trabalho.
COPY --from=publish /app/publish .

# Eleva os privilégios para root para permitir a instalação de pacotes
USER root

# Instala os certificados SSL raiz
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates


# Define o comando de entrada (ENTRYPOINT) que executa a aplicação. Aqui o comando executa a API com o argumento '--environment' configurado para 'Development'.
ENTRYPOINT ["dotnet", "TarefasApp.API.dll", "--environment", "Production"]


