/// <binding />
'use strict';

module.exports = function (grunt) {

    // Time how long tasks take. Can help when optimizing build times
    require('time-grunt')(grunt);
    require('load-grunt-tasks')(grunt);

    grunt.initConfig({

        sass: {
            dist: {
                options: {
					includePaths: [
						'src/styles/govuk_template/assets/stylesheets',
						'src/styles/govuk_frontend_toolkit/stylesheets'
					  ],
                    outputStyle: 'compressed',
                    noCache: true,
                    sourceMap: false,
                    precision: 3
                },
                files: [{
                    expand: true,
                    cwd: 'src/styles/sass/',
                    src: '*.scss',
                    dest: 'dist/css/',
                    ext: '.css'
                }]
            }
        },

        autoprefixer: {
            options: {
                browsers: ['last 2 versions', 'ie 9'],
                cascade: false,
                map: false
            },
            dist: {
                files: [{
                    expand: true,
                    cwd: 'dist/css/',
                    src: '{,*/}*.css',
                    dest: 'dist/css/'
                }]
            }
        },

        svgmin: {
            dist: {
                files: [{
                    expand: true,
                    cwd: 'src/images/',
                    src: '{,*/}*.svg',
                    dest: 'dist/images/'
                }]
            }
        },

        // Watches files for changes and runs tasks based on the changed files
        watch: {
            styles: {
                    files: ['src/styles/{,*/}*.scss'],
                    tasks: ['sass', 'autoprefixer']
            },

            svg: {
                    files: ['src/images/{,*/}*.svg'],
                    tasks: ['svgmin']
            }
        }
    });

    grunt.loadNpmTasks('grunt-autoprefixer');

    grunt.registerTask('build', [
      'sass',
      'autoprefixer',
      'svgmin'
    ]);

    grunt.registerTask('default', [
      'build'
    ]);
};