/// <reference path="../../../scripts/typings/durandal/durandal.d.ts" />
/// <reference path="../../../scripts/typings/knockout/knockout.d.ts" />
import app = require('durandal/app');
import ko = require('knockout');

module Woorkie.App.Home.ViewModels {
    export class HomeViewModel {
        name: KnockoutObservable<string>;

        constructor() {
            this.name = ko.observable(null);
        }

        public sayHello() {
            app.showMessage('Hello ' + this.name() + '! Nice to meet you.', 'Greetings');
        }
    }
}

return Woorkie.App.Home.ViewModels.HomeViewModel;