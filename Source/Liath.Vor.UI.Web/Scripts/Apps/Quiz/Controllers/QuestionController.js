quizApp.controller("questionController", function ($scope, $http, $log) {

  $scope.thisExam = {};
  $scope.buttons = {
    backButtonDisabled: true
  };


  $scope.getExam = function (id) {
    $http.get("../../Question/Exam/" + id)
			.success(function (response) {
			  $scope.createExamFromResponse(response);
			  $scope.setButtonState();
			})
			.error(function (data, status, headers, config) {
			  $log.error(data, status, headers, config);
			});
  }

  $scope.createExamFromResponse = function (response) {    
    $scope.thisExam = response.Quiz;
    $scope.thisExam.examID = response.ExamID;
    $scope.thisExam.currentQuestionID = response.CurrentQuestionID;

    _.each($scope.thisExam.Questions, function (question) {
      if (question.Type === 0) { // if it's exclusive
        question.selectedOption = $scope.getExclusiveSelectedOption(question, response.Answers);
      } else {
        _.each(question.Options, function(option) {
          option.selected = $scope.isOptionSelected(question, option, response.Answers);
        });
      }
    });
  }

  $scope.getExclusiveSelectedOption = function (thisQuestion, allAnswers) {

    var val = -1;
    _.each(allAnswers, function (answer) {
      if (thisQuestion.QuestionID === answer.QuestionID) {
        _.each(answer.AnswerOptions, function (selectedOption) {
          val = selectedOption.OptionID;
        });
      }
    });

    return val;
  }

  $scope.isOptionSelected = function (thisQuestion, thisOption, allAnswers) {

    var val = false;
    _.each(allAnswers, function(answer) {
      if (thisQuestion.QuestionID === answer.QuestionID) {
        _.each(answer.AnswerOptions, function (selectedOption) {
          if (selectedOption.OptionID === thisOption.OptionID) {
            val = true;
          }
        });
      }
    });

    return val;
  }

  $scope.setButtonState = function () {
    $scope.buttons.backButtonDisabled = _.first($scope.thisExam.Questions).QuestionID === $scope.thisExam.currentQuestionID;
  }

  $scope.nextQuestion = function () {
    $scope.submitAnswer(true);
  }

  $scope.submitAnswer = function (isForwards) {

    var selectedOptions = new Array();
    _.each($scope.thisExam.Questions, function(question) {
      if (question.QuestionID === $scope.thisExam.currentQuestionID) {
        if (question.Type === 0) { // exlusive
          selectedOptions.push(question.selectedOption);
        } else {
          _.each(question.Options, function(option) {
            if (option.selected) {
              selectedOptions.push(option.OptionID);
            }
          });
        }
      }
    });

    $http.post("../../Question/Answer", {
      ExamID: $scope.thisExam.examID,
      QuestionID: $scope.thisExam.currentQuestionID,
      IsForwards: isForwards,
      Options: selectedOptions
    })
			.success(function (response) {
			  $scope.thisExam.currentQuestionID = response.NextQuestionID;
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