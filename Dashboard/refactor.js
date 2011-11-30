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

function Controller(options) {
    var onLinkClickedListener;

    function onLinkClicked(listener) {
        onLinkClickedListener = listener;
    }

    options.view.onLinkClicked(function (id) {
        onLinkClickedListener(id);
    });

    return {
        onLinkClicked: onLinkClicked // This will be called by the view
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

