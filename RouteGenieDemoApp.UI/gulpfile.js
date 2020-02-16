﻿/// <binding AfterBuild='default' />
// Initialize modules
// Importing specific gulp API functions lets us write them below as series() instead of gulp.series()
const { src, dest, watch, series, parallel } = require('gulp');
// Importing all the Gulp-related packages we want to use
const sourcemaps = require('gulp-sourcemaps');
const sass = require('gulp-sass');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const postcss = require('gulp-postcss');
const autoprefixer = require('autoprefixer');
const cssnano = require('cssnano');
var replace = require('gulp-replace');


// File paths
const files = {
    scssPath: 'App_Scripts/Scss/**/*.scss',
    jsPath: 'App_Scripts/Js/**/*.js'
};

// Sass task: compiles the style.scss file into style.css
function scssTask() {
    return src(files.scssPath)
        .pipe(sourcemaps.init()) // initialize sourcemaps first
        .pipe(sass()) // compile SCSS to CSS
        .pipe(postcss([autoprefixer(), cssnano()])) // PostCSS plugins
        .pipe(sourcemaps.write('.')) // write sourcemaps file in current directory
        .pipe(dest('Content')
        ); // put final CSS in dist folder
}

// JS task: concatenates and uglifies JS files to script.js
function jsTask() {
    return src([
        files.jsPath
        //,'!' + 'includes/js/jquery.min.js', // to exclude any specific files
    ])
        .pipe(concat('app-script.js'))
        .pipe(uglify())
        .pipe(dest('Scripts')
        );
}

function watchTask() {
    watch([files.scssPath, files.jsPath],
        { interval: 1000, usePolling: true }, //Makes docker work
        series(
            parallel(scssTask, jsTask)

        )
    );
}
// Export the default Gulp task so it can be run
// Runs the scss and js tasks simultaneously, 
//then watch task
exports.default = series(
    parallel(scssTask, jsTask),
    watchTask
);