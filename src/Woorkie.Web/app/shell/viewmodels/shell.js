define(["require", "exports", "plugins/router"], function(require, exports, router) {
    var Woorkie;
    (function (Woorkie) {
        (function (App) {
            (function (Shell) {
                (function (ViewModels) {
                    var ShellViewModel = (function () {
                        function ShellViewModel() {
                            this.router = router;
                        }
                        ShellViewModel.prototype.activate = function () {
                            this.router.map([
                                { route: "", title: "Home", moduleId: "home/viewmodels/home", nav: true }
                            ]).buildNavigationModel();

                            return router.activate();
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
