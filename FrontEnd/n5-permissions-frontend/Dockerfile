
FROM node:18-alpine AS build
WORKDIR /app

COPY package.json ./
COPY package-lock.json ./ 


RUN npm install --legacy-peer-deps 


COPY . .

ARG REACT_APP_API_URL
ENV REACT_APP_API_URL=${REACT_APP_API_URL}

RUN npm run build


FROM nginx:stable-alpine
WORKDIR /usr/share/nginx/html

RUN rm -rf ./*
COPY --from=build /app/build .



EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]