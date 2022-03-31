const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/api/unp/",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7198',
        secure: false
    });

    app.use(appProxy);
};
