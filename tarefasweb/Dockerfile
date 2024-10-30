# Etapa 1: Construir o projeto Angular com Node
FROM node:18-alpine AS build

# Define o diretório de trabalho
WORKDIR /app

# Copia package.json e package-lock.json para instalar as dependências
COPY tarefasweb/package*.json ./

# Instala as dependências (incluindo o Bootstrap)
RUN npm install --legacy-peer-deps

# Copia o código fonte do projeto Angular
COPY tarefasweb/ .

# Constrói o projeto Angular para produção
RUN npm run build -- --output-path=dist/tarefasweb && ls -la /app/dist/tarefasweb

# Etapa 2: Servir o projeto Angular com Nginx
FROM nginx:alpine

# Garante que o diretório do Nginx exista
RUN mkdir -p /usr/share/nginx/html

# Copia os arquivos construídos do Angular para o diretório do Nginx
COPY --from=build /app/dist/tarefasweb/browser /usr/share/nginx/html

# Copia o arquivo de configuração do Nginx
COPY tarefasweb/nginx.conf /etc/nginx/nginx.conf

# Expõe a porta do Nginx
EXPOSE 4200

# Inicia o Nginx
CMD ["nginx", "-g", "daemon off;"]

#CMD ["tail", "-f", "/dev/null"]