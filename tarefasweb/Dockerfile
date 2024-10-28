# Use uma imagem do Node 18 como base
FROM node:18-alpine

# Cria um diretório de trabalho no contêiner
WORKDIR /app

# Copia o arquivo package.json e package-lock.json para o contêiner
COPY tarefasweb/package*.json ./

# Instala as dependências (inclui Bootstrap)
RUN npm install --legacy-peer-deps

# Copia todos os arquivos do projeto Angular para o contêiner
COPY tarefasweb/ .

# Constrói o projeto Angular para produção
RUN npm run build --prod

RUN npm run build --prod && ls -la /app/dist/tarefasweb

# Instala o servidor Nginx para servir o projeto Angular
RUN apk add --no-cache nginx

# Garante que o diretório do Nginx existe antes de copiar os arquivos
RUN mkdir -p /usr/share/nginx/html

# Copia o arquivo de configuração do Nginx
COPY tarefasweb/nginx.conf /etc/nginx/nginx.conf

# Remove o conteúdo padrão do Nginx e copia os arquivos do build do Angular
RUN rm -rf /usr/share/nginx/html/* && \
    cp -r /app/dist/tarefasweb/* /usr/share/nginx/html


# Comando para iniciar o Nginx
CMD ["nginx", "-g", "daemon off;"]
