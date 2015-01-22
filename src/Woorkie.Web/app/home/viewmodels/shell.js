define(["require", "exports", 'durandal/app', 'knockout'], function(require, exports, app, ko) {
    var Woorkie;
    (function (Woorkie) {
        (function (App) {
            (function (Shell) {
                (function (ViewModels) {
                    var ShellViewModel = (function () {
                        function ShellViewModel() {
                            this.name = ko.observable(null);
                        }
                        ShellViewModel.prototype.sayHello = function () {
                            app.showMessage('Hello ' + this.name() + '! Nice to meet you.', 'Greetings');
                        };
                        return ShellViewModel;
                    })();
                    ViewModels.ShellViewModel = ShellViewModel;
                })(Shell.ViewModels || (Shell.ViewModels = {}));
                var ViewModels = Shell.ViewModels;
            })(App.Shell || (App.Shell = {}));
            var Shell = App.Shell;
        })(Woorkie.App || (Woorkie.App = {}));
        var App = Woorkie.App;
    })(Woorkie || (Woorkie = {}));

    return Woorkie.App.Shell.ViewModels.ShellViewModel;
});
//# sourceMappingURL=shell.js.map
