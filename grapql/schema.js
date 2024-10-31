const { GraphQLObjectType, GraphQLString, GraphQLSchema, GraphQLNonNull, GraphQLFloat } = require('graphql');
const { sequelize, Cotizacion } = require('./database');

// Definir el tipo Cotizacion
const CotizacionType = new GraphQLObjectType({
    name: 'Cotizacion',
    fields: {
        id: { type: GraphQLString },
        fecha: { type: GraphQLString },
        cotizacion: { type: GraphQLFloat },
        cotizacion_oficial: { type: GraphQLFloat }
    }
});

//obtener una cotización por fecha
const RootQuery = new GraphQLObjectType({
    name: 'RootQueryType',
    fields: {
        cotizacion: {
            type: CotizacionType,
            args: { fecha: { type: new GraphQLNonNull(GraphQLString) } },
            resolve(parent, args) {
                return Cotizacion.findOne({
                    where: sequelize.where(sequelize.fn('DATE', sequelize.col('fecha')), args.fecha)
                });
            }
        }
    }
});

// agregar una nueva cotización
const Mutation = new GraphQLObjectType({
    name: 'Mutation',
    fields: {
        addCotizacion: {
            type: CotizacionType,
            args: {
                fecha: { type: new GraphQLNonNull(GraphQLString) },
                cotizacion: { type: new GraphQLNonNull(GraphQLFloat) },
                cotizacion_oficial: { type: new GraphQLNonNull(GraphQLFloat) }
            },
            resolve(parent, args) {
                return Cotizacion.create({
                    fecha: args.fecha,
                    cotizacion: args.cotizacion,
                    cotizacion_oficial: args.cotizacion_oficial
                });
            }
        }
    }
});

// Exportar el esquema
module.exports = new GraphQLSchema({
    query: RootQuery,
    mutation: Mutation
});
