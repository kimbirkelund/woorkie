/// <reference path="../scripts/typings/durandal/durandal.d.ts" />
/// <reference path="../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../scripts/typings/requirejs/require.d.ts" />
require.config({
    paths: {
        'text': '/Scripts/text',
        'durandal': '/Scripts/durandal',
        'plugins': '/Scripts/durandal/plugins',
        'transitions': '/Scripts/durandal/transitions',
        "knockout": "/Scripts/knockout-3.1.0",
        "jquery": "/Scripts/jquery-1.9.1"
    }
});

define(['durandal/app', 'durandal/viewLocator', 'durandal/system'], boot);

function boot(app, viewLocator, system) {
    system.debug(true);

    app.title = 'Durandal Starter Kit';

    app.configurePlugins({
        router: true,
        dialog: true
    });

    app.start()
        .then(() => {
            viewLocator.useConvention();

            app.setRoot("shell/viewmodels/shell");
        });
}