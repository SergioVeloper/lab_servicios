const { Sequelize, DataTypes } = require('sequelize');

const sequelize = new Sequelize('bd_bnb', 'root', '', {
    host: 'localhost',
    dialect: 'mysql'
});

const Cotizacion = sequelize.define('Cotizacion', {
    id: {
        type: DataTypes.BIGINT,
        allowNull: false,  // Corregido: era 'allouwNull'
        primaryKey: true,
        autoIncrement: true,
        unsigned: true
    },
    fecha: {
        type: DataTypes.DATE,
        allowNull: false
    },
    cotizacion: {
        type: DataTypes.FLOAT,
        allowNull: false
    },
    cotizacion_oficial: {
        type: DataTypes.FLOAT,
        allowNull: false
    }
}, {
    tableName: 'cotizaciones',
    timestamps: true,
    createdAt: 'created_at',
    updatedAt: 'updated_at',
});

// Exporta tanto sequelize como Cotizacion si necesitas usar ambos en otros archivos
module.exports = { sequelize, Cotizacion };
