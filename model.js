var jwt = require('jsonwebtoken');
var uuid = require('uuid/v4');
var bcrypt = require('bcrypt');
var dbModel = require('./dbModel');


var model = module.exports;

model.generateToken = function(type, req, callback) {
    if(type === 'refreshToken') {
        callback(null, null);
        return;
    }

    var token = jwt.sign({
        user: req.body.username,
        email: req.body.username,
        exp: process.env.AUTH_ACCESS_TOKEN_LIFETIME,
        sub: req.body.client_id,
        iss: process.env.AUTH_JWT_ISSUER,
        aud: 'JahShaka'
    }, process.env.AUTH_JWT_SECRET);
    
    callback(null, token);
}

model.saveRefreshToken = function (refreshToken, clientId, expires, userId, callback) {
    dbModel.RefreshToken.create({
        refreshToken: refreshToken,
        clientId:clientId,
        userId:userId,
        expires:expires
    }).then(function(token){
        callback(null);
    });
};

model.saveAccessToken = function (token, clientId, expires, userId, callback) {
    callback(null);
};

model.getAccessToken = function (bearerToken, callback) {
    try {
        var decoded = jwt.verify(bearerToken, process.env.AUTH_JWT_SECRET, { 
            ignoreExpiration: true //check
        });
        callback(null, {
            accessToken: bearerToken,
            clientId: decoded.sub,
            userId: decoded.user,
            expires: new Date(decoded.exp * 1000)
        });
    } catch(e) {    
        callback(e);
    }
};

model.getRefreshToken = function (refreshToken, callback) {
    dbModel.RefreshToken.findOne({ refreshToken: refreshToken }).then(function(token) {
        callback(null,token.refreshToken);
    });
};

model.getClient = function (clientId, clientSecret, callback) {
    if (clientSecret === null) {
        return dbModel.OauthClients.findOne({ clientId: clientId }).then(function(client) {
            callback(null,client);
        });
    }

    dbModel.OauthClients.findOne({ 
        clientId: clientId, 
        clientSecret: clientSecret 
    }).then(function(client) {
        callback(null,client.clientId);
    });
};

model.grantTypeAllowed = function (clientId, grantType, callback) {
  callback(false, true);
};

model.getUser = function (username, password, callback) {
    dbModel.Users.findOne({username: username }).then( 
        function(user) {
            bcrypt.compare(password,user.password, function(err,res) {
                if(res) {
                    callback(null, user.id);
                } else {
                    callback(err)
                }
            });
        }
    );
};
