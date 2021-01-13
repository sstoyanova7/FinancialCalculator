const path = require("path");
const HtmlWebpackPlugin = require('html-webpack-plugin')

const CURRENT_PATH = __dirname;

module.exports = {
    mode: "development",

    stats: { modules: false },

    context: CURRENT_PATH,

    devtool: "source-map",

    entry: {
        app: "./wwwroot/src/index.js"
    },

    performance: {
        hints: false
    },

    output: {
        filename: "[name].js",
        publicPath: "dist/",
        path: path.join(CURRENT_PATH, "./wwwroot/dist"),
        library: "financialcalculator"
    },



    plugins: [
        new HtmlWebpackPlugin()
    ],

    resolve: {
        extensions: [".js", ".jsx", ".css"],
        alias: {
        }
    },

    optimization: {
        runtimeChunk: "single",
        splitChunks: {
            cacheGroups: {
                vendor: {
                    test: /[\\/]node_modules[\\/]/,
                    name: "vendor",
                    chunks: "all"
                }
            }
        }
    },

    module: {
        rules: [
            {
                test: /\.m?jsx?$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-react'],
                        plugins: ['@babel/plugin-proposal-class-properties']
                    }
                }
            },
            {
                test: /\.css$/i,
                use: ['style-loader', 'css-loader'],
                exclude: /node_modules/
            },
            {
                test: /\.(png|jpe?g|gif)$/,
                loader: "file-loader",
                exclude: /node_modules/
            }
        ]
    }
};