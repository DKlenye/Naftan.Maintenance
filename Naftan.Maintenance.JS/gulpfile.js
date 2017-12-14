/// <binding AfterBuild='js, js.min, css.min' />
"use strict";

var paths = {
    jsDestination: "../Naftan.Maintenance.WebApplication/Scripts/",
    jsWebixExtensionsSrc:"./src/webix/*.js",
    jsSrc: "./src/**/*.js",
    content: "../Naftan.Maintenance.WebApplication/Content/",
    cssSrc:"./css/**/*.css"
};

var gulp = require('gulp'),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    cssmin = require("gulp-cssmin");


var getJS = function () {
    return [paths.jsWebixExtensionsSrc, paths.jsSrc];
}

gulp.task("js.min", function () {
    return gulp.src(getJS())
        .pipe(concat("maintenance.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.jsDestination));
});

gulp.task("js", function () {
    return gulp.src(getJS())
        .pipe(concat('maintenance_debug.js'))
        .pipe(gulp.dest(paths.jsDestination));
});

gulp.task("css.min", function () {
    return gulp.src([paths.cssSrc])
        .pipe(concat("app.css"))
        .pipe(cssmin())
        .pipe(gulp.dest(paths.content));
});