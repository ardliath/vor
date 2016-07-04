quizApp.controller("questionController", function ($scope, $http, $log) {
  $scope.Message = "Hello World";

  $scope.question = {};

  $scope.getQuestion = function(id) {
    $http.get("../../Question/Question/" + id)
			.success(function (response) {
        $scope.question = response;
      })
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }
});