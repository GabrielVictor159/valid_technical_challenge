FROM node:20-alpine

WORKDIR /app

COPY 0-Presentation/Store.Web/package*.json ./

RUN npm install

COPY 0-Presentation/Store.Web/ .

EXPOSE 55541

CMD ["npm", "run", "dev", "--", "--host"]