quizApp.controller("questionController", function ($scope, $http, $log) {

  $scope.exam = {};
  $scope.buttons = {
    backButtonDisabled: true
  };


  $scope.getExam = function (id) {
    $http.get("../../Question/Exam/" + id)
			.success(function (response) {
			  $scope.exam = response;
			  $scope.setButtonState();
			})
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }

  $scope.setButtonState = function () {
    $scope.buttons.backButtonDisabled = _.first($scope.exam.Quiz.Questions).QuestionID === $scope.exam.CurrentQuestionID;
  }

  $scope.nextQuestion = function () {
    $http.post("../../Question/Answer", {
      ExamID: $scope.exam.ExamID,
      QuestionID: $scope.exam.CurrentQuestionID,
      Answers: [1, 2, 3]
      })
			.success(function (response) {			  
			  $scope.setButtonState();
			})
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }


  $scope.previousQuestion = function () {
  }
});