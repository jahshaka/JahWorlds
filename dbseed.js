'use strict'

require('dotenv').load();

var dbModel = require('./dbModel');
var bcrypt = require('bcrypt');
var uuid = require('uuid/v4');

console.log(uuid());

dbModel.OauthClients.sync({force:true}).then(function() {
    return dbModel.OauthClients.create({
        clientId:'jahworlds_dev',
        clientSecret:'secret_secret_secret'
    });
});

bcrypt.hash('testpass', 10, function(err, hash) {
    dbModel.Users.sync({force:true}).then(function() {
        return dbModel.Users.create({
            id: uuid(),
            email:'test@test.com',
            password:hash
        });
    });
});