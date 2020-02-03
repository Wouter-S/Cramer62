const webpack = require('webpack')
const path = require("path");
const VueLoaderPlugin = require('vue-loader/lib/plugin')


module.exports = {
    mode: "development",
    plugins: [
        new VueLoaderPlugin()
    ],
    entry: {
        admin: [
            'vue',
            "./wwwroot/js/index.admin.js",
        ],
        touch: [
            'vue',
            "./wwwroot/js/index.js",
        ],
        mobile: [
            'vue',
            "./wwwroot/js/index.mobile.js",
        ]
    },
    resolve: {
        alias: {
            "vue$": "vue/dist/vue.esm.js",
            "axios$": "axios/dist/axios.min.js"
        },
    },
    output: {
        filename: "./wwwroot/dist/js/bundle.[name].js",
        path: path.resolve(__dirname, ""),
        libraryTarget: 'var',
        library: 'lib'
    },
    externals: [
        {
            "window": "window"
        }
    ],

    module: {
        rules: [
            {
                // vue-loader config to load `.vue` files or single file components.
                test: /\.vue$/,
                loader: 'vue-loader',
                options: {
                    loaders: {
                        // https://vue-loader.vuejs.org/guide/scoped-css.html#mixing-local-and-global-styles
                        css: ['vue-style-loader', {
                            loader: 'css-loader',
                        }],
                        js: [
                            'babel-loader',
                        ],
                    },
                    cacheBusting: true,
                },
            },
            {
                // This is required for other javascript modules you are gonna write besides vue.
                test: /\.js$/,
                loader: 'babel-loader',
                include: [
                    require.resolve('bootstrap-vue') // Whitelist bootstrap-vue

                    //resolve('src'),
                    //resolve('node_modules/webpack-dev-server/client'),
                ],
            },


            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ]


            }
        ],
    }
};