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
    $scope.submitAnswer(true);
  }

  $scope.submitAnswer = function (isForwards) {

    var selectedOptions = new Array();
    _.each($scope.exam.Quiz.Questions, function(question) {
      if (question.QuestionID === $scope.exam.CurrentQuestionID) {
        _.each(question.Options, function(option) {
          if (option.selected) {
            selectedOptions.push(option.OptionID);
          }
        });
      }
    });

    $http.post("../../Question/Answer", {
      ExamID: $scope.exam.ExamID,
      QuestionID: $scope.exam.CurrentQuestionID,
      IsFowards: isForwards,
      Options: selectedOptions
    })
			.success(function (response) {
			  $scope.setButtonState();
			})
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }

  $scope.previousQuestion = function () {
    $scope.submitAnswer(false);
  }
});