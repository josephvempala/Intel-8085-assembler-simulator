const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports={
    mode:'development',
    entry: './src/index.js',
    module:{
        rules:[
            {
                test: /\.ts$/,
                use: 'ts-loader',
                include: [path.resolve(__dirname,'src')]
            }
        ]
    },
    resolve:{
        extensions:['.ts','.js']
    },
    output:{
        filename: 'bundle.[contenthash].js',
        path: path.resolve(__dirname, '../wwwroot')
    },
    plugins:[
        new HtmlWebpackPlugin({
        filename:'./index.html',
        template:'./src/index.html'
    })],
};