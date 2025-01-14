﻿//设置工作路径
var ASSETS_PATH = "/public/assets";
$js.WORKPATH = "/public/assets/js/";
window.jr = $js;
window.j6 = $js;
window.cms = $js;

//加载插件/库文件
function ld(libName, path) {
  (function (j, _path) {
    j.xhr.get(
      { url: _path + libName + ".js", async: false, random: false },
      function (script) {
        j.eval(script);
      }
    );
  })($js, path || $js.WORKPATH);
}
/****************  页面处理事件 **************/
var _scripts = document.getElementsByTagName("SCRIPT");
var _sloc = _scripts[_scripts.length - 1].src; //Script Location
var _hp = {
  //Script Handle Params
  loadUI: $js.request("ui", _sloc) == "1", //load ui lib
  hoverCList: $js.request("hover", _sloc).indexOf("clist") != -1, //Hover Category List
  hoverAList: $js.request("hover", _sloc).indexOf("alist") != -1, //Hover Archive List
  plugins: $js.request("ld", _sloc),
};

var plugins = null;
if (_hp.loadUI) {
  plugins = ["ui", "scrollbar", "scroller", "form"];
} else if (_hp.plugins) {
  plugins = _hp.plugins.split(",");
}
if (plugins) {
  for (var i = 0; i < plugins.length; i++) {
    ld(plugins[i]);
  }
}

/** 加载图标字体 */
function loadIconFont() {
  var c = document.createElement("link");
  c.rel = "stylesheet";
  c.href = ASSETS_PATH + "/iconfont.css";
  document.head.appendChild(c);
}
/** 延迟加载图片 */
var observer = new IntersectionObserver(function (changes) {
  changes.forEach(function (it) {
    if (it.isIntersecting) {
      var container = it.target;
      container.setAttribute("src", container.getAttribute("data-src"));
      observer.unobserve(container);
    }
  });
});

/**                      
 <img class="lazy" src="${page.fpath}/images/lazy_holder.gif" 
 data-src="${page.tpath}/images/map-address.png" alt="">
*/
function lazyObserve() {
  var arr = Array.from(document.querySelectorAll(".lazy"));
  arr.forEach(function (item) {
    observer.observe(item);
  });
}

/** 初始化轮播器 */
function initSwiper() {
  if ($js.$fn("#swiper").len() > 0) {
    // 自动初始化id为swiper的轮播器
    $jr.scroller(
      "swiper",
      { direction: "left", unit: 1200, pagerid: "swiper-indicator" },
      4000
    );
  }
  if ($js.$fn("#notice-swiper").len() > 0) {
    // 自动初始化id为notice的轮播器
    $js.scroller("notice-swiper", { direction: "up", unit: 20 }, 3000);
  }
}

/** 初始化水平切换 */
function initXPage() {
  /**
   *  <div class="container flex gap-x-4 x-page">
        <div class="flex align-center justify-center x-page__left">
          <div class="arraw arraw-left">
            <i class="icon icon-chevronleft"></i>
          </div>
        </div>
        <div class="flex1 x-page__container">
          <div class="x-page__page row-apprise__list">
            <div class="apprise-item">
              <div class="apprise-item__content">
                <div class="bubble">
                  1服务态度好，服务周到，环境优美，值得推荐，有效帮助我解决了很多问题，谢谢！
                  林老师很用心，很负责，服务态度很好！
                  <i class="bubble__bottom"></i>
                </div>
              </div>
              <div class="apprise-item__user flex gap-x-6">
                <div class="apprise-item__user--img">
                  <img
                    src="${page.tpath}/images/profile-photo.png"
                    alt="用户头像"
                  />
                </div>
                <div class="apprise-item__user--name">
                  <div class="text-lg">春天</div>
                  <div class="text-sm">某外贸公司职员</div>
                </div>
              </div>
            </div>
            <div class="apprise-item">
              <div class="apprise-item__content">
                <div class="bubble">
                  2服务态度好，服务周到，环境优美，值得推荐，有效帮助我解决了很多问题，谢谢！
                  林老师很用心，很负责，服务态度很好！
                  <i class="bubble__bottom"></i>
                </div>
              </div>
              <div class="apprise-item__user flex gap-x-6">
                <div class="apprise-item__user--img">
                  <img
                    src="${page.tpath}/images/profile-photo.png"
                    alt="用户头像"
                  />
                </div>
                <div class="apprise-item__user--name">
                  <div class="text-lg">春天</div>
                  <div class="text-sm">某外贸公司职员</div>
                </div>
              </div>
            </div>
          </div>
          <div class="x-page__page row-apprise__list">
            <div class="apprise-item">
              <div class="apprise-item__content">
                <div class="bubble">
                  2服务态度好，服务周到，环境优美，值得推荐，有效帮助我解决了很多问题，谢谢！
                  林老师很用心，很负责，服务态度很好！
                  <i class="bubble__bottom"></i>
                </div>
              </div>
              <div class="apprise-item__user flex gap-x-6">
                <div class="apprise-item__user--img">
                  <img
                    src="${page.tpath}/images/profile-photo.png"
                    alt="用户头像"
                  />
                </div>
                <div class="apprise-item__user--name">
                  <div class="text-lg">春天</div>
                  <div class="text-sm">某外贸公司职员</div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="flex align-center justify-center x-page__right">
          <div class="arraw arraw-right">
            <i class="icon icon-chevronright"></i>
          </div>
        </div>
      </div>
   */
  var xs = $jr.$(".x-page");
  var sc = xs.find(".x-page__container");
  // 设置默认显示的屏数
  sc.attr("page", "0");
  // 获取屏数
  var pages = sc.find(".x-page__page");
  if (pages.len() > 0) {
    // 第一屏显示
    pages.get(0).addClass("x-page__active");
  }
  var changePage = function (p) {
    // 将原来的页面设置为不显示
    var currentPage = parseInt(sc.attr("page"));
    pages.get(currentPage).removeClass("x-page__active");
    // 计算当前页码
    var nowPage = currentPage + p;
    if (nowPage < 0) {
      nowPage = pages.len() - 1;
    }
    if (nowPage > pages.len() - 1) {
      // 如果超出，则第一页
      nowPage = 0;
    }
    pages.each(function (idx, p) {
      if (idx == nowPage) {
        p.addClass("x-page__active");
      } else {
        p.removeClass("x-page__active");
      }
    });
    // 设置当前页码
    sc.attr("page", nowPage);
  };
  var scp = sc.parent();
  if (scp) {
    // 查找上级容器
    scp.find(".x-page__left").click(function () {
      changePage(-1);
    });
    scp.find(".x-page__right").click(function () {
      changePage(1);
    });
  }
}

function initMobileNavigator() {
  var nav = $js.$fn(".toggle-navigator");
  var navToggle = $js.$fn(".nav-toggle");
  var remove = function () {
    nav.removeClass("expanded");
    navToggle.removeClass("expanded");
    navToggle.find("i").attr("class", "fa fa-bars iconfont icon-menu");
  };
  navToggle.click(function () {
    var expand = this.hasClass("expanded");
    if (expand) {
      this.removeClass("expanded");
      nav.removeClass("expanded");
      // nav.slideUp("fast", function () {
      //   nav.css({ display: "none" });
      // });
    } else {
      this.addClass("expanded");
      nav.addClass("expanded");
    }
    this.find("i").attr(
      "class",
      expand
        ? "fa fa-bars iconfont icon-menu"
        : "fa fa-close iconfont icon-cross"
    );
  });
  nav.find(".toggle-navigator__mask").click(remove);
  /** 点击链接关闭菜单 */
  nav.find(".navigator__item").click(remove);
}

/** 加载图标字体及样式 */
loadIconFont();

$js.event.add(window, "load", function () {
  if (_hp.hoverNavi && _auto_navigator_ele) {
    clearInterval(_auto_navigator_timer);
  }

  var loc = window.location.pathname;

  lazyObserve();
  initSwiper();
  initXPage();
  // 绑定手机页面,导航菜单
  initMobileNavigator();
  /****************** 设置分类菜单 *******************/

  //根据className设置Hover状态
  var setHoverByClassName = function (e) {
    var lis = e.childNodes;
    var link;
    var isHovered = false;

    var setHover = function (_loc) {
      for (var i = 0; i < lis.length; i++) {
        if (lis[i].nodeName[0] == "#") continue;
        link = lis[i].getElementsByTagName("A")[0];
        if (link.href.indexOf(_loc) != -1) {
          lis[i].className +=
            lis[i].className.indexOf("current") == -1 ? " current" : "";
          isHovered = true;
          break;
        }
      }
    };

    //全局匹配
    setHover(loc);

    //模糊匹配
    if (!isHovered) {
      var splitIndex = loc.lastIndexOf("/");
      if (splitIndex == loc.length - 1) {
        splitIndex = loc.substring(0, loc.length - 1).lastIndexOf("/");
      }
      setHover(loc.substring(0, splitIndex + 1));
    }
  };

  // 设置CList选中效果 (2012-11-03) **
  var _e_clist = document.getElementsByClassName("clist");
  if (_hp.hoverCList && _e_clist.length != 0) {
    setHoverByClassName(_e_clist[0]);
  }

  // 设置AList选中效果 (2012-11-03) **
  _e_clist = document.getElementsByClassName("alist");
  if (_hp.hoverAList && _e_clist.length != 0) {
    setHoverByClassName(_e_clist[0]);
  }

  // 选项卡

  $js.$fn(".tab").each(function (i, e) {
    var tabFN = function () {
      var t = this;
      var parent = this.parent();
      var active_i = -1;
      var active = function (e, b) {
        if (b) e.addClass("active");
        else e.removeClass("active");
      };
      parent.find(".tab").each(function (i, e) {
        var same = e.raw() == t.raw();
        if (same) active_i = i;
        active(e, same);
      });
      while (parent && parent.find(".frame").len() == 0) {
        parent = parent.parent();
      }
      parent.find(".frame").each(function (i, e) {
        active(e, active_i == i);
      });
    };
    if (e.hasClass("tab-hover") && document.documentElement.offsetWidth > 991) {
      e.mouseover(tabFN);
    } else {
      e.click(tabFN);
    }
  });

  // 将元素绝对定位
  $js.event.add(document, "scroll", function () {
    var scrollTop = document.documentElement.scrollTop;
    // 头部元素自动浮动
    var header = $js.$fn(".header");
    if (!header.hasClass("header-nofix")) {
      if (scrollTop > 0) {
        header.addClass("header-fixed");
      } else {
        header.removeClass("header-fixed");
      }
    }
    // 其他设置了.scroll-link的元素自动浮动
    var fixedArr = $js.$fn(".scroll-link");
    fixedArr.each(function (i, e) {
      var top = e.attr("offsetTop") + e.parent().attr("offsetTop");
      if (scrollTop > top) {
        if (e.css().position !== "fixed") {
          var left = e.attr("offsetLeft");
          var width = e.attr("offsetWidth");
          e.css({
            position: "fixed",
            top: "0",
            left: left + "px",
            width: width + "px",
          });
          e.addClass("scroll-linked");
        }
      } else {
        e.css({
          position: "unset",
          top: "inherit",
          left: "inherit",
          width: "inherit",
        });
        e.removeClass("scroll-linked");
      }
    });
  });

  // 滚动到目标
  var scrollLock = 0;
  $js.$fn(".scroll-to").click(function () {
    if (scrollLock == 1) return;
    scrollLock = 1;
    var target = this.attr("target");
    if (!target) throw '.scoll-top missing attribute "target"';
    //var ele = this;
    var target = $js.$fn(target);
    var offset = parseInt(this.attr("offset") || 0);
    var y = target.attr("offsetTop") + offset;
    var doc = document.documentElement;
    var timer = setInterval(function () {
      var setup = (y - doc.scrollTop) / 5;
      setup = setup > 0 ? Math.floor(setup) : Math.ceil(setup);
      if (Math.abs(setup) == 0) {
        doc.scrollTop = y;
        clearInterval(timer);
        scrollLock = 0;
      } else {
        doc.scrollTop += setup;
      }
    }, 10);
  });

  // Exchange
  $js.$fn(".ui-exchange").each(function (i, e) {
    e = e.raw();
    var v = null;
    var d = null;
    var f = null;
    var g = "exchange";
    switch (e.nodeName) {
      case "IMG":
        f = "src";
        break;
      default:
        f = "innerHTML";
        break;
    }
    if (f == null) return;
    v = e[f];
    d = e.getAttribute(g);
    if (d) {
      $js.event.add(
        e,
        "mouseover",
        (function (a, b, c) {
          return function () {
            a[b] = c;
          };
        })(e, f, d)
      );
      $js.event.add(
        e,
        "mouseout",
        (function (a, b, c) {
          return function () {
            a[b] = c;
          };
        })(e, f, v)
      );
    }
  });
  // 初始化wow.js
  if (window.WOW && !window.wowInit) {
    new WOW().init();
    window.wowInit = true;
  }
});

/***********************  设置自动时间  ***********************/
/*
var ele_dts = document.getElementsByClassName('autotime');
var weeks = new Array('日', '一', '二', '三', '四', '五', '六');
var cmath = function (v) {
    if (v < 10) return '0' + v;
    return v;
};
var setDate = function () {
    var dt = new Date();
    var str = cmath(dt.getFullYear()) + '年' + cmath(dt.getMonth() + 1) + '月' + cmath(dt.getDate()) + '日&nbsp;/&nbsp;周' +
          weeks[dt.getDay()] + '&nbsp;' + cmath(dt.getHours()) + ':' + cmath(dt.getMinutes()) + ':' + cmath(dt.getSeconds());

    for (var i = 0; i < ele_dts.length; i++) {
        ele_dts[i].innerHTML = str;
    }
};
if (ele_dts.length != 0) {
    setDate();
    setInterval(setDate, 1000);
}
*/
