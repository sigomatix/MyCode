// View has link
// Link clicked in View -> Controller notified of the click + id -> Mediator Notified + Id -> Other Controller Notified + Id

function View() {
    var controller;

    function setController(newController) {
	    controller = newController;
    }

    function init() {
	    $(".theLink").click(function () {
		    controller.onLinkClicked($(this).attr("id"));
	    });
    }

    init();

    return {
	    setController:setController
    }

}

function Controller(options) {

	function onLinkClicked(id) {
		options.onLinkClicked(id); // This will notify the nediator
	}

	return {
		onLinkClicked: onLinkClicked // This will be called by the view
	}
}

function OtherController(options) {

	function onLinkClicked(id) {
		alert("I will do something with " + id);
	}

	return {
		onLinkClicked: onLinkClicked // This will be called by the view
	}
}

function Mediator(options) {
	var view = new options.viewConstructor();
	var otherController = new options.otherControllerConstructor();
	var controller = new options.controllerConstructor({
		onLinkClicked: function (id) {
				       otherController.onLinkClicked(id);
			       }
	});
	view.setController(controller);
}


////////
function Event() {
    var listener;

    return function(){
        if(typeof(arguments[0]) === "function"){
            listener = arguments[0];
        }
        else if(typeof(listener) === "function"){
            listener.apply(this, arguments);
        }
    }
}

function View() {
    var onLinkClicked = Event();

    function init() {
	$(".theLink").click(function (e) {
	    onLinkClicked($(this).attr("id"));
	});
    }
    
    init();

    return {
        onLinkClicked:onLinkClicked
    }
}






function Controller(options) {
	var onLinkClicked=Event();

	options.view.onLinkClicked(function (id) {
		onLinkClicked(id);
	});

    return {
        onLinkClicked:onLinkClicked
    }
}

function OtherController(options) {
	var onLinkClicked=Event();

	onLinkClicked(function(id) {
		alert("I will do something with " + id);
	});

	return {
		onLinkClicked: onLinkClicked 
	}
}

function Mediator(options) {
    var view = new options.viewConstructor();
    var otherController = new options.otherControllerConstructor();
    var controller = new options.controllerConstructor({ view: view });
    controller.onLinkClicked(function (id) {
        otherController.onLinkClicked(id);
    });
}


