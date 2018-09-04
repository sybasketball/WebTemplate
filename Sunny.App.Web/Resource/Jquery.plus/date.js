function getDate(dates) {
    var dd = new Date();
    dd.setDate(dd.getDate() + dates);
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1;
    var d = dd.getDate();

    return y + "-" + m + "-" + d;
}
function getMonday() {
    var d = new Date();
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var date = d.getDate();

    // 周
    var day = d.getDay();
    var monday = day != 0 ? day - 1 : 6; // 本周一与当前日期相差的天数
    return monday;
}

function getMonth(type, months) {
    var d = new Date();
    var year = d.getFullYear();
    var month = d.getMonth() + 1;

    if (months != 0) {
        // 如果本月为12月，年份加1，月份为1，否则月份加1。
        if (month == 12 && months > 0) {
            year++;
            month = 1;
        } else if (month == 1 && months < 0) {
            year--;
            month = 12;
        } else {
            month = month + months;
        }
    }
    var date = d.getDate();
    var firstday = year + "-" + month + "-" + 1;
    var lastday = "";
    if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
        lastday = year + "-" + month + "-" + 31;
    } else if (month == 2) {
        // 判断是否为闰年（能被4整除且不能被100整除 或 能被100整除且能被400整除）
        if ((year % 4 == 0 && year% 100 != 0) || (year% 100 == 0 && year% 400 == 0)) {
            lastday = year + "-" + month + "-" + 29;
        } else {
            lastday = year + "-" + month + "-" + 28;
        }
    } else {
        lastday = year + "-" + month + "-" + 30;
    }
    var day = "";
    if (type == "s") {
        day = firstday;
    } else {
        day = lastday;
    }
    return day;
}

function getQFMonth(month) {
    var quarterMonthStart = 0;
    var spring = 1; //春  
    var summer = 4; //夏  
    var fall = 7; //秋  
    var winter = 10; //冬  
    if (month < 3) {
        return spring;
    }
    if (month < 6) {
        return summer;
    }
    if (month < 9) {
        return fall;
    }
    return winter;
};

function getQF(type, months) {
    var d = new Date();
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var qfmonth = getQFMonth(month);

    if (months != 0) {
        if (qfmonth == 10 && months > 0) {
            year++;
            qfmonth = 1;
        } else if (qfmonth == 1 && months < 0) {
            year--;
            qfmonth = 10;
        } else {
            qfmonth = qfmonth + months;
        }
    }

    var fd = year + "-" + qfmonth + "-" + 1;
    var ed = "";
    if (qfmonth == 1 || qfmonth == 10) {
        ed = year + "-" + (qfmonth + 2) + "-" + 31;
    } else {
        ed = year + "-" + (qfmonth + 2) + "-" + 30;
    }

    var qf = "";
    if (type == "s") {
        qf = fd;
    } else {
        qf = ed;
    }
    return qf;
}

function getYears(type, years) {
    var d = new Date();
    var year = d.getFullYear();

    var fd = (year + years) + "-" + 01 + "-" + 01;
    var ed = (year + years) + "-" + 12 + "-" + 31;

    var yr = "";
    if (type == "s") {
        yr = fd;
    } else {
        yr = ed;
    }
    return yr;
}
//+---------------------------------------------------  
//| 求两个时间的天数差 日期格式为 YYYY-MM-dd   
//+---------------------------------------------------  
function daysBetween(DateOne, DateTwo) {
    var OneMonth = DateOne.substring(5, DateOne.lastIndexOf('-'));
    var OneDay = DateOne.substring(DateOne.length, DateOne.lastIndexOf('-') + 1);
    var OneYear = DateOne.substring(0, DateOne.indexOf('-'));

    var TwoMonth = DateTwo.substring(5, DateTwo.lastIndexOf('-'));
    var TwoDay = DateTwo.substring(DateTwo.length, DateTwo.lastIndexOf('-') + 1);
    var TwoYear = DateTwo.substring(0, DateTwo.indexOf('-'));

    var cha = ((Date.parse(OneMonth + '/' + OneDay + '/' + OneYear) - Date.parse(TwoMonth + '/' + TwoDay + '/' + TwoYear)) / 86400000);
    return Math.abs(cha);
}
//字符串转日期格式，strDate要转为日期格式的字符串
function getDateObj(strDate) {
    var date = eval('new Date(' + strDate.replace(/\d+(?=-[^-]+$)/,
             function (a) { return parseInt(a, 10) - 1; }).match(/\d+/g) + ')');
    return date;
}
//字符串转成Time(dateDiff)所需方法
function stringToTime(string) {
    var f = string.split(' ', 2);
    var d = (f[0] ? f[0] : '').split('-', 3);
    var t = (f[1] ? f[1] : '').split(':', 3);
    return (new Date(
    parseInt(d[0], 10) || null,
    (parseInt(d[1], 10) || 1) - 1,
    parseInt(d[2], 10) || null,
    parseInt(t[0], 10) || null,
    parseInt(t[1], 10) || null,
    parseInt(t[2], 10) || null
    )).getTime();
}
function dateDiff(date1, date2) {
    var type1 = typeof date1, type2 = typeof date2;
    if (type1 == 'string')
        date1 = stringToTime(date1);
    else if (date1.getTime)
        date1 = date1.getTime();
    if (type2 == 'string')
        date2 = stringToTime(date2);
    else if (date2.getTime)
        date2 = date2.getTime();
    return (date2 - date1) / (1000 * 60 * 60 * 24); //结果是秒
}