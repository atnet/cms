
/*��ʱ�� */
function timer(times, callback) { this.times = times; this.callback = callback; }
timer.prototype.start = function (func) {
    var t, i = 0, _t = this;
    t = setInterval(function () { i++; if (func) func(); if (i == _t.times) { clearInterval(t); if (_t.callback) _t.callback(); } }, 1);
};


/******************** DIV SCROLLER ************************/

//����ʱ��:2011/07/14
function scroller(elemID, params, interval) {

    /* ��ʼ����ֵ */
    this.$P = null;                                 //��Ԫ��
    this.$C = null;                                 //��Ԫ��
    this.$L = null;                                 //��Ԫ���б�
    this.pagePanel = null;                          //�����������������params.pagerid,���Զ����÷�������

    this.index = 0;                                 //��ǰ��ʾ��ͼƬ
    this.offset = 0;                                //Ҫ������ƫ����
    this.scroll = 0;                                //�Ѿ�������ƫ����
    this.scrollUnit = params.unit || 5;                //ÿ�ι����ĵ�λ
    this.lock = false;                              //����������������һ��
    this.direction = params.direction || 'left';    //��������
    this.interval = interval;                       //�������ʱ��


    this.timer = null;                              //��Ԫ�ؼ�ʱ��
    this.taskTimer = null;


    //-------------------- ��ȡԪ�� ----------------------//

    //��Ԫ��
    this.$P = document.getElementById(elemID);
    this.$P.style.cssText += 'overflow:hidden;position:relative;';

    //�����б�ĸ�Ԫ��(��һ��UL)
    //UL���li��ʽӦ��padding���ԣ�������margin����

    var _elist = this.$P.getElementsByTagName('UL');
    if (_elist.length == 0) {
        alert('�޷��ҵ�Ԫ��IDΪ:' + this.$P.getAttribute('id') + '�µ�ULԪ��!'); return false;
    } else {
        this.$C = _elist[0];
        this.$L = this.$C.getElementsByTagName('LI');

        //����Ԫ�ص���Ԫ���������ֵ,���ڶ�λ
        for (var i = 0; i < this.$L.length; i++) {
            this.$L[i].setAttribute('scroll-index', i);
        }

    }

    //-------------------- У�鷽�� ----------------------//

    if (this.direction == 'left') {
        //�������������Ϊһ��
        var totalWidth = 0;
        for (var i = 0; i < this.$L.length; i++) {
            totalWidth += this.$L[i].offsetWidth;
        }
        this.$C.style.width = totalWidth + 'px';   //�Ƿ������
    }
    else if (this.direction == 'top') {
    } else {
        alert('��֧�ַ���left��top'); return false;
    }


    //---------------- ��ʼ��������ʾ���� ----------------//

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



    //-------------------- ��ʼ���� ---------------------//
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
    //BUG:�������󣬵�һ����ת����2�ŵ�����Ϊ0
    //

    //��ȡͼƬ����
    this.index = parseInt(this.$L[0].getAttribute('scroll-index'));

    if (this.index == this.$L.length) {
        this.index = 0;
    }

    //��ʾ����
    this.logger((this.index + 1) + '/' + this.$L.length);

    if (this.pagePanel) {
        var es = this.pagePanel.getElementsByTagName('A');
        for (var i = 0; i < es.length; i++) {
            es[i].className = i == this.index ? 'current' : '';
        }
    }
};


//��������
scroller.prototype.setIndex = function (_index) {
    this.lock = true;
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

//��һ��
scroller.prototype.prev = function () {
    //����Ѿ����������Ч
    if (this.lock || this.scroll > this.scrollUnit) {
        return false;
    }
    else {
        this.lock = true;
        var t = this;
        this._start(true, true, function () { t.lock = false; });
    }
};

//��һ��
scroller.prototype.next = function () {
    //����Ѿ����������Ч
    if (this.lock || this.scroll > this.scrollUnit) {
        return false;
    }
    else {
        this.lock = true;
        var t = this;
        this._start(!true, true, function () { t.lock = false; });
    }
};



//��ʼ
scroller.prototype._start = function (asc, internal, call) {
    if (this.lock && !internal) { return false; }

    //��һ��
    if (!asc) {
        this._sc_left(call);
    } else {
        this._sc_right(call);
    }

    //ͬ��״̬
    this._async();
};

//���¿�ʼ
scroller.prototype._restart = function () {
    var t = this;
    //�ظ�����
    if (this.taskTimer) {
        clearTimeout(this.taskTimer);
    }
    this.taskTimer = setTimeout(function () { t._start(); }, this.interval);
};

//�������
scroller.prototype._sc_left = function (call) {
    var ref = this;
    var _offset = ref.$L[0].offsetWidth;

    this.timer = setInterval(function () {

        if (ref.scroll == _offset) {
            clearInterval(ref.timer);
            ref.scroll = 0;

            //��ӵ�����
            var node = ref.$L[0];
            ref.$C.removeChild(node);
            ref.$C.appendChild(node);

            //ִ�л�ִ����
            if (call) { call(); }

            ref._restart();


            ref.$C.style.marginLeft = '0px';
            return false;
        }

        ref.scroll += ref.scrollUnit;

        if (ref.scroll > _offset) {
            ref.scroll = _offset;
        }
        ref.$C.style.marginLeft = (-ref.scroll) + 'px';

    }, 10);

};

//���ɹ���
scroller.prototype._sc_right = function (call) {

    //��һ��
    var ref = this;
    var _offset = ref.$L[ref.$L.length - 1].offsetWidth;

    //׷�ӵ�ǰ��
    var node = ref.$C.lastChild;
    ref.$C.removeChild(node);
    ref.$C.insertBefore(node, ref.$L[0]);
    //����λ��
    ref.scroll = -_offset;


    this.timer = setInterval(function () {

        if (ref.scroll == 0) {
            clearInterval(ref.timer);

            //ִ�л�ִ����
            if (call) { call(); }

            ref._restart();


            ref.$C.style.marginLeft = '0px';
            return false;
        }

        ref.scroll += ref.scrollUnit;

        if (ref.scroll > 0) {
            ref.scroll = 0;
        }
        ref.$C.style.marginLeft = (ref.scroll) + 'px';
    }, 10);
};



//
// DIV����Ч��
// ����ʱ�䣺2011-10-26 14:51
// _setup����ÿ���������
//
function roller(arguments) {
    //����
    this.elem = arguments.elem;
    this.direction = arguments.direction;
    this.pix = arguments.pix;
    this.elem.style.cssText += 'overflow:hidden;';
}
roller.prototype.start = function (_setup, callback) {

    var _elem = this.elem,
            _pix = this.pix,
            _setup = _setup | 1;

    //i��jΪ����
    var i, j;

    //��ʱ��
    var timer;

    var callbackFunc = function () { if (callback != null) callback(); };

    //
    // ���Ϻ������߼�һ��,���º�������֮
    //
    switch (this.direction) {
        case "up":
            i = _pix; j = 0;
            timer = setInterval(function () {
                i -= _setup;
                if (i < 0) {
                    i = 0;
                    clearInterval(timer); callbackFunc();
                }
                _elem.style.height = i.toString() + "px";
            }, 10);
            break;

        case "left":
            i = _pix; j = 0;
            timer = setInterval(function () {
                i -= _setup;
                if (i < 0) {
                    i = 0;
                    clearInterval(timer); callbackFunc();
                }
                _elem.style.width = i.toString() + "px";
            }, 10);
            break;

        case "down":
            i = 0; j = _pix;
            timer = setInterval(function () {
                i += _setup;
                if (i > j) {
                    i = j;
                    clearInterval(timer); callbackFunc();
                }
                _elem.style.height = i.toString() + "px";
            }, 10);
            break;

        case "right":
            i = 0; j = _pix;
            timer = setInterval(function () {
                i += _setup;
                if (i > j) {
                    i = j;
                    clearInterval(timer); callbackFunc();
                }
                _elem.style.width = i.toString() + "px";
            }, 10);
            break;

    }

};



/************************Dialog****************************/
/*
* name :Dialog
* date :2010/11/29
* update:2011/11/07
*/

//
// ģ̬������ʽ����ʾ�����룺
//.dialog .bglayer{background:black;opacity:0.6;filter:alpha(opacity=60);}
//.dialog .box{border:solid 1px #0066cc;background:white;padding:5px;}
//.dialog .box .loader{width:80px;background:url(/style/admin/ajax-loader.gif) center center no-repeat;height:30px;}
//.dialog .box .title{font-size:14px;font-weight:bold;color:#006699;background:#f5f5f5;padding:0 10px;line-height:25px;border-bottom:solid 1px #e5e5e5;margin:-5px -5px 5px -5px;}
//.dialog .box .close{padding:0 5px;font-weight:normal;}
//

//�Ի��������
//����ʵ��ʱ�������������ر�ʱ��ȫ�����������Ƴ�
var dialogPanel = new Array();

//
// ����ʾ����
// var d=new dialog({title:'�Ի���',usedrag:true,style:'dialog default'});
// d.async('ajaxtest');
//

function dialog(_dialog) {

    //ģ̬��ı��
    this.id = new Date().getMilliseconds() + parseInt(Math.random() * 100);
    this.title = _dialog.title;                                                     //����
    this.usedrag = _dialog.usedrag;                                                 //�Ƿ�ʹ����ק
    this.style = _dialog.style || 'dialog';                                         //�Ի�����ʽ,Ĭ��dialog
    this.setupFade = !_dialog.setupFade ? _dialog.setupFade : true;                 //�Ƿ񽥽���ȥ
    this.onclose;                                                                   //�رպ���,�������false,�򲻹ر�

    //������С��������ҳ��С��
    this.canvas = {
        width: document.documentElement.clientWidth,
        clientHeight: document.documentElement.clientHeight,
        height: document.documentElement.clientHeight > document.body.clientHeight
                ? document.documentElement.clientHeight : document.body.clientHeight
    };


    //ģ̬���λ��
    this.point = {
        x: parseInt((this.canvas.width) / 2) + document.documentElement.scrollLeft,
        y: parseInt((this.canvas.clientHeight) / 2) + document.documentElement.scrollTop
    };

    //��ӵ�����������
    dialogPanel.push(this);


    //��ʼ��
    var initialize = function (dialog) {

        //���Panel
        var panel = document.createElement('div');
        panel.id = 'panel_' + dialog.id;
        panel.className = dialog.style;
        document.body.appendChild(panel);

        //����ڸǲ�
        var elem = document.createElement('div');
        elem.className = 'bglayer';
        elem.style.cssText = 'z-index:99;position:absolute;top:0;left:0;width:' + dialog.canvas.width + 'px;height:' + dialog.canvas.height + 'px;';
        panel.appendChild(elem);

        //���ģ̬����
        elem = document.createElement("DIV");
        elem.className = 'box';
        //elem.id = 'box_' + dialog.id;
        elem.style.cssText = 'z-index:100;position:absolute;left:' + (dialog.point.x) + "px;top:" + (dialog.point.y) + 'px;';
        panel.appendChild(elem);

        //�����ʾ����
        if (dialog.title) {
            var titleElem = document.createElement("div");
            titleElem.className = 'title';
            titleElem.innerHTML = dialog.title + '<a href="javascript:closeDialog(\'' + dialog.id + '\');" class="close" style="position:absolute;right:5px;top:0;text-decoration:none;font-family:Verdana" title="�رմ���">X</a>';
            elem.appendChild(titleElem);
        }

    };

    initialize(this);

    /****************************  ��غ��� *******************************/

    //����Ի����λ��
    this.fixBoxPosition = function (relay) {

        var box = this.getPanel().getElementsByTagName('DIV')[1];


        //��ʱ����
        if (!relay) {

            this.point.x = (this.canvas.width - box.offsetWidth) / 2 + document.documentElement.scrollLeft;
            this.point.y = (this.canvas.clientHeight - box.offsetHeight) / 2 + document.documentElement.scrollTop;
            box.style.left = this.point.x + 'px';
            box.style.top = this.point.y + 'px';

            //���ʹ���϶�
            if (this.title && this.usedrag) {
                new drag(box.getElementsByTagName('div')[0]).start(box);              //ʹ����ק����
            }

        } else {

            var dialog = this;
            var i = box.offsetWidth;
            //���ö��,IE�»��޷��������offsetHeight;
            var timer = setInterval(function () {

                dialog.point.x = (dialog.canvas.width - box.offsetWidth) / 2 + document.documentElement.scrollLeft;
                dialog.point.y = (dialog.canvas.clientHeight - box.offsetHeight) / 2 + document.documentElement.scrollTop;

                box.style.left = dialog.point.x + 'px';
                box.style.top = dialog.point.y + 'px';

                if (i != box.offsetWidth) {

                    clearInterval(timer);

                    //���ʹ���϶�
                    if (dialog.title && dialog.usedrag) {
                        new drag(box.getElementsByTagName('div')[0]).start(box);              //ʹ����ק����
                    }

                }
            }, 1);
        }


    };
}

//�Ի�������
dialog.prototype.getPanel = function () { return document.getElementById('panel_' + this.id); };

//�첽��ȡ
dialog.prototype.async = function (uri, method, params, func) {

    var boxElem = this.getPanel().getElementsByTagName('DIV')[1]; //������2����

    //����������ʾ
    var loader = document.createElement('DIV');
    loader.className = 'loader';
    boxElem.appendChild(loader);
    //�����ʽ��δ��������ʾͼƬ
    if (loader.offsetWidth == 0) {
        loader.innerHTML = '<div style="line-height:25px;text-align:center;color:#006699;width:100px">loading...</div>';
    }


    var removeLoader = function () {
        boxElem.removeChild(loader);
    };
    //�첽��ȡ����

    var ajax = new ajaxObj();
    if (!method || method.toLowerCase() == "get") {
        ajax.get(uri, function (x) {
            removeLoader();
            boxElem.innerHTML += x;
            if (func) func(x);
        });
    } else {
        ajax.post(uri, params, function (x) {
            removeLoader();
            boxElem.innerHTML += x;
            if (func) func(x);
        }, function (x) {
            removeLoader();
        });

    }
    this.fixBoxPosition(true);   //����λ��
};

//����ַ
dialog.prototype.open = function (uri, width, height, scroll) {
    var boxElem = this.getPanel().getElementsByTagName('DIV')[1]; //������2����
    boxElem.innerHTML += "<iframe frameborder='0' scrolling='" + scroll + "' src='" + uri + "' width='" + width + "' style='padding:0' height='" + height + "'></iframe>";
    this.fixBoxPosition();
};

//д������
dialog.prototype.write = function (html) {
    var boxElem = this.getPanel().getElementsByTagName('DIV')[1]; //������2����
    if (!this.title) {
        boxElem.innerHTML = html;
    } else {
        var divs = boxElem.getElementsByTagName('DIV');
        for (var i = 1; i < divs.length; i++) {
            boxElem.removeChild(divs[i]);
        }
        boxElem.innerHTML += html;
    }
    this.fixBoxPosition();
};

//
// �رնԻ���
// callbackΪ�ر�ʱ�����Ļ�ִ����
//
dialog.prototype.close = function (callback) {
    if (this.onclose != null && !this.onclose()) {
        return false;
    }

    var opacity;    //͸����
    var panel = this.getPanel();

    //�Ƴ�ģ̬����
    panel.removeChild(panel.getElementsByTagName('div')[1]);

    //�ڸǲ�
    var bgLayer = panel.getElementsByTagName('div')[0];

    //�Ƴ����
    var closeDialog = function () {
        document.body.removeChild(panel); if (callback) callback();
    };

    //�Ƿ񽥽��ĵ�ȥ
    if (!this.setupFade) {
        closeDialog(); return false;
    }

    //��ȡ͸����
    var allowFilter = document.body.filters != undefined;
    //    if (allowFilter) {
    //        opacity = bgLayer.filters('alpha').opacity ||60;
    //    } else {
    //        opacity = bgLayer.style.opacity||60;
    //    }

    opacity = 60;

    //��ʱ�����ڸǵ�͸����
    var timer = setInterval(function () {
        if (allowFilter) {
            opacity -= opacity < 10 ? 1 : 10;           //IE�����������ִ�н���(1:22)
            bgLayer.filters('alpha').opacity = opacity;
        }
        else {
            bgLayer.style.opacity = (--opacity) / 100;
        }

        //��͸���ȵ��ڻ����0,���Ƴ�panel��ִ�л�ִ����
        if (opacity <= 0) {
            //�����ʱ��
            clearInterval(timer);
            closeDialog();
        }
    }, 1);
};


//�رմ���
function closeDialog(id) {
    for (var i in dialogPanel) {
        if (dialogPanel[i].id == id) dialogPanel[i].close();
    }
}


//
// ��ק����
// 2011-11-08
//

function drag(elem) {
    this.elem = elem;
}
drag.prototype.start = function (obj) {
    var o = this.elem;
    var obj = obj ? obj : o;
    var sx, sy;
    o.style.cursor = "move";
    addListener(o, "mousedown", function (e) {
        e || event;
        if (e.button == 1 || e.button == 0) {
            sx = e.clientX - obj.offsetLeft; sy = e.clientY - obj.offsetTop;
            addListener(document, "mousemove", move, false);
            addListener(document, "mouseup", stopDrag, false);
        }
    }, false);

    var stopDrag = function () {
        removeListener(document, "mousemove", move, false);
        removeListener(document, "mouseup", stopDrag, false);
    };

    var move = function (e) {
        e || event;
        window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();
        if (e.preventDefault) e.preventDefault();                       //��������ǽ��firefox�϶������.
        with (obj.style) {
            position = "absolute";
            left = e.clientX - sx + "px";
            top = e.clientY - sy + "px";
        }
    };
};
