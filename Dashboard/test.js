test("When the link is clicked Then the controller should be notified", function(){
	appendFixture("<a class='theLink'></a>");

	var view = new View ();
	var controller = {onLinkClicked:stubbedFunction();}
	view.setController(controller);

	$(".thelink").click();

	strictEqual(controller.onLinkClicked.called, true);
});

test("When the controller is notified of a click event Then it should trigger a click event", function(){
	
	var options = {onLinkClicked:stubbedFunction()};
	var controller = new Controller(options);
	controller.onLinkClicked();

	strictEqual(options.onLinkClicked.called, true);
});

test("When the controller trigger a click event then the mediator should have the other controller notified", function(){
	var controller = {};
	var controllerConstructor = function(options){
		controller.options = options;
		return controller;
	}
	var otherController = {onLinkClicked:stubbedFunction()};
	var mediator = new Mediator({
		controllerConstructor:controllerConstructor,
		otherControllerConstructor:function(){return OtherController;}
	});

	controller.options.onLinkClicked();

	strictEqual(otherController.onLinkClicked.called, true);
});
















////

test("When the link is clicked Then a click event should be triggered", function(){
	appendFixture("<a class='theLink'></a>");

	var view = new View ();
	var listener = stubbedFunction();
	view.onLinkClicked(listener);

	$(".thelink").click();

	strictEqual(listener.called, true);
});

test("When the controller is notified of a click event Then it should trigger a click event", function(){
	
	var controller = new Controller();
	var listener = stubbedFunction();
	controller.onLinkClicked(listener);
	controller.onLinkClicked();

	strictEqual(listener.called, true);
});

test("When the controller trigger a click event then the mediator should have the other controller notified", function(){
	var controller = {onLinkClicked:Event()};
	var otherController = {onLinkClicked:Event()};
	var mediator = new Mediator({
		controllerConstructor:function(){return controller;},
		otherControllerConstructor:function(){return OtherController;}
	});

	var listener = stubbedFunction();
	otherController.onLinkClicked(listener);

	controller.onLinkClicked();

	strictEqual(listener.called, true);
});


















