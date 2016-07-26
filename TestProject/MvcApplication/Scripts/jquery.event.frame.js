// jquery.events.frame.js
// 1.1 - lite
// Stephen Band
// 
// Project home:
// webdev.stephband.info/events/frame/
//
// Source:
// http://github.com/stephband/jquery.event.frame

(function (e, t) {
    function r(e, t) {
        function i() { n.frameCount++; e.call(n) }
        var n = this, r;
        this.frameDuration = t || 25;
        this.frameCount = -1;
        this.start = function () {
            i(); r = setInterval(i, this.frameDuration)
        };
        this.stop = function () { clearInterval(r); r = null }
    }
    function i() {
        var t = e.event.special.frame.handler, n = e.Event("frame"),
            r = this.array, i = r.length;
        n.frameCount = this.frameCount;
        while (i--) { t.call(r[i], n) }
    }
    var n;
    if (!e.event.special.frame) {
        e.event.special.frame = {
            setup: function (e, t) {
                if (n) { n.array.push(this) }
                else {
                    n = new r(i, e && e.frameDuration); n.array = [this];
                    var s = setTimeout(function () { n.start(); clearTimeout(s); s = null }, 0)
                }
                return
            },
            teardown: function (e) {
                var r = n.array, i = r.length;
                while (i--) {
                    if (r[i] === this) { r.splice(i, 1); break }
                }
                if (r.length === 0) { n.stop(); n = t } return
            },
            handler: function (t) { e.event.handle.apply(this, arguments) }
        }
    }
})(jQuery)