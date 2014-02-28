
var NoteApp = angular.module("NoteApp", ["ngResource", "ngRoute"]).
    config(function($routeProvider) {
        $routeProvider.
            when('/', { controller: ListCtrl, templateUrl: 'list.html' }).
            when('/new', { controller: CreateCtrl, templateUrl: 'details.html' }).
              when('/edit/:editId', { controller: EditCtrl, templateUrl: 'details.html' }).
            otherwise({ redirectTo: '/' });
    });
    
   
NoteApp.factory('Note', function($resource) {
    return $resource('/api/Note/:id', { id: '@id' }, { update: { method: 'PUT' } });
});

var EditCtrl = function ($scope, $location, $routeParams, Note) {
    $scope.action = "Update";
    var id = $routeParams.editId;
    $scope.note = Note.get({ id: id });

    $scope.save = function() {
        Note.update({ id: id }, $scope.note, function() {
            $location.path('/');
        });
    };
};
var CreateCtrl = function ($scope, $location, Note) {
    $scope.action = "Add";
    $scope.save = function () {
        
        Note.save($scope.note, function() {
            $location.path('/');
        });
    };
};

var ListCtrl = function ($scope, $location, Note) {
    $scope.search = function () {
        Note.query({
            q: $scope.query,
            sort: $scope.sort_order,
            desc: $scope.is_desc,
            offset: $scope.offset,
            limit: $scope.limit,
            
        },
            function (data) {
              
                $scope.more = data.length === 20;
               
                $scope.notes = $scope.notes.concat(data);
            }
        );
    };
    
    $scope.sort = function (col) {
        if ($scope.sort_order == col) {
            $scope.is_desc = !$scope.is_desc;
        } else {

            $scope.sort_order = col;
            $scope.is_desc = false;
        }
        $scope.sort_order = col;
        $scope.reset();
    };

    $scope.reset = function () {
       
        $scope.limit = 20;
        $scope.offset = 0;
        $scope.notes = [];
        $scope.more = true;
        $scope.search();
    };


    $scope.show_more = function() {
        $scope.offset += $scope.limit;
        $scope.search();
    };
    
    $scope.has_more = function () {
       
        return $scope.more;
    };

    $scope.delete = function () {
        var id = this.note.Id;
        Note.delete({id: id}, function() {
            $('#note_'+ id).fadeOut();
        });
    };
    
    $scope.sort_order = "CreateDate";
    $scope.is_desc = false;
   
    
    $scope.reset();
};





/*Inside controller something is automatically available, that is $scope.
  This is javascript object wih special property and behaviour.
  ANything inside $scope is automatically bound to HTML page.
*/



