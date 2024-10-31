const express = require('express');
const { graphqlHTTP } = require('express-graphql');
const schema = require('./schema');
const { sequelize } = require('./database'); // Corrige el nombre a "sequelize"

const app = express();

app.use('/graphql', graphqlHTTP({
    schema: schema,
    graphiql: true
}));

sequelize.authenticate() // Cambia "sequilize" a "sequelize"
    .then(() => {
        console.log('Database connected');
        app.listen(4000, () => {
            console.log('Server running on http://localhost:4000/graphql');
        });
    })
    .catch(err => {
        console.error('Unable to connect to the database:', err);
    });
