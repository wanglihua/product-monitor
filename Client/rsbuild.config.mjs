import { defineConfig } from '@rsbuild/core';
import { pluginReact } from '@rsbuild/plugin-react';

// https://rsbuild.dev/zh/config/index

export default defineConfig({
    dev: {
        hmr: true,
    },
    html: {
        template: "./index.html",
    },
    output: {
        cleanDistPath: true,
        filenameHash: 'contenthash:8',
        distPath: {
            root: '../Web/wwwroot',
            html: './',
            js: 'static/js',
            css: 'static/css',
            font: 'static/font',
            image: 'static/image',
            assets: 'static/assets',
        },
        copy: [
            { from: "./public/favicon.ico", to: "favicon.ico" },
        ],
    },
    performance: {
        chunkSplit: {
            strategy: 'split-by-experience',
        },
    },
    plugins: [
        pluginReact(),
    ],
    server: {
        base: '/',
        open: true,
        port: 3000,
        printUrls: true,
        proxy: {
            '/api': 'http://localhost:5001',
        },
        publicDir: {
            copyOnBuild: false,
            name: './public',
        },
    },
    source: {
        entry: {
            index: './src/index.js',
        }
    },
});
