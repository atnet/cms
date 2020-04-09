/* 警告:此文件由系统自动生成,请勿修改,因为可能导致您的更改丢失! */
//
//文件：滚动效果插件
//版本: 1.0
//时间：2011-10-01
//

/******************** DIV SCROLLER ************************/

//创建时间:2011/07/14
function scroller(elemId, params, interval) {

    /* 初始变量值 */
    this.$P = null;                                 //父元素
    this.$C = null;                                 //子元素
    this.$L = null;                                 //子元素列表
    this.pagePanel = null;                          //分条容器，如果设置params.pagerid,则自动启用分条容器

    this.index = 0;                                 //当前显示的图片
    this.offset = 0;                                //要滚动的偏移量
    this.scroll = 0;                                //已经滚动的偏移量
    this.scrollUnit = params.unit || 5;                //每次滚动的单位
    this.lock = false;                              //锁定滚动，用于下一张
    this.direction = params.direction || 'left';    //滚动方向
    this.interval = interval;                       //滚动间隔时间


    this.timer = null;                              //子元素计时器
    this.taskTimer = null;


    //-------------------- 获取元素 ----------------------//

    //父元素
    this.$P = document.getElementById(elemId);
    this.$P.style.cssText += 'overflow:hidden;position:relative;';

    //滚动列表的父元素(第一个UL)
    //UL里的li样式应用padding属性，而不是margin属性

    var _elist = this.$P.getElementsByTagName('UL');
    if (_elist.length == 0) {
        alert('无法找到元素ID为:' + this.$P.getAttribute('id') + '下的UL元素!'); return false;
    } else {
        this.$C = _elist[0];
        this.$L = this.$C.getElementsByTagName('LI');
    }

    //-------------------- 校验方向 ----------------------//

    if (this.direction == 'left') {
        //向左滚动则设置为一行
        var totalWidth = 0;
        var width = 0;
        for (var i = 0; i < this.$L.length; i++) {

            //获取总宽度
            width = this.$L[i].offsetWidth;
            if (width == 0) {
                width = this.$P.offsetWidth;
            }
            totalWidth += width;
            //显示在一行上
            this.$L[i].style.cssText += 'float:left;width:'+width+'px';
            //给子元素的子元素添加索引值,用于定位
            this.$L[i].setAttribute('scroll-index', i);

        }
        this.$C.style.width = totalWidth + 'px';   //是否会两行


        //---------------- 初始化分条显示容器 ----------------//
        this.pagePanel = document.getElementById(params.pagerid);

        if (this.pagePanel) {
            for (var i = 0; i < this.$L.length; i++) {
                var e = document.createElement("A");
                if (i == 0) {
                    e.className = 'current';
                }
                e.innerHTML = (i + 1).toString();
                e.href = 'javascript:;';
                e.onclick = (function (t, _i) {
                    return function () {
                        t.setIndex(_i);
                    };
                })(this, i);

                this.pagePanel.appendChild(e);
            }
        }

    }
    else if (this.direction == 'up') {
        this.scrollUnit = params.unit || null;                //每次滚动的单位
        this.$C.style.cssText += 'float:left';

    } else {
        alert('仅支持方向：left和up'); return false;
    }

    //-------------------- 开始启动 ---------------------//
    //this._start();
    this._restart();

}

scroller.prototype.logger = function (text) {
    var _logger = document.getElementById('scroll-logger');
    if (_logger) {
        _logger.innerHTML = text;
    }
};

scroller.prototype._async = function () {
    //
    //BUG:索引有误，第一次跳转，第2张的索引为0
    //

    //获取图片索引
    this.index = parseInt(this.$L[0].getAttribute('scroll-index'));

    if (this.index == this.$L.length) {
        this.index = 0;
    }

    //显示条数
    this.logger((this.index + 1) + '/' + this.$L.length);

    if (this.pagePanel) {
        var es = this.pagePanel.getElementsByTagName('A');
        for (var i = 0; i < es.length; i++) {
            es[i].className = i == this.index ? 'current' : '';
        }
    }
};


//设置索引
scroller.prototype.setIndex = function (_index) {
    this.lock = true;
    var offset = 0;
    var skipNum = -1;
    for (var i = 0; i < this.$L.length; i++) {
        if (parseInt(this.$L[i].getAttribute('scroll-index')) == _index) {
            var node = this.$L[i];
            this.$C.removeChild(node);
            this.$C.insertBefore(node, this.$L[0]);
            break;
        }
    }
    this.$C.style.marginLeft = '0px';

    this.index = _index;
    this.lock = false;
    this._async();
    this._restart();
};

//上一张
scroller.prototype.prev = function () {
    //如果已经点击则点击无效
    if (this.lock || this.scroll > this.scrollUnit) {
        return false;
    }
    else {
        this.lock = true;
        var t = this;
        this._start(true, true, function () { t.lock = false; });
    }
};

//下一张
scroller.prototype.next = function () {
    //如果已经点击则点击无效
    if (this.lock || this.scroll > this.scrollUnit) {
        return false;
    }
    else {
        this.lock = true;
        var t = this;
        this._start(!true, true, function () { t.lock = false; });
    }
};



//开始
scroller.prototype._start = function (asc, internal, call) {
    if (this.lock && !internal) { return false; }


    if (this.direction == "left") {
        //下一张
        if (!asc) {
            this._sc_left(call);
        } else {
            this._sc_right(call);
        }

        //同步状态
        this._async();
    } else if (this.direction == "up") {
        this._sc_up(call);
    }
};

//重新开始
scroller.prototype._restart = function () {
    var t = this;
    //重复滚动
    if (this.taskTimer) {
        clearTimeout(this.taskTimer);
    }
    this.taskTimer = setTimeout(function () { t._start(); }, this.interval);
};

//向左滚动
scroller.prototype._sc_left = function (call) {
    var ref = this;
    var _offset = ref.$L[0].offsetWidth;

    this.timer = setInterval(function () {

        if (ref.scroll == _offset) {
            clearInterval(ref.timer);
            ref.scroll = 0;

            //添加到后面
            var node = ref.$L[0];
            ref.$C.removeChild(node);
            ref.$C.appendChild(node);

            //执行回执函数
            if (call) { call(); }

            ref._restart();


            ref.$C.style.marginLeft = '0px';
            return false;
        }

        ref.scroll += ref.scrollUnit / 10;

        if (ref.scroll > _offset) {
            ref.scroll = _offset;
        }
        ref.$C.style.marginLeft = (-ref.scroll) + 'px';

    }, 10);

};

//向由滚动
scroller.prototype._sc_right = function (call) {

    //上一张
    var ref = this;
    var _offset = ref.$L[ref.$L.length - 1].offsetWidth;

    //追加到前面
    var node = ref.$C.lastChild;
    ref.$C.removeChild(node);
    ref.$C.insertBefore(node, ref.$L[0]);
    //重设位置
    ref.scroll = -_offset;


    this.timer = setInterval(function () {

        if (ref.scroll == 0) {
            clearInterval(ref.timer);

            //执行回执函数
            if (call) { call(); }

            ref._restart();


            ref.$C.style.marginLeft = '0px';
            return false;
        }

        ref.scroll += ref.scrollUnit / 10;

        if (ref.scroll > 0) {
            ref.scroll = 0;
        }
        ref.$C.style.marginLeft = (ref.scroll) + 'px';
    }, 10);
};

//向由滚动
scroller.prototype._sc_up = function (call) {

    //上一张
    var ref = this;
    var _height = ref.$C.clientHeight;
    var _offset = ref.$L[0].offsetHeight;
    var _scroll = 0;

    this.timer = setInterval(function () {
        if (ref.scroll >= _height) {
            clearInterval(ref.timer);

            var _unit = _height / (1000 / 10 / 2);
            var _timer = setInterval(function () {
                ref.scroll -= _unit;
                if (ref.scroll - _unit < 0) {
                    ref.scroll = 0;
                }
                if (ref.scroll == 0) {
                    clearInterval(_timer);
                    ref._restart();
                }
                ref.$P.scrollTop = ref.scroll;
            }, 10);

            return false;
        }
        //转到一圈
        if (_scroll == _offset) {
            clearInterval(ref.timer);
            ref.scroll += _scroll;
            ref._restart();

        } else {

            //一圈内转动循环
            _scroll += ref.scrollUnit / 10;

            if (_scroll + ref.scrollUnit / 10 > _offset) {
                _scroll = _offset;
            }
            ref.$P.scrollTop = ref.scroll + _scroll;
        }
    }, 10);
};






$jr.extend({
    scroller: function (id, params, interval) {
        return new scroller(id, params, interval);
    }
});