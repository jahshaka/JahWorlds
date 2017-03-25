var Sequelize = require('sequelize');
var sequelize = new Sequelize(process.env.DB_CONNECTION_STRING);

var dbModel = module.exports;

dbModel.RefreshToken = sequelize.define('refreshToken',{
    refreshToken: {
        type: Sequelize.STRING,
        allowNull: false,
        field:'refresh_token',
        primaryKey: true
    },
    clientId: {
        type: Sequelize.STRING,
        allowNull: false,
        field: 'client_id'
    },
    userId: {
        type: Sequelize.UUID,
        allowNull: false,
        field: 'user_id'/*,
        validate: {
            isUUID:4
        }*/
    },
    expires: {
        type: Sequelize.DATE,
        allowNull:false,
        field: 'expires'
    },
}, {
    underscored: true,
    tableName: 'refresh_token',
    freezeTableName:true,
    timestamps: true,
    updatedAt:false
});

dbModel.OauthClients = sequelize.define('oauthClients', {
    clientId: {
        type: Sequelize.STRING,
        allowNull: false,
        field: 'client_id',
        primaryKey:true
    },
    clientSecret: {
        type: Sequelize.STRING,
        allowNull: false,
        field: 'client_secret'
    }
}, {
    underscored: true,
    tableName: 'oauth_clients',
    freezeTableName:true,
    timestamps: true,
    updatedAt:false
});

dbModel.Users = sequelize.define('users', {
    id: {
        type: Sequelize.UUID,
        allowNull: false,
        field: 'id',
        primaryKey:true,
        validate: {
            isUUID:4
        }
    },
    email: {
        type: Sequelize.STRING,
        allowNull: false,
        field: 'email',
        validate: {
            isEmail:true
        }
    },
    password: {
        type:Sequelize.STRING,
        allowNull: false,
        field: 'password'
    }
}, {
    underscored: true,
    tableName: 'users',
    freezeTableName:true,
    timestamps: true
});

sequelize.sync();