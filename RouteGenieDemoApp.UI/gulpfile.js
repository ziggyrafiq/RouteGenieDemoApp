/// <binding />

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
const imagemin = require('gulp-imagemin');
var replace = require('gulp-replace');


// File paths
const files = {
    scssPath: 'App_Dev_Assets/Scss/**/*.scss',
    jsPath: 'App_Dev_Assets/Js/**/*.js',
    imagePath: 'App_Dev_Assets/Images/**/*'
};

// Sass task: compiles the style.scss file into app-style.css
function scssTask() {
    return src(files.scssPath)
        .pipe(sourcemaps.init())
        .pipe(sass()) // compile SCSS to CSS
        //.pipe(postcss([autoprefixer(), cssnano()])) 
        //.pipe(sourcemaps.write('.'))
        .pipe(dest('Content')
        ); 
}

// JS task: concatenates and uglifies JS files to script.js
function jsTask() {
    return src([files.jsPath])
        .pipe(concat('app-script.js'))
        .pipe(uglify())
        .pipe(dest('Scripts')
        );
}


//image task
function imageTask() {
    return src([files.imagePath])
        .pipe(imagemin({
            progressive: true,
            optimizationLevel:7
        }))
        .pipe(dest('Content/Images')
        );
}


function watchTask() {
    watch([files.scssPath, files.jsPath, files.imagePath],
        { interval: 1000, usePolling: true }, //Makes docker work
        series(
            parallel(scssTask, jsTask, imageTask)

        )
    );
}
// Export the default Gulp task so it can be run
// Runs the scss, js & image tasks simultaneously, 
//then watch task
exports.default = series(
    parallel(scssTask, jsTask, imageTask),
    watchTask
);