quizApp.controller("questionController", function ($scope, $http, $log) {  

  $scope.exam = {};

  $scope.getExam = function(id) {
    $http.get("../../Question/Exam/" + id)
			.success(function (response) {
			  $scope.exam = response;
      })
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }
});