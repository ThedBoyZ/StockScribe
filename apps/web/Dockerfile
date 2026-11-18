# syntax=docker/dockerfile:1
#
# StockScribe web · Nuxt SPA built to static files, served by nginx (ADR-0004).
# Build context: apps/web
#
#   docker build -t stockscribe-web apps/web

# ---------- build: nuxt generate ----------
FROM node:24-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
# Skip postinstall (`nuxt prepare`) here: the sources aren't copied yet. `generate` prepares anyway.
RUN npm ci --ignore-scripts

COPY . .
RUN npm run generate

# ---------- web: nginx serves .output/public ----------
FROM nginx:1.29-alpine AS web
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/.output/public /usr/share/nginx/html
EXPOSE 80
