module.exports = function (grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        less: {
            dev: {
                options: {
                    sourceMap: true,
                    dumpLineNumbers: 'comments',
                    relativeUrls: true,
                    plugins: [
                            new (require('less-plugin-autoprefix'))({ browsers: ["last 2 versions"] })
                    ]
                },
                files: {
                    'Content/bootstrap.debug.css': 'Content/bootstrap.less',
                    'Content/select2.debug.css': 'Content/select2.less',
                    //'Content/Site.debug.css': 'Content/Site.less',
                }
            },
            production: {
                options: {
                    sourceMap: true,
                    compress: true,
                    relativeUrls: true,
                    plugins: [
                            new (require('less-plugin-autoprefix'))({ browsers: ["last 2 versions"] })
                    ]
                },
                files: {
                    'Content/bootstrap.css': 'Content/bootstrap.less',
                    'Content/select2.css': 'Content/select2.less',
                    //'Content/Site.css': 'Content/Site.less',
                }
            }
        },

    });

    grunt.loadNpmTasks('grunt-contrib-less');

    grunt.registerTask('default', ['less']);
    grunt.registerTask('production', ['less:production']);
    grunt.registerTask('dev', ['less:dev']);
};

// https://www.orderfactory.com/articles/Bootstrap-LESS-Grunt-VS.html
// cd $(ProjectDir)
// call grunt --no-color