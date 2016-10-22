'use strict';
var app = angular.module('myApp', ['ngRoute', 'smart-table', 'ngAnimate', 'ivh.treeview', 'ui.bootstrap', 'ngFileUpload', 'ngTinyScrollbar', 'fancyboxplus', 'blockUI', 'anguFixedHeaderTable', 'textAngular', 'googlechart', 'ui.select', 'ngSanitize'])
.config(function ($routeProvider, $locationProvider, blockUIConfig) {


    var id = ':ViewId';
    $routeProvider
        .when('/',
        {
            //redirectTo: '/'
            templateUrl: 'Home/Index'
        })

        .when('/Index',
        {
            templateUrl: 'Home/Index'
        })

        .when('/RedirectToMain',
        {
            templateUrl: 'Home/RedirectToMain'
        })
         .when('/Error',
        {

            controller: function () {
                toastr.error('sdsddad');
            },
            //templateUrl: $location.path()
            template: 'You do not have permission'
        })

        .when('/Login',
        {
            templateUrl: '/Account/Login'
        })
        .when('/Account/Login',
        {
            templateUrl: '/Account/Login'
        })
        .when('/Contact',
        {
            templateUrl: '/Home/Contact'
        })
        .when('/Register',
        {
            templateUrl: '/Account/Register',
            controller: 'RegisterController'
        })
         .when('/About',
        {
            templateUrl: '/Home/About'
        })
        .when('/ChangePassword',
        {
            templateUrl: '/Account/Manage'
        })
         .when('/Nutritions',
        {
            templateUrl: '/Nutrition/Nutritions',
            controller: 'NutritionController',
           
        })
         .when('/EditNutrition/:id',
        {
            templateUrl: '/Nutrition/EditNutrition',
            controller: 'NutritionController',
           
        })
         .when('/Ingredients',
        {
            templateUrl: '/Ingredient/Ingredients',
            controller: 'IngredientController',
        })
        .when('/IngredientList',
        {
            templateUrl: '/Ingredient/IngredientList',
            controller: 'IngredientController',
        })
         .when('/EditIngredient/:id',
        {
            templateUrl: '/Ingredient/EditIngredient',
            controller: 'IngredientController',
        })
         .when('/NutritionFacts',
        {
            templateUrl: '/NutritionFacts/NutritionFacts',
            controller: 'NutritionFactsController'
        })
         .when('/StandardRecipe',
        {
            templateUrl: '/StandardRecipe/StandardRecipe',
            controller: 'StandardRecipeController'
        })
         .when('/Calculator',
        {
            templateUrl: '/Calculator/Calculator',
            controller: 'CalculatorController'
        })
          .when('/Unit',
        {
            templateUrl: '/Unit/AddUnit',
            controller: 'UnitController'
        })
         .when('/EditUnit/:id',
        {
            templateUrl: '/Unit/EditUnit',
            controller: 'UnitController',
        })
        
        //If link not found
    .otherWise
    {
        redirectTo: '/'
    }


});

app.filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    }
});

app.filter('startsWithLetter', function () {
    return function (items, letter) {
        var filtered = [];
        var letterMatch = new RegExp(letter, 'i');
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (letterMatch.test(item.name.substring(0, 1))) {
                filtered.push(item);
            }
        }
        return filtered;
    };
});