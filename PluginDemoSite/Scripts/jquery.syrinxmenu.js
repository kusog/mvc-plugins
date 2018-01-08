(function ($) {
    function SyrinxMenu(element, options) {
        var self = this,
            $el = $(element),
            op = self.options = $.extend({}, self.defaultOptions, self._getObject($el.data("menuOptions")), options);
        self.$el = $el;
        self._closetimer = null;
        self._openMenus = new Array();

        $el.on("mouseenter", ".SubMenuParent", function (event) {
            var t = $(this),
                tparent = t.offsetParent(".SubMenu").prev(".SubMenuParent"),
                bheight = $(window).height(),
                bwidth = $(window).width(),
                bhScr = $(window).scrollLeft(),
                bvScr = $(window).scrollTop();

            self._closeMenus(tparent, t);
            self._addMenuInArray(t);
            if (!t.hasClass("active"))
                t.addClass("active");
            var cm = t.next(".SubMenu");
            var cmw = cm.width(), cmh = cm.height();

            if (t.hasClass("HorzMenu")) {
                if (t.offset().left + t.width() + cmw > (bwidth + bhScr))
                    cm.css("left", t.position().left - cmw);
                else
                    cm.css("left", t.position().left + t.width())
                cm.css("top", t.position().top);

            }
            else {
                if ((t.offset().top + t.height() + cmh > (bheight + bvScr)) && (t.position().top - cmh >= 0))
                    cm.css("top", t.position().top - cmh);
                else
                    cm.css("top", t.position().top + t.height());

                if (t.offset().left + cmw > (bwidth + bhScr)) {
                    if (t.offset().left + t.width() > (bwidth + bhScr))
                        cm.css("left", bwidth + bhScr - cmw);
                    else
                        cm.css("left", t.position().left + t.width() - cmw);
                }
                else
                    cm.css("left", t.position().left)
            }

            if (cm.hasClass("fadeMenu"))
                cm.fadeIn("def");
            else if (cm.hasClass("slideMenu"))
                cm.slideDown("def");
            else if (cm.hasClass("toggleMenu"))
                cm.toggle("def");
            else
                cm.css("display", "block");
            self._menuItemOpen = true;
            event.stopPropagation();
        }).on("mouseleave", ".SubMenuParent", function (event) {
            var s = $(event.relatedTarget).offsetParent(".SubMenu").prev(".SubMenuParent");
            var tp = $(this).offsetParent(".SubMenu").prev(".SubMenuParent");
            if (s.length == 0 || s.get(0) != tp.get(0)) {
                self._setMenuTimer();
            }
            else if (s.get(0) == tp.get(0)) {
                self._cancelTimer();
                self._closetimer = window.setTimeout(function () { self._closeMenus(tp); }, self.options.timeout);
            }
            else {
                self._closeMenus(s);
            }
        }).on("mouseenter", ".SubMenu", function (event) {
            self._closeMenus($(this).prev(".SubMenuParent").eq(0));
            event.stopPropagation();
        }).on("mouseleave", ".SubMenu", function (event) {
            self._setMenuTimer();
        }).on("click", ".SubMenu", function (event) {
            self._closeMenus();
        });
    }

    $.extend(SyrinxMenu.prototype, {
        defaultOptions: {
            timeout: 300
        },
        _menuItemOpen: false,

        _setMenuTimer: function() {
            var self = this;
            self._cancelTimer();
            self._closetimer = window.setTimeout(function() {self._closeMenus()}, self.options.timeout);
        },

        _closeMenus: function (inMenParent, inMen) {
            var self = this,
                openMenus = self._openMenus;
            if (typeof inMenParent == "number")
                inMenParent = null;
            self._cancelTimer();
            for (var i = openMenus.length - 1; i >= 0; i--) {
                if (inMenParent == null || inMenParent == 0 || inMenParent.get(0) != openMenus[i].get(0)) {
                    if (inMen != null && inMen.get(0) == openMenus[i].get(0))
                        break;
                    openMenus[i].removeClass("active");
                    var cm = openMenus[i].next(".SubMenu");
                    if (cm.hasClass("fadeMenu"))
                        cm.fadeOut("def");
                    else if (cm.hasClass("slideMenu"))
                        cm.slideUp("def");
                    else if (cm.hasClass("toggleMenu"))
                        cm.toggle("def");
                    else
                        cm.css("display", "none");
                    openMenus.pop();
                }
                else if (inMenParent.get(0) == openMenus[i].get(0))
                    break;
            }
            if (openMenus.length == 0)
                self._menuItemOpen = false;
        },

        _cancelTimer: function () {
            var self = this;
            if (self._closetimer) {
                window.clearTimeout(self._closetimer);
                self._closetimer = null;
            }
        },

        _addMenuInArray: function (men) {
            var openMenus = this._openMenus;
            for (var i = 0; i < openMenus.length - 1; i++)
                if (men.get(0) == openMenus[i].get(0))
                    return;
            openMenus.push(men);
        },

        _getObject: function (css) {
            if(typeof(css) == "string")
                return (css != null && css.length != 0) ? eval("(" + css + ")") : null;
            return css;
        }
    });


    $.fn.syrinxMenu = function (op) {
        var passed = Array.prototype.slice.call(arguments, 1);
        var rc = this;
        this.each(function () {
            var plugin = $(this).data('syrinxMenu');
            if (undefined === plugin) {
                var $el = $(this);
                plugin = new SyrinxMenu(this, op);
                $el.data('syrinxMenu', plugin, this.href);
            }
            else if (plugin[op]) {
                rc = plugin[op].apply(plugin, passed);
            }
        });
        return rc;
    }

    $(".syrinx-menu").syrinxMenu({});
})(jQuery);

