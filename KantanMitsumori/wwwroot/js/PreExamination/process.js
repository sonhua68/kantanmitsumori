var ua = (function() {
    return {
        ltIE7 : typeof window.addEventListener == "undefined" && typeof document.querySelectorAll == "undefined",
        ltIE8 : typeof window.addEventListener == "undefined" && typeof document.getElementsByClassName == "undefined"
    }
})();

//document.write('<script type="text/javascript" src="' + settings.context + 'process.js"></script>');