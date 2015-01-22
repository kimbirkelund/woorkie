/// <reference path="../../../scripts/typings/durandal/durandal.d.ts" />
/// <reference path="../../../scripts/typings/knockout/knockout.d.ts" />
import app = require('durandal/app');
import ko = require('knockout');
import router = require("plugins/router");

module Woorkie.App.Shell.ViewModels {
    export class ShellViewModel {
        private router: DurandalRouter;

        constructor() {
            this.router = router;
        }

        public activate(): JQueryPromise<any> {
            this.router.map([
                { route: "", title: "Home", moduleId: "home/viewmodels/home", nav: true }
            ]).buildNavigationModel();

            return router.activate();
        }
    }
}

return Woorkie.App.Shell.ViewModels.ShellViewModel;