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

function View() {
	var onLinkClickedListener;

	function onLinkClicked(listener) {
		onLinkClickedListener = listener;
	}

	function init() {
		$(".theLink").click(function () {
			onLinkClickedListener($(this).attr("id"));
		});
	}

	return {
		onLinkClicked: onLinkClicked
	}

}

function Event(name) {
    return function(){
        if(typeof(arguments[0]) === "function"){
            this[name+"Listener"] = arguments[0];
        }
        else if(typeof(this[name+"Listener"]) === "function"){
            this[name+"Listener"].apply(this, arguments);
        }
    }
}

function View() {
    var exp;

    function init() {
        $(".theLink").click(function (e) {
            exp.onLinkClicked($(this).attr("id"));
        });
    }
    
    init();

    exp = {
        onLinkClicked:Event("onLinkClicked")
    }

    return exp;
}

function Controller(options) {
    var exp;

    options.view.onLinkClicked(function (id) {
        exp.onLinkClicked(id);
    });

    exp = {
        onLinkClicked:Event("onLinkClicked")
    }

    return exp;
}

function Mediator(options) {

    var view = new options.viewConstructor();
    var otherController = new options.otherControllerConstructor();
    var controller = new options.controllerConstructor({ view: view });

    controller.onLinkClicked(function (id) {
        otherController.onLinkClicked(id);
    });

}

