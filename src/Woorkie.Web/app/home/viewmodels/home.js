define(["require", "exports", 'durandal/app', 'knockout'], function(require, exports, app, ko) {
    var Woorkie;
    (function (Woorkie) {
        (function (App) {
            (function (Home) {
                (function (ViewModels) {
                    var HomeViewModel = (function () {
                        function HomeViewModel() {
                            this.name = ko.observable(null);
                        }
                        HomeViewModel.prototype.sayHello = function () {
                            app.showMessage('Hello ' + this.name() + '! Nice to meet you.', 'Greetings');
                        };
                        return HomeViewModel;
                    })();
                    ViewModels.HomeViewModel = HomeViewModel;
                })(Home.ViewModels || (Home.ViewModels = {}));
                var ViewModels = Home.ViewModels;
            })(App.Home || (App.Home = {}));
            var Home = App.Home;
        })(Woorkie.App || (Woorkie.App = {}));
        var App = Woorkie.App;
    })(Woorkie || (Woorkie = {}));

    return Woorkie.App.Home.ViewModels.HomeViewModel;
});
//# sourceMappingURL=home.js.map
