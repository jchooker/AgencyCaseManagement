const path = require('path');

module.exports = {
    //mode: 'development', //change to production for release UPDATE: no mode passed here, passed via CLI flag
                            //..see csproj .NET build config
    entry: {
        //maps your source files to output bundles
        'dashboard-index': './wwwroot/js/views/dashboard/index.js',
        //add more entries here as your project grows
        //'someother-page': './wwwroot/js/views/someother/page.js'
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot/js/dist'), //output to wwwroot/js/dist
        clean: true,

    },
    resolve: {
        extensions: ['.js'],
    },
};