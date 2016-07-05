quizApp.controller("questionController", function ($scope, $http, $log) {  

  $scope.quiz = {};

  $scope.getQuiz = function(id) {
    $http.get("../../Question/Quiz/" + id)
			.success(function (response) {
			  $scope.quiz = response;
      })
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }
});