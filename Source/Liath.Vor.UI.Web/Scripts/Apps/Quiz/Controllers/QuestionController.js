quizApp.controller("questionController", function ($scope, $http, $log) {  

  $scope.exam = {};
  $scope.buttons = {
    backButtonDisabled: true
  };


  $scope.getExam = function(id) {
    $http.get("../../Question/Exam/" + id)
			.success(function (response) {
			  $scope.exam = response;
        $scope.setButtonState();
      })
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }

  $scope.setButtonState = function() {
    $scope.buttons.backButtonDisabled = _.first($scope.exam.Quiz.Questions).QuestionID === $scope.exam.CurrentQuestionID;
  }
});