var path = require('path');
const TerserPlugin = require("terser-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

var config = {
    entry: ["./wwwroot-src/index.js","./wwwroot-src/css/index.scss"],
    output:{
        path: path.resolve(__dirname,'wwwroot'),
        filename: 'index.js',
        publicPath: '/wwwroot'
    },
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                  {
                    loader: MiniCssExtractPlugin.loader
                  },
                  "css-loader",
                  "sass-loader"
                ]
              }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
          filename: "index.css"
        }),
        new CleanWebpackPlugin()
    ],
}

module.exports = (env, argv) => {
    if (argv.mode === 'development') {
        config.devtool = 'source-map';
    }
    
    if (argv.mode === 'production') {
        config.optimization = {
            minimize: true,
            minimizer: [new TerserPlugin()],
          }
    }
    
    return config;
};