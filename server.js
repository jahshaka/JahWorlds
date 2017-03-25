var express = require('express');
var morgan = require('morgan');
var bodyParser = require('body-parser');
var oauthserver = require('oauth2-server');
require('dotenv').load();
var Sequelize = require('sequelize');
var model = require('./model');

var app = express();

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

app.oauth = oauthserver({
    model: model,
    grants: ['password', 'refresh_token'],
    debug: true,
    accessTokenLifetime: process.env.AUTH_ACCESS_TOKEN_LIFETIME,
    refreshTokenLifetime: process.env.AUTH_REFRESH_TOKEN_LIFETIME
});

app.all('/oauth/token', app.oauth.grant());

app.use(app.oauth.errorHandler());

app.listen(process.env.APP_PORT);