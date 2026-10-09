// wwwroot/js/gameGuard.js

window.gameGuard = {

    enable: function () {

        window.redPandaBeforeUnload = function (e) {

            e.preventDefault();
            e.returnValue = "";

            return "";
        };

        window.addEventListener(
            "beforeunload",
            window.redPandaBeforeUnload);
    },

    disable: function () {

        if (window.redPandaBeforeUnload) {

            window.removeEventListener(
                "beforeunload",
                window.redPandaBeforeUnload);
        }
    }
};